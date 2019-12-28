using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using KaPai.Pay.Merchant.Entities;
using OSharp.Entity;
using OSharp.Mapping;

namespace KaPai.Pay.Merchant.Dtos
{

    [MapFrom(typeof(MerchantExtra))]
    [Description("商户读取信息")]
    public class MerchantExtraOutputDtoLimit : IOutputDto
    {
        public MerchantExtraOutputDtoLimit()
        {
        }


        /// <summary>
        /// 获取或设置 附加参数
        /// </summary>
        [DisplayName("附加参数")]
        public IDictionary<string,object> Extra { get; set; }

        /// <summary>
        /// 获取或设置 接入秘钥
        /// </summary>
        [DisplayName("接入秘钥")]
        public string Key { get; set; }

        /// <summary>
        /// 获取或设置 创建时间
        /// </summary>
        [DisplayName("创建时间")]
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// 获取或设置 用户ID
        /// </summary>
        [DisplayName("用户ID"), UserFlag]
        public int UserId { get; set; }

        /// <summary>
        /// 获取或设置 上级用户ID
        /// </summary>
        [DisplayName("上级用户ID")]
        public int? PUserId { get; set; }


    }
}
