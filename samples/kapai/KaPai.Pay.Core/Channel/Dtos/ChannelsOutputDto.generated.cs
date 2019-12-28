// -----------------------------------------------------------------------
// <auto-generated>
//    此代码由代码生成器生成。
//    手动更改此文件可能导致应用程序出现意外的行为。
//    如果重新生成代码，对此文件的任何修改都会丢失。
//    如果需要扩展此类：可遵守如下规则进行扩展：
//      1.横向扩展：如需添加额外的属性，可新建文件“ChannelsOutputDto.cs”的分部类“public partial class ChannelsOutputDto”}添加属性
// </auto-generated>
//
//  <copyright file="ChannelsOutputDto.generated.cs">
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
    [MapFrom(typeof(Channels))]
    [Description("通道列表信息")]
    public partial class ChannelsOutputDto : IOutputDto, IDataAuthEnabled
    {
        /// <summary>
        /// 初始化一个<see cref="ChannelsOutputDto"/>类型的新实例
        /// </summary>
        public ChannelsOutputDto()
        { }

        /// <summary>
        /// 初始化一个<see cref="ChannelsOutputDto"/>类型的新实例
        /// </summary>
        public ChannelsOutputDto(Channels entity)
        {
            Id = entity.Id;
            Code = entity.Code;
            Name = entity.Name;
            Status = entity.Status;
            Minimum = entity.Minimum;
            Maxmum = entity.Maxmum;
            CreatorId = entity.CreatorId;
            CreatedTime = entity.CreatedTime;
            LastUpdaterId = entity.LastUpdaterId;
            LastUpdatedTime = entity.LastUpdatedTime;
        }

        /// <summary>
        /// 获取或设置 编号
        /// </summary>
        [DisplayName("编号")]
        public Guid Id { get; set; }

        /// <summary>
        /// 获取或设置 通道代码
        /// </summary>
        [DisplayName("通道代码")]
        public string Code { get; set; }

        /// <summary>
        /// 获取或设置 通道名称
        /// </summary>
        [DisplayName("通道名称")]
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
        /// 获取或设置 是否可更新的数据权限状态
        /// </summary>
        public bool Updatable { get; set; }

        /// <summary>
        /// 获取或设置 是否可删除的数据权限状态
        /// </summary>
        public bool Deletable { get; set; }

    }
}