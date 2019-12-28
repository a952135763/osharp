// -----------------------------------------------------------------------
// <auto-generated>
//    此代码由代码生成器生成。
//    手动更改此文件可能导致应用程序出现意外的行为。
//    如果重新生成代码，对此文件的任何修改都会丢失。
//    如果需要扩展此类，可以遵守如下规则进行扩展：
//      1.横向扩展：如需给当前实体 Channels 添加方法，可新建文件“ChannelServiceBase.Channels.cs”的分部类“public abstract partial class ChannelServiceBase”添加功能
//      2.纵向扩展：如需要重写当前实体 Channels 的业务实现，可新建文件“ChannelService.Channels.cs”的分部类“public partial class ChannelService”对现有方法进行方法重写实现
// </auto-generated>
//
//  <copyright file="ChannelServiceBase.Channels.generated.cs">
//      KaPai©2019 Microsoft Corporation. All rights reserved. 
//  </copyright>
//  <site></site>
//  <last-editor>KaPai</last-editor>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using OSharp.Data;
using OSharp.Dependency;
using OSharp.Extensions;
using OSharp.Mapping;

using KaPai.Pay.Channel.Dtos;
using KaPai.Pay.Channel.Entities;


namespace KaPai.Pay.Channel
{
    public abstract partial class ChannelServiceBase
    {
        /// <summary>
        /// 获取 通道列表信息查询数据集
        /// </summary>
        public IQueryable<Channels> Channelses
        {
            get { return ChannelsRepository.QueryAsNoTracking(); }
        }

        /// <summary>
        /// 检查通道列表信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的通道列表信息编号</param>
        /// <returns>通道列表信息是否存在</returns>
        public virtual Task<bool> CheckChannelsExists(Expression<Func<Channels, bool>> predicate, Guid id = default(Guid))
        {
            return ChannelsRepository.CheckExistsAsync(predicate, id);
        }
        
        /// <summary>
        /// 添加通道列表信息
        /// </summary>
        /// <param name="dtos">要添加的通道列表信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        public virtual Task<OperationResult> CreateChannelses(params ChannelsInputDto[] dtos)
        {
            Check.Validate<ChannelsInputDto, Guid>(dtos, nameof(dtos));
            return ChannelsRepository.InsertAsync(dtos);
        }
        
        /// <summary>
        /// 更新通道列表信息
        /// </summary>
        /// <param name="dtos">包含更新信息的通道列表信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        public virtual Task<OperationResult> UpdateChannelses(params ChannelsInputDto[] dtos)
        {
            Check.Validate<ChannelsInputDto, Guid>(dtos, nameof(dtos));
            return ChannelsRepository.UpdateAsync(dtos);
        }
    }
}
