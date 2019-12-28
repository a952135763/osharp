// -----------------------------------------------------------------------
// <once-generated>
//     这个文件只生成一次，再次生成不会被覆盖。
//     可以在此类进行继承重写来扩展基类 OrdersControllerBase
// </once-generated>
//
//  <copyright file="Orders.cs">
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
using OSharp.Filter;

using KaPai.Pay.Merchant;
using KaPai.Pay.Merchant.Dtos;
using KaPai.Pay.Merchant.Entities;
using Microsoft.AspNetCore.Mvc;
using OSharp.Core.Modules;
using OSharp.Data;
using OSharp.Entity;
using KaPai.Pay.My;
using Microsoft.EntityFrameworkCore;
using OSharp.AspNetCore.UI;

namespace KaPai.Pay.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// 管理控制器: 商户订单列表信息
    /// </summary>
    public class OrdersController : OrdersControllerBase
    {
        protected IRepository<Orders, Guid> OrderRepository;
        /// <summary>
        /// 初始化一个<see cref="OrdersController"/>类型的新实例
        /// </summary>
        public OrdersController(IMerchantContract merchantContract,
            IFilterService filterService, IRepository<Orders, Guid> orderRepository)
            : base(merchantContract, filterService)
        {
            OrderRepository = orderRepository;
        }


        /// <summary>
        /// 全量读取
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public override PageData<OrdersOutputDto> Read(PageRequest request)
        {
            Check.NotNull(request, nameof(request));

            // 默认启用最近 排序
            request.AddDefaultSortCondition( new SortCondition("CreatedTime",ListSortDirection.Descending));

            Expression<Func<Orders, bool>> predicate = FilterService.GetDataFilterExpression<Orders>(request.FilterGroup);
            
            var page = MerchantContract.Orderses.Include(e=>e.Channel).Include(e=>e.BackLog)
                .ToPage<Orders, OrdersOutputDto>(predicate, request.PageCondition);

            return page.ToPageData();
        }


        /// <summary>
        /// 读取更多信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleInfo]
        [DependOnFunction("Read")]
        [Description("商户读取")]
        public async Task<AjaxResult> ReadMore()
        {
            OperationResult result = new OperationResult(OperationResultType.Error, "未实现");
            return result.ToAjaxResult();
        }
    }
}
