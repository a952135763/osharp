using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Hangfire;
using KaPai.Pay.Hangfire;

namespace KaPai.Pay.Merchant
{
    public partial interface IMerchantContract
    {
        


        /// <summary>
        /// 把订单从 待处理 或 处理中 更新到 超时
        /// </summary>
        /// <param name="orderid">订单ID</param>
        /// <returns>操作结果</returns>
        Task<bool> OrderTimeOut(string orderid);


        /// <summary>
        /// 把订单 从 处理中 更新到 已付款,并启动回调,结算任务
        /// </summary>
        /// <param name="orderid">订单ID</param>
        /// <param name="payamount">付款金额单位分</param>
        /// <param name="settletask">是否启动结算任务</param>
        /// <param name="callback">是否启动回调任务</param>
        /// <returns>操作结果</returns>
        Task<bool> OrderPayment(string orderid, long payamount, bool settletask = true,bool callback = true);


        /// <summary>
        /// 订单结算 流程处理
        /// </summary>
        /// <param name="userid">结算的用户id</param>
        /// <param name="orderid">订单ID</param>
        /// <returns></returns>
        Task<bool> OrderSettle(int userid,string orderid);


        /// <summary>
        /// 订单回调
        /// </summary>
        /// <param name="orderid">回调订单ID</param>
        /// <returns></returns>
        [Semaphore("OrderCallback")]
        Task<bool> OrderCallback(string orderid);



        /// <summary>
        /// 初始化商户
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        Task<bool> MerchantInitialization(int userid, int? puserid = null, string extrastr = null);


        /// <summary>
        /// 更新商户上级用户
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="puserid">设置的上级ID</param>
        /// <param name="commit">是否自动提交事务</param>
        /// <returns>成功或失败</returns>
        Task<bool> MerchantUpdatePUserId(Guid id, int? puserid, bool commit = false);



        /// <summary>
        /// 刷新秘钥
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<string> MerchantUpdateKey(Guid id, bool commit = false);


        #region 商户 Amounts 表数据 Redis 化操作
        /// <summary>
        /// 在合适的时候 投递此任务把 余额实时数据 同步到数据库
        /// </summary>
        /// <param name="userid">商户ID</param>
        /// <returns>成功或失败</returns>
        [Semaphore("MerchantRedisToDate{0}")]
        [DisplayName("商户{0},同步到数据库")]
        Task<bool> MerchantRedisToDate(int userid);

        /// <summary>
        /// 商户积分实时修改 并记录日志
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="amount">要操作的积分</param>
        /// <returns>返回更新后的余额积分</returns>
        Task<long> MerchantAmountChange(int userid, long amount, string payid, string paytype, string msg);

        /// <summary>
        /// 获取商户实时余额
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        Task<long> MerchantGetAmount(int userid);

        /// <summary>
        /// 实时修改冻结积分
        /// </summary>
        /// <returns></returns>
        Task<long> MerchantFreezeAmountChange(int userid, long amount);

        /// <summary>
        /// 获取 冻结积分
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        Task<long> MerchantGetFreezeAmount(int userid);

        /// <summary>
        /// 获取商户总积分
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        Task<long> MerchantGetAccumulative(int userid);

        /// <summary>
        /// 实时修改商户总积分
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        Task<long> MerchantAccumulativeChang(int userid, long amount);


        Task<long> MerchantGetAvailable(int userid);


        #endregion





    }

}
