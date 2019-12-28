// -----------------------------------------------------------------------
// <once-generated>
//     这个文件只生成一次，再次生成不会被覆盖。
//     可以在此类进行继承重写来扩展基类 ProvideServiceBase
// </once-generated>
//
//  <copyright file="IProvideService.cs">
//      KaPai©2019 Microsoft Corporation. All rights reserved. 
//  </copyright>
//  <site></site>
//  <last-editor>KaPai</last-editor>
// -----------------------------------------------------------------------

using System;


namespace KaPai.Pay.Provide
{
    /// <summary>
    /// 业务实现基类：供应商模块
    /// </summary>
    public partial class ProvideService : ProvideServiceBase
    {
        /// <summary>
        /// 初始化一个<see cref="ProvideService"/>类型的新实例
        /// </summary>
        public ProvideService(IServiceProvider provider)
            : base(provider)
        { }

    }
}
