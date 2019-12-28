using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using OSharp.Entity;

namespace KaPai.Pay.OpenApi.Dtos
{
    [Description("创建订单Dto,不映射数据库")]
    public partial class OrderDto : IInputDto<Guid>
    {
        [Required(ErrorMessage = "{0}是必须的!")]
        [DisplayName("商户ID")]
        public int MerchantId { get; set; }

        [Required(ErrorMessage = "{0}是必须的!")]
        [DisplayName("支付方式代码")]
        public string Code { get; set; }

        [Required(ErrorMessage = "{0}是必须的!")]
        [DisplayName("签名")]
        public string Sign { get; set; }

        [Required(ErrorMessage = "{0}是必须的!")]
        [DisplayName("订单金额,'分'为单位")] 
        public long CreatedAmount { get; set; }

        //商户订单id
        [Required(ErrorMessage = "{0}是必须的!")]
        [DisplayName("订单号")]
        public string OrderId { get; set; }

        [Required(ErrorMessage = "{0}是必须的!")]
        [DisplayName("异步回调地址")]
        public string AsynUrl { get; set; }

        [Required(ErrorMessage = "{0}是必须的!")]
        [DisplayName("客户Ip地址")]
        public string ClientIp { set; get; }

        [Required(ErrorMessage="{0}是必须的!")]
        [DisplayName("10位时间戳")]
        public long Time { get; set; }


        [StringLength(100),]
        [DisplayName("订单备注")]
        public string Remark { get; set; }

        //客户唯一码
        [DisplayName("客户唯一码")]
        public string ClientId { set; get; }

        public Guid Id { get; set; } = default(Guid);
    }
}
