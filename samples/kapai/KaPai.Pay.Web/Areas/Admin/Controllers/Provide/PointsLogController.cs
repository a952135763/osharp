// -----------------------------------------------------------------------
// <once-generated>
//     这个文件只生成一次，再次生成不会被覆盖。
//     可以在此类进行继承重写来扩展基类 PointsLogControllerBase
// </once-generated>
//
//  <copyright file="PointsLog.cs">
//      KaPai©2019 Microsoft Corporation. All rights reserved. 
//  </copyright>
//  <site></site>
//  <last-editor>KaPai</last-editor>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using OSharp.Filter;

using KaPai.Pay.Provide;
using KaPai.Pay.Provide.Dtos;


namespace KaPai.Pay.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// 管理控制器: 积分变动记录信息
    /// </summary>
    public class PointsLogController : PointsLogControllerBase
    {
        /// <summary>
        /// 初始化一个<see cref="PointsLogController"/>类型的新实例
        /// </summary>
        public PointsLogController(IProvideContract provideContract,
            IFilterService filterService)
            : base(provideContract, filterService)
        { }

        public override PageData<PointsLogOutputDto> Read(PageRequest request)
        {
            request.AddDefaultSortCondition(new SortCondition("CreatedTime", ListSortDirection.Descending));
            return base.Read(request);
        }
    }
}
