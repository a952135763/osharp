// -----------------------------------------------------------------------
// <auto-generated>
//    此代码由代码生成器生成。
//    手动更改此文件可能导致应用程序出现意外的行为。
//    如果重新生成代码，对此文件的任何修改都会丢失。
//    如果需要扩展此类：可遵守如下规则进行扩展：
//      1.横向扩展：如需添加额外的属性，可新建文件“AmountsLogOutputDto.cs”的分部类“public partial class AmountsLogOutputDto”}添加属性
// </auto-generated>
//
//  <copyright file="AmountsLogOutputDto.generated.cs">
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
    /// 输入DTO：余额变动记录信息
    /// </summary>
    [MapFrom(typeof(AmountsLog))]
    [Description("余额变动记录信息")]
    public partial class AmountsLogOutputDto : IOutputDto, IDataAuthEnabled
    {
        /// <summary>
        /// 初始化一个<see cref="AmountsLogOutputDto"/>类型的新实例
        /// </summary>
        public AmountsLogOutputDto()
        { }

        /// <summary>
        /// 初始化一个<see cref="AmountsLogOutputDto"/>类型的新实例
        /// </summary>
        public AmountsLogOutputDto(AmountsLog entity)
        {
            Id = entity.Id;
            UserId = entity.UserId;
            PayID = entity.PayID;
            PayType = entity.PayType;
            AfterAmounts = entity.AfterAmounts;
            BeforeAmounts = entity.BeforeAmounts;
            Amounts = entity.Amounts;
            Remarks = entity.Remarks;
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
        /// 获取或设置 记录ID
        /// </summary>
        [DisplayName("记录ID")]
        public string PayID { get; set; }

        /// <summary>
        /// 获取或设置 影响类型
        /// </summary>
        [DisplayName("影响类型")]
        public string PayType { get; set; }

        /// <summary>
        /// 获取或设置 影响后金额
        /// </summary>
        [DisplayName("影响后金额")]
        public long AfterAmounts { get; set; }

        /// <summary>
        /// 获取或设置 影响前金额
        /// </summary>
        [DisplayName("影响前金额")]
        public long BeforeAmounts { get; set; }

        /// <summary>
        /// 获取或设置 影响金额
        /// </summary>
        [DisplayName("影响金额")]
        public long Amounts { get; set; }

        /// <summary>
        /// 获取或设置 备注说明
        /// </summary>
        [DisplayName("备注说明")]
        public string Remarks { get; set; }

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
