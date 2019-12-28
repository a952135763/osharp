// -----------------------------------------------------------------------
// <once-generated>
//     这个文件只生成一次，再次生成不会被覆盖。
//     可以在此类进行继承重写来扩展基类 PointsControllerBase
// </once-generated>
//
//  <copyright file="Points.cs">
//      KaPai©2019 Microsoft Corporation. All rights reserved. 
//  </copyright>
//  <site></site>
//  <last-editor>KaPai</last-editor>
// -----------------------------------------------------------------------

using System;

using OSharp.Filter;

using KaPai.Pay.Provide;


namespace KaPai.Pay.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// 管理控制器: 供应商实时积分信息
    /// </summary>
    public class PointsController : PointsControllerBase
    {
        /// <summary>
        /// 初始化一个<see cref="PointsController"/>类型的新实例
        /// </summary>
        public PointsController(IProvideContract provideContract,
            IFilterService filterService)
            : base(provideContract, filterService)
        { }
    }
}
