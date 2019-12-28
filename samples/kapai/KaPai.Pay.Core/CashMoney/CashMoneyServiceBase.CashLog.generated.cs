// -----------------------------------------------------------------------
// <auto-generated>
//    此代码由代码生成器生成。
//    手动更改此文件可能导致应用程序出现意外的行为。
//    如果重新生成代码，对此文件的任何修改都会丢失。
//    如果需要扩展此类，可以遵守如下规则进行扩展：
//      1.横向扩展：如需给当前实体 CashLog 添加方法，可新建文件“CashMoneyServiceBase.CashLog.cs”的分部类“public abstract partial class CashMoneyServiceBase”添加功能
//      2.纵向扩展：如需要重写当前实体 CashLog 的业务实现，可新建文件“CashMoneyService.CashLog.cs”的分部类“public partial class CashMoneyService”对现有方法进行方法重写实现
// </auto-generated>
//
//  <copyright file="CashMoneyServiceBase.CashLog.generated.cs">
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

using KaPai.Pay.CashMoney.Dtos;
using KaPai.Pay.CashMoney.Entities;


namespace KaPai.Pay.CashMoney
{
    public abstract partial class CashMoneyServiceBase
    {
        /// <summary>
        /// 获取 提现记录信息查询数据集
        /// </summary>
        public IQueryable<CashLog> CashLogs
        {
            get { return CashLogRepository.QueryAsNoTracking(); }
        }

        /// <summary>
        /// 检查提现记录信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的提现记录信息编号</param>
        /// <returns>提现记录信息是否存在</returns>
        public virtual Task<bool> CheckCashLogExists(Expression<Func<CashLog, bool>> predicate, Guid id = default(Guid))
        {
            return CashLogRepository.CheckExistsAsync(predicate, id);
        }
        
        /// <summary>
        /// 添加提现记录信息
        /// </summary>
        /// <param name="dtos">要添加的提现记录信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        public virtual Task<OperationResult> CreateCashLogs(params CashLogInputDto[] dtos)
        {
            
            // Todo: 提现操作 需要实现
            Check.Validate<CashLogInputDto, Guid>(dtos, nameof(dtos));
            return CashLogRepository.InsertAsync(dtos);
        }
    }
}