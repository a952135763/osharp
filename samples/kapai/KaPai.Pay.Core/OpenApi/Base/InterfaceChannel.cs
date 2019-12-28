using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using KaPai.Pay.Merchant.Entities;
using KaPai.Pay.OpenApi.Dtos;
using KaPai.Pay.Provide.Entities;
using OSharp.Data;
using StackExchange.Redis;

namespace KaPai.Pay.OpenApi.Base
{
    /// <summary>
    /// 通道处理接口
    /// 所有通道类都必须实现此接口
    /// </summary>
    public interface INterfaceChannel
    {

        /// <summary>
        /// 通道创建订单调用
        /// </summary>
        /// <returns></returns>
        Task<OperationResult<OrderOutDto>> CreateOrder(Orders order);

        /// <summary>
        /// 通道选取收款号处理
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        Task<OperationResult<PortionOrderOut>> ArticleAssortHandle(Orders order, PortionInDto dto);

        /// <summary>
        /// 通道订单完成付款调用
        /// </summary>
        /// <returns></returns>
        Task<bool> OrderComplete(Orders order);

        /// <summary>
        /// 通道订单超时调用
        /// </summary>
        /// <returns></returns>
        Task<bool> OrderTimeOut(Orders order);

        /// <summary>
        /// 创建全部完成
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        Task<bool> CreateEnd();

        /// <summary>
        /// 更新全部完成
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        Task<bool> UpdateEnd();

        /// <summary>
        /// 通道创建收款实体单个调用
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        Task<ArticleEntities> CreateArticleEntitieses(ArticleEntities m);

        /// <summary>
        /// 通道修改收款实体单个调用
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        Task<ArticleEntities> UpdataArticleEntitieses(ArticleEntities m);

        /// <summary>
        /// 为通道订单设置收款号
        /// </summary>
        /// <returns></returns>
        Task<bool> OrderSetArticle(Orders order,Guid articleIdGuid);


        /// <summary>
        /// 设置 或 获取账号在线状态
        /// </summary>
        /// <param name="m"></param>
        /// <param name="online"></param>
        /// <returns></returns>
        Task<bool> ArticleOnline(ArticleEntities m, bool? online = null);
    }
}
