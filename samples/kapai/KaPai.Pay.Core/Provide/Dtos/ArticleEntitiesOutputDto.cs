using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using AutoMapper.Configuration.Annotations;

namespace KaPai.Pay.Provide.Dtos
{
    public partial class ArticleEntitiesOutputDto
    {



        [DisplayName("分类名称")]
        public string ArticleAssort_Name { get; set; }
    }
}
