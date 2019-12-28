// -----------------------------------------------------------------------
// <once-generated>
//     这个文件只生成一次，再次生成不会被覆盖。
//     可以在此类进行继承重写来扩展基类 AmountsLogControllerBase
// </once-generated>
//
//  <copyright file="AmountsLog.cs">
//      KaPai©2019 Microsoft Corporation. All rights reserved. 
//  </copyright>
//  <site></site>
//  <last-editor>KaPai</last-editor>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using OSharp.Filter;

using KaPai.Pay.Merchant;
using KaPai.Pay.Merchant.Dtos;


namespace KaPai.Pay.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// 管理控制器: 余额变动记录信息
    /// </summary>
    public class AmountsLogController : AmountsLogControllerBase
    {
        /// <summary>
        /// 初始化一个<see cref="AmountsLogController"/>类型的新实例
        /// </summary>
        public AmountsLogController(IMerchantContract merchantContract,
            IFilterService filterService)
            : base(merchantContract, filterService)
        { }

        public override PageData<AmountsLogOutputDto> Read(PageRequest request)
        {
            request.AddDefaultSortCondition(new SortCondition("CreatedTime", ListSortDirection.Descending));

            return base.Read(request);
        }
    }
}
