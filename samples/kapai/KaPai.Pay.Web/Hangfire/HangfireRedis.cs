using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.Pro;
using Hangfire.AspNetCore;
using Hangfire.Pro.Redis;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OSharp.Extensions;
using OSharp.Hangfire;
using StackExchange.Redis;

namespace KaPai.Pay.Web.Hangfire
{
    public class HangfireRedis: HangfirePack
    {
        

        /**
         * 配置 Hangfire 使用的存储 以及 Redis 设置
         */
        protected override Action<IGlobalConfiguration> GetHangfireAction(IServiceCollection services)
        {
            
            IConfiguration configuration = services.GetConfiguration();
            string storageConnectionString = configuration["OSharp:Hangfire:StorageConnectionString"].CastTo<string>();
            if (storageConnectionString != null)
            {
                GlobalConfiguration.Configuration.UseBatches();
                return config =>
                {
                    config.UseRedisStorage(storageConnectionString,
                            new RedisStorageOptions() {Prefix = "Task:"});
                };
                
            }

            return config => { };
        }

        /**
         * 注册 Hangfire 
         */
        public override void UsePack(IApplicationBuilder app)
        {
            IServiceProvider serviceProvider = app.ApplicationServices;
            IConfiguration configuration = serviceProvider.GetService<IConfiguration>();
            bool enabled = configuration["OSharp:Hangfire:Enabled"].CastTo(false);
            if (!enabled)
            {
                return;
            }
            //注册 Hangfire 获取对象 执行后台任务的时候 可以直接执行任意模块下面的方法,但是 某些需要 HttpContext 无法执行
            GlobalConfiguration.Configuration.UseActivator(new JobActivatorIoc(serviceProvider));

            IGlobalConfiguration globalConfiguration = serviceProvider.GetService<IGlobalConfiguration>();
            globalConfiguration.UseLogProvider(new AspNetCoreLogProvider(serviceProvider.GetService<ILoggerFactory>()));

            BackgroundJobServerOptions serverOptions = GetBackgroundJobServerOptions(configuration);
            app.UseHangfireServer(serverOptions);

            string url = configuration["OSharp:Hangfire:DashboardUrl"].CastTo("/hangfire");
            DashboardOptions dashboardOptions = GetDashboardOptions(configuration);
            app.UseHangfireDashboard(url, dashboardOptions);

            IHangfireJobRunner jobRunner = serviceProvider.GetService<IHangfireJobRunner>();
            jobRunner?.Start();


            IsEnabled = true;
        }
    }
}
