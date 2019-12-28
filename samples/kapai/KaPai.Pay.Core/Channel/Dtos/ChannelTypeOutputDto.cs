using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace KaPai.Pay.Channel.Dtos
{
    public partial class ChannelTypeOutputDto
    {

        /// <summary>
        /// 通道名称
        /// </summary>
        [DisplayName("通道名称")]
        public string Channel_Name { get; set; }
    }
}
