using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using OSharp.Entity;

namespace KaPai.Pay.Systems.Entities
{
    [Description("地区信息")]
    public class GlobalRegion : EntityBase<int>
    {

        [DisplayName("上级Id")]
        public int Pid { get; set; }

        [DisplayName("地区名称")]
        [Required, StringLength(100)]
        public string Name { get; set; }


        [DisplayName("地区级别")]
        public int Level { get; set; }

        [DisplayName("地区邮编"), StringLength(50)]
        public string Postcode { get; set; }
    }
}
