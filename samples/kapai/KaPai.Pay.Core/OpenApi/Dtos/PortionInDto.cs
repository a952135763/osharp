using System;
using System.Collections.Generic;
using System.Text;
using OSharp.Entity;

namespace KaPai.Pay.OpenApi.Dtos
{
    public class PortionInDto : IInputDto<Guid>
    {
        /// <summary>
        /// 客户ID
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// 订单ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        public double Longitude { get; set; }

    }
}
