using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using KaPai.Pay.Channel.Entities;
using KaPai.Pay.Identity.Entities;
using OSharp.Entity;

namespace KaPai.Pay.Merchant.Dtos
{
   public class PerchenLimitOut
    {

        /// <summary>
        /// 获取或设置 费率名称
        /// </summary>
        [DisplayName("费率名称")]
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 费率值
        /// </summary>
        [DisplayName("费率值")]
        public long Value { get; set; }

        /// <summary>
        /// 获取或设置 删除时间
        /// </summary>
        [DisplayName("删除时间")]
        public DateTime? DeletedTime { get; set; }

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
        /// 获取或设置 用户主键ID
        /// </summary>
        [DisplayName("用户ID"), UserFlag]
        public int UserId { get; set; }

        /// <summary>
        /// 获取或设置 通道主键
        /// </summary>
        [DisplayName("主键")]
        public Guid ChannelId { get; set; }


    }
}
