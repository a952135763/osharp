using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using KaPai.Pay.SignalR.HubPro.Event;

namespace KaPai.Pay.SignalR.HubPro
{

    //SignalR 单例模块接口 方便core核心 调用
    public interface IHubPro
    {

        /// <summary>
        /// 从客户端关闭连接
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        Task HubClose(string ex);

        /// <summary>
        /// 下发分类和账号
        /// </summary>
        /// <param name="articleOut"></param>
        /// <param name="articleEntitiesesOutData"></param>
        /// <returns></returns>
        Task Open(ArticleOutData[] articleOut, ArticleEntitiesesOutData[] articleEntitiesesOutData);

        /// <summary>
        /// 开始监听订单消息返回
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        Task GrabReturn(string status);

        /// <summary>
        /// 停止推送订单消息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        Task StopGrab(string msg);
       
        /// <summary>
        /// 推送订单消息 
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        Task Order(OrderOut order);

        /// <summary>
        /// 发送抢单返回消息！ 抢单成功的订单信息 或者 抢单失败消息......
        /// </summary>
        /// <param name="res"></param>
        /// <returns></returns>
        Task OrderReturn(OrderReturn res);

        /// <summary>
        /// 发送订单确认返回信息
        /// </summary>
        /// <param name="arrivalOut"></param>
        /// <returns></returns>
        Task ArrivalReturn(ArrivalOut arrivalOut);


    }
}
