// -----------------------------------------------------------------------
// <auto-generated>
//    此代码由代码生成器生成。
//    手动更改此文件可能导致应用程序出现意外的行为。
//    如果重新生成代码，对此文件的任何修改都会丢失。
//    如果需要扩展此类，可以遵守如下规则进行扩展：
//      1.横向扩展：如需给当前控制器添加API，请在控制器类型 OrderBackLogController.cs 进行添加
//      2.纵向扩展：如需要重写当前控制器的API实现，请在控制器类型 OrderBackLogController.cs 进行继承重写
// </auto-generated>
//
//  <copyright file="OrderBackLogBase.generated.cs">
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

using KaPai.Pay.Merchant;
using KaPai.Pay.Merchant.Dtos;
using KaPai.Pay.Merchant.Entities;


namespace KaPai.Pay.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// 管理控制器基类: 订单回调记录信息
    /// </summary>
    [ModuleInfo(Position = "Merchant", PositionName = "商户模块")]
    [Description("管理-订单回调记录信息")]
    public abstract class OrderBackLogControllerBase : AdminApiController
    {
        /// <summary>
        /// 初始化一个<see cref="OrderBackLogController"/>类型的新实例
        /// </summary>
        protected OrderBackLogControllerBase(IMerchantContract merchantContract,
            IFilterService filterService)
        {
            MerchantContract = merchantContract;
            FilterService = filterService;
        }

        /// <summary>
        /// 获取或设置 数据过滤服务对象
        /// </summary>
        protected IFilterService FilterService { get; }

        /// <summary>
        /// 获取或设置 商户模块业务契约对象
        /// </summary>
        protected IMerchantContract MerchantContract { get; }
        
        /// <summary>
        /// 读取订单回调记录列表信息
        /// </summary>
        /// <param name="request">页请求信息</param>
        /// <returns>订单回调记录列表分页信息</returns>
        [HttpPost]
        [ModuleInfo]
        [Description("读取")]
        public virtual PageData<OrderBackLogOutputDto> Read(PageRequest request)
        {
            Check.NotNull(request, nameof(request));


            Expression<Func<OrderBackLog, bool>> predicate = FilterService.GetExpression<OrderBackLog>(request.FilterGroup);
            var page = MerchantContract.OrderBackLogs.ToPage<OrderBackLog, OrderBackLogOutputDto>(predicate, request.PageCondition);

            return page.ToPageData();
        }
        
        /// <summary>
        /// 删除订单回调记录信息
        /// </summary>
        /// <param name="ids">订单回调记录信息编号</param>
        /// <returns>JSON操作结果</returns>
        [HttpPost]
        [ModuleInfo]
        [DependOnFunction("Read")]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("删除")]
        public virtual async Task<AjaxResult> Delete(Guid[] ids)
        {
            Check.NotNull(ids, nameof(ids));
            OperationResult result = await MerchantContract.DeleteOrderBackLogs(ids);
            return result.ToAjaxResult();
        }
    }
}
