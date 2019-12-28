using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using KaPai.Pay.Merchant.Entities;
using OSharp.Entity;
using OSharp.Mapping;

namespace KaPai.Pay.Merchant.Dtos
{

    [MapFrom(typeof(Amounts))]
    [Description("商户读取余额信息")]
    public class MerchantAmountOutLimit : IOutputDto
    {
        /// <summary>
        /// 获取或设置 余额
        /// </summary>
        [DisplayName("当前余额")]
        public long Amount { get; set; }

        /// <summary>
        /// 获取或设置 累计余额
        /// </summary>
        [DisplayName("累计余额")]
        public long Accumulative { get; set; }


        /// <summary>
        /// 获取或设置 用户ID
        /// </summary>
        [DisplayName("用户ID"), UserFlag]
        public int UserId { get; set; }

        [DisplayName("冻结积分")]
        public long FreezeAmount { get; set; }

    }
}
