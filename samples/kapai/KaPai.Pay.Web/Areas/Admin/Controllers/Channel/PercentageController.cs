// -----------------------------------------------------------------------
// <once-generated>
//     这个文件只生成一次，再次生成不会被覆盖。
//     可以在此类进行继承重写来扩展基类 PercentageControllerBase
// </once-generated>
//
//  <copyright file="Percentage.cs">
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

using KaPai.Pay.Channel;
using KaPai.Pay.Channel.Dtos;
using KaPai.Pay.Channel.Entities;
using Microsoft.EntityFrameworkCore;
using OSharp.AspNetCore.UI;
using OSharp.Data;
using OSharp.Entity;


namespace KaPai.Pay.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// 管理控制器: 费率列表信息
    /// </summary>
    public class PercentageController : PercentageControllerBase
    {

        protected readonly IServiceProvider serviceProvider;

        protected IRepository<Percentage, Guid> PerchenRepository;
        /// <summary>
        /// 初始化一个<see cref="PercentageController"/>类型的新实例
        /// </summary>
        public PercentageController(IChannelContract channelContract,
            IFilterService filterService, IServiceProvider serviceProvider, IRepository<Percentage, Guid> perchenRepository)
            : base(channelContract, filterService)
        {
            this.serviceProvider = serviceProvider;
            PerchenRepository = perchenRepository;
        }


        public override PageData<PercentageOutputDto> Read(PageRequest request)
        {
            Check.NotNull(request, nameof(request));

            request.AddDefaultSortCondition(new SortCondition("CreatedTime", ListSortDirection.Descending));

            Expression<Func<Percentage, bool>> predicate = FilterService.GetExpression<Percentage>(request.FilterGroup);

            
            var page = ChannelContract.Percentages.Include(e=>e.Channel).ToPage<Percentage, PercentageOutputDto>(predicate, request.PageCondition);

            return page.ToPageData();
        }

        public override async Task<AjaxResult> Update(PercentageInputDto[] dtos)
        {
            Check.NotNull(dtos, nameof(dtos));
            OperationResult result = await ChannelContract.UpdatePercentages(dtos);
            return result.ToAjaxResult();
        }

        public override async Task<AjaxResult> Create(PercentageInputDto[] dtos)
        {
            Check.NotNull(dtos, nameof(dtos));
            OperationResult result = await ChannelContract.CreatePercentages(dtos);
            return result.ToAjaxResult();
        }
    }

     
}
