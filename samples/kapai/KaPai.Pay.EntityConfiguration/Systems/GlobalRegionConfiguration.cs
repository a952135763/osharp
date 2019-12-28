// -----------------------------------------------------------------------
//  <copyright file="AuditEntity.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-08-02 4:17</last-date>
// -----------------------------------------------------------------------

using System;

using KaPai.Pay.Systems.Entities;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

using OSharp.Entity;



namespace KaPai.Pay.EntityConfiguration.Systems
{
    public class GlobalRegionConfiguration : EntityTypeConfigurationBase<GlobalRegion, int>
    {
        public override void Configure(EntityTypeBuilder<GlobalRegion> builder)
        {
            builder.HasIndex(d => d.Level);
            builder.HasIndex(d => d.Pid);
        }
    }
}