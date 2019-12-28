using System;
using System.Collections.Generic;
using System.Text;
using MessagePack;
using Microsoft.AspNetCore.SignalR;
using OSharp.EventBuses;

namespace KaPai.Pay.SignalR.HubPro.Event
{
    public class MsgDataDto : EventDataBase
    {

        public string Cmd { get; set; }

        public object Data { get; set; }

        public Type DataType { get; set; }

        public HubCallerContext CallerContext { get; set; }

        public Guid ChennelGuid { get; set; }
    }


    public class GrabOrderInData
    {
        //抢单订单号
        public Guid OrderId { get; set; }


        //抢单时间
        public long Time { get; set; }
    }

    public class SatrtGrabOrderInData
    {
        public Guid ArticleAssortId { get; set; }
        public Guid ArticleId { get; set; }

    }

    [MessagePackObject]
    public class ArticleOutData
    {
        [Key("value")]
        public Guid Id { get; set; }

        [Key("label")]
        public string Name { get; set; }

    }


    [MessagePackObject]
    public class ArticleEntitiesesOutData
    {

        [Key("value")]
        public Guid Id { get; set; }

        [Key("label")]
        public string Name { get; set; }

        [Key("pid")]
        public Guid ArticleAssortId { get; set; }
    }


    /// <summary>
    /// 抢单返回 类型
    /// </summary>
    [MessagePackObject]
    public class OrderReturn
    {

        /// <summary>
        /// 动作 0为删除  1为增加
        /// </summary>
        [Key("action")]
        public int Action { get; set; }

        /// <summary>
        /// 订单Id
        /// </summary>
        [Key("orderid")]
        public Guid OrderId { set; get; } = Guid.Empty;

        /// <summary>
        /// 订单金额
        /// </summary>
        [Key("amount")]
        public long Amount { get; set; } = 0;

        /// <summary>
        /// 订单创建时间
        /// </summary>
        [Key("createdTime")]
        public DateTime CreatedTime { get; set; } = DateTime.Now;

        [Key("outTime")] 
        public DateTime OutTime { get; set; } = DateTime.Now;

        [Key("article")] 
        public string Article { get; set; } = "";
    }

    [MessagePackObject]
    public class OrderOut
    {
        /// <summary>
        /// 动作 0为删除  1为增加
        /// </summary>
        [Key("action")]
        public int Action { get; set; }

        /// <summary>
        /// 订单Id
        /// </summary>
        [Key("orderid")]
        public Guid OrderId { set; get; }

        /// <summary>
        /// 订单金额
        /// </summary>
        [Key("amount")]
        public long Amount { get; set; }

        /// <summary>
        /// 订单创建时间
        /// </summary>
        [Key("createdTime")]
        public DateTime CreatedTime { get; set; }
    }

    [MessagePackObject]
    public class ArrivalOut
    {
        /// <summary>
        /// 动作 0为删除  1为增加
        /// </summary>
        [Key("action")]
        public int Action { get; set; }

        /// <summary>
        /// 订单Id
        /// </summary>
        [Key("orderid")]
        public Guid OrderId { set; get; } = Guid.Empty;
    }
}
