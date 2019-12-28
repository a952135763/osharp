using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace KaPai.Pay.Provide.Dtos
{
    public partial class ArticleAssortOutputDto
    {

        /// <summary>
        /// 获取或设置 通道ID
        /// </summary>
        [DisplayName("通道名称")]
        public String Channel_Name { get; set; }
    }
}
