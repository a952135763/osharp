// -----------------------------------------------------------------------
// <once-generated>
//     这个文件只生成一次，再次生成不会被覆盖。
//     可以在此类进行继承重写来扩展基类 MerchantServiceBase
// </once-generated>
//
//  <copyright file="IMerchantService.cs">
//      KaPai©2019 Microsoft Corporation. All rights reserved. 
//  </copyright>
//  <site></site>
//  <last-editor>KaPai</last-editor>
// -----------------------------------------------------------------------

using System;
using System.Threading.Tasks;


namespace KaPai.Pay.Merchant
{
    /// <summary>
    /// 业务实现基类：商户模块
    /// </summary>
    public partial class MerchantService : MerchantServiceBase
    {
        /// <summary>
        /// 初始化一个<see cref="MerchantService"/>类型的新实例
        /// </summary>
        public MerchantService(IServiceProvider provider)
            : base(provider)
        { }




   
    }
}
