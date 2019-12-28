using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace KaPai.Pay.Channel.Dtos
{
    public class ChannelTypeInHttpput
    {

        /// <summary>
        /// 获取或设置 编号
        /// </summary>
        [DisplayName("编号")]
        public Guid Id { get; set; }

        /// <summary>
        /// 获取或设置 通道收款号参数模板
        /// </summary>
        [DisplayName("收款号参数模板")]
        public IList<object> ChannelJson { get; set; }

        /// <summary>
        /// 获取或设置 通道ID
        /// </summary>
        [DisplayName("通道ID")]
        public Guid ChannelId { get; set; }
    }
}
