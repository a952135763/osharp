using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using KaPai.Pay.OpenApi.Dtos;
using OSharp.Data;

namespace KaPai.Pay.OpenApi
{
    public interface IOpenContracr
    {
        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<OperationResult<OrderOutDto>> CreateOrder(OrderDto dto);


        /// <summary>
        /// 传递客户Id 为客户选择合适的收款号
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<OperationResult<PortionOrderOut>> PortionOrder(PortionInDto indto);

    }
}
