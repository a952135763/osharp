using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using KaPai.Pay.Channel;
using KaPai.Pay.Identity.Entities;
using KaPai.Pay.Merchant;
using KaPai.Pay.Merchant.Dtos;
using KaPai.Pay.My;
using KaPai.Pay.Provide;
using KaPai.Pay.Provide.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OSharp.AspNetCore.Mvc.Filters;
using OSharp.AspNetCore.UI;
using OSharp.Caching;
using OSharp.Core.Modules;
using OSharp.Data;
using OSharp.Mapping;

namespace KaPai.Pay.Web.Areas.Admin.Controllers.FunctionApi
{

    [ModuleInfo(Position = "FunctionApi", PositionName = "公共Api")]
    [Description("管理信息的时候,需要的公共API")]
    public class FunctionApiController : AdminApiController
    {
        protected readonly IMerchantContract MerchantContract;
        protected readonly IProvideContract ProvideContract;
        protected readonly UserManager<User> UserManager;
        protected readonly IChannelContract ChannelContract;

        public FunctionApiController(
            IProvideContract provideContract,
            IMerchantContract merchantContract,
            UserManager<User> userManager, 
            IChannelContract channelContract)
        {
            ProvideContract = provideContract;
            MerchantContract = merchantContract;
            UserManager = userManager;
            ChannelContract = channelContract;
        }


        [HttpGet]
        [ModuleInfo]
        [Description("获取收款通道附加参数(必选)")]
        public OperationResult<IDictionary<string, object>> ReadChannelType(string articleAssortId)
        {
            Check.NotNull(articleAssortId, "ArticleAssortId");
            var articleAssortguid = Guid.Parse(articleAssortId);
            var articleAssort = ProvideContract.ArticleAssorts.Join(ChannelContract.ChannelTypes, c => c.ChannelId,
                a => a.ChannelId, (a, b) => new { a, b }).First(d => d.a.Id == articleAssortguid);
            var dic = articleAssort.b.ChannelJson.ToIDictionary();
            return new OperationResult<IDictionary<string, object>>(OperationResultType.Success, "成功", dic);
        }

        [HttpGet]
        [ModuleInfo]
        [Description("读取所有分类和通道ID(必选)")]
        public OperationResult<ArticleAssortOutputDto[]> ReadCategory()
        {
            var categories = ProvideContract.ArticleAssorts.Include(d => d.Channel).ToCacheArray(d => d.MapTo<ArticleAssortOutputDto>());
            return new OperationResult<ArticleAssortOutputDto[]>(OperationResultType.Success, "成功", categories);
        }

        [HttpGet]
        [ModuleInfo]
        [Description("根据条件读取用户ID(必选)")]
        public async Task<OperationResult<object[]>> ReadAllMerchant(int type)
        {
            switch (type)
            {
                case 0:
                    {
                        var merArray = MerchantContract.MerchantExtras.Select(m => new { label = m.UserId.ToString(), value = m.UserId.ToString() })
                            .ToCacheList();
                        var proArray = ProvideContract.ProvideExtras.Select(m => new { label = m.UserId.ToString(), value = m.UserId.ToString() }).ToCacheList();
                        merArray.AddRange(proArray);
                        var sDistinct = merArray.Distinct();
                        return new OperationResult<object[]>(OperationResultType.Success,"OK", sDistinct.ToArray());
                    }
                case 1:
                    {
                        var merArray = MerchantContract.MerchantExtras.Select(m => new { label = m.UserId.ToString(), value = m.UserId.ToString() })
                            .ToCacheList();
                        return new OperationResult<object[]>(OperationResultType.Success, "OK", merArray.ToArray());

                    }
                case 2:
                    {
                        var proArray = ProvideContract.ProvideExtras.Select(m => new { label = m.UserId.ToString(), value = m.UserId.ToString() }).ToCacheList();
                        return new OperationResult<object[]>(OperationResultType.Success, "OK", proArray.ToArray());
                    }
                default:
                    return new OperationResult<object[]>(OperationResultType.Error, "无法处理请求");
            }
        }

        [HttpPost]
        [ModuleInfo]
        [Description("创建收款实体")]
        [UnitOfWork]
        public async Task<AjaxResult> CreateArticleEntity(ArticleEntitiesInputDto[] input)
        {
            Check.NotNull(input,nameof(input));

            var s = await ProvideContract.CreateArticleEntitieses(input);
            return s.ToAjaxResult();
        }

        [HttpPost]
        [ModuleInfo]
        [Description("读取全部通道(必选)")]
        public async Task<OperationResult<object[]>> ReadChannels()
        {
            var data = ChannelContract.Channelses.Select(c => new { c.Name, c.Id }).ToCacheArray();
            return new OperationResult<object[]>(OperationResultType.Success,"OK", data);
        }

        [HttpGet]
        [ModuleInfo]
        [Description("读取用户未设置的费率(必选)")]
        public async Task<OperationResult<string[]>> ReadPercentageName([NotNull]int userId, [NotNull]Guid channelId)
        {


            User user = await UserManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return new OperationResult<string[]>(OperationResultType.Error,"未有此用户");
            }
            bool ispro =user.UserRoles.Any(R => R.Role.Name == "供应商");
            bool ismer = user.UserRoles.Any(R => R.Role.Name == "商户");
            var pernames = ChannelContract.Percentages.Where(p => p.UserId == userId).Where(p=>p.ChannelId == channelId).Select(p => p.Name).ToArray();
            List<string> eList = new List<string>();

            if (ispro)
            {
                eList.AddRange(new[] { "供应商费率", "供应商反点" }.Except(pernames).ToList());

            }
            if (ismer)
            {
                eList.AddRange(new[] { "普通费率", "上级反点" }.Except(pernames).ToList());
            }
            return new OperationResult<string[]>(OperationResultType.Success,"Ok",eList.ToArray());
        }


        [HttpPost]
        [ModuleInfo]
        [Description("读取秘钥")]
        public AjaxResult ReadKey(MerchantExtraInputDto dto)
        {
            OperationResult result = new OperationResult(OperationResultType.Error, "未实现");
            return result.ToAjaxResult();
        }

        [HttpPost]
        [ModuleInfo]
        [DependOnFunction("ReadKey")]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("刷新秘钥")]
        public async Task<AjaxResult> UpdateKey(MerchantExtraInputDto dto)
        {
            Check.Validate(dto, nameof(dto));

            var a = await MerchantContract.MerchantUpdateKey(dto.Id);
            if (string.IsNullOrEmpty(a))
            {
                return new OperationResult(OperationResultType.NoChanged, "接入秘钥刷新失败,请联系客服").ToAjaxResult();
            }
            OperationResult result = new OperationResult(OperationResultType.Success, "接入秘钥刷新成功");
            return result.ToAjaxResult();
        }

        [HttpPost]
        [ModuleInfo]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("更新上级用户")]
        public bool UpdatePUserId(MerchantExtraInputDto dto)
        {
            Check.Validate(dto, nameof(dto));
            MerchantContract.MerchantUpdatePUserId(dto.Id, dto.PUserId);
            return true;
        }
    }
}
