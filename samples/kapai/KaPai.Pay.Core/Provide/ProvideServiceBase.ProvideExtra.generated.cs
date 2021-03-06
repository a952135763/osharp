// -----------------------------------------------------------------------
// <auto-generated>
//    此代码由代码生成器生成。
//    手动更改此文件可能导致应用程序出现意外的行为。
//    如果重新生成代码，对此文件的任何修改都会丢失。
//    如果需要扩展此类，可以遵守如下规则进行扩展：
//      1.横向扩展：如需给当前实体 ProvideExtra 添加方法，可新建文件“ProvideServiceBase.ProvideExtra.cs”的分部类“public abstract partial class ProvideServiceBase”添加功能
//      2.纵向扩展：如需要重写当前实体 ProvideExtra 的业务实现，可新建文件“ProvideService.ProvideExtra.cs”的分部类“public partial class ProvideService”对现有方法进行方法重写实现
// </auto-generated>
//
//  <copyright file="ProvideServiceBase.ProvideExtra.generated.cs">
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

using KaPai.Pay.Provide.Dtos;
using KaPai.Pay.Provide.Entities;


namespace KaPai.Pay.Provide
{
    public abstract partial class ProvideServiceBase
    {
        /// <summary>
        /// 获取 供应商参数信息查询数据集
        /// </summary>
        public IQueryable<ProvideExtra> ProvideExtras
        {
            get { return ProvideExtraRepository.QueryAsNoTracking(); }
        }

        /// <summary>
        /// 检查供应商参数信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的供应商参数信息编号</param>
        /// <returns>供应商参数信息是否存在</returns>
        public virtual Task<bool> CheckProvideExtraExists(Expression<Func<ProvideExtra, bool>> predicate, Guid id = default(Guid))
        {
            return ProvideExtraRepository.CheckExistsAsync(predicate, id);
        }
        
        /// <summary>
        /// 更新供应商参数信息
        /// </summary>
        /// <param name="dtos">包含更新信息的供应商参数信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        public virtual Task<OperationResult> UpdateProvideExtras(params ProvideExtraInputDto[] dtos)
        {
            Check.Validate<ProvideExtraInputDto, Guid>(dtos, nameof(dtos));
            return ProvideExtraRepository.UpdateAsync(dtos);
        }
    }
}
