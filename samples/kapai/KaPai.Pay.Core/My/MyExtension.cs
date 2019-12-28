using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using KaPai.Pay.Channel.Entities;
using KaPai.Pay.Merchant.Entities;
using KaPai.Pay.OpenApi.Base;
using KaPai.Pay.Provide.Entities;
using OSharp.Extensions;
using OSharp.Filter;

namespace KaPai.Pay.My
{

    public static class MyExtension
    {

        public static IDictionary<string, object> ToIDictionary(this JsonDocument filter)
        {
         
            return  JsonHelp.JsonDocumentToDictionary(filter);

        }

        /// <summary>
        /// 获取 INterfaceChannel 操作实例!
        /// 在 CreateScope 作用域 请传递 provider
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static INterfaceChannel GetChannelBase(this Channels channel,IServiceProvider provider = null)
        {
            return OpenApi.OpenApi.GetINterfaceChannel(channel.Code, provider);
        }

        /// <summary>
        /// 获取 INterfaceChannel 操作实例!
        /// 在 CreateScope 作用域 请传递 provider
        /// </summary>
        /// <param name="order"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static INterfaceChannel GetChannelBase(this Orders order, IServiceProvider provider = null)
        {
            return OpenApi.OpenApi.GetINterfaceChannel(order.Channel.Code, provider);
        }

        /// <summary>
        /// 获取 INterfaceChannel 操作实例!
        /// 在 CreateScope 作用域 请传递 provider
        /// </summary>
        /// <param name="m"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static INterfaceChannel GetChannelBase(this ArticleEntities m, IServiceProvider provider = null)
        {
            return OpenApi.OpenApi.GetINterfaceChannel(m.ArticleAssort.Channel.Code, provider);
        }


    }
}
