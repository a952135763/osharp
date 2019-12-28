// -----------------------------------------------------------------------
//  <copyright file="RoleConfiguration.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-27 4:48</last-date>
// -----------------------------------------------------------------------

using System;

using KaPai.Pay.Identity.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using OSharp.Entity;


namespace KaPai.Pay.EntityConfiguration.Identity
{
    public class RoleConfiguration : EntityTypeConfigurationBase<Role, int>
    {
        /// <summary>
        /// 重写以实现实体类型各个属性的数据库配置
        /// </summary>
        /// <param name="builder">实体类型创建器</param>
        public override void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasIndex(m => new { m.NormalizedName, m.DeletedTime }).HasName("RoleNameIndex").IsUnique();

            builder.Property(m => m.ConcurrencyStamp).IsConcurrencyToken();

            builder.HasData(new Role() { Id = 1,Name = "系统管理员", NormalizedName = "系统管理员", Remark = "系统最高权限管理角色", ConcurrencyStamp = "97313840-7874-47e5-81f2-565613c8cdcc", IsAdmin = true, IsSystem = true, CreatedTime = new DateTime(1970, 1, 1) });
           
            builder.HasData(new Role() { Id = 2, Name = "供应商", NormalizedName = "供应商", Remark = "业务供应商", ConcurrencyStamp = "0147D7FA-BDA8-4319-BC54-EBD59B6BD8F6", IsAdmin = false, IsSystem = true, CreatedTime = new DateTime(1970, 1, 1) });

            builder.HasData(new Role() { Id = 3, Name = "商户", NormalizedName = "商户", Remark = "业务商户", ConcurrencyStamp = "14B6B0D4-D9B9-41B6-8018-4E0EE4A16F94", IsAdmin = false, IsSystem = true, CreatedTime = new DateTime(1970, 1, 1) });

        }
    }
}