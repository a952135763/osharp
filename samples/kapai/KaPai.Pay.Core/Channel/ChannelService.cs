// -----------------------------------------------------------------------
// <once-generated>
//     这个文件只生成一次，再次生成不会被覆盖。
//     可以在此类进行继承重写来扩展基类 ChannelServiceBase
// </once-generated>
//
//  <copyright file="IChannelService.cs">
//      KaPai©2019 Microsoft Corporation. All rights reserved. 
//  </copyright>
//  <site></site>
//  <last-editor>KaPai</last-editor>
// -----------------------------------------------------------------------

using System;


namespace KaPai.Pay.Channel
{
    /// <summary>
    /// 业务实现基类：通道模块
    /// </summary>
    public partial class ChannelService : ChannelServiceBase
    {
        /// <summary>
        /// 初始化一个<see cref="ChannelService"/>类型的新实例
        /// </summary>
        public ChannelService(IServiceProvider provider)
            : base(provider)
        { }
    }
}
