// -----------------------------------------------------------------------
// <auto-generated>
//    此代码由代码生成器生成。
//    手动更改此文件可能导致应用程序出现意外的行为。
//    如果重新生成代码，对此文件的任何修改都会丢失。
//    如果需要扩展此类：可遵守如下规则进行扩展：
//      1.横向扩展：如需添加额外的属性，可新建文件“AmountsOutputDto.cs”的分部类“public partial class AmountsOutputDto”}添加属性
// </auto-generated>
//
//  <copyright file="AmountsOutputDto.generated.cs">
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

using KaPai.Pay.Merchant.Entities;
using KaPai.Pay.Identity.Dtos;

namespace KaPai.Pay.Merchant.Dtos
{
    /// <summary>
    /// 输入DTO：商户实时余额信息
    /// </summary>
    [MapFrom(typeof(Amounts))]
    [Description("商户实时余额信息")]
    public partial class AmountsOutputDto : IOutputDto, IDataAuthEnabled
    {
        /// <summary>
        /// 初始化一个<see cref="AmountsOutputDto"/>类型的新实例
        /// </summary>
        public AmountsOutputDto()
        { }

        /// <summary>
        /// 初始化一个<see cref="AmountsOutputDto"/>类型的新实例
        /// </summary>
        public AmountsOutputDto(Amounts entity)
        {
            Id = entity.Id;
            UserId = entity.UserId;
            Amount = entity.Amount;
            Accumulative = entity.Accumulative;
            CreatedTime = entity.CreatedTime;
        }

        /// <summary>
        /// 获取或设置 编号
        /// </summary>
        [DisplayName("编号")]
        public Guid Id { get; set; }

        /// <summary>
        /// 获取或设置 用户ID
        /// </summary>
        [DisplayName("用户ID")]
        public int UserId { get; set; }

        /// <summary>
        /// 获取或设置 余额
        /// </summary>
        [DisplayName("余额")]
        public long Amount { get; set; }

        /// <summary>
        /// 获取或设置 累计余额
        /// </summary>
        [DisplayName("累计余额")]
        public long Accumulative { get; set; }

        /// <summary>
        /// 获取或设置 创建时间
        /// </summary>
        [DisplayName("创建时间")]
        public DateTime CreatedTime { get; set; }

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