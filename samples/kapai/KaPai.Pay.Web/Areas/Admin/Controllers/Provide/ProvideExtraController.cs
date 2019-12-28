// -----------------------------------------------------------------------
// <once-generated>
//     这个文件只生成一次，再次生成不会被覆盖。
//     可以在此类进行继承重写来扩展基类 ProvideExtraControllerBase
// </once-generated>
//
//  <copyright file="ProvideExtra.cs">
//      KaPai©2019 Microsoft Corporation. All rights reserved. 
//  </copyright>
//  <site></site>
//  <last-editor>KaPai</last-editor>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Linq.Expressions;
using OSharp.Filter;

using KaPai.Pay.Provide;
using KaPai.Pay.Provide.Dtos;
using KaPai.Pay.Provide.Entities;
using OSharp.Data;
using OSharp.Entity;


namespace KaPai.Pay.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// 管理控制器: 供应商参数信息
    /// </summary>
    public class ProvideExtraController : ProvideExtraControllerBase
    {
        /// <summary>
        /// 初始化一个<see cref="ProvideExtraController"/>类型的新实例
        /// </summary>
        public ProvideExtraController(IProvideContract provideContract,
            IFilterService filterService)
            : base(provideContract, filterService)
        { }

        public override PageData<ProvideExtraOutputDto> Read(PageRequest request)
        {
            Check.NotNull(request, nameof(request));

            request.AddDefaultSortCondition(new SortCondition("CreatedTime",ListSortDirection.Descending));

            Expression<Func<ProvideExtra, bool>> predicate = FilterService.GetExpression<ProvideExtra>(request.FilterGroup);
            var page = ProvideContract.ProvideExtras.ToPage<ProvideExtra, ProvideExtraOutputDto>(predicate, request.PageCondition);

            return page.ToPageData();
        }
    }
}
