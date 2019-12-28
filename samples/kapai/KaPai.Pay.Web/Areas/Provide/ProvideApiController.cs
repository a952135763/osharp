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


namespace KaPai.Pay.Web.Areas.Provide
{
    [AreaInfo("Provide", Display = "供应商公开API")]
    [RoleLimit]
    public abstract class ProvideApiController : AreaApiController
    { }
}