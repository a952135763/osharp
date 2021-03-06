//------------------------------------------------------------------------------
// <auto-generated>
//    此代码由代码生成器生成。
//    手动更改此文件可能导致应用程序出现意外的行为。
//    如果重新生成代码，对此文件的任何修改都会丢失。
//    如果需要扩展此类：可遵守如下规则进行扩展：
//      1.横向扩展：如需添加额外的属性，可新建文件“AmountsLog.cs”的分部类“public partial class AmountsLog”}添加属性
// </auto-generated>
//
//  <copyright file="AmountsLog.generated.cs">
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

namespace KaPai.Pay.Merchant.Entities
{
    /// <summary>
    /// 实体类：余额变动记录信息
    /// </summary>
    [Description("余额变动记录信息")]
    public partial class AmountsLog : EntityBase<Guid>, ICreatedTime
    {
        /// <summary>
        /// 获取或设置 记录ID
        /// </summary>
        [DisplayName("记录ID"), StringLength(50)]
        public string PayID { get; set; }

        /// <summary>
        /// 获取或设置 影响类型
        /// </summary>
        [DisplayName("影响类型"), StringLength(50)]
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
        [DisplayName("备注说明"), StringLength(200)]
        public string Remarks { get; set; }

        /// <summary>
        /// 获取或设置 创建时间
        /// </summary>
        [DisplayName("创建时间")]
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// 获取或设置 用户ID
        /// </summary>
        [DisplayName("用户ID"), UserFlag]
        public int UserId { get; set; }

        /// <summary>
        /// 获取或设置 用户
        /// </summary>
        [DisplayName("用户")]
        public virtual User User { get; set; }

    }
}

