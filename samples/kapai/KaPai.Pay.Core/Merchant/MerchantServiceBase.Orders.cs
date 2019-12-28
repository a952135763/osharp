using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.States;
using KaPai.Pay.Channel.Entities;
using KaPai.Pay.Hangfire;
using KaPai.Pay.Identity.Entities;
using KaPai.Pay.Merchant.Dtos;
using KaPai.Pay.Merchant.Entities;
using KaPai.Pay.My;
using KaPai.Pay.Provide;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using OSharp.Entity;
using OSharp.Extensions;
using OSharp.Json;
using OSharp.Mapping;

namespace KaPai.Pay.Merchant
{
    public abstract partial class MerchantServiceBase
    {
        protected IRepository<User, int> UserRepository => ServiceProvider.GetService<IRepository<User, int>>();
        protected IRepository<Percentage, Guid> PercentageRepository => ServiceProvider.GetService<IRepository<Percentage, Guid>>();

        protected IProvideContract ProvideContract =>ServiceProvider.GetService<IProvideContract>();


        /// <summary>
        /// 把订单从 待处理 或 处理中 更新到 超时
        /// </summary>
        /// <param name="orderid">订单ID</param>
        /// <returns>操作结果</returns>
        public async Task<bool> OrderTimeOut(string orderid)
        {
            var order = OrdersRepository.Query(o => o.Status < 2).FirstOrDefault(o => o.Id == Guid.Parse(orderid));
            if (order == null) return false;
            order.Status = 5;
            order.LastUpdatedTime = DateTime.Now;
            order.LastUpdaterId = 0;

            // 订单超时调用
            var complete = await order.Channel.GetChannelBase(ServiceProvider).OrderTimeOut(order);

            var count = await OrdersRepository.UpdateAsync(order);
            if (count <= 0) return false;
            OrdersRepository.UnitOfWork.Commit();
            return true;
        }


        /// <summary>
        /// 把订单 从 处理中 更新到 已付款,并启动回调,结算任务
        /// </summary>
        /// <param name="orderid">订单ID</param>
        /// <param name="payamount">付款金额</param>
        /// <param name="settletask">是否启动结算回调动作 false</param>
        /// <param name="callback">是否启动回调任务</param>
        /// <returns>操作结果</returns>
        public async Task<bool> OrderPayment(string orderid, long payamount, bool settletask = true, bool callback = true)
        {
            var order = OrdersRepository.Query(o => o.Status == 1).FirstOrDefault(o => o.Id == Guid.Parse(orderid));
            if (order == null) return false;
            order.Status = 2;
            order.PayAmount = payamount;

            
            // 订单完成付款调用
            var complete = await order.Channel.GetChannelBase(ServiceProvider).OrderComplete(order);

            var outJobId = order.JobId;
            if (settletask)
            {
                order.JobId = BatchJob.StartNew((x) =>
                {
                    // 延迟30秒启动 订单结算任务
                    x.Schedule<IMerchantContract>(m => m.OrderSettle(order.UserId, orderid),TimeSpan.FromSeconds(30));
                    // 根据判断是否启动回调任务
                    if (callback)
                    {
                        x.Enqueue<IMerchantContract>(m => m.OrderCallback(orderid));
                    }

                }, $"订单:{order.Id},流程");

            }


            var count = await OrdersRepository.UpdateAsync(order);
            if (count < 1)
            {
                BatchJob.Cancel(order.JobId); // 更新失败 取消结算任务
                return false;
            }
            BatchJob.Cancel(outJobId);// 取消超时任务
            OrdersRepository.UnitOfWork.Commit();
            return true;
        }


        /// <summary>
        /// 订单结算任务
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public async Task<bool> OrderSettle(int userid, string orderid)
        {
            var user = UserRepository.Query(u => u.Id == userid).FirstOrDefault();
            if (user == null)
            {
                Logger.Log(LogLevel.Error, "未找到用户{0}-{1}", userid, orderid);
                return false;
            }
            var order = OrdersRepository.Query(o => o.Id == Guid.Parse(orderid)).FirstOrDefault(o => o.Status == 2);
            if (order == null)
            {
                Logger.Log(LogLevel.Error, "未找到结算订单...{0}", orderid);
                return false;
            }
            // 更改成已结算状态
            order.Status = 3;

            var count = await OrdersRepository.UpdateAsync(order);
            if (count < 1)
            {
                throw new Exception($"订单结算更新失败:{orderid}");
            }
            // 是否是商户
            var isMerchant = user.UserRoles.Any(r => r.Role.Name == "商户");

            // 是否是供应商
            var isProvide = user.UserRoles.Any(r => r.Role.Name == "供应商");

            // 如果是供应商 也是商户 
            if (isMerchant && isProvide)
            {
                // 商户不结算积分  供应商正常扣除积分
                if (await ProvideContract.ProvideOrderSettle(order.ArticleAssort.UserId, order))
                {
                    Logger.Log(LogLevel.Error, "供应商积分变动失败 {0},{1}", userid, orderid);
                    return false;
                }
                OrdersRepository.UnitOfWork.Commit();
                return true;
            }

            // 不是供应商  但是是商户!
            if (!isProvide && isMerchant)
            {
                // 正常商户订单积分结算流程
                if (!await OrderSettleHandleAsync(user, order))
                {
                    Logger.Log(LogLevel.Error, "商户积分变动失败 {0},{1}", userid, orderid);
                    return false;
                }
                // 处理供应商积分 
                if (!await ProvideContract.ProvideOrderSettle(order.ArticleAssort.UserId, order))
                {
                    Logger.Log(LogLevel.Error, "供应商积分变动失败 {0},{1}", userid, orderid);
                    // 供应商
                    return false;
                }
                OrdersRepository.UnitOfWork.Commit();
                return true;
            }
            Logger.Log(LogLevel.Error, "未处理的流程 {0},{1}", isMerchant, isProvide);
            return false;
        }


        public async Task<bool> OrderSettleHandleAsync(User user, Orders order)
        {
            var userExtra = MerchantExtraRepository.QueryAsNoTracking(m => m.UserId == user.Id).FirstOrDefault();
            if (userExtra == null)
            {
                Logger.Log(LogLevel.Error, "MerchantExtra未分配...{0},{1}", order.Id, user.Id);
                return false;
            }

            var defaultper = PercentageRepository.QueryAsNoTracking(p => p.ChannelId == order.ChannelId).Where(p => p.UserId == user.Id)
                .FirstOrDefault(p => p.Name == "普通费率");
            if (defaultper == null)
            {
                Logger.Log(LogLevel.Error, "未找到'普通费率'...{0}", order.Id);
                return false;
            }
            // 没有上级用户,使用普通费率进行计算
            var useramount = order.CreatedAmount * (1 - Convert.ToDecimal(defaultper.Value) / 1000);


            // 判断用户是否有上级
            if (userExtra.PUserId != null)
            {
                // 有上级用户,给上级用户增加指定积分
                // 默认为0嘛 没有
                var puserper = PercentageRepository.QueryAsNoTracking(p => p.ChannelId == order.ChannelId)
                    .Where(p => p.UserId == user.Id).Where(p => p.Name == "上级反点").Select(p => p.Value)
                    .FirstOrDefault();

                // 计算上级用户增加金额
                var amount = useramount - order.CreatedAmount * (1 - Convert.ToDecimal(defaultper.Value) / 1000 - Convert.ToDecimal(puserper) / 1000);

                // 重新计算 商户增加金额
                useramount -= amount;
                if (amount > 0)
                {
                    // 给上级代理增加积分 
                 await MerchantAmountChange(userExtra.PUserId.Value, (long) amount, order.Id.ToString(), "下级结算",
                        $"下级{user.Id},结算");
                }
            }
            // 启动给用户变动积分任务 增加积分 
            // 实时更新商户余额
            await MerchantAmountChange(user.Id, (long) useramount, order.Id.ToString(), "订单结算", "积分结算");
            return true;
        }




        // 回调订单
        public async Task<bool> OrderCallback(string orderid)
        {

            return false;
            var order = OrdersRepository.GetFirst(o => o.Id == Guid.Parse(orderid));
            if (order.Status == 2 || order.Status == 3)
            {
                var outputDto = new OrdersOutBack(order) {Status = 1};
                var singstr = OpenApi.OpenApi.SignString(outputDto, new[] { "Sign" });
                var userkey = MerchantExtraRepository.QueryAsNoTracking(m => m.UserId == outputDto.UserId).Select(d => d.Key)
                    .FirstOrDefault();
                var sign = OpenApi.OpenApi.HmacMd5String($"{singstr}{userkey}", outputDto.Time);
                outputDto.Sign = sign;
                var postjson = outputDto.ToJsonString();
                // 开始访问地址
                var count = OrderBackLogRepository.Query(q => q.OrderId == outputDto.Id).Count();
                try
                {
                    HttpClient http = ServiceProvider.GetService<HttpClient>();
                    http.DefaultRequestHeaders.Add("Content-type", "application/json");
                    var message = await http.PostAsync(order.AsynUrl, new StringContent(postjson));
                    var body = await message.Content.ReadAsStringAsync();
                    if (message.IsSuccessStatusCode)
                    {
                        if ("OK".Equals(body))
                        {
                            // 回调成功!
                            await OrderBackLogRepository.InsertAsync(new OrderBackLog()
                            {
                                Message = "回调成功",
                            });
                            OrderBackLogRepository.UnitOfWork.Commit();
                            return true;
                        }
                    }
                    if (body.Length > 50) body = body.Substring(0, 50);
                    await OrderBackLogRepository.InsertAsync(new OrderBackLog()
                    {
                        Message = $"回调失败,HTTP状态码:{message.StatusCode},回复内容:{body}",
                        OrderId = outputDto.Id,
                    });
                }
                catch (Exception e)
                {
                    await OrderBackLogRepository.InsertAsync(new OrderBackLog()
                    {
                        Message = $"回调失败,引发错误:{e.Message},{e.Source}",
                        OrderId = outputDto.Id,
                    });
                    
                }
                OrderBackLogRepository.UnitOfWork.Commit();
            }
            return false;
        }
        

    }
}
