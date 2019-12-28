using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OSharp.Core.Packs;
using OSharp.Exceptions;
using OSharp.Extensions;
using StackExchange.Redis;

namespace KaPai.Pay.Web.Startups
{

    [Description("Redis重写实现")]
    public class RedisPack : OsharpPack
    {
        private bool _enabled = false;

        public override PackLevel Level => PackLevel.Framework;

        public override int Order => 100;

        public override IServiceCollection AddServices(IServiceCollection services)
        {

            IConfiguration configuration = services.GetConfiguration();
            _enabled = configuration["OSharp:Redis:Enabled"].CastTo(false);
            if (!_enabled)
            {
                throw new OsharpException("redis缓存未设置");
            }

            string config = configuration["OSharp:Redis:Configuration"];
            if (config.IsNullOrEmpty())
            {
                throw new OsharpException("配置文件中Redis节点的Configuration不能为空");
            }
            string name = configuration["OSharp:Redis:InstanceName"].CastTo("RedisName");


            services.RemoveAll(typeof(IDistributedCache));
            services.AddStackExchangeRedisCache(opts =>
            {
                opts.Configuration = config;
                opts.InstanceName = name;
            });
            // 添加一个 IDatabase 和 ConnectionMultiplexer
            var c = ConnectionMultiplexer.Connect(config);
            services.TryAddSingleton<ConnectionMultiplexer>(c);
            var g = c.GetDatabase(2);
            services.TryAddSingleton<IDatabase>(g);
            return services;
        }

        public override void UsePack(IServiceProvider provider)
        {
            IsEnabled = _enabled;
        }
    }
}
