// -----------------------------------------------------------------------
// <once-generated>
//     这个文件只生成一次，再次生成不会被覆盖。
//     可以在此类进行继承重写来扩展基类 MerchantExtraControllerBase
// </once-generated>
//
//  <copyright file="MerchantExtra.cs">
//      KaPai©2019 Microsoft Corporation. All rights reserved. 
//  </copyright>
//  <site></site>
//  <last-editor>KaPai</last-editor>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using KaPai.Pay.Identity.Entities;
using OSharp.Filter;

using KaPai.Pay.Merchant;
using KaPai.Pay.Merchant.Dtos;
using KaPai.Pay.Merchant.Entities;
using KaPai.Pay.Provide;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OSharp.AspNetCore.Mvc.Filters;
using OSharp.AspNetCore.UI;
using OSharp.Caching;
using OSharp.Core.Modules;
using OSharp.Data;
using OSharp.Entity;


namespace KaPai.Pay.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// 管理控制器: 商户参数信息
    /// </summary>
    public class MerchantExtraController : MerchantExtraControllerBase
    {

        protected readonly IProvideContract ProvideContract;

        /// <summary>
        /// 初始化一个<see cref="MerchantExtraController"/>类型的新实例
        /// </summary>
        public MerchantExtraController(IMerchantContract merchantContract,
            IFilterService filterService, IProvideContract provideContract)
            : base(merchantContract, filterService)
        {
            ProvideContract = provideContract;
        }

        public override PageData<MerchantExtraOutputDto> Read(PageRequest request)
        {

            Check.NotNull(request, nameof(request));

            request.AddDefaultSortCondition(new SortCondition("CreatedTime", ListSortDirection.Descending));

            Expression<Func<MerchantExtra, bool>> predicate = FilterService.GetExpression<MerchantExtra>(request.FilterGroup);
            var page = MerchantContract.MerchantExtras.ToPage<MerchantExtra, MerchantExtraOutputDto>(predicate, request.PageCondition);

            return page.ToPageData();
        }


        public override async Task<AjaxResult> Update(MerchantExtraInputDto[] dtos)
        {
            Check.NotNull(dtos, nameof(dtos));

            OperationResult result = await MerchantContract.UpdateMerchantExtras(dtos);
            return result.ToAjaxResult();
        }

    }
}
