// -----------------------------------------------------------------------
// <once-generated>
//     这个文件只生成一次，再次生成不会被覆盖。
//     可以在此类的AddServices方法给“Merchant”模块添加自定义服务配对，或者在UsePack方法进行模块初始化
// </once-generated>
//
//  <copyright file="IMerchantPack.cs">
//      KaPai©2019 Microsoft Corporation. All rights reserved. 
//  </copyright>
//  <site></site>
//  <last-editor>KaPai</last-editor>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using OSharp.Core.Packs;


namespace KaPai.Pay.Merchant
{
    /// <summary>
    /// 商户模块
    /// </summary>
    [Description("商户模块")]
    public class MerchantPack : OsharpPack
    {
        /// <summary>将模块服务添加到依赖注入服务容器中</summary>
        /// <param name="services">依赖注入服务容器</param>
        /// <returns></returns>
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            services.TryAddScoped<IMerchantContract, MerchantService>();

            return services;
        }

        /// <summary>
        /// 应用模块服务
        /// </summary>
        /// <param name="provider">服务提供者</param>
        public override void UsePack(IServiceProvider provider)
        {
            base.UsePack(provider);
        }
    }
}
