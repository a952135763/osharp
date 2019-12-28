// -----------------------------------------------------------------------
// <once-generated>
//     这个文件只生成一次，再次生成不会被覆盖。
//     可以在此类进行继承重写来扩展基类 UserChannelControllerBase
// </once-generated>
//
//  <copyright file="UserChannel.cs">
//      KaPai©2019 Microsoft Corporation. All rights reserved. 
//  </copyright>
//  <site></site>
//  <last-editor>KaPai</last-editor>
// -----------------------------------------------------------------------

using System;

using OSharp.Filter;

using KaPai.Pay.Channel;


namespace KaPai.Pay.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// 管理控制器: 用户开启通道信息
    /// </summary>
    public class UserChannelController : UserChannelControllerBase
    {
        /// <summary>
        /// 初始化一个<see cref="UserChannelController"/>类型的新实例
        /// </summary>
        public UserChannelController(IChannelContract channelContract,
            IFilterService filterService)
            : base(channelContract, filterService)
        { }
    }
}
