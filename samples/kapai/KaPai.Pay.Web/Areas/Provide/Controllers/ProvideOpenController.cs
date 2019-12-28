using KaPai.Pay.CashMoney;
using KaPai.Pay.Channel;
using KaPai.Pay.Identity.Entities;
using KaPai.Pay.Merchant;
using KaPai.Pay.My;
using KaPai.Pay.Provide;
using KaPai.Pay.Provide.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OSharp.Caching;
using OSharp.Core.Modules;
using OSharp.Data;
using OSharp.Filter;
using OSharp.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using OSharp.AspNetCore.Mvc.Filters;
using OSharp.AspNetCore.UI;

namespace KaPai.Pay.Web.Areas.Provide.Controllers
{
    [ModuleInfo(Position = "Provide", PositionName = "供应商公开API接口")]
    [Description("供应商公开API接口")]
    // todo:需求修改供应商公开接口
    public class ProvideOpenController : ProvideApiController
    {
        protected readonly IMerchantContract MerchantContract;
        protected readonly IFilterService FilterService;
        protected readonly UserManager<User> UserManager;
        protected readonly IChannelContract ChannelsContract;
        protected readonly ICashMoneyContract CashMoneyContract;
        protected readonly IProvideContract ProvideContract;

        public ProvideOpenController(
            IMerchantContract merchantContract,
            IFilterService filterService,
            UserManager<User> userManager,
            IChannelContract channelsController,
            ICashMoneyContract cashMoneyContract, 
            IProvideContract provideContract) : base()
        {
            this.MerchantContract = merchantContract;
            this.FilterService = filterService;
            this.UserManager = userManager;
            this.ChannelsContract = channelsController;
            this.CashMoneyContract = cashMoneyContract;
            ProvideContract = provideContract;
        }

        private int LoginId()
        {

            var Userid = Convert.ToInt32(UserManager.GetUserId(User));
            if (User.IsInRole("供应商"))
            {
                if (Userid <= 0)
                {
                    throw new Exception("未登录");
                }

                return Userid;
            }

            throw new Exception("不是 供应商");
        }

        [HttpPost]
        [ModuleInfo]
        [Description("供应商-创建收款实体")]
        [UnitOfWork]
        public async Task<AjaxResult> CreateArticleEntity(ArticleEntitiesInputDto[] input)
        {
            Check.NotNull(input,nameof(input));
            var id = LoginId();
            input = input.Select(d =>
            {
                d.UserId = id;
                return d;
            }).ToArray();
            var s = await ProvideContract.CreateArticleEntitieses(input);
            return s.ToAjaxResult();
        }

        [HttpGet]
        [ModuleInfo]
        [Description("供应商-读取通道待填参数")]
        public OperationResult<IDictionary<string,object>> ReadChannelType(string articleAssortId)
        {
            Check.NotNull(articleAssortId, "ArticleAssortId");
            var id = LoginId();
            var articleAssortguid = Guid.Parse(articleAssortId);
            var articleAssort = ProvideContract.ArticleAssorts.Join(ChannelsContract.ChannelTypes, c => c.ChannelId,
                a => a.ChannelId, (a, b) => new {a, b}).First(d=>d.a.Id == articleAssortguid);
            var dic = articleAssort.b.ChannelJson.ToIDictionary();
            return new OperationResult<IDictionary<string, object>>(OperationResultType.Success,"成功",dic);
        }

        [HttpGet]
        [ModuleInfo]
        [Description("供应商-读取所有分类和通道ID")]
        public OperationResult<ArticleAssortOutputDto[]> ReadCategory()
        {
            var id = LoginId();
            var categories = ProvideContract.ArticleAssorts.Include(d=>d.Channel).ToCacheArray(d=>d.MapTo<ArticleAssortOutputDto>());
            return new OperationResult<ArticleAssortOutputDto[]>(OperationResultType.Success,"成功",categories);
        }

    }
}