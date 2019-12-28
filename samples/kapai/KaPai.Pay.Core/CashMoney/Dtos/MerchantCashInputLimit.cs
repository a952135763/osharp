using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using OSharp.Entity;

namespace KaPai.Pay.CashMoney
{
   public class MerchantCashInputLimit: IInputDto<Guid>
    {
        [DisplayName("编号")]
        public Guid Id { get; set; }

        [DisplayName("收款号ID")]
        public Guid Bank { get; set; }

        [DisplayName("备注")]

        public string Remarks { get; set; }

        [DisplayName("提现积分")]

        public long Point { get; set; }
    }
}
