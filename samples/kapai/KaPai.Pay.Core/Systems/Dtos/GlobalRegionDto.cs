using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using KaPai.Pay.Systems.Entities;
using OSharp.Entity;
using OSharp.Mapping;

namespace KaPai.Pay.Systems.Dtos
{
    [MapFrom(typeof(GlobalRegion))]
   public class GlobalRegionDto : IOutputDto
    {
        [DisplayName("Id")]
        public int Id { get; set; }

        [DisplayName("上级Id")]
        public int Pid { get; set; }

        [DisplayName("地区名称")]
        public string Name { get; set; }


        [DisplayName("地区级别")]
        public int Level { get; set; }

        [DisplayName("地区邮编")]
        public string Postcode { get; set; }


    }
}
