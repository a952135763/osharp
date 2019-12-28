using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using KaPai.Pay.Infos;
using KaPai.Pay.OpenApi.Base;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OSharp.Core.Packs;
using OSharp.Dependency;

namespace KaPai.Pay.OpenApi
{

    /// <summary>
    /// 信息模块
    /// </summary>
    [Description("公开Api模块")]
    class OpenApiPack : OsharpPack
    {
        /// <summary>将模块服务添加到依赖注入服务容器中</summary>
        /// <param name="services">依赖注入服务容器</param>
        /// <returns></returns>
        public override IServiceCollection AddServices(IServiceCollection services)
        {

            // 添加所有实现了InterfaceChannel的类
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(INterfaceChannel))))
                .ToArray();
            foreach (Type type in types)
            {
                services.TryAddScoped(type);
            }

            services.TryAddScoped<IOpenContracr, OpenApi>();
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
