//------------------------------------------------------------------------------
// <auto-generated>
//    此代码由代码生成器生成。
//    手动更改此文件可能导致应用程序出现意外的行为。
//    如果重新生成代码，对此文件的任何修改都会丢失。
//    如果需要扩展此类：可遵守如下规则进行扩展：
//      1.横向扩展：如需添加额外的属性，可新建文件“CashLog.cs”的分部类“public partial class CashLog”}添加属性
// </auto-generated>
//
//  <copyright file="CashLog.generated.cs">
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

namespace KaPai.Pay.CashMoney.Entities
{
    /// <summary>
    /// 实体类：提现记录信息
    /// </summary>
    [Description("提现记录信息")]
    public partial class CashLog : EntityBase<Guid>, ISoftDeletable, ICreationAudited<int>, IUpdateAudited<int>
    {
        /// <summary>
        /// 获取或设置 名称
        /// </summary>
        [DisplayName("名称"), StringLength(30)]
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 开户银行
        /// </summary>
        [DisplayName("开户银行"), StringLength(30)]
        public string BankType { get; set; }

        /// <summary>
        /// 获取或设置 收款账号
        /// </summary>
        [DisplayName("收款账号"), StringLength(50)]
        public string Account { get; set; }

        /// <summary>
        /// 获取或设置 收款银行
        /// </summary>
        [DisplayName("收款银行"), StringLength(100)]
        public string BankName { get; set; }


        [DisplayName("提现金额")]
        public long Amount { get; set; }

        /// <summary>
        /// 获取或设置 任务Id
        /// </summary>
        [DisplayName("任务Id"), StringLength(50)]
        public string JobId { get; set; }

        /// <summary>
        /// 获取或设置 备注
        /// </summary>
        [DisplayName("备注"), StringLength(100)]
        public string Remarks { get; set; }

        /// <summary>
        /// 获取或设置 处理状态
        /// </summary>
        [DisplayName("处理状态")]
        public int Status { get; set; }

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
        /// 获取或设置 用户ID
        /// </summary>
        [DisplayName("用户ID"), UserFlag]
        public int UserId { get; set; }

        /// <summary>
        /// 获取或设置 处理用户ID
        /// </summary>
        [DisplayName("处理用户ID")]
        public int? ProUserId { get; set; }

        /// <summary>
        /// 获取或设置 提现用户
        /// </summary>
        [DisplayName("提现用户")]
        public virtual User User { get; set; }

        /// <summary>
        /// 获取或设置 处理用户
        /// </summary>
        [DisplayName("处理用户")]
        public virtual User ProcessUser { get; set; }

    }
}
