// -----------------------------------------------------------------------
// <once-generated>
//     这个文件只生成一次，再次生成不会被覆盖。
//     可以在此类进行继承重写来扩展基类 ChannelsControllerBase
// </once-generated>
//
//  <copyright file="Channels.cs">
//      KaPai©2019 Microsoft Corporation. All rights reserved. 
//  </copyright>
//  <site></site>
//  <last-editor>KaPai</last-editor>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using OSharp.Filter;

using KaPai.Pay.Channel;
using KaPai.Pay.Channel.Dtos;
using KaPai.Pay.Channel.Entities;
using Microsoft.AspNetCore.Mvc;
using OSharp.AspNetCore.UI;
using OSharp.Caching;
using OSharp.Core.Modules;
using OSharp.Data;
using OSharp.Entity;


namespace KaPai.Pay.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// 管理控制器: 通道列表信息
    /// </summary>
    public class ChannelsController : ChannelsControllerBase
    {
        /// <summary>
        /// 初始化一个<see cref="ChannelsController"/>类型的新实例
        /// </summary>
        public ChannelsController(IChannelContract channelContract,
            IFilterService filterService)
            : base(channelContract, filterService)
        { }


        public override PageData<ChannelsOutputDto> Read(PageRequest request)
        {
            Check.NotNull(request, nameof(request));

            request.AddDefaultSortCondition(new SortCondition("CreatedTime", ListSortDirection.Descending));

            Expression<Func<Channels, bool>> predicate = FilterService.GetExpression<Channels>(request.FilterGroup);
            var page = ChannelContract.Channelses.ToPage<Channels, ChannelsOutputDto>(predicate, request.PageCondition);

            return page.ToPageData();
        }

    }
}
