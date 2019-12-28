using System;
using System.Collections.Generic;
using System.Text;
using KaPai.Pay.CashMoney.Entities;
using OSharp.Entity;

namespace KaPai.Pay.Merchant.Dtos
{
   public class MerchantBankOutLimit:IOutputDto
    {
        public MerchantBankOutLimit(BankList eBankList)
        {
            if (eBankList == null) return;
            value = eBankList.Id.ToString();
            label = $"{eBankList.Name}-{eBankList.Account}-{eBankList.BankName}";

        }

        public string value { get; set; }
        public String label { get; set; }
    }
}
