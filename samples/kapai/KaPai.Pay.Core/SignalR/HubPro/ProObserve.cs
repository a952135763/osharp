using KaPai.Pay.Channel;
using KaPai.Pay.Channel.Entities;
using KaPai.Pay.Merchant;
using KaPai.Pay.Merchant.Entities;
using KaPai.Pay.My;
using KaPai.Pay.OpenApi.Base;
using KaPai.Pay.Provide;
using KaPai.Pay.SignalR.HubPro.Event;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using OSharp.Entity;
using OSharp.Exceptions;
using OSharp.Security.Claims;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace KaPai.Pay.SignalR.HubPro
{
    public class ProObserve
    {

        protected readonly IServiceProvider ServiceProvider;
        protected readonly HubPro Hub;


        private IMerchantContract Merchant => ServiceProvider.GetService<IMerchantContract>();
        private IProvideContract Provide => ServiceProvider.GetService<IProvideContract>();
        private IChannelContract Channel => ServiceProvider.GetService<IChannelContract>();
        private IDistributedCache Cache => ServiceProvider.GetService<IDistributedCache>();
        private IRepository<Orders, Guid> OrdersRepository => ServiceProvider.GetService<IRepository<Orders, Guid>>();
        private IDatabase Database => ServiceProvider.GetService<IDatabase>();

        // 通道的 INterfaceChannel 对象
        private INterfaceChannel _channelBase;

        public ProObserve(HubPro hub, IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            Hub = hub;
        }

        public async Task Run(NowData data)
        {
            Channels channel = Channel.Channelses.FirstOrDefault(c => c.Id == Hub.ChnnelGuid);
            if (channel == null)
            {
                throw new OsharpException($"未找到对应通道:{Hub.ChnnelGuid}");
            }
            _channelBase = channel.GetChannelBase(ServiceProvider);
            MyDynamic dataobj = null;
            if (data.Data != null)
            {
                dataobj = new MyDynamic(new ConcurrentDictionary<string, object>(data.Data));
            }
            // 处理各种事件
            Task<bool> res = data.Cmd switch
            {
                Cmd.Open => OnOpen(dataobj),
                Cmd.Close => OnClose(dataobj),
                Cmd.StartGrab => OnStartGrab(dataobj),
                Cmd.StopGrab => OnStopGrab(dataobj),
                Cmd.GrabOrder => OnGrabOrder(dataobj),
                Cmd.Arrival => OnArrival(dataobj),
                _ => throw new OsharpException($"无法处理此消息类型:{data.Cmd}"),
            };
            await res;
        }



        #region 事件触发

        private async Task<bool> OnOpen(dynamic data)
        {
            if (!await CheckArticle()) return false;
            var article = Provide.ArticleEntitieses
                .Where(a => a.UserId == Hub.Context.User.Identity.GetUserId<int>())
                .Where(a => a.Status == 1)
                .Where(a => a.ArticleAssort.ChannelId == Hub.ChnnelGuid)
                .Select(a => new ArticleEntitiesesOutData
                {
                    Id = a.Id,
                    ArticleAssortId = a.ArticleAssortId,
                    Name = $"{a.Extra.RootElement.GetProperty("zhanghao").GetString()}({a.Name}):{a.Remarks}",
                }).ToArray();
            var articleAssorts = Provide.ArticleAssorts.Where(a => a.ChannelId == Hub.ChnnelGuid)
                .Select(a => new ArticleOutData { Name = a.Name, Id = a.Id }).ToArray();
            await Hub.Clients.Client(Hub.Context.ConnectionId).Open(articleAssorts, article);
            return true;
        }

        private async Task<bool> OnClose(dynamic data)
        {
            // 判断是否设置了 监听账号
            if (Hub.Context.Items.ContainsKey("ArticleAssortId"))
            {
                Hub.Context.Items.Remove("ArticleAssortId", out object articleEntitiesid);
                // 把监听账号设置为不在线
                var articleEntities =
                    Provide.ArticleEntitieses.First(a => a.Id == Guid.Parse(articleEntitiesid.ToString()));
                await _channelBase.ArticleOnline(articleEntities, false);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 开始监听订单
        /// </summary>
        /// <returns></returns>
        private async Task<bool> OnStartGrab(dynamic data)
        {

            SatrtGrabOrderInData inData = data;
            var articleEntities = Provide.ArticleEntitieses
                .Where(e => e.Id == inData.ArticleId)
                .Where(e => e.ArticleAssortId == inData.ArticleAssortId)
                .Include(e => e.ArticleAssort.Channel)
                .FirstOrDefault(e => e.Status == 1);

            if (articleEntities == null)
            {
                await Hub.Clients.Client(Hub.Context.ConnectionId).GrabReturn("账号状态异常,请更换账号");
            }
            else
            {
                if (Hub.Context.Items.TryAdd("ArticleAssortId", inData.ArticleId.ToString()))
                {
                    var channelBase = articleEntities.GetChannelBase(ServiceProvider);
                    await channelBase.ArticleOnline(articleEntities, true);

                    // 设置映射 监听账号 和连接ID的映射
                    await Database.HashSetAsync("HubPro", inData.ArticleId.ToString(), Hub.Context.ConnectionId);

                    

                    await Hub.Clients.Client(Hub.Context.ConnectionId).GrabReturn("Ok");
                }
                else
                {
                    await Hub.Clients.Client(Hub.Context.ConnectionId).GrabReturn("你已经监听过了");
                }
            }
            return true;
        }

        /// <summary>
        /// 停止监听订单
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private async Task<bool> OnStopGrab(dynamic data)
        {
            // 删除抢单监听账号
            if (Hub.Context.Items.Remove("ArticleAssortId", out object articleEntitiesid))
            {
                var articleEntities = Provide.ArticleEntitieses.Where(a => a.Id == Guid.Parse(articleEntitiesid.ToString()))
                    .Include(a => a.ArticleAssort).Include(a => a.ArticleAssort.Channel).FirstOrDefault();

                await articleEntities.GetChannelBase(ServiceProvider).ArticleOnline(articleEntities, false);

                await Hub.Clients.Client(Hub.Context.ConnectionId).StopGrab("Ok");

                return true;
            }
            await Hub.Clients.Client(Hub.Context.ConnectionId).StopGrab("已经停止了!");
            return true;
        }

        /// <summary>
        /// 抢对应订单
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private async Task<bool> OnGrabOrder(dynamic data)
        {

            GrabOrderInData indata = data;
            if (Hub.Context.Items.TryGetValue("ArticleAssortId", out object ArticleAssortId))
            {
                var article = Provide.ArticleEntitieses.Where(a => a.Id == Guid.Parse(ArticleAssortId.ToString()))
                    .FirstOrDefault(a => a.Status == 1);
                if (article != null)
                {
                    if (await CheckArticle())
                    {
                        Orders order = OrdersRepository.Query(o => o.ArticleAssortId == null).Where(o => o.Status == 0)
                            .Where(o => o.ChannelId == Hub.ChnnelGuid)
                            .FirstOrDefault(o => o.Id == indata.OrderId);
                        if (order != null)
                        {

                            // 判断可用积分是否大于订单积分
                            if (await Provide.ProvideGetAvailable(article.UserId) > order.CreatedAmount)
                            {
                                order.Status = 1;
                                // 调用
                                await _channelBase.OrderSetArticle(order, Guid.Parse(ArticleAssortId.ToString()));
                                try
                                {
                                    var count = await OrdersRepository.UpdateAsync(order);
                                    // 抢单成功 并且 分冻结成功
                                    if (count > 0 && await Provide.ProvideOrderPoint(article.UserId, order))
                                    {

                                        // 提交事务!
                                        OrdersRepository.UnitOfWork.Commit();
                                        // 此号抢单成功! 发送抢单成功消息! 同时要广播 删除此订单
                                        await Hub.Clients.Client(Hub.Context.ConnectionId)
                                            .OrderReturn(new OrderReturn()
                                            {
                                                Action = 1,
                                                OrderId = order.Id,
                                                Amount = order.CreatedAmount,
                                                CreatedTime = order.CreatedTime,
                                                OutTime = order.CreatedTime + TimeSpan.FromMinutes(10),
                                                Article = $"名称:{article.Name}-{article.Remarks}"
                                            });
                                        return true;
                                    }
                                }
                                catch (DbUpdateConcurrencyException e)
                                {
                                    // 乐观并发失败... 已经被人抢了
                                }
                            }
                            else
                            {
                                await Hub.Clients.Client(Hub.Context.ConnectionId).OrderReturn(
                                    new OrderReturn()
                                    {
                                        Action = 2,
                                        OrderId = indata.OrderId,
                                        Amount = 0,
                                        CreatedTime = DateTime.Now
                                    });
                                return true;
                            }
                        }
                    }
                }
                else
                {
                    // 2为此账号状态不正确 无法抢单
                    await Hub.Clients.Client(Hub.Context.ConnectionId).OrderReturn(new OrderReturn()
                    {
                        Action = 2,
                        OrderId = indata.OrderId,
                        Amount = 0,
                        CreatedTime = DateTime.Now
                    });
                    return false;
                }
            }

            // 0 抢单失败!
            await Hub.Clients.Client(Hub.Context.ConnectionId).OrderReturn(new OrderReturn()
            {
                Action = 0,
                OrderId = indata.OrderId,
                Amount = 0,
                CreatedTime = DateTime.Now
            });
            return false;
        }

        private async Task<bool> OnArrival(dynamic data)
        {
            GrabOrderInData indata = data;
            if (Hub.Context.Items.TryGetValue("ArticleAssortId", out object ArticleAssortId))
            {
                var Article = Provide.ArticleEntitieses
                    .FirstOrDefault(a => a.Id == Guid.Parse(ArticleAssortId.ToString()));
                // 查找是否有这个账号
                if (Article != null)
                {
                    Orders order = OrdersRepository.Query(o => o.ArticleAssortId == Article.Id)
                        .Where(o => o.Status == 1).Where(o => o.ChannelId == Hub.ChnnelGuid)
                        .FirstOrDefault(o => o.Id == indata.OrderId);
                    if (order != null && await Merchant.OrderPayment(order.Id.ToString(), order.CreatedAmount))
                    {
                        // 返回确认成功信息
                        await Hub.Clients.Client(Hub.Context.ConnectionId)
                            .ArrivalReturn(new ArrivalOut
                            {
                                Action = 1,
                                OrderId = order.Id
                            });
                        return true;
                    }
                }
            }

            return false;
        }

        #endregion



        /// <summary>
        /// 监测是否有未完成订单，有就发送显示订单详情
        /// </summary>
        /// <returns>一切正常返回true</returns>
        protected async Task<bool> CheckArticle()
        {
            var order = Merchant.Orderses
                .Join(Provide.ArticleEntitieses, a => a.ArticleAssortId, o => o.Id,
                    (o, a) => new { o, a })
                .Where(o => o.o.Status < 2)
                .FirstOrDefault(o => o.a.ArticleAssort.ChannelId == Hub.ChnnelGuid);

            if (order == null) return true;

            Hub.Context.Items.TryAdd("ArticleAssortId", order.a.Id.ToString());
            await Hub.Clients.Client(Hub.Context.ConnectionId).OrderReturn(new OrderReturn()
            {
                Action = 1,
                OrderId = order.o.Id,
                Amount = order.o.CreatedAmount,
                CreatedTime = order.o.CreatedTime,
                OutTime = order.o.CreatedTime + TimeSpan.FromMinutes(10),
                Article = $"名称:{order.a.Name}-{order.a.Remarks}"
            });
            return false;
        }
    }
}
