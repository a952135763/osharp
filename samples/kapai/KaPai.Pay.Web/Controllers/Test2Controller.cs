using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using KaPai.Pay.Identity.Entities;
using KaPai.Pay.Merchant;
using KaPai.Pay.Provide.Dtos;
using KaPai.Pay.Provide.Entities;
using KaPai.Pay.SignalR.HubPro;
using KaPai.Pay.SignalR.HubPro.Event;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyModel;

using OSharp.AspNetCore.Mvc;
using OSharp.AspNetCore.UI;
using OSharp.Collections;
using OSharp.Core.Modules;
using OSharp.Entity;
using OSharp.Mapping;


namespace KaPai.Pay.Web.Controllers
{
    public class Test2Controller : ApiController
    {
        private readonly DefaultDbContext _dbContext;
        private readonly IHubContext<HubPro, IHubPro> _hubContext;
        private readonly IMerchantContract _merchantContract;

        public Test2Controller(DefaultDbContext dbContext,IHubContext<HubPro,IHubPro> hub, IMerchantContract merchantContract)
        {
            _dbContext = dbContext;
            _hubContext = hub;
            _merchantContract = merchantContract;
        }

        /// <summary>
        /// 功能注释
        /// </summary>
        /// <returns>返回数据</returns>
        [HttpGet]
        [ModuleInfo]
        [Description("测试一下")]
        public string Test02()
        {
            return DependencyContext.Default.CompileLibraries.Select(m => $"{m.Name},{m.Version}").ExpandAndToString("\r\n");
        }

        [HttpGet]
        public string AddOrder()
        {
            var g = Guid.NewGuid();
           var s = _hubContext.Clients.All.Order(new OrderOut
            {
                Action = 1,
                Amount = 100,
                CreatedTime = DateTime.Now,
                OrderId = g
            });
           s.Wait();
           return g.ToString();
        }

        [HttpGet]
        public string RemoveOrder(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var s = _hubContext.Clients.All.Order(new OrderOut
                {
                    Action = 0,
                    Amount = 100,
                    CreatedTime = DateTime.Now,
                    OrderId = Guid.Parse(id)
                });
                s.Wait();
                return "ok";
            }
            return "error";
        }


        [HttpGet]
        public string OrderSettleTest()
        {


            BackgroundJob.Enqueue<IMerchantContract>(m => m.OrderCallback("ecfe326b-de36-43ed-b5ac-ab07010e4c63"));
           

            return "ok";
        }


        [HttpGet]
        public string Testmap()
        {
            var s = new ArticleEntitiesInputDto()
            {
                Extra = new Dictionary<string, object>
                {
                    {"test", "go"}, {"test1", 123}, {"test2", true}, {"test3", null}
                }
            };





            var m = s.MapTo<ArticleEntities>();

            return "OK";
        }
    }
}
