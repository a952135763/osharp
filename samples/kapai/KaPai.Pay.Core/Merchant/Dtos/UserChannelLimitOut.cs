using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using OSharp.Entity;

namespace KaPai.Pay.Merchant.Dtos
{
   public class UserChannelLimitOut:IOutputDto
    {

        public UserChannelLimitOut()
        {

        }
        /// <summary>
        /// 获取或设置 状态
        /// </summary>
        [DisplayName("状态")]
        public int Status { get; set; }

        /// <summary>
        /// 获取或设置 创建时间
        /// </summary>
        [DisplayName("创建时间")]
        public DateTime CreatedTime { get; set; }


        /// <summary>
        /// 获取或设置 更新时间
        /// </summary>
        [DisplayName("更新时间")]
        public DateTime? LastUpdatedTime { get; set; }

        /// <summary>
        /// 获取或设置 用户主键
        /// </summary>
        [DisplayName("用户ID"), UserFlag]
        public int UserId { get; set; }

        /// <summary>
        /// 获取或设置 通道主键
        /// </summary>
        [DisplayName("通道ID")]
        public Guid ChannelId { get; set; }

        [DisplayName("通道名称")]
        public string Channel_Name { get; set; }
    }
}
