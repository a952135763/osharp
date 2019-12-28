// -----------------------------------------------------------------------
// <once-generated>
//     这个文件只生成一次，再次生成不会被覆盖。
//     可以在此类进行继承重写来扩展基类 ChannelTypeControllerBase
// </once-generated>
//
//  <copyright file="ChannelType.cs">
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
    /// 管理控制器: 通道供应商账号类型信息
    /// </summary>
    public class ChannelTypeController : ChannelTypeControllerBase
    {
        /// <summary>
        /// 初始化一个<see cref="ChannelTypeController"/>类型的新实例
        /// </summary>
        public ChannelTypeController(IChannelContract channelContract,
            IFilterService filterService)
            : base(channelContract, filterService)
        { }

        public override PageData<ChannelTypeOutputDto> Read(PageRequest request)
        {
            Check.NotNull(request, nameof(request));

            request.AddDefaultSortCondition(new SortCondition("CreatedTime", ListSortDirection.Descending));

            
            Expression<Func<ChannelType, bool>> predicate = FilterService.GetExpression<ChannelType>(request.FilterGroup);
            var page = ChannelContract.ChannelTypes.Include(c=>c.Channel).ToPage<ChannelType, ChannelTypeOutputDto>(predicate, request.PageCondition);

            return page.ToPageData();
        }

        public override async Task<AjaxResult> Create(ChannelTypeInputDto[] dtos)
        {

            Check.NotNull(dtos, nameof(dtos));

            OperationResult result = await ChannelContract.CreateChannelTypes(dtos);
            return result.ToAjaxResult();
        }

        public override async Task<AjaxResult> Update(ChannelTypeInputDto[] dtos)
        {
            Check.NotNull(dtos, nameof(dtos));


            OperationResult result = await ChannelContract.UpdateChannelTypes(dtos);
            return result.ToAjaxResult();
        }
    }
}
