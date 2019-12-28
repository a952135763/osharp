// -----------------------------------------------------------------------
//  <copyright file="Startup.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-27 4:50</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using AutoMapper.Configuration;
using IP2Region;
using KaPai.Pay.My;
using KaPai.Pay.SignalR.HubPro;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore;
using OSharp.AspNetCore;


namespace KaPai.Pay.Web
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(
                p=>
                {
                    p.AddPolicy("corsa", c =>
                    {
                        c.AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials()
                            .WithOrigins("http://149.129.121.16");
                    });
                    p.AddPolicy("corsdev", c =>
                    {
                        c.AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials()
                            .WithOrigins("http://localhost:4203");
                    });
                });
            services.AddHttpClient();
#pragma warning disable CA2000 // 丢失范围之前释放对象
            var _search = new DbSearcher(Environment.CurrentDirectory + @"/ip2region.db");
#pragma warning restore CA2000 // 丢失范围之前释放对象
                              // MemorySearch 是线程安全的
            _search.MemorySearch("183.192.62.65");

            services.TryAddSingleton(_search);

            services.AddOSharp<AspOsharpPackManager>();

        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseCors("corsdev");
            }
            else
            {
                app.UseExceptionHandler("/#/500");
                app.UseCors("corsa");
                // 不强制重定向到https 后端接口直接暴露需要添加https

                // app.UseHsts();
                // app.UseHttpsRedirection();
            }

            app
                //.UseMiddleware<NodeNoFoundHandlerMiddleware>()
                .UseMiddleware<NodeExceptionHandlerMiddleware>()
                .UseDefaultFiles()
                .UseStaticFiles()
                .UseCors("cors")
                .UseOSharp()
                .UseEndpoints(e =>
                {
                    e.MapHub<HubPro>("/hub/prohub");
                });
        }
    }
}