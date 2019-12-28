// -----------------------------------------------------------------------
// <auto-generated>
//    此代码由代码生成器生成。
//    手动更改此文件可能导致应用程序出现意外的行为。
//    如果重新生成代码，对此文件的任何修改都会丢失。
//    如果需要扩展此类，可以遵守如下规则进行扩展：
//      1.横向扩展：如需给当前控制器添加API，请在控制器类型 PointsLogController.cs 进行添加
//      2.纵向扩展：如需要重写当前控制器的API实现，请在控制器类型 PointsLogController.cs 进行继承重写
// </auto-generated>
//
//  <copyright file="PointsLogBase.generated.cs">
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

using KaPai.Pay.Provide;
using KaPai.Pay.Provide.Dtos;
using KaPai.Pay.Provide.Entities;


namespace KaPai.Pay.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// 管理控制器基类: 积分变动记录信息
    /// </summary>
    [ModuleInfo(Position = "Provide", PositionName = "供应商模块")]
    [Description("管理-积分变动记录信息")]
    public abstract class PointsLogControllerBase : AdminApiController
    {
        /// <summary>
        /// 初始化一个<see cref="PointsLogController"/>类型的新实例
        /// </summary>
        protected PointsLogControllerBase(IProvideContract provideContract,
            IFilterService filterService)
        {
            ProvideContract = provideContract;
            FilterService = filterService;
        }

        /// <summary>
        /// 获取或设置 数据过滤服务对象
        /// </summary>
        protected IFilterService FilterService { get; }

        /// <summary>
        /// 获取或设置 供应商模块业务契约对象
        /// </summary>
        protected IProvideContract ProvideContract { get; }
        
        /// <summary>
        /// 读取积分变动记录列表信息
        /// </summary>
        /// <param name="request">页请求信息</param>
        /// <returns>积分变动记录列表分页信息</returns>
        [HttpPost]
        [ModuleInfo]
        [Description("读取")]
        public virtual PageData<PointsLogOutputDto> Read(PageRequest request)
        {
            Check.NotNull(request, nameof(request));

            Expression<Func<PointsLog, bool>> predicate = FilterService.GetExpression<PointsLog>(request.FilterGroup);
            var page = ProvideContract.PointsLogs.ToPage<PointsLog, PointsLogOutputDto>(predicate, request.PageCondition);

            return page.ToPageData();
        }
    }
}
