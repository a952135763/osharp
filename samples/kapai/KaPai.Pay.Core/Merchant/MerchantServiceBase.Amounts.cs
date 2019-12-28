using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaPai.Pay.Merchant.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace KaPai.Pay.Merchant
{
    public abstract partial class MerchantServiceBase
    {
        protected readonly string TypeName = "Merchant";

        // 获取实时库
        protected IDatabase Database => ServiceProvider.GetService<IDatabase>();

        /// <summary>
        /// 把商户实时信息 同步到数据库
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public async Task<bool> MerchantRedisToDate(int userid)
        {
            var amo = await AmountsRepository.Query(a => a.UserId == userid).FirstOrDefaultAsync();
            if (amo == null)
            {
                return false;
            }
            amo.Amount = await MerchantGetAmount(userid);
            amo.FreezeAmount = await MerchantGetFreezeAmount(userid);
            amo.Accumulative = await MerchantGetAccumulative(userid);
            var count = await AmountsRepository.UpdateAsync(amo);
            if (count < 1)
            {
                return false;
            }
            AmountsRepository.UnitOfWork.Commit();
            return true;
        }


        /// <summary>
        /// 商户积分 实时修改! 
        /// </summary>
        /// <returns>修改后的余额积分</returns>
        public async Task<long> MerchantAmountChange(int userid, long amount, string payid, string paytype, string msg)
        {

            if (userid < 1 || amount == 0 || string.IsNullOrEmpty(payid) || string.IsNullOrEmpty(paytype))
            {
                throw new Exception("参数验证错误");
            }
            var log = new AmountsLog
            {
                Amounts = amount,
                PayType = paytype,
                PayID = payid,
                Remarks = msg,
                UserId = userid
            };
            var haCount = AmountsLogRepository.Query(l => l.PayID == payid)
                .Where(l => l.PayType == log.PayType).Any(l => l.UserId == userid);
            if (haCount)
            {
                Logger.Log(LogLevel.Error, "商户积分更新失败(确保payid,userid,唯一)...,{0},{1},{2},{3}", userid, amount, payid, msg);
                throw new Exception("商户积分更新失败");
            }

          

            var newFreezeAmount = await Database.HashIncrementAsync(TypeName, $"{userid}:Amount", amount);
            var oldAmount = newFreezeAmount - amount;
            log.AfterAmounts = newFreezeAmount;
            log.BeforeAmounts = oldAmount;
            log.CreatedTime = DateTime.Now;
            var count = await AmountsLogRepository.InsertAsync(log);
            if (count < 1)
            {
                Logger.Log(LogLevel.Error, "商户积分记录增加失败...,{0},{1},{2},{3}", userid, amount, payid, msg);

                await Database.HashIncrementAsync(TypeName, $"{userid}:Amount", -amount);
                throw new Exception("商户积分记录增加失败");
            }
            return newFreezeAmount;
        }


        /// <summary>
        /// 获取用户 实时积分
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public async Task<long> MerchantGetAmount(int userid)
        {

           return (long)await Database.HashGetAsync(TypeName, $"{userid}:Amount");

        }


        /// <summary>
        /// 增加或减少冻结积分 没有持久化到数据库
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public async Task<long> MerchantFreezeAmountChange(int userid, long amount)
        {

            if (amount == 0)
            {
                throw new Exception("amount 不能为 0");
            }
            // 等待原子操作
            var newFreezeAmount = await Database.HashIncrementAsync(TypeName, $"{userid}:FreezeAmount", amount);
            return newFreezeAmount;
        }

        /// <summary>
        /// 读取已冻结金额
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public async Task<long> MerchantGetFreezeAmount(int userid)
        {
            return (long)await Database.HashGetAsync(TypeName, $"{userid}:FreezeAmount");
        }


        public async Task<long> MerchantAccumulativeChang(int userid, long amount)
        {
            if (amount == 0)
            {
                throw new Exception("amount 不能为 0");
            }
            
            var newFreezeAmount = await Database.HashIncrementAsync(TypeName, $"{userid}:Accumulative", amount);
            return newFreezeAmount;
        }

        public async Task<long> MerchantGetAvailable(int userid)
        {
            // 必须原子计算
            var lua = LuaScript.Prepare("return redis.call('hget',@TypeName,@Amount) - redis.call('hget',@TypeName,@FreezeAmount)");
            var r = (long) await Database.ScriptEvaluateAsync(lua, new { TypeName = (RedisKey)TypeName, Amount = $"{userid}:Amount", FreezeAmount = $"{userid}:FreezeAmount" });
            return r;
        }

        public async Task<long> MerchantGetAccumulative(int userid)
        {

            return (long)await Database.HashGetAsync(TypeName, $"{userid}:Accumulative");
        }
    }
}
