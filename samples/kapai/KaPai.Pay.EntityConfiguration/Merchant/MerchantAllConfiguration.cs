using System;
using System.Collections.Generic;
using System.Text;
using KaPai.Pay.Merchant.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


//乐观并发处理字段 使用PstgerSql 默认并发字段
namespace KaPai.Pay.EntityConfiguration.Merchant
{
    public partial class AmountsConfiguration
    {

        partial void EntityConfigurationAppend(EntityTypeBuilder<Amounts> builder)
        {
            builder.Property<uint>("xmin")
                .HasColumnType("xid")
                .ValueGeneratedOnAddOrUpdate()
                .IsConcurrencyToken();
        }

    }

    public partial class AmountsLogConfiguration
    {
        partial void EntityConfigurationAppend(EntityTypeBuilder<AmountsLog> builder)
        {
            builder.Property<uint>("xmin")
                .HasColumnType("xid")
                .ValueGeneratedOnAddOrUpdate()
                .IsConcurrencyToken();
        }
    }

    public partial class MerchantExtraConfiguration
    {
        partial void EntityConfigurationAppend(EntityTypeBuilder<MerchantExtra> builder)
        {
            builder.Property<uint>("xmin")
                .HasColumnType("xid")
                .ValueGeneratedOnAddOrUpdate()
                .IsConcurrencyToken();
        }
    }

    public partial class OrdersConfiguration
    {
        partial void EntityConfigurationAppend(EntityTypeBuilder<Orders> builder)
        {
            builder.Property<uint>("xmin")
                .HasColumnType("xid")
                .ValueGeneratedOnAddOrUpdate()
                .IsConcurrencyToken();
        }
    }
}
