using System;
using System.Collections.Generic;
using System.Text;

namespace KaPai.Pay.Merchant.Dtos
{
    public partial class MerchantExtraInputDto
    {

        /// <summary>
        /// 上级用户ID
        /// </summary>
        public int? PUserId { get; set; }

        /// <summary>
        /// 接入秘钥
        /// </summary>
        public string Key { get; set; }
    }
}
