// -----------------------------------------------------------------------
// <auto-generated>
//    此代码由代码生成器生成。
//    手动更改此文件可能导致应用程序出现意外的行为。
//    如果重新生成代码，对此文件的任何修改都会丢失。
//    如果需要扩展此类：可遵守如下规则进行扩展：
//      1.横向扩展：如需添加额外的属性，可新建文件“OrdersInputDto.cs”的分部类“public partial class OrdersInputDto”}添加属性
// </auto-generated>
//
//  <copyright file="OrdersInputDto.generated.cs">
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


namespace KaPai.Pay.Merchant.Dtos
{
    /// <summary>
    /// 输入DTO：商户订单列表信息
    /// </summary>
    [MapTo(typeof(Orders))]
    [Description("商户订单列表信息")]
    public partial class OrdersInputDto : IInputDto<Guid>
    {
        /// <summary>
        /// 获取或设置 编号
        /// </summary>
        [DisplayName("编号")]
        public Guid Id { get; set; }

    }
}
