using System;
using System.Collections.Generic;
using System.Text;
using KaPai.Pay.Channel.Entities;
using OSharp.Entity;

namespace KaPai.Pay.Merchant.Dtos
{
    public class MerchantChannel : IOutputDto
    {
        public UserChannel userChannel { get; set; }

        public Percentage percentage { get; set; }
    }
}
