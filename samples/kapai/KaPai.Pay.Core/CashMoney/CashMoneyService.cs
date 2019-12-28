// -----------------------------------------------------------------------
// <once-generated>
//     这个文件只生成一次，再次生成不会被覆盖。
//     可以在此类进行继承重写来扩展基类 CashMoneyServiceBase
// </once-generated>
//
//  <copyright file="ICashMoneyService.cs">
//      KaPai©2019 Microsoft Corporation. All rights reserved. 
//  </copyright>
//  <site></site>
//  <last-editor>KaPai</last-editor>
// -----------------------------------------------------------------------

using System;


namespace KaPai.Pay.CashMoney
{
    /// <summary>
    /// 业务实现基类：提现模块模块
    /// </summary>
    public partial class CashMoneyService : CashMoneyServiceBase
    {
        /// <summary>
        /// 初始化一个<see cref="CashMoneyService"/>类型的新实例
        /// </summary>
        public CashMoneyService(IServiceProvider provider)
            : base(provider)
        { }
    }
}
