// -----------------------------------------------------------------------
//  <copyright file="SignalrPack.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor></last-editor>
//  <last-date>2018-07-26 12:15</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OSharp.AspNetCore;
using OSharp.Core.Packs;
using OSharp.Dependency;
using OSharp.Exceptions;
using OSharp.Extensions;


namespace KaPai.Pay.Web.Startups
{

    [Description("SignalR,实时通信模块")]
    public  class SignalRPack : OsharpPack
    {
        public override PackLevel Level => PackLevel.Framework;

        public override int Order => 101;

        public override IServiceCollection AddServices(IServiceCollection services)
        {

            IConfiguration configuration = services.GetConfiguration();

            string config = configuration["OSharp:Redis:Configuration"];
            if (config.IsNullOrEmpty())
            {
                throw new OsharpException("配置文件中Redis节点的Configuration不能为空");
            }
            string name = configuration["OSharp:Redis:InstanceName"].CastTo("RedisName");

            // 设置SignalR底板 方便在分布式部署
            services.AddSignalR()
                .AddStackExchangeRedis(config, o =>
                {
                    o.Configuration.ChannelPrefix = "SignalR";
                })
                .AddMessagePackProtocol();

            return services;
        }

        public override void UsePack(IServiceProvider provider)
        {

            base.UsePack(provider);
        }
    }
}