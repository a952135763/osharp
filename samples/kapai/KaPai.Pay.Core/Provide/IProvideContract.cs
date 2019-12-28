using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading.Tasks;
using KaPai.Pay.Hangfire;
using KaPai.Pay.Merchant.Entities;

namespace KaPai.Pay.Provide
{

    public partial interface IProvideContract
    {

        /// <summary>
        /// 供应商处理分配订单 冻结订单金额
        /// </summary>
        /// <param name="userid">供应商ID</param>
        /// <param name="order">订单实体</param>
        /// <returns></returns>
        Task<bool> ProvideOrderPoint(int userid, [NotNull]Orders order);


        /// <summary>
        /// 供应商处理结算订单 扣除冻结积分，余额积分
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        Task<bool> ProvideOrderSettle(int userid, [NotNull]Orders order);


        /// <summary>
        /// 初始化供应商
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        Task<bool> ProvideInitialization(int userid, int? puserid = null, string extrastr = null);

        #region Points 表 Redis 化操作

        /// <summary>
        /// 实时更改 供应商积分
        /// </summary>
        /// <param name="userid">供应商ID</param>
        /// <param name="amount">变动积分,负数为扣款</param>
        /// <param name="payid">变动ID,</param>
        /// <param name="paytype">变动来源</param>
        /// <param name="msg">变动说明</param>
        /// <returns></returns>
        Task<long> ProvidePointChange(int userid, long amount, string payid, string paytype, string msg);


        /// <summary>
        /// 获取供应商实时积分
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        Task<long> ProvideGetPoint(int userid);

        /// <summary>
        /// 实时修改冻结积分
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        Task<long> ProvideFreezePointChange(int userid, long point);

        /// <summary>
        /// 获取冻结积分
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        Task<long> ProvideGetFreezePoint(int userid);


        /// <summary>
        /// 实时获取用户可用积分
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        Task<long> ProvideGetAvailable(int userid);


        /// <summary>
        /// 获取总积分
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        Task<long> ProvideGetAccumulative(int userid);

        /// <summary>
        /// 实时修改总积分
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        Task<long> ProvideAccumulativeChange(int userid, long point);


        /// <summary>
        /// 把商户实时信息 同步到数据库
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        [Semaphore("ProvideRedisToDate{0}")]
        [DisplayName("供应商{0},同步到数据库")]
        Task<bool> ProvideRedisToDate(int userid);
        #endregion

    }
}
