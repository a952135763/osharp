using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KaPai.Pay.Provide;
using KaPai.Pay.Provide.Entities;
using KaPai.Pay.SignalR.HubPro.Event;
using MessagePack;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OSharp.Dependency;
using OSharp.Entity;
using OSharp.EventBuses;
using OSharp.Security.Claims;
using static System.String;

namespace KaPai.Pay.SignalR.HubPro
{
    public class HubPro : Hub<IHubPro>, ISingletonDependency
    {

        protected readonly IServiceProvider ServiceProvider;


        //填写通道ID
        public Guid ChnnelGuid = Guid.Parse("ed66ac76-15a7-4e10-b0ee-6f664495fb17");


        /// <summary>
        /// Hub 无法注入业务模块 使用事件总线来代理处理
        /// </summary>
        public HubPro( IServiceProvider serviceProvider)
        {
           
            ServiceProvider = serviceProvider;
        }

        [HubMethodName("News")]
        public async Task DoNews(NowData data)
        {
            // 消息处理
            await ServiceProvider.ExecuteScopedWorkAsync((p) => MsgTask(p, data));
        }

        private async Task MsgTask(IServiceProvider provider, NowData data)
        {
            var log = provider.GetService<ILoggerFactory>().CreateLogger<HubPro>();
            try
            {
                // 使用一个类来代理处理,比较方便
                await new ProObserve(this, provider).Run(data);
            }
            catch (Exception e)
            {
                log.Log(LogLevel.Error, e, $"处理消息发生错误:{e.Message}");
            }
        }


        public override async Task OnConnectedAsync()
        {
            //客户连接 身份验证客户是否供应商  然后查询是否拥有未处理订单  未处理就进入处理流程
            if (!Context.User.Identity.IsAuthenticated)
            {
                await Clients.Client(Context.ConnectionId).HubClose("not certified");
                Context.Abort();
                return;
            }
            var userid = Context.User.Identity.GetUserId<int>();
            await DoNews(new NowData()
            {
                Cmd = Cmd.Open,
                Data = new Dictionary<string, object> { { "Userid", userid } }
            });

        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userid = Context.User.Identity.GetUserId<int>();
            await DoNews(new NowData()
            {
                Cmd = Cmd.Close,
                Data = new Dictionary<string, object> { { "Userid", userid } }
            });

            /**
            if (Context.Items.ContainsKey("ArticleAssortId"))
            {
                Context.Items.Remove("ArticleAssortId", out object articleEntitiesid);
                // 设置为不在线
            }

            #region 掉线退出所有分组
            try
            {
                List<Task> waitList = new List<Task>();
                foreach (var groupName in GetItemsGroups(Context.Items))
                {
                    waitList.Add(Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName));
                }

                Task.WaitAll(waitList.ToArray());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            #endregion

            await base.OnDisconnectedAsync(exception);**/
        }






        /// <summary>
        /// 判断是否加入过分组,并且断连时 退出所有分组
        /// </summary>
        /// <param name="items"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool AddItemsGroups(IDictionary<object, object> items, string name)
        {
            if (items.ContainsKey("Groups"))
            {
                IDictionary<string, bool> grDictionary = items["Groups"] as ConcurrentDictionary<string, bool>;
                return !grDictionary.ContainsKey(name) && grDictionary.TryAdd(name, true);
            }
            else
            {
                IDictionary<string, bool> grDictionary = new ConcurrentDictionary<string, bool>();
                return grDictionary.TryAdd(name, true) && items.TryAdd("Groups", grDictionary);
            }
        }

        public static bool RemoveItemsGroup(IDictionary<object, object> items, string name)
        {

            if (items.ContainsKey("Groups"))
            {
                IDictionary<string, bool> grDictionary = items["Groups"] as ConcurrentDictionary<string, bool>;
                return grDictionary.Remove(name, out _);
            }
            return false;

        }

        /// <summary>
        /// 获取已经入的分组
        /// </summary>
        /// <returns></returns>
        public static string[] GetItemsGroups(IDictionary<object, object> items)
        {
            if (!items.ContainsKey("Groups")) return Array.Empty<string>();

            IDictionary<string, bool> grDictionary = items["Groups"] as ConcurrentDictionary<string, bool>;
            return grDictionary.Keys.ToArray();
        }

    }


    [MessagePackObject]
    public class NowData
    {
        [Key("Cmd")]
        public Cmd Cmd { get; set; }
        [Key("Data")]
        public Dictionary<string, object> Data { get; set; }
        public override string ToString()
        {
            return $"Cmd:{Cmd},Data[{Join(',', Data.Select(a => $"{a.Key}:{a.Value}"))}]";
        }
    }

    public enum Cmd
    {
        Open,
        Close,
        StartGrab,
        StopGrab,
        GrabOrder,
        Arrival,
    }
}
