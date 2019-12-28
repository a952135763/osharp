// -----------------------------------------------------------------------
// <once-generated>
//     这个文件只生成一次，再次生成不会被覆盖。
//     可以在此类进行继承重写来扩展基类 ArticleAssortControllerBase
// </once-generated>
//
//  <copyright file="ArticleAssort.cs">
//      KaPai©2019 Microsoft Corporation. All rights reserved. 
//  </copyright>
//  <site></site>
//  <last-editor>KaPai</last-editor>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Threading.Tasks;
using OSharp.Filter;

using KaPai.Pay.Provide;
using KaPai.Pay.Provide.Dtos;
using KaPai.Pay.Provide.Entities;
using Microsoft.EntityFrameworkCore;
using OSharp.AspNetCore.UI;
using OSharp.Data;
using OSharp.Entity;


namespace KaPai.Pay.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// 管理控制器: 收款分类列表信息
    /// </summary>
    public class ArticleAssortController : ArticleAssortControllerBase
    {
        /// <summary>
        /// 初始化一个<see cref="ArticleAssortController"/>类型的新实例
        /// </summary>
        public ArticleAssortController(IProvideContract provideContract,
            IFilterService filterService)
            : base(provideContract, filterService)
        { }

        public override PageData<ArticleAssortOutputDto> Read(PageRequest request)
        {
            Check.NotNull(request, nameof(request));

            request.AddDefaultSortCondition(new SortCondition("CreatedTime", ListSortDirection.Descending));

            Expression<Func<ArticleAssort, bool>> predicate = FilterService.GetExpression<ArticleAssort>(request.FilterGroup);
            var page = ProvideContract.ArticleAssorts.Include(a=>a.Channel).ToPage<ArticleAssort, ArticleAssortOutputDto>(predicate, request.PageCondition);

            return page.ToPageData();
        }

        public override async Task<AjaxResult> Create(ArticleAssortInputDto[] dtos)
        {
            Check.NotNull(dtos, nameof(dtos));
            OperationResult result = await ProvideContract.CreateArticleAssorts(dtos);
            return result.ToAjaxResult();
        }
    }
}
