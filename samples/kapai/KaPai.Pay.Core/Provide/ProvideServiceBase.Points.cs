using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hangfire;
using JetBrains.Annotations;
using KaPai.Pay.Channel;
using KaPai.Pay.Merchant;
using KaPai.Pay.Merchant.Entities;
using KaPai.Pay.Provide.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using OSharp.Entity;
using StackExchange.Redis;

namespace KaPai.Pay.Provide
{
    public abstract partial class ProvideServiceBase
    {
        protected IMerchantContract MerchantContract => ServiceProvider.GetService<IMerchantContract>();

        protected IRepository<Orders, Guid> OrderRepository => ServiceProvider.GetService<IRepository<Orders, Guid>>();

        protected IChannelContract ChannelContract => ServiceProvider.GetService<IChannelContract>();

        protected IDatabase Database => ServiceProvider.GetService<IDatabase>();


        protected readonly string TypeName = "Provide";

        /// <summary>
        /// 供应商订单 冻结积分
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public async Task<bool> ProvideOrderPoint(int userid, [NotNull]Orders order)
        {

            var provideExtra = ProvideExtraRepository.Query(u => u.UserId == userid)
                .FirstOrDefault();
            if (provideExtra == null)
            {
                // 未分配ProvideExtra实体
                Logger.Log(LogLevel.Error, "未分配ProvideExtra实体'...{0}", userid);
                return false;
            }
            // 默认没有上级的费率
            var defaultPer = ChannelContract.Percentages
                .Where(p => p.ChannelId == order.ChannelId).Where(p => p.UserId == provideExtra.UserId)
                .FirstOrDefault(p => p.Name == "供应商费率");

            if (defaultPer == null)
            {
                Logger.Log(LogLevel.Error, "未找到'供应商费率'...{0}", order.Id);
                return false;
            }
            var providential = order.CreatedAmount * (1 - Convert.ToDecimal(defaultPer.Value) / 1000);
            if (provideExtra.PUserId != null)
            {
                // 有上级
                var ppercen = ChannelContract.Percentages
                    .Where(p => p.ChannelId == order.ChannelId).Where(p => p.UserId == provideExtra.UserId).Where(p => p.Name == "供应商反点").Select(p => p.Value)
                    .FirstOrDefault();
                // 重新计算 代扣积分
                providential =  order.CreatedAmount * (1 - (Convert.ToDecimal(defaultPer.Value) - Convert.ToDecimal(ppercen)) / 1000);
            }
            // 冻结对应积分  
            await ProvideFreezePointChange(userid, (long)providential);
            return true;
        }

        // 订单结算... 
        public async Task<bool> ProvideOrderSettle(int userid, [NotNull]Orders order)
        {
            var provideExtra = ProvideExtraRepository.Query(u => u.UserId == userid)
                .FirstOrDefault();
            if (provideExtra == null)
            {
                // 未分配ProvideExtra实体
                Logger.Log(LogLevel.Error, "未分配ProvideExtra实体'...{0}", userid);
                return false;
            }
            var defaultPer = ChannelContract.Percentages
                .Where(p => p.ChannelId == order.ChannelId).Where(p => p.UserId == provideExtra.UserId).Where(p => p.Name == "供应商费率").Select(p => p.Value)
                .FirstOrDefault();

            var providential = order.CreatedAmount * (1 - Convert.ToDecimal(defaultPer) / 1000); // 计算出默认处理的积分

            if (provideExtra.PUserId != null)
            {

                var perVal = ChannelContract.Percentages
                    .Where(p => p.ChannelId == order.ChannelId).Where(p => p.UserId == provideExtra.UserId).Where(p => p.Name == "供应商反点").Select(p => p.Value)
                    .FirstOrDefault();

                // 计算出有上级用户扣的分
                var kou = order.CreatedAmount * (1 - (Convert.ToDecimal(defaultPer) - Convert.ToDecimal(perVal)) / 1000);

                // 有上级用户扣分 - 无上级扣分
                var point = kou - providential;
                // 结算 上级用户增加积分
                await ProvidePointChange(provideExtra.PUserId.Value, (long)point, order.Id.ToString(), "订单结算1",
                    $"下级:{provideExtra.UserId},订单结算");

                providential = kou;

            }
            // 把冻结积分 和 余额积分 一起扣除
            await ProvidePointChange(userid, -(long)providential, order.Id.ToString(), "订单结算",
                $"订单结算");
            await ProvideFreezePointChange(userid, -(long)providential);
            return true;
        }




        /// <summary>
        /// 实时更改供应商积分
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="point"></param>
        /// <param name="payid"></param>
        /// <param name="paytype"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public async Task<long> ProvidePointChange(int userid, long point, string payid, string paytype, string msg)
        {
            // 更改供应商积分记录
            var log = new PointsLog
            {
                PayId = payid,
                PayType = paytype,
                Point = point,
                UserId = userid,
                Remarks = msg,
            };
            var haCount = PointsLogRepository.Query(p => p.PayType == log.PayType).Where(p => p.PayId == log.PayId)
                .Any(p => p.UserId == userid);
            if (haCount)
            {
                Logger.Log(LogLevel.Error, "供应商积分更新失败(确保payid,userid,唯一)...,{0},{1},{2},{3}", userid, point, payid, msg);
                throw new Exception("积分更新失败");
            }
            

            var newPoint = await Database.HashIncrementAsync(TypeName, $"{userid}:Point", point);
            var oldPoint = newPoint - point;
            log.BeforePoint = oldPoint;
            log.AfterPoint = newPoint;
            if (await PointsLogRepository.InsertAsync(log) < 1)
            {
                await Database.HashIncrementAsync(TypeName, $"{userid}:Point", -point);
                Logger.Log(LogLevel.Error, "供应商积分变动日志插入失败...,{0},{1},{2},{3}", userid, point, payid, msg);
                throw new Exception("供应商积分变动日志插入失败");
            }
            PointsLogRepository.UnitOfWork.Commit();
            return newPoint;
        }

        /// <summary>
        /// 获取供应商实时积分
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public async Task<long> ProvideGetPoint(int userid)
        {
            return (long)await Database.HashGetAsync(TypeName, $"{userid}:Point");
        }

        /// <summary>
        /// 实时更改供应商冻结积分
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public async Task<long> ProvideFreezePointChange(int userid, long point)
        {
            if (point == 0)
            {
                throw new Exception("point 不能为0");
            }
            return await Database.HashIncrementAsync(TypeName, $"{userid}:FreezePoint", point);

        }

        /// <summary>
        /// 实时获取供应商 冻结积分
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public async Task<long> ProvideGetFreezePoint(int userid)
        {
            return (long)await Database.HashGetAsync(TypeName, $"{userid}:FreezePoint");
        }

        /// <summary>
        /// 获取供应商 可用积分
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public async Task<long> ProvideGetAvailable(int userid)
        {
            // 必须原子计算 ...
            var lua = LuaScript.Prepare("return redis.call('hget',@TypeName,@Point) - redis.call('hget',@TypeName,@FreezePoint)");
            var r = (long)await Database.ScriptEvaluateAsync(lua, new { TypeName = (RedisKey)TypeName, Point = $"{userid}:Point", FreezePoint = $"{userid}:FreezePoint" });
            return r;
        }

        /// <summary>
        /// 实时获取供应商累计积分
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public async Task<long> ProvideGetAccumulative(int userid)
        {
            return (long)await Database.HashGetAsync(TypeName, $"{userid}:Accumulative");

        }

        /// <summary>
        /// 实时增加供应商累计积分 累计积分只加不减
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public async Task<long> ProvideAccumulativeChange(int userid, long point)
        {
            if (point == 0)
            {
                throw new Exception("point 不能为0");
            }
            return await Database.HashIncrementAsync(TypeName, $"{userid}:Accumulative", point);

        }

        /// <summary>
        /// 从Redis 同步 积分数据到数据库
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public async Task<bool> ProvideRedisToDate(int userid)
        {
            var pon = PointsRepository.Query(p => p.UserId == userid).FirstOrDefault();
            if (pon == null)
            {
                return false;
            }
            pon.Accumulative = await ProvideGetAccumulative(userid);
            pon.FreezePoint = await ProvideGetFreezePoint(userid);
            pon.Point = await ProvideGetPoint(userid);
            if (await PointsRepository.UpdateAsync(pon) < 1)
            {
                return false;
            }
            PointsRepository.UnitOfWork.Commit();
            return true;

        }
    }
}
