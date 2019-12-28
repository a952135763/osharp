using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using KaPai.Pay.Identity;
using KaPai.Pay.Merchant;
using KaPai.Pay.OpenApi.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OSharp.Data;
using System.Linq;
using System.Security.Cryptography;
using KaPai.Pay.Channel;
using KaPai.Pay.Merchant.Entities;
using Microsoft.JSInterop.Infrastructure;
using OSharp.Entity;
using OSharp.Extensions;
using Hangfire;
using KaPai.Pay.My;
using KaPai.Pay.OpenApi.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OSharp.Dependency;
using OSharp.Exceptions;
using StackExchange.Redis;

namespace KaPai.Pay.OpenApi
{
    public partial class OpenApi : IOpenContracr
    {
        protected readonly IMerchantContract Merchant;
        protected readonly ILogger<OpenApi> Logger;
        protected readonly IChannelContract Channel;
        protected readonly IRepository<Orders, Guid> OrderRepository;
        protected readonly IServiceProvider Provider;
        protected readonly IDatabase Database;

        public OpenApi(IMerchantContract merchant,
            ILoggerFactory logger,
            IChannelContract iContract,
            IRepository<Orders, Guid> orderRepository,
            IServiceProvider provider,
            IDatabase database)
        {
            Merchant = merchant;
            Logger = logger.CreateLogger<OpenApi>();
            Channel = iContract;
            OrderRepository = orderRepository;
            Provider = provider;
            Database = database;
        }

        public async Task<OperationResult<OrderOutDto>> CreateOrder(OrderDto dto)
        {
            dto.Validate();
            //验证订单生成时间
            var payDateTime = StampToDateTime(dto.Time);
            var span = DateTime.Now - payDateTime;
            if (span.TotalSeconds > 3 * 60)
            {
                return new OperationResult<OrderOutDto>(OperationResultType.ValidError, "订单时间间隔过长");
            }

            if (!dto.ClientIp.IsIpAddress())
            {
                return new OperationResult<OrderOutDto>(OperationResultType.ValidError, "ClientIp 必须为Ip文本");

            }

            //签名验证开始
            var merchantKey = Merchant.MerchantExtras.Where(p => p.UserId == dto.MerchantId).Select(p => p.Key).FirstOrDefault();
            if (string.IsNullOrEmpty(merchantKey))
            {
                return new OperationResult<OrderOutDto>(OperationResultType.ValidError, "错误商户ID");
            }
            var signString = SignString(dto);
            var hmd5 = HmacMd5String($"{signString}{merchantKey}", dto.Time);
            if (dto.Sign.ToUpper() != hmd5)
            {
                return new OperationResult<OrderOutDto>(OperationResultType.ValidError, "签名错误");
            }

            //验证通道全局开关
            var channel = Channel.Channelses.FirstOrDefault(p => p.Code == dto.Code);
            if (channel == null || channel.Status == 0)
            {
                return new OperationResult<OrderOutDto>(OperationResultType.Error, "系统通道关闭");
            }

            //通道限额验证
            if (dto.CreatedAmount < channel.Minimum)
            {
                return new OperationResult<OrderOutDto>(OperationResultType.Error, $"通道金额最低限制为:{channel.Minimum}");
            }
            if (dto.CreatedAmount > channel.Maxmum)
            {
                return new OperationResult<OrderOutDto>(OperationResultType.Error, $"通道金额最高限制为:{channel.Maxmum}");
            }

            //验证通道用户开关
            var userchannel = Channel.UserChannels.Where(p => p.UserId == dto.MerchantId).FirstOrDefault(p => p.ChannelId == channel.Id);

            if (userchannel == null || userchannel.Status == 0)
            {
                return new OperationResult<OrderOutDto>(OperationResultType.Error, "商户通道关闭");
            }
            //订单ID重复验证
            var hasorder = await Merchant.CheckOrdersExists(p => p.Orderid == dto.OrderId && p.UserId == dto.MerchantId);
            if (hasorder)
            {
                return new OperationResult<OrderOutDto>(OperationResultType.Error, "订单Id发生重复错误");
            }
            // todo:通道切换的代码



            var orders = new Orders
            {
                Status = 0,
                ClientIp = dto.ClientIp,
                AsynUrl = dto.AsynUrl,
                ChannelId = channel.Id,
                CreatedAmount = dto.CreatedAmount,
                CreatedTime = DateTime.Now,
                Orderid = dto.OrderId,
                Remark = dto.Remark,
                UserId = dto.MerchantId,
            };
            var fullname = $"KaPai.Pay.OpenApi.Base.{channel.Code}";
            var t = Type.GetType(fullname);
            if (t == null)
            {
                return new OperationResult<OrderOutDto>(OperationResultType.Error, "此通道尚未实现,敬请期待！");
            }
            INterfaceChannel obj = (INterfaceChannel)Provider.GetService(t);
            OperationResult<OrderOutDto> res = await obj.CreateOrder(orders);
            return res;

        }

        /// <summary>
        /// 分配收款账号
        /// </summary>
        /// <param name="indto"></param>
        /// <returns></returns>
        public async Task<OperationResult<PortionOrderOut>> PortionOrder(PortionInDto indto)
        {
            // 把ClientId转换为 Guid 无法转换则获取一条新的
            Guid ClientId = default(Guid);
            if (!Guid.TryParse(indto.ClientId, out ClientId))
            {
                ClientId = Guid.NewGuid();
                indto.ClientId = ClientId.ToString();
            }
            var oreder = await OrderRepository.Query(o => o.Id == indto.Id)
                .Where(o => o.Status == 0)
                .FirstOrDefaultAsync(o => string.IsNullOrEmpty(o.ClientId));
            if (oreder == null)
            {
                return new OperationResult<PortionOrderOut>(OperationResultType.Error, "无法处理订单",new PortionOrderOut(){ ClientId = indto.ClientId });
            }

            try
            {

               // 通道自己处理
               var res = await oreder.Channel.GetChannelBase().ArticleAssortHandle(oreder, indto);
                // 更新客户端ID
                oreder.ClientId = indto.ClientId;
               await OrderRepository.UpdateAsync(oreder);
               return res;
            }
            catch (OsharpException)
            {
                return new OperationResult<PortionOrderOut>(OperationResultType.Error, "系统错误", new PortionOrderOut() { ClientId = indto.ClientId });
            }
        }


        /// <summary>
        /// 获取目前时间搓
        /// </summary>
        /// <returns></returns>
        public static long GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);
        }

        /// <summary>
        /// HmacMd5  签名生成
        /// </summary>
        /// <param name="str">代签名文本</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public static string HmacMd5String(string str, object password)
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

        /// <summary>
        /// 时间戳转换成datetime
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static DateTime StampToDateTime(long time)
        {
            var date = (new DateTime(1970, 1, 1, 0, 0, 0, 0)) + TimeSpan.FromSeconds(time);
            return date.ToLocalTime();
        }


        /// <summary>
        /// 通用的取对象签名
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="dto">对象实例</param>
        /// <param name="shield">屏蔽的字段</param>
        /// <returns></returns>
        public static string SignString<T>(T dto, string[] shield = null)
        {
            if (shield == null) shield = new[] { "Sign", "Id" };
            StringBuilder sbBuilder = new StringBuilder();
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            Type t = dto.GetType();
            foreach (var propertyInfo in t.GetProperties())
            {
                if (propertyInfo.Name.IsIn(shield)) continue;
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

        /// <summary>
        /// 获取 INterfaceChannel 进行操作
        /// 注意使用 IServiceProvider.CreateScope 创建的作用域需要传递第二个参数
        /// </summary>
        /// <param name="channelCode">通道代码</param>
        /// <param name="provider">IServiceProvider.CreateScope 创建的作用域</param>
        /// <returns></returns>
        public static INterfaceChannel GetINterfaceChannel(string channelCode,IServiceProvider provider = null)
        {
            if (string.IsNullOrEmpty(channelCode)) return null;
            var key = $"KaPai.Pay.OpenApi.Base.{channelCode}";
            var @type = Type.GetType(key);
            if (@type == null)
            {
                throw new OsharpException($"无法获取:{key}");
            }

            if (provider == null)
            {
                return (INterfaceChannel)ServiceLocator.Instance.GetService(@type);
            }
            else
            {
                return (INterfaceChannel)provider.GetService(@type);
            }

            
        }
    }
}
