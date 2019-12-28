using System;
using System.Collections.Generic;
using System.Text;
using KaPai.Pay.Channel.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


//乐观并发处理字段 使用PstgerSql 默认并发字段
namespace KaPai.Pay.EntityConfiguration.Channel
{
    public partial class ChannelsConfiguration
    {
        partial void EntityConfigurationAppend(EntityTypeBuilder<Channels> builder)
        {
            builder.Property<uint>("xmin")
                .HasColumnType("xid")
                .ValueGeneratedOnAddOrUpdate()
                .IsConcurrencyToken();
        }
    }
    public partial class ChannelTypeConfiguration
    {
        partial void EntityConfigurationAppend(EntityTypeBuilder<ChannelType> builder)
        {
            builder.Property<uint>("xmin")
                .HasColumnType("xid")
                .ValueGeneratedOnAddOrUpdate()
                .IsConcurrencyToken();
        }
    }
    public partial class PercentageConfiguration
    {
        partial void EntityConfigurationAppend(EntityTypeBuilder<Percentage> builder)
        {
            builder.Property<uint>("xmin")
                .HasColumnType("xid")
                .ValueGeneratedOnAddOrUpdate()
                .IsConcurrencyToken();
        }
    }
    public partial class  UserChannelConfiguration
    {
        partial void EntityConfigurationAppend(EntityTypeBuilder<UserChannel> builder)
        {
            builder.Property<uint>("xmin")
                .HasColumnType("xid")
                .ValueGeneratedOnAddOrUpdate()
                .IsConcurrencyToken();
        }
    }
}
