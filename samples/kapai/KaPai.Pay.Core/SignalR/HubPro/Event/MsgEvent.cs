using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using KaPai.Pay.Channel;
using KaPai.Pay.Merchant;
using KaPai.Pay.Merchant.Entities;
using KaPai.Pay.My;
using KaPai.Pay.Provide;
using MessagePack;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using OSharp.EventBuses;
using Microsoft.Extensions.DependencyInjection;
using OSharp.Json;
using OSharp.Security.Claims;
using OSharp.Entity;

namespace KaPai.Pay.SignalR.HubPro.Event
{
    public class MsgEvent : EventHandlerBase<MsgDataDto>
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly IHubContext<HubPro, IHubPro> _hubContext;


        /// <summary>
        /// IProvideContract 契约
        /// </summary>
        private IProvideContract Provide => _serviceProvider.GetService<IProvideContract>();


        private IMerchantContract Merchant => _serviceProvider.GetService<IMerchantContract>();

        private IRepository<Orders, Guid> OrdersRepository => _serviceProvider.GetService<IRepository<Orders, Guid>>();


        /// <summary>
        /// 这里能正常依赖注入 注入服务对象  注入HUB 处理对象
        /// </summary>
        public MsgEvent(IServiceProvider serviceProvider, IHubContext<HubPro, IHubPro> hub)
        {
            _serviceProvider = serviceProvider;
            _hubContext = hub;
        }

        /// <summary>
        /// 不处理同步事件
        /// </summary>
        /// <param name="eventData"></param>
        public override void Handle(MsgDataDto eventData)
        {
            throw new NotImplementedException();
        }

        public override Task HandleAsync(MsgDataDto eventData,
            CancellationToken cancelToken = new CancellationToken())
        {
            try
            {
                var t = this.GetType();
                MethodInfo comMethod = t.GetMethod(eventData.Cmd);
                return (Task) comMethod.Invoke(this, new object[] {eventData});
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return Task.CompletedTask;
        }


        /// <summary>
        /// 客户发送确认收款信息!
        /// </summary>
        /// <param name="eventData"></param>
        /// <returns></returns>
        public async Task Arrival(MsgDataDto eventData)
        {
            GrabOrderInData indata = eventData.Data as GrabOrderInData;

            if (eventData.CallerContext.Items.TryGetValue("ArticleAssortId", out object ArticleAssortId))
            {
                var Article = Provide.ArticleEntitieses
                    .FirstOrDefault(a => a.Id == Guid.Parse(ArticleAssortId.ToString()));
                // 查找是否有这个账号
                if (Article != null)
                {
                    Orders order = OrdersRepository.Query(o => o.ArticleAssortId == Article.Id)
                        .Where(o => o.Status == 1).Where(o => o.ChannelId == eventData.ChennelGuid)
                        .FirstOrDefault(o => o.Id == indata.OrderId);
                    if (order != null && await Merchant.OrderPayment(order.Id.ToString(), order.CreatedAmount))
                    {
                        // 返回确认成功信息
                        await _hubContext.Clients.Client(eventData.CallerContext.ConnectionId)
                            .ArrivalReturn(new ArrivalOut
                            {
                                Action = 1,
                                OrderId = order.Id
                            });
                        return;
                    }
                }
            }

            // 返回确认失败信息
            await _hubContext.Clients.Client(eventData.CallerContext.ConnectionId)
                .ArrivalReturn(new ArrivalOut
                {
                    Action = 0,
                    OrderId = indata.OrderId
                });
        }


        /// <summary>
        /// 用户抢单操作,抢单失败让用户删除订单
        /// </summary>
        /// <param name="eventData"></param>
        /// <returns></returns>
        public async Task GrabOrder(MsgDataDto eventData)
        {
            GrabOrderInData indata = eventData.Data as GrabOrderInData;
            if (eventData.CallerContext.Items.TryGetValue("ArticleAssortId", out object ArticleAssortId))
            {
                var article = Provide.ArticleEntitieses.Where(a => a.Id == Guid.Parse(ArticleAssortId.ToString()))
                    .FirstOrDefault(a => a.Status == 1);
                if (article != null)
                {
                    if (await CheckArticle(eventData))
                    {
                        Orders order = OrdersRepository.Query(o => o.ArticleAssortId == null).Where(o => o.Status == 0)
                            .Where(o => o.ChannelId == eventData.ChennelGuid)
                            .FirstOrDefault(o => o.Id == indata.OrderId);
                        if (order != null)
                        {
                             // await Provide.ProvidePointChange(article.UserId, 10000, Guid.NewGuid().ToString(), "积分充值","测试充值积分");

                            // 判断可用积分是否大于订单积分 并且冻结积分成功
                            if (await Provide.ProvideGetAvailable(article.UserId) > order.CreatedAmount)
                            {
                               
                                order.Status = 1;

                                //获取通道
                                var channel = _serviceProvider.GetService<IChannelContract>().Channelses
                                    .FirstOrDefault(c => c.Id == eventData.ChennelGuid);
                                // 调用
                                await channel.GetChannelBase(_serviceProvider).OrderSetArticle(order, Guid.Parse(ArticleAssortId.ToString()));
                                try
                                {
                                    var count = await OrdersRepository.UpdateAsync(order);
                                    // 抢单成功 并且 分冻结成功
                                    if (count > 0 && await Provide.ProvideOrderPoint(article.UserId, order))
                                    {
                                        // 提交事务!
                                        OrdersRepository.UnitOfWork.Commit();
                                        // 此号抢单成功! 发送抢单成功消息! 同时要广播 删除此订单
                                        await _hubContext.Clients.Client(eventData.CallerContext.ConnectionId)
                                            .OrderReturn(new OrderReturn()
                                            {
                                                Action = 1,
                                                OrderId = order.Id,
                                                Amount = order.CreatedAmount,
                                                CreatedTime = order.CreatedTime,
                                                OutTime = order.CreatedTime + TimeSpan.FromMinutes(10),
                                                Article = $"名称:{article.Name}-{article.Remarks}"
                                            });
                                        return;
                                    }
                                }
                                catch (DbUpdateConcurrencyException e)
                                {
                                    // 乐观并发失败... 已经被人抢了
                                }
                            }
                            else
                            {
                                await _hubContext.Clients.Client(eventData.CallerContext.ConnectionId).OrderReturn(
                                    new OrderReturn()
                                    {
                                        Action = 2,
                                        OrderId = indata.OrderId,
                                        Amount = 0,
                                        CreatedTime = DateTime.Now
                                    });
                                return;
                            }
                        }
                    }
                }
                else
                {
                    // 2为此账号状态不正确 无法抢单
                    await _hubContext.Clients.Client(eventData.CallerContext.ConnectionId).OrderReturn(new OrderReturn()
                    {
                        Action = 2,
                        OrderId = indata.OrderId,
                        Amount = 0,
                        CreatedTime = DateTime.Now
                    });
                    return;
                }
            }

            // 0 抢单失败!
            await _hubContext.Clients.Client(eventData.CallerContext.ConnectionId).OrderReturn(new OrderReturn()
            {
                Action = 0,
                OrderId = indata.OrderId,
                Amount = 0,
                CreatedTime = DateTime.Now
            });
        }


        /// <summary>
        /// 停止监听订单!暂时退出订阅组就行了
        /// </summary>
        /// <param name="eventData"></param>
        /// <returns></returns>
        public async Task StopGrab(MsgDataDto eventData)
        {
            //加入的分组名.. 
            string groupname = "all";
            if (HubPro.RemoveItemsGroup(eventData.CallerContext.Items, groupname))
            {
                //从分组 里面删除用户
                await _hubContext.Groups.RemoveFromGroupAsync(eventData.CallerContext.ConnectionId, groupname);



                // 删除抢单监听账号
                eventData.CallerContext.Items.Remove("ArticleAssortId",out object articleEntitiesid);

                var articleEntities = Provide.ArticleEntitieses.Where(a => a.Id == Guid.Parse(articleEntitiesid.ToString()))
                    .Include(a => a.ArticleAssort).Include(a => a.ArticleAssort.Channel).FirstOrDefault();

                await articleEntities.GetChannelBase(_serviceProvider).ArticleOnline(articleEntities, false);

                await _hubContext.Clients.Client(eventData.CallerContext.ConnectionId).StopGrab("Ok");

                return;
            }

            await _hubContext.Clients.Client(eventData.CallerContext.ConnectionId).StopGrab("已经停止了!");
        }


        /// <summary>
        /// 开始监听订单!
        /// </summary>
        /// <param name="eventData"></param>
        /// <returns></returns>
        public async Task StartGrabOrder(MsgDataDto eventData)
        {
            SatrtGrabOrderInData inData = eventData.Data as SatrtGrabOrderInData;
            var articleEntities = Provide.ArticleEntitieses
                .Where(e => e.Id == inData.ArticleId)
                .Where(e => e.ArticleAssortId == inData.ArticleAssortId)
                .FirstOrDefault(e => e.Status == 1);
            if (articleEntities == null)
            {
                await _hubContext.Clients.Client(eventData.CallerContext.ConnectionId).GrabReturn("账号状态异常,请更换账号");
            }
            else
            {
                //加入某个分组
                string groupname = "all";
                if (HubPro.AddItemsGroups(eventData.CallerContext.Items, groupname))
                {
                    eventData.CallerContext.Items.TryAdd("ArticleAssortId", inData.ArticleId.ToString());

                    // 设置账号在线
                    await articleEntities.GetChannelBase(_serviceProvider).ArticleOnline(articleEntities, true);

                    await _hubContext.Groups.AddToGroupAsync(eventData.CallerContext.ConnectionId, groupname);
                    await _hubContext.Clients.Client(eventData.CallerContext.ConnectionId).GrabReturn("Ok");
                }
                else
                {
                    await _hubContext.Clients.Client(eventData.CallerContext.ConnectionId).GrabReturn("你已经监听过了");
                }
            }
        }


        /// <summary>
        /// 用户认证成功,获取分类和开启的账号！发送数据 
        /// </summary>
        /// <param name="eventData"></param>
        /// <returns></returns>
        public async Task Open(MsgDataDto eventData)
        {
            // 监测是否有未完成订单! 没有返回 true
            if (await CheckArticle(eventData))
            {
                var article = Provide.ArticleEntitieses
                    .Where(a => a.UserId == eventData.CallerContext.User.Identity.GetUserId<int>())
                    .Where(a => a.Status == 1)
                    .Where(a => a.ArticleAssort.ChannelId == eventData.ChennelGuid)
                    .Select(a => new ArticleEntitiesesOutData
                    {
                        Id = a.Id,
                        ArticleAssortId = a.ArticleAssortId,
                        Name = $"{a.Extra.RootElement.GetProperty("zhanghao").GetString()}({a.Name}):{a.Remarks}",
                    }).ToArray();

                var articleAssorts = Provide.ArticleAssorts.Where(a => a.ChannelId == eventData.ChennelGuid)
                    .Select(a => new ArticleOutData {Name = a.Name, Id = a.Id}).ToArray();
                await _hubContext.Clients.Client(eventData.CallerContext.ConnectionId).Open(articleAssorts, article);
            }
        }


        /// <summary>
        /// 监测是否有未完成订单，有就发送显示订单详情
        /// </summary>
        /// <returns></returns>
        protected async Task<bool> CheckArticle(MsgDataDto eventData)
        {
            var order = Merchant.Orderses
                .Join(Provide.ArticleEntitieses, a => a.ArticleAssortId, o => o.Id,
                    (o, a) => new {o, a})
                .Where(o => o.o.Status < 2)
                .FirstOrDefault(o => o.a.ArticleAssort.ChannelId == eventData.ChennelGuid);
            if (order != null)
            {
                eventData.CallerContext.Items.TryAdd("ArticleAssortId", order.a.Id.ToString());
                await _hubContext.Clients.Client(eventData.CallerContext.ConnectionId).OrderReturn(new OrderReturn()
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
            return true;
        }


    }
}