
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using OSharp.Entity;
using OSharp.Mapping;

using KaPai.Pay.Merchant.Entities;
using KaPai.Pay.Identity.Dtos;
using KaPai.Pay.Channel.Dtos;
using KaPai.Pay.Provide.Dtos;

namespace KaPai.Pay.Merchant.Dtos
{
    public partial class OrdersOutputDto
    {

       /**
        public OrdersOutputDto(Orders _entity)
        {
            ChannelName = _entity.Channel.Name;
        }**/

        /// <summary>
        /// 获取通道名字,
        /// </summary>
        [DisplayName("通道名称")]
        public string Channel_Name { get; set; }

        [DisplayName("回调状态")]
        public int CallBack { get; set; }
    }
   


}

