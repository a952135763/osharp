using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace KaPai.Pay.OpenApi.Dtos
{
    [Description("订单返回,不map实体")]
    public partial  class OrderOutDto
    {
        //系统记录号
        public Guid SysId { get; set; }

        //商户备注
        public string Remark { get; set; }

        //商户订单号
        public string OrderId { get; set; }

        //订单金额
        public long CreatedAmount { get; set; }

        //用户ID
        public int MerchantId { get; set; }

        //系统时间搓
        public long Time { get; set; }

        // 把用户重定向至此页面
        public string PayUrl { get; set; }
    }
}
