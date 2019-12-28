// -----------------------------------------------------------------------
// <once-generated>
//     这个文件只生成一次，再次生成不会被覆盖。
//     可以在此类进行继承重写来扩展基类 BankListControllerBase
// </once-generated>
//
//  <copyright file="BankList.cs">
//      KaPai©2019 Microsoft Corporation. All rights reserved. 
//  </copyright>
//  <site></site>
//  <last-editor>KaPai</last-editor>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using OSharp.Filter;

using KaPai.Pay.CashMoney;
using KaPai.Pay.CashMoney.Dtos;
using KaPai.Pay.CashMoney.Entities;
using KaPai.Pay.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using OSharp.AspNetCore.UI;
using OSharp.Data;
using OSharp.Entity;


namespace KaPai.Pay.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// 管理控制器: 收款账号信息
    /// </summary>
    public class BankListController : BankListControllerBase
    {
        protected readonly UserManager<User> UserManager;
        /// <summary>
        /// 初始化一个<see cref="BankListController"/>类型的新实例
        /// </summary>
        public BankListController(ICashMoneyContract cashMoneyContract,
            IFilterService filterService, UserManager<User> userManager)
            : base(cashMoneyContract, filterService)
        {
            UserManager = userManager;
        }


        public override PageData<BankListOutputDto> Read(PageRequest request)
        {

            Check.NotNull(request, nameof(request));
            request.AddDefaultSortCondition(new SortCondition("CreatedTime", ListSortDirection.Descending));

            Expression<Func<BankList, bool>> predicate = FilterService.GetExpression<BankList>(request.FilterGroup);
            var page = CashMoneyContract.BankLists.ToPage<BankList, BankListOutputDto>(predicate, request.PageCondition);

            return page.ToPageData();
        }

        public override async Task<AjaxResult> Create(BankListInputDto[] dtos)
        {
            // 根据权限 是商户就给自己加
            Check.NotNull(dtos, nameof(dtos));

            if (User.IsInRole("商户"))
            {

                int userid = Convert.ToInt32(UserManager.GetUserId(User));
                dtos = dtos.Select(a =>
               {
                   a.UserId = userid;
                   return a;
               }).ToArray();
            }

            OperationResult result = await CashMoneyContract.CreateBankLists(dtos);
            return result.ToAjaxResult();
        }


        public override async Task<AjaxResult> Update(BankListInputDto[] dtos)
        {
            // TODO:更新貌似用自己的 就行了
            Check.NotNull(dtos, nameof(dtos));
            OperationResult result = await CashMoneyContract.UpdateBankLists(dtos);
            return result.ToAjaxResult();

        }

        public override async Task<AjaxResult> Delete(Guid[] ids)
        {
            // 删除貌似没有问题
            Check.NotNull(ids, nameof(ids));
            OperationResult result = await CashMoneyContract.DeleteBankLists(ids);
            return result.ToAjaxResult();
        }
    }
}
