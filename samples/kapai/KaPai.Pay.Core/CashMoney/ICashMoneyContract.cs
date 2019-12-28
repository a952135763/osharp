using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OSharp.Data;

namespace KaPai.Pay.CashMoney
{
    public partial interface ICashMoneyContract
    {
        /// <summary>
        /// 创建提现记录
        /// </summary>
        /// <param name="userid">创建用户</param>
        /// <param name="dtos">创建信息</param>
        /// <returns></returns>
        Task<OperationResult> CreateCashLogs(int userid, MerchantCashInputLimit dto, bool commit = true);
    }
}
