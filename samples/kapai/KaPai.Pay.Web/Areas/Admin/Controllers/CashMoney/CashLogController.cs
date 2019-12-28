// -----------------------------------------------------------------------
// <once-generated>
//     这个文件只生成一次，再次生成不会被覆盖。
//     可以在此类进行继承重写来扩展基类 CashLogControllerBase
// </once-generated>
//
//  <copyright file="CashLog.cs">
//      KaPai©2019 Microsoft Corporation. All rights reserved. 
//  </copyright>
//  <site></site>
//  <last-editor>KaPai</last-editor>
// -----------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using OSharp.Filter;

using KaPai.Pay.CashMoney;
using KaPai.Pay.CashMoney.Dtos;
using KaPai.Pay.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using OSharp.AspNetCore.UI;
using OSharp.Data;


namespace KaPai.Pay.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// 管理控制器: 提现记录信息
    /// </summary>
    public class CashLogController : CashLogControllerBase
    {
        protected readonly UserManager<User> UserManager;
        /// <summary>
        /// 初始化一个<see cref="CashLogController"/>类型的新实例
        /// </summary>
        public CashLogController(ICashMoneyContract cashMoneyContract,
            IFilterService filterService, UserManager<User> userManager)
            : base(cashMoneyContract, filterService)
        {
            this.UserManager = userManager;
        }


        public override async Task<AjaxResult> Create(MerchantCashInputLimit[] dtos)
        {
            // 此方法留给管理使用

            Check.NotNull(dtos, nameof(dtos));
            if (User.IsInRole("商户"))
            {
                int userid = Convert.ToInt32(UserManager.GetUserId(User));
                OperationResult result = await CashMoneyContract.CreateCashLogs(userid, dtos[0]);
                return result.ToAjaxResult();
            }
            return new OperationResult(OperationResultType.Error).ToAjaxResult();
        }

    }
}
