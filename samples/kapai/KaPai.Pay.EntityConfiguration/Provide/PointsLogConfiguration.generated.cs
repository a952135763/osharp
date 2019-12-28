//------------------------------------------------------------------------------
// <auto-generated>
//    此代码由代码生成器生成。
//    手动更改此文件可能导致应用程序出现意外的行为。
//    如果重新生成代码，对此文件的任何修改都会丢失。
//    如果需要扩展此类：可遵守如下规则进行扩展：
//      1.横向扩展：如需添加额外的映射，可新建文件“PointsLogConfiguration.cs”的分部类“public partial class PointsLogConfiguration”}实现分部方法 EntityConfigurationAppend 进行扩展
// </auto-generated>
//
//  <copyright file="PointsLogConfiguration.generated.cs">
//      KaPai©2019 Microsoft Corporation. All rights reserved. 
//  </copyright>
//  <site></site>
//  <last-editor>KaPai</last-editor>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using OSharp.Entity;

using KaPai.Pay.Provide.Entities;
using KaPai.Pay.Identity.Entities;


namespace KaPai.Pay.EntityConfiguration.Provide
{
    /// <summary>
    /// 实体配置类：积分变动记录信息
    /// </summary>
    public partial class PointsLogConfiguration : EntityTypeConfigurationBase<PointsLog, Guid>
    {
        /// <summary>
        /// 重写以实现实体类型各个属性的数据库配置
        /// </summary>
        /// <param name="builder">实体类型创建器</param>
        public override void Configure(EntityTypeBuilder<PointsLog> builder)
        {
            builder.HasOne<User>(m => m.User).WithMany().HasForeignKey(m => m.UserId).OnDelete(DeleteBehavior.Cascade);

            EntityConfigurationAppend(builder);
        }

        /// <summary>
        /// 额外的数据映射
        /// </summary>
        partial void EntityConfigurationAppend(EntityTypeBuilder<PointsLog> builder);
    }
}

