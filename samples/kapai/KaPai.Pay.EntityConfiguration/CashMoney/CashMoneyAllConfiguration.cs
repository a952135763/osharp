using System;
using System.Collections.Generic;
using System.Text;
using KaPai.Pay.CashMoney.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

//乐观并发处理字段 使用PstgerSql 默认并发字段
namespace KaPai.Pay.EntityConfiguration.CashMoney
{
    public partial class BankListConfiguration
    {
        partial void EntityConfigurationAppend(EntityTypeBuilder<BankList> builder)
        {
            builder.Property<uint>("xmin")
                .HasColumnType("xid")
                .ValueGeneratedOnAddOrUpdate()
                .IsConcurrencyToken();
        }
    }

    public partial class CashLogConfiguration
    {
        partial void EntityConfigurationAppend(EntityTypeBuilder<CashLog> builder)
        {
            builder.Property<uint>("xmin")
                .HasColumnType("xid")
                .ValueGeneratedOnAddOrUpdate()
                .IsConcurrencyToken();
        }
    }

}
