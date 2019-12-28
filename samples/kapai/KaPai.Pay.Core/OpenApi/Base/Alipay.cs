using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Hangfire;
using KaPai.Pay.Merchant;
using KaPai.Pay.Merchant.Entities;
using KaPai.Pay.OpenApi.Dtos;
using KaPai.Pay.Provide.Entities;
using Microsoft.Extensions.Logging;
using OSharp.Data;
using OSharp.Entity;
using StackExchange.Redis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using OSharp.Exceptions;
using IP2Region;
using KaPai.Pay.Provide;
using KaPai.Pay.SignalR.HubPro;
using KaPai.Pay.SignalR.HubPro.Event;
using Microsoft.AspNetCore.SignalR;

namespace KaPai.Pay.OpenApi.Base
{

    [DisplayName("Alipay通道处理类")]
    public class Alipay : INterfaceChannel
    {
        protected readonly IServiceProvider ServiceProvider;
        protected readonly IRepository<Orders, Guid> OrderRepository;
        protected readonly ILogger<Alipay> Logger;
        protected readonly IDatabase Database;
        protected readonly IHubContext<HubPro, IHubPro> HubContext;
        protected readonly IProvideContract ProvideContract;

        protected readonly string HashName = "ClientPayLog:Alipay";
        protected readonly string ListName = "Channel:ArticleAssort:Alipay";

        protected readonly List<Task> CreateTasks = new List<Task>();
        protected readonly List<Task> UpdateTasks = new List<Task>();

        public Alipay(
            IServiceProvider provider,
            ILoggerFactory logger,
            IRepository<Orders, Guid> orderRepository,
            IDatabase database, IHubContext<HubPro, IHubPro> hubContext, 
            IProvideContract provideContract)
        {
            ServiceProvider = provider;
            OrderRepository = orderRepository;
            Database = database;
            HubContext = hubContext;
            ProvideContract = provideContract;
            Logger = logger.CreateLogger<Alipay>();
        }



        /// <summary>
        /// 创建订单处理
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public async Task<OperationResult<OrderOutDto>> CreateOrder(Orders order)
        {
            string id = order.Id.ToString();
            // 启动超时任务处理 ...
            order.JobId = BatchJob.StartNew(x =>
            {
                x.Schedule<IMerchantContract>(m => m.OrderTimeOut(id), TimeSpan.FromMinutes(30)); // 30分钟后订单状态超时

            }, $"订单:{id},超时任务");
            var count = await OrderRepository.InsertAsync(order);
            if (count < 1)
            {
                BatchJob.Cancel(order.JobId);
                Logger.Log(LogLevel.Debug, "用户:{0},订单:{1},插入失败", order.UserId, order.Orderid);
                return new OperationResult<OrderOutDto>(OperationResultType.Error, "系统发生错误,请联系网站管理员");
            }

            // todo:需要获取跳转域名
            var url = "http://localhost:4203/";
            var dto = new OrderOutDto
            {
                SysId = order.Id,
                MerchantId = order.UserId,
                CreatedAmount = order.CreatedAmount,
                OrderId = order.Orderid,
                Remark = order.Remark,
                Time = OpenApi.GetTimeStamp(),
                PayUrl = $"{url}#/payorder/test/{order.Id}", // 拼接完整URL
            };
            return new OperationResult<OrderOutDto>(OperationResultType.Success, "正常", dto);
        }

        /// <summary>
        /// 收款号选取处理
        /// </summary>
        /// <param name="order"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<OperationResult<PortionOrderOut>> ArticleAssortHandle(Orders order, PortionInDto dto)
        {

            // 根据客户ID 生成分布式锁 键值
            string key = $"Lock:Alipay:{dto.ClientId}";
            string val = Guid.NewGuid().ToString("N");

            if (Database.StringGet($"Lock:Alipay:{order.Id.ToString()}").IsNullOrEmpty)
            {
                // 10分钟 只准分配一次
                Database.StringSet($"Lock:Alipay:{order.Id.ToString()}", 1, TimeSpan.FromMinutes(10));

                if (Database.LockTake(key, val, TimeSpan.FromSeconds(30)))
                {
                    try
                    {
                        // 获取历史数据

                        var payLog = await Database.SortedSetLengthAsync($"{HashName}:{dto.ClientId}");
                        if (payLog < 10)
                        {
                            // 默认选取收款账号
                            await DefaultArticleAssort(order, dto);
                        }
                        else
                        {
                            // 根据历史记录选择账号
                            await PayLogArticleAssort(order, dto);
                        }
                        return new OperationResult<PortionOrderOut>(OperationResultType.Success, $"ok...{payLog}", new PortionOrderOut() { ClientId = dto.ClientId });
                    }
                    finally
                    {
                        Database.LockRelease(key, val);
                    }

                }
                Logger.LogError($"锁{key}获取失败,{dto.Id}");
                return new OperationResult<PortionOrderOut>(OperationResultType.Error, "分配账号超时", new PortionOrderOut() { ClientId = dto.ClientId });

            }
            return new OperationResult<PortionOrderOut>(OperationResultType.Success, "处理冷却", new PortionOrderOut() { ClientId = dto.ClientId });
        }

        /// <summary>
        /// 通道创建收款号操作
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public async Task<ArticleEntities> CreateArticleEntitieses(ArticleEntities m)
        {

            if (m.Extra.RootElement.TryGetProperty("region", out JsonElement regionElement))
            {
                if (regionElement.GetProperty("ValueType").GetInt32() == 8)
                {
                    var regionId = regionElement.GetProperty("Value").GetInt32();


                    // 选择的地区权重列表
                    CreateTasks.Add(Database.SortedSetAddAsync($"{ListName}:region:{regionId}", m.Id.ToString(), 0, CommandFlags.FireAndForget));
                    // 默认地区权重列表
                    CreateTasks.Add(Database.SortedSetAddAsync($"{ListName}:region:1", m.Id.ToString(), 0, CommandFlags.FireAndForget));
                    // 账号的开关信息
                    CreateTasks.Add(Database.StringSetBitAsync($"{ListName}:status:{m.Id}", 0, m.Status == 1, CommandFlags.FireAndForget));

                    return m;
                }
            }
            throw new OsharpException($"region 未定义 或 region值不正确");
        }

        public async Task<bool> CreateEnd()
        {
            Database.WaitAll(CreateTasks.ToArray());
            CreateTasks.Clear();
            return true;
        }


        /// <summary>
        /// 更新通道收款号操作
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public async Task<ArticleEntities> UpdataArticleEntitieses(ArticleEntities m)
        {
            // 账号开通则单独记录
            var key = $"{ListName}:status:{m.Id}";
            UpdateTasks.Add(Database.StringSetBitAsync(key, 0, m.Status == 1, CommandFlags.FireAndForget));
            return m;
        }


        /// <summary>
        /// 全部更新完成
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public async Task<bool> UpdateEnd()
        {
            Database.WaitAll(UpdateTasks.ToArray());
            UpdateTasks.Clear();
            return true;
        }

        /// <summary>
        /// 订单设置收款号
        /// </summary>
        /// <returns></returns>
        public async Task<bool> OrderSetArticle(Orders order, Guid articleIdGuid)
        {

            // 为订单赋值 订单设置收款号
            order.ArticleAssortId = articleIdGuid;
            await OrderRepository.UpdateAsync(order);
            return true;
        }

        /// <summary>
        /// 设置账号在线
        /// </summary>
        /// <param name="m"></param>
        /// <param name="online"></param>
        /// <returns></returns>
        public async Task<bool> ArticleOnline(ArticleEntities m, bool? online = null)
        {
            string key = $"{ListName}:status:{m.Id}";
            if (!online.HasValue)
            {
                return  await ArticleAssortOnline(key);
            }
            return await Database.StringSetBitAsync(key, 1, online.Value);
        }

        /// <summary>
        /// 通道订单完成付款调用 抛出错误会中断结算
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public async Task<bool> OrderComplete(Orders order)
        {
            //订单付款了 增加历史收款记录
            var paykey = $"{HashName}:{order.ClientId}";
            if (order.ArticleAssortId == null)
            {
                throw new OsharpException($"未设置收款号:{order.Id}");
            }
            string articleid = order.ArticleAssortId.ToString();
            var hAsync = await Database.SortedSetRankAsync(paykey, articleid);
            if (!hAsync.HasValue)
            {
                await Database.SortedSetAddAsync(paykey, articleid, 0);
            }

            return true;
        }

        /// <summary>
        /// 通道订单超时调用 是否分配收款号 已分配需要解冻金额
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public async Task<bool> OrderTimeOut(Orders order)
        {
            // 已经分配收款号!
            if (order.ArticleAssort != null)
            {
                var amount = order.CreatedAmount;
                var userid = order.ArticleAssort.UserId;
                // 解冻积分
                await ProvideContract.ProvideFreezePointChange(userid, -amount);
            }
            return true;
        }

        /// <summary>
        /// 默认的选取收款号
        /// </summary>
        private async Task DefaultArticleAssort(Orders order, PortionInDto dto)
        {
            string regionkey = $"{ListName}:region";
            List<Task> tasklist = new List<Task>();

            // todo:根据经纬获取地址

            // 根据IP获取地址信息
            var ipService = ServiceProvider.GetService<DbSearcher>();
            var reBlock = ipService.MemorySearch(order.ClientIp);

            string nowkey = $"{regionkey}:{reBlock.CityID}";

            string statuskey = $"{ListName}:status";

            // 根据地址信息 获取所有待选账号
            var articlelist = new List<string>();

            // 遍历区域账号 并查找他是否开通 开通则添加至待选
            var scan = Database.SortedSetScan(nowkey);
            foreach (var entry in scan)
            {
                var status = await ArticleAssortStatus($"{statuskey}:{ entry.Element}");
                if (status)
                {
                    articlelist.Add(entry.Element);
                }
                if (articlelist.Count >= 10)
                {
                    break;
                }
            }

            // 如果目标待选区域没有账号 则从默认区域读取所有信息
            if (articlelist.Count <= 0)
            {
                nowkey = $"{regionkey}:1";
                scan = Database.SortedSetScan(nowkey);
                foreach (var entry in scan)
                {

                    var status = await ArticleAssortStatus($"{statuskey}:{ entry.Element}");
                    if (status)
                    {
                        articlelist.Add(entry.Element);
                    }
                    if (articlelist.Count >= 10)
                    {
                        break;
                    }
                }
            }

            foreach (var @value in articlelist)
            {
                // 递增score
                tasklist.Add(Database.SortedSetIncrementAsync(nowkey, @value, 1, CommandFlags.FireAndForget));
            }
            // 合并操作
            Database.WaitAll(tasklist.ToArray());

            // 普通自有通道只选取一个收款实体 调用 OrderSetArticle 即可



            // 获取连接ID
            var sender = await Database.HashGetAsync("HubPro", articlelist.Select(a => (RedisValue)a).ToArray());
            // 向选取的收款号,发送抢单消息 可能需要设置为后台发送
            await HubContext.Clients.Clients(sender.ToStringArray()).Order(new OrderOut
            {
                Action = 1,
                Amount = order.CreatedAmount,
                OrderId = order.Id,
                CreatedTime = order.CreatedTime,
            });
        }

        /// <summary>
        /// 尝试根据历史记录选取收款号
        /// </summary>
        /// <returns></returns>
        private async Task PayLogArticleAssort(Orders order, PortionInDto dto)
        {
            var logkey = $"{HashName}:{dto.ClientId}";
            string statuskey = $"{ListName}:status";
            List<string> articlelist = new List<string>();

            var scan = Database.SortedSetScan(logkey);

            foreach (SortedSetEntry setEntry in scan)
            {
                var status = await ArticleAssortStatus($"{statuskey}:{ setEntry.Element}");
                if (status)
                {
                    articlelist.Add(setEntry.Element);
                }
                if (articlelist.Count >= 10)
                {
                    break;
                }
            }

            if (articlelist.Count <= 0)
            {
                // 没有开启的账号
                await DefaultArticleAssort(order, dto);
                return;
            }

            List<Task> tasklist = new List<Task>();
            foreach (var @value in articlelist)
            {
                // 递增score
                tasklist.Add(Database.SortedSetIncrementAsync(logkey, @value, 1, CommandFlags.FireAndForget));
            }
            // 合并操作
            Database.WaitAll(tasklist.ToArray());




            // 获取连接ID
            var sender = await Database.HashGetAsync("HubPro", articlelist.Select(a => (RedisValue)a).ToArray());
            // 向选取的收款号,发送抢单消息 可能需要设置为后台发送
            await HubContext.Clients.Clients(sender.ToStringArray()).Order(new OrderOut
            {
                Action = 1,
                Amount = order.CreatedAmount,
                OrderId = order.Id,
                CreatedTime = order.CreatedTime,
            });
        }


        /// <summary>
        /// 取当前账号是否可用
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private async Task<bool> ArticleAssortStatus(string key)
        {

            /**
            var status = await Database.StringGetAsync(key);
            if (status.HasValue)
            {
                byte[] s = status;
                var i = s[0];
                var isOpen = (i | 1 << 7) == i;
                var isOnline = (i | 1 << 6) == i;
                return isOpen && isOnline;
            }
            return false;**/

            var lua = LuaScript.Prepare("return redis.call('getbit',@key,@offset0) && redis.call('getbit',@key,@offset1)");
            var isok = (bool)await Database.ScriptEvaluateAsync(lua, new { key = key, offset0 = 0, offset1 = 1 });
            return isok;
        }

        /// <summary>
        /// 获取是否在线
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private async Task<bool> ArticleAssortOnline(string key)
        {
            return await Database.StringGetBitAsync(key, 1);
        }

        /// <summary>
        /// 获取是否开通
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private async Task<bool> ArticleAssortOpen(string key)
        {
            return await Database.StringGetBitAsync(key, 0);
        }
    }
}
