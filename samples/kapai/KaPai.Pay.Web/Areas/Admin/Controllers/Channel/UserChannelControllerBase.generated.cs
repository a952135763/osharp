// -----------------------------------------------------------------------
// <auto-generated>
//    此代码由代码生成器生成。
//    手动更改此文件可能导致应用程序出现意外的行为。
//    如果重新生成代码，对此文件的任何修改都会丢失。
//    如果需要扩展此类，可以遵守如下规则进行扩展：
//      1.横向扩展：如需给当前控制器添加API，请在控制器类型 UserChannelController.cs 进行添加
//      2.纵向扩展：如需要重写当前控制器的API实现，请在控制器类型 UserChannelController.cs 进行继承重写
// </auto-generated>
//
//  <copyright file="UserChannelBase.generated.cs">
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

using Microsoft.AspNetCore.Mvc;

using OSharp.AspNetCore.Mvc;
using OSharp.AspNetCore.Mvc.Filters;
using OSharp.AspNetCore.UI;
using OSharp.Caching;
using OSharp.Core.Functions;
using OSharp.Core.Modules;
using OSharp.Data;
using OSharp.Entity;
using OSharp.Filter;
using OSharp.Security;

using KaPai.Pay.Channel;
using KaPai.Pay.Channel.Dtos;
using KaPai.Pay.Channel.Entities;


namespace KaPai.Pay.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// 管理控制器基类: 用户开启通道信息
    /// </summary>
    [ModuleInfo(Position = "Channel", PositionName = "通道模块")]
    [Description("管理-用户开启通道信息")]
    public abstract class UserChannelControllerBase : AdminApiController
    {
        /// <summary>
        /// 初始化一个<see cref="UserChannelController"/>类型的新实例
        /// </summary>
        protected UserChannelControllerBase(IChannelContract channelContract,
            IFilterService filterService)
        {
            ChannelContract = channelContract;
            FilterService = filterService;
        }

        /// <summary>
        /// 获取或设置 数据过滤服务对象
        /// </summary>
        protected IFilterService FilterService { get; }

        /// <summary>
        /// 获取或设置 通道模块业务契约对象
        /// </summary>
        protected IChannelContract ChannelContract { get; }
        
        /// <summary>
        /// 读取用户开启通道列表信息
        /// </summary>
        /// <param name="request">页请求信息</param>
        /// <returns>用户开启通道列表分页信息</returns>
        [HttpPost]
        [ModuleInfo]
        [Description("读取")]
        public virtual PageData<UserChannelOutputDto> Read(PageRequest request)
        {
            Check.NotNull(request, nameof(request));

            Expression<Func<UserChannel, bool>> predicate = FilterService.GetExpression<UserChannel>(request.FilterGroup);
            var page = ChannelContract.UserChannels.ToPage<UserChannel, UserChannelOutputDto>(predicate, request.PageCondition);

            return page.ToPageData();
        }
        
        /// <summary>
        /// 新增用户开启通道信息
        /// </summary>
        /// <param name="dtos">用户开启通道信息输入DTO</param>
        /// <returns>JSON操作结果</returns>
        [HttpPost]
        [ModuleInfo]
        [DependOnFunction("Read")]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("新增")]
        public virtual async Task<AjaxResult> Create(UserChannelInputDto[] dtos)
        {
            Check.NotNull(dtos, nameof(dtos));
            OperationResult result = await ChannelContract.CreateUserChannels(dtos);
            return result.ToAjaxResult();
        }
        
        /// <summary>
        /// 更新用户开启通道信息
        /// </summary>
        /// <param name="dtos">用户开启通道信息输入DTO</param>
        /// <returns>JSON操作结果</returns>
        [HttpPost]
        [ModuleInfo]
        [DependOnFunction("Read")]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("更新")]
        public virtual async Task<AjaxResult> Update(UserChannelInputDto[] dtos)
        {
            Check.NotNull(dtos, nameof(dtos));
            OperationResult result = await ChannelContract.UpdateUserChannels(dtos);
            return result.ToAjaxResult();
        }
    }
}
