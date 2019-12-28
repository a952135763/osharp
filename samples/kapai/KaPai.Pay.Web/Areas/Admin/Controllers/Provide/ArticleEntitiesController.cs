// -----------------------------------------------------------------------
// <once-generated>
//     这个文件只生成一次，再次生成不会被覆盖。
//     可以在此类进行继承重写来扩展基类 ArticleEntitiesControllerBase
// </once-generated>
//
//  <copyright file="ArticleEntities.cs">
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
using KaPai.Pay.Identity.Entities;
using OSharp.Filter;

using KaPai.Pay.Provide;
using KaPai.Pay.Provide.Dtos;
using KaPai.Pay.Provide.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OSharp.AspNetCore.UI;
using OSharp.Core.Modules;
using OSharp.Data;
using OSharp.Entity;


namespace KaPai.Pay.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// 管理控制器: 收款号列表信息
    /// </summary>
    public class ArticleEntitiesController : ArticleEntitiesControllerBase
    {
        protected readonly UserManager<User> UserManager;


        /// <summary>
        /// 初始化一个<see cref="ArticleEntitiesController"/>类型的新实例
        /// </summary>
        public ArticleEntitiesController(IProvideContract provideContract,
            IFilterService filterService, UserManager<User> userManager)
            : base(provideContract, filterService)
        {
            UserManager = userManager;
        }


        public override PageData<ArticleEntitiesOutputDto> Read(PageRequest request)
        {

            Check.NotNull(request, nameof(request));

            request.AddDefaultSortCondition(new SortCondition("CreatedTime", ListSortDirection.Descending));

            Expression<Func<ArticleEntities, bool>> predicate = FilterService.GetExpression<ArticleEntities>(request.FilterGroup);
            var page = ProvideContract.ArticleEntitieses.Include(e=>e.ArticleAssort).ToPage<ArticleEntities, ArticleEntitiesOutputDto>(predicate, request.PageCondition);

            return page.ToPageData();
        }

        public override async Task<AjaxResult> Update(ArticleEntitiesInputDto[] dtos)
        {
            Check.NotNull(dtos, nameof(dtos));
            OperationResult result = await ProvideContract.UpdateArticleEntitieses(dtos);
            return result.ToAjaxResult();
        }



        public override async Task<AjaxResult> Create(ArticleEntitiesInputDto[] dtos)
        {
            Check.NotNull(dtos, nameof(dtos));
            // 是供应商增加 把所有实体 所属ID改为供应商ID
            if (User.IsInRole("供应商"))
            {
                var userid = Convert.ToInt32(UserManager.GetUserId(User));
                dtos = dtos.Select(d =>
                {
                    d.UserId = userid;
                    return d;
                }).ToArray();
            }
            OperationResult result = await ProvideContract.CreateArticleEntitieses(dtos);
            return result.ToAjaxResult();
        }

        /// <summary>
        ///  获取统计信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleInfo]
        [DependOnFunction("Read")]
        [Description("统计信息")]
        public async Task<AjaxResult> ReadStatistics(ArticleEntitiesInputDto dtos)
        {


            OperationResult result = new OperationResult(OperationResultType.Error,"未实现");
            return result.ToAjaxResult();
        }

        /// <summary>
        /// 读取用户ID
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleInfo]
        [DependOnFunction("Read")]
        [Description("管理角色读取更多信息")]
        public async Task<AjaxResult> ReadMore()
        {
            OperationResult result = new OperationResult(OperationResultType.Error, "未实现");
            return result.ToAjaxResult();
        }
    }
}
