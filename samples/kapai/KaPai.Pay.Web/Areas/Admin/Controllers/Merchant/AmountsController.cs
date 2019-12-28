// -----------------------------------------------------------------------
// <once-generated>
//     这个文件只生成一次，再次生成不会被覆盖。
//     可以在此类进行继承重写来扩展基类 AmountsControllerBase
// </once-generated>
//
//  <copyright file="Amounts.cs">
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
    /// 管理控制器: 商户实时余额信息
    /// </summary>
    public class AmountsController : AmountsControllerBase
    {
        /// <summary>
        /// 初始化一个<see cref="AmountsController"/>类型的新实例
        /// </summary>
        public AmountsController(IMerchantContract merchantContract,
            IFilterService filterService)
            : base(merchantContract, filterService)
        { }
    }
}
