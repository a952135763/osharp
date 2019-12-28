using System;
using System.Collections.Generic;
using System.Text;
using KaPai.Pay.Provide.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


//乐观并发处理字段 使用PstgerSql 默认并发字段
namespace KaPai.Pay.EntityConfiguration.Provide
{

    public partial class ArticleAssortConfiguration
    {
        partial void EntityConfigurationAppend(EntityTypeBuilder<ArticleAssort> builder)
        {
            builder.Property<uint>("xmin")
                .HasColumnType("xid")
                .ValueGeneratedOnAddOrUpdate()
                .IsConcurrencyToken();
        }
    }

    public partial class ArticleEntitiesConfiguration
    {
        partial void EntityConfigurationAppend(EntityTypeBuilder<ArticleEntities> builder)
        {
            builder.Property<uint>("xmin")
                .HasColumnType("xid")
                .ValueGeneratedOnAddOrUpdate()
                .IsConcurrencyToken();
        }

    }

    public partial class PointsConfiguration
    {
        partial void EntityConfigurationAppend(EntityTypeBuilder<Points> builder)
        {
            builder.Property<uint>("xmin")
                .HasColumnType("xid")
                .ValueGeneratedOnAddOrUpdate()
                .IsConcurrencyToken();
        }

    }

    public partial class PointsLogConfiguration
    {
        partial void EntityConfigurationAppend(EntityTypeBuilder<PointsLog> builder)
        {
            builder.Property<uint>("xmin")
                .HasColumnType("xid")
                .ValueGeneratedOnAddOrUpdate()
                .IsConcurrencyToken();
        }
    }

    public partial class ProvideExtraConfiguration
    {

        partial void EntityConfigurationAppend(EntityTypeBuilder<ProvideExtra> builder)
        {
            builder.Property<uint>("xmin")
                .HasColumnType("xid")
                .ValueGeneratedOnAddOrUpdate()
                .IsConcurrencyToken();
        }
    }

}
