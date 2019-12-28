using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using KaPai.Pay.Merchant;
using KaPai.Pay.Merchant.Entities;
using KaPai.Pay.My;
using KaPai.Pay.OpenApi;
using KaPai.Pay.OpenApi.Dtos;
using KaPai.Pay.SignalR.HubPro;
using KaPai.Pay.SignalR.HubPro.Event;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using OSharp.AspNetCore.Mvc;
using OSharp.AspNetCore.Mvc.Filters;
using OSharp.AspNetCore.UI;
using OSharp.Core.Modules;
using OSharp.Data;
using OSharp.Entity;
using OSharp.Json;
using StackExchange.Redis;

namespace KaPai.Pay.Web.Controllers
{

    [ModuleInfo(Position = "OpenApi", PositionName = "公开接口")]
    [Description("公开-系统接口")]
    public class OpenApiController : ApiController
    {

        private readonly IOpenContracr _openContracr;
        private readonly IHubContext<HubPro, IHubPro> _hubContext;
        private readonly IMerchantContract _merchantContract;

        public OpenApiController(
            IOpenContracr openContracr,
            IHubContext<HubPro, IHubPro> hub,
            IMerchantContract merchantContract)
        {
            _openContracr = openContracr;
            _hubContext = hub;
            _merchantContract = merchantContract;
        }

        /// <summary>
        /// 为指定客户对指定订单分配收款号
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleInfo]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("为订单分配收款号")]
        public async Task<AjaxResult> PortionOrder(PortionInDto dto)
        {
            Check.NotNull(dto, nameof(dto));
            var res = await _openContracr.PortionOrder(dto);
            return res.ToAjaxResult();
        }


        [HttpPost]
        [ModuleInfo]
        [Description("获取订单信息")]
        public async Task<AjaxResult> ReadOrderInfo(ReadorderDto dto)
        {
            Check.NotNull(dto,nameof(dto));
            // 去重
            dto.Pops = dto.Pops.Distinct().ToArray();
            // 查询订单信息
            var order = await _merchantContract.Orderses.Include(o => o.ArticleAssort).Include(o => o.Channel)
                .Where(o => o.Status < 2)
                .FirstOrDefaultAsync(o => o.Id == dto.Id);
            if (order == null)
            {
                return new OperationResult<object>(OperationResultType.QueryNull, "订单状态异常,请重新提交").ToAjaxResult();
            }
            // 未分配账号
            if (order.ArticleAssort == null)
            {
                return new OperationResult<object>(OperationResultType.Error, "系统正在加紧处理中").ToAjaxResult();
            }
            // 已经分配了 输出数据
            var extraDictionary = order.ArticleAssort.Extra.ToIDictionary();
            // 把需要的数据 遍历进输出对象
            var dicti = new Dictionary<string, object>();
            foreach (string key in dto.Pops)
            {
                if (extraDictionary.ContainsKey(key))
                {
                    dicti.TryAdd(key, extraDictionary[key]);
                }
            }
            // 添加默认需要输出的数据
            dynamic m = new ExpandoObject();
            m.CreatedAmount = order.CreatedAmount;
            m.CreatedTime = order.CreatedTime;
            m.ClientId = order.ClientId;
            m.ClientIp = order.ClientIp;
            m.Channel = order.Channel.Name;
            m.OutTime = order.CreatedTime + TimeSpan.FromMinutes(10);
            m.Dic = dicti;
            return new OperationResult<object>(OperationResultType.Success, "查询成功", m).ToAjaxResult();
        }

        [HttpPost]
        [ModuleInfo]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("创建订单接口")]
        public async Task<AjaxResult> CreateOrder(OrderDto dto)
        {
            OperationResult<OrderOutDto> result = await _openContracr.CreateOrder(dto);
            return result.ToAjaxResult();
        }

        [HttpGet]
        [ModuleInfo]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("测试创建订单接口")]
        public async Task<AjaxResult> TestOrder()
        {
            var t = GetTimeStamp();
            var test = new OrderDto();
            test.MerchantId = 2;
            test.AsynUrl = "http://localhost:7003/openapi/asyn";
            test.ClientIp = "127.0.0.1";
            test.Code = "Alipay";
            test.OrderId = Guid.NewGuid().ToString();
            test.CreatedAmount = 1000;
            test.Time = t;
            var signString = SignString(test);
            var merchantKey = "4f5f0b40fc4c42c1b033bc9ff5705de0";
            var hmd5 = HmacMd5String($"{signString}{merchantKey}", test.Time);
            test.Sign = hmd5;
            OperationResult<OrderOutDto> result = await _openContracr.CreateOrder(test);
            if (result.Succeeded)
            {
                await _hubContext.Clients.All.Order(new OrderOut
                {
                    Action = 1,
                    Amount = test.CreatedAmount,
                    CreatedTime = DateTime.Now,
                    OrderId = result.Data.SysId
                });
            }

            return result.ToAjaxResult();
        }



        private long GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);
        }

        //HmacMd5 签名!
        private string HmacMd5String(string str, object password)
        {
            using HMACMD5 provider = new HMACMD5(Encoding.UTF8.GetBytes(Convert.ToString(password)));
            byte[] hashedPassword = provider.ComputeHash(Encoding.UTF8.GetBytes(str));
            StringBuilder displayString = new StringBuilder();
            foreach (var t in hashedPassword)
            {
                displayString.Append(t.ToString("X2"));
            }
            return displayString.ToString().ToUpper();
        }

        //通用的取签名文本,最后带&号
        private string SignString<T>(T dto)
        {

            StringBuilder sbBuilder = new StringBuilder();
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            Type t = dto.GetType();
            foreach (var propertyInfo in t.GetProperties())
            {
                if (propertyInfo.Name.Equals("Sign") || propertyInfo.Name.Equals("Id")) continue;
                var v = propertyInfo.GetValue(dto, null);
                if (v == null) continue;
                var str = Convert.ToString(v);
                if (!string.IsNullOrEmpty(str))
                {
                    dictionary.Add(propertyInfo.Name, str);
                }
            }
            //排序
            dictionary = dictionary.OrderBy(d => d.Key).ToDictionary(d => d.Key, d => d.Value);
            foreach (var row in dictionary)
            {
                sbBuilder.AppendFormat("{0}={1}&", row.Key, row.Value);
            }
            return sbBuilder.ToString();
        }



    }

    public class ReadorderDto
    {
        public string ClientId { get; set; }

        /// <summary>
        /// 读取的参数列表
        /// </summary>
        public string[] Pops
        {
            get;
            set;
        }
        /// <summary>
        /// 订单Id
        /// </summary>
        public Guid Id { get; set; }
    }

}
