// -----------------------------------------------------------------------
// <auto-generated>
//    此代码由代码生成器生成。
//    手动更改此文件可能导致应用程序出现意外的行为。
//    如果重新生成代码，对此文件的任何修改都会丢失。
//    如果需要扩展此类，请在控制器类型 CashMoneyService 进行继承重写
// </auto-generated>
//
//  <copyright file="ICashMoneyServiceBase.generated.cs">
//      KaPai©2019 Microsoft Corporation. All rights reserved. 
//  </copyright>
//  <site></site>
//  <last-editor>KaPai</last-editor>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;

using OSharp.Core.Systems;
using OSharp.Data;
using OSharp.Entity;
using OSharp.EventBuses;
using OSharp.Extensions;
using OSharp.Identity;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using KaPai.Pay.CashMoney.Dtos;
using KaPai.Pay.CashMoney.Entities;


namespace KaPai.Pay.CashMoney
{
    /// <summary>
    /// 业务实现基类：提现模块模块
    /// </summary>
    public abstract partial class CashMoneyServiceBase : ICashMoneyContract
    {
        /// <summary>
        /// 初始化一个<see cref="CashMoneyService"/>类型的新实例
        /// </summary>
        protected CashMoneyServiceBase(IServiceProvider provider)
        {
            ServiceProvider = provider;
            Logger = provider.GetLogger(GetType());
        }
    
        #region 属性

        /// <summary>
        /// 获取或设置 服务提供者对象
        /// </summary>
        protected IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// 获取或设置 日志对象
        /// </summary>
        protected ILogger Logger { get; }

        /// <summary>
        /// 获取或设置 收款账号信息仓储对象
        /// </summary>
        protected IRepository<BankList, Guid> BankListRepository => ServiceProvider.GetService<IRepository<BankList, Guid>>();
        
        /// <summary>
        /// 获取或设置 提现记录信息仓储对象
        /// </summary>
        protected IRepository<CashLog, Guid> CashLogRepository => ServiceProvider.GetService<IRepository<CashLog, Guid>>();
        
        /// <summary>
        /// 获取 事件总线
        /// </summary>
        protected IEventBus EventBus => ServiceProvider.GetService<IEventBus>();

        /// <summary>
        /// 获取 设置存储对象
        /// </summary>
        protected IKeyValueStore KeyValueStore => ServiceProvider.GetService<IKeyValueStore>();

        #endregion
    }
}