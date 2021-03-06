// -----------------------------------------------------------------------
// <auto-generated>
//    此代码由代码生成器生成。
//    手动更改此文件可能导致应用程序出现意外的行为。
//    如果重新生成代码，对此文件的任何修改都会丢失。
//    如果需要扩展此类：可遵守如下规则进行扩展：
//      1.横向扩展：如需添加额外的属性，可新建文件“ChannelsInputDto.cs”的分部类“public partial class ChannelsInputDto”}添加属性
// </auto-generated>
//
//  <copyright file="ChannelsInputDto.generated.cs">
//      KaPai©2019 Microsoft Corporation. All rights reserved. 
//  </copyright>
//  <site></site>
//  <last-editor>KaPai</last-editor>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using OSharp.Entity;
using OSharp.Mapping;

using KaPai.Pay.Channel.Entities;


namespace KaPai.Pay.Channel.Dtos
{
    /// <summary>
    /// 输入DTO：通道列表信息
    /// </summary>
    [MapTo(typeof(Channels))]
    [Description("通道列表信息")]
    public partial class ChannelsInputDto : IInputDto<Guid>
    {
        /// <summary>
        /// 获取或设置 编号
        /// </summary>
        [DisplayName("编号")]
        public Guid Id { get; set; }

        /// <summary>
        /// 获取或设置 通道代码
        /// </summary>
        [DisplayName("通道代码"), Required, StringLength(50)]
        public string Code { get; set; }

        /// <summary>
        /// 获取或设置 通道名称
        /// </summary>
        [DisplayName("通道名称"), Required, StringLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 状态
        /// </summary>
        [DisplayName("状态")]
        public int Status { get; set; }

        /// <summary>
        /// 获取或设置 最小限制
        /// </summary>
        [DisplayName("最小限制")]
        public int Minimum { get; set; }

        /// <summary>
        /// 获取或设置 最大限制
        /// </summary>
        [DisplayName("最大限制")]
        public int Maxmum { get; set; }

    }
}
