// -----------------------------------------------------------------------
// <auto-generated>
//    此代码由代码生成器生成。
//    手动更改此文件可能导致应用程序出现意外的行为。
//    如果重新生成代码，对此文件的任何修改都会丢失。
//    如果需要扩展此类，可以遵守如下规则进行扩展：
//      1.横向扩展：如需给当前实体 ArticleEntities 添加方法，可新建文件“ProvideServiceBase.ArticleEntities.cs”的分部类“public abstract partial class ProvideServiceBase”添加功能
//      2.纵向扩展：如需要重写当前实体 ArticleEntities 的业务实现，可新建文件“ProvideService.ArticleEntities.cs”的分部类“public partial class ProvideService”对现有方法进行方法重写实现
// </auto-generated>
//
//  <copyright file="ProvideServiceBase.ArticleEntities.generated.cs">
//      KaPai©2019 Microsoft Corporation. All rights reserved. 
//  </copyright>
//  <site></site>
//  <last-editor>KaPai</last-editor>
// -----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using JetBrains.Annotations;
using KaPai.Pay.My;
using KaPai.Pay.OpenApi.Base;
using OSharp.Data;
using OSharp.Dependency;
using OSharp.Extensions;
using OSharp.Mapping;

using KaPai.Pay.Provide.Dtos;
using KaPai.Pay.Provide.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OSharp.Caching;
using OSharp.Entity;
using OSharp.Exceptions;


namespace KaPai.Pay.Provide
{
    public abstract partial class ProvideServiceBase
    {
        /// <summary>
        /// 获取 收款号列表信息查询数据集
        /// </summary>
        public IQueryable<ArticleEntities> ArticleEntitieses
        {
            get { return ArticleEntitiesRepository.QueryAsNoTracking(); }
        }

        /// <summary>
        /// 检查收款号列表信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的收款号列表信息编号</param>
        /// <returns>收款号列表信息是否存在</returns>
        public virtual Task<bool> CheckArticleEntitiesExists(Expression<Func<ArticleEntities, bool>> predicate, Guid id = default(Guid))
        {
            return ArticleEntitiesRepository.CheckExistsAsync(predicate, id);
        }

        /// <summary>
        /// 添加收款号列表信息
        /// </summary>
        /// <param name="dtos">要添加的收款号列表信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        public async virtual Task<OperationResult> CreateArticleEntitieses(ArticleEntitiesInputDto[] dtos)
        {
            Check.Validate<ArticleEntitiesInputDto, Guid>(dtos, nameof(dtos));
            ConcurrentDictionary<string, INterfaceChannel> sDictionary = new ConcurrentDictionary<string, INterfaceChannel>();
            var res = await ArticleEntitiesRepository.InsertAsync(dtos, null, (dto, entities) =>
             {

                 var code = ArticleAssortRepository.Query(a => a.Id == dto.ArticleAssortId).Include(a => a.Channel).Select(a => a.Channel).ToCacheArray(120);
                 if (code.Length > 1)
                 {
                     var obj = code[0].GetChannelBase();
                     sDictionary.TryAdd(code[0].Code, obj);
                     return obj.CreateArticleEntitieses(entities);
                 }
                 else
                 {
                     throw new OsharpException($"无法获取到对应的通道:{entities.Id}");
                 }
             });
            var r = sDictionary.Select(a => a.Value.CreateEnd()).ToArray();
            return res;
        }

        /// <summary>
        /// 更新收款号列表信息
        /// </summary>
        /// <param name="dtos">包含更新信息的收款号列表信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        public async virtual Task<OperationResult> UpdateArticleEntitieses(params ArticleEntitiesInputDto[] dtos)
        {
            Check.Validate<ArticleEntitiesInputDto, Guid>(dtos, nameof(dtos));
            ConcurrentDictionary<string, INterfaceChannel> sDictionary = new ConcurrentDictionary<string, INterfaceChannel>();


            var res = await ArticleEntitiesRepository.UpdateAsync(dtos, null, (dto, entities) =>
            {
                var code = ArticleAssortRepository.Query(a => a.Id == dto.ArticleAssortId).Include(a => a.Channel).Select(a=>a.Channel).ToCacheArray(120);
                if (code.Length > 1)
                {
                    var obj = code[0].GetChannelBase();
                    sDictionary.TryAdd(code[0].Code, obj);
                    return obj.UpdataArticleEntitieses(entities);
                }
                else
                {
                    throw new OsharpException($"无法获取到对应的通道:{entities.Id}");

                }
            });
            var r = sDictionary.Select(a => a.Value.UpdateEnd()).ToArray();
            return res;
        }
    }
}
