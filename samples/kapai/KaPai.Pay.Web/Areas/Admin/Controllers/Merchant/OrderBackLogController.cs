// -----------------------------------------------------------------------
// <once-generated>
//     这个文件只生成一次，再次生成不会被覆盖。
//     可以在此类进行继承重写来扩展基类 OrderBackLogControllerBase
// </once-generated>
//
//  <copyright file="OrderBackLog.cs">
//      KaPai©2019 Microsoft Corporation. All rights reserved. 
//  </copyright>
//  <site></site>
//  <last-editor>KaPai</last-editor>
// -----------------------------------------------------------------------

using System;

using OSharp.Filter;

using KaPai.Pay.Merchant;


namespace KaPai.Pay.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// 管理控制器: 订单回调记录信息
    /// </summary>
    public class OrderBackLogController : OrderBackLogControllerBase
    {
        /// <summary>
        /// 初始化一个<see cref="OrderBackLogController"/>类型的新实例
        /// </summary>
        public OrderBackLogController(IMerchantContract merchantContract,
            IFilterService filterService)
            : base(merchantContract, filterService)
        { }
    }
}
