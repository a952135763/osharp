// -----------------------------------------------------------------------
//  <copyright file="AdminApiController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-27 4:50</last-date>
// -----------------------------------------------------------------------

using Microsoft.AspNetCore.Mvc;

using OSharp.AspNetCore.Mvc;
using OSharp.Core;


namespace KaPai.Pay.Web.Areas.Merchant
{
    [AreaInfo("Merchant", Display = "商户公开Api")]
    [RoleLimit]
    public abstract class MerchantApiController : AreaApiController
    { }
}