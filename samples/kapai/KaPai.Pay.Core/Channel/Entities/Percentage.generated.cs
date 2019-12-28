//------------------------------------------------------------------------------
// <auto-generated>
//    此代码由代码生成器生成。
//    手动更改此文件可能导致应用程序出现意外的行为。
//    如果重新生成代码，对此文件的任何修改都会丢失。
//    如果需要扩展此类：可遵守如下规则进行扩展：
//      1.横向扩展：如需添加额外的属性，可新建文件“Percentage.cs”的分部类“public partial class Percentage”}添加属性
// </auto-generated>
//
//  <copyright file="Percentage.generated.cs">
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

using KaPai.Pay.Identity.Entities;

namespace KaPai.Pay.Channel.Entities
{
    /// <summary>
    /// 实体类：费率列表信息
    /// </summary>
    [Description("费率列表信息")]
    public partial class Percentage : EntityBase<Guid>, ISoftDeletable, ICreationAudited<int>, IUpdateAudited<int>
    {
        /// <summary>
        /// 获取或设置 费率名称
        /// </summary>
        [DisplayName("费率名称"), Required, StringLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 费率值
        /// </summary>
        [DisplayName("费率值")]
        public long Value { get; set; }

        /// <summary>
        /// 获取或设置 删除时间
        /// </summary>
        [DisplayName("删除时间")]
        public DateTime? DeletedTime { get; set; }

        /// <summary>
        /// 获取或设置 创建者
        /// </summary>
        [DisplayName("创建者")]
        public int? CreatorId { get; set; }

        /// <summary>
        /// 获取或设置 创建时间
        /// </summary>
        [DisplayName("创建时间")]
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// 获取或设置 更新者
        /// </summary>
        [DisplayName("更新者")]
        public int? LastUpdaterId { get; set; }

        /// <summary>
        /// 获取或设置 更新时间
        /// </summary>
        [DisplayName("更新时间")]
        public DateTime? LastUpdatedTime { get; set; }

        /// <summary>
        /// 获取或设置 用户主键ID
        /// </summary>
        [DisplayName("用户主键ID"), UserFlag]
        public int UserId { get; set; }

        /// <summary>
        /// 获取或设置 通道主键
        /// </summary>
        [DisplayName("通道主键")]
        public Guid ChannelId { get; set; }

        /// <summary>
        /// 获取或设置 用户
        /// </summary>
        [DisplayName("用户")]
        public virtual User User { get; set; }

        /// <summary>
        /// 获取或设置 通道
        /// </summary>
        [DisplayName("通道")]
        public virtual Channels Channel { get; set; }

    }
}

