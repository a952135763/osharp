using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using KaPai.Pay.CashMoney;
using KaPai.Pay.Channel;
using KaPai.Pay.Identity.Entities;
using KaPai.Pay.Merchant;
using KaPai.Pay.Merchant.Dtos;
using KaPai.Pay.Merchant.Entities;
using KaPai.Pay.My;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OSharp.AspNetCore.UI;
using OSharp.Caching;
using OSharp.Core.Modules;
using OSharp.Data;
using OSharp.Filter;
using OSharp.Mapping;

namespace KaPai.Pay.Web.Areas.Merchant.Controllers
{
    [ModuleInfo(Position = "Merchant", PositionName = "商户模块")]
    [Description("商户公开API")]
    // todo:需求修改商户公开接口
    public class MerchantOpenController : MerchantApiController
    {
        protected readonly IMerchantContract MerchantContract;
        protected readonly IFilterService FilterService;
        protected readonly UserManager<User> UserManager;
        protected readonly IChannelContract ChannelsContract;
        protected readonly ICashMoneyContract CashMoneyContract;

        public MerchantOpenController(
            IMerchantContract merchantContract,
            IFilterService filterService,
            UserManager<User> userManager,
            IChannelContract channelsController,
            ICashMoneyContract cashMoneyContract) : base()
        {
            this.MerchantContract = merchantContract;
            this.FilterService = filterService;
            this.UserManager = userManager;
            this.ChannelsContract = channelsController;
            this.CashMoneyContract = cashMoneyContract;
        }

        private int LoginId()
        {
            var Userid = Convert.ToInt32(UserManager.GetUserId(User));
            if (User.IsInRole("商户"))
            {
                if (Userid <= 0)
                {
                    throw new Exception("未登录");
                }
                return Userid;
            }
            throw new Exception("不是 商户");
        }


        [HttpGet]
        [ModuleInfo]
        [Description("商户-读取信息")]
        public async Task<OperationResult<MerchantExtraOutputDtoLimit>> Read()
        {
            var userid = LoginId();
            var extra = await MerchantContract.MerchantExtras.FirstOrDefaultAsync(m => m.UserId == userid);
            if (extra == null)
                return new OperationResult<MerchantExtraOutputDtoLimit>(OperationResultType.QueryNull, "无法获取");

            var dto = extra.MapTo<MerchantExtraOutputDtoLimit>();
            var result = new OperationResult<MerchantExtraOutputDtoLimit>(OperationResultType.Success, "获取成功", dto);
            return result;
        }

        [HttpGet]
        [ModuleInfo]
        [DependOnFunction("Read")]
        [Description("商户-读取实时金额")]
        public async Task<OperationResult<MerchantAmountOutLimit>> ReadPoint()
        {
            var userid = LoginId();
            MerchantAmountOutLimit o = new MerchantAmountOutLimit
            {
                Amount = await MerchantContract.MerchantGetAmount(userid),
                Accumulative = await MerchantContract.MerchantGetAccumulative(userid),
                FreezeAmount = await MerchantContract.MerchantGetFreezeAmount(userid),
                UserId = userid
            };
            return new OperationResult<MerchantAmountOutLimit>(OperationResultType.Success, "成功", o);
        }


        /// <summary>
        /// 读取所有下级用户
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleInfo]
        [DependOnFunction("Read")]
        [Description("商户-读取下级用户")]
        public PageData<MerchantExtraOutputDtoLimit> ReadSubordinate(PageCondition page)
        {
            var userid = LoginId();
            var subordinate =
                MerchantContract.MerchantExtras.ToPageCache<MerchantExtra, MerchantExtraOutputDtoLimit>(
                    extra => extra.PUserId == userid, page);
            return subordinate.ToPageData();
        }

        /// <summary>
        /// 获取开通的 通道信息
        /// </summary>
        [HttpPost]
        [ModuleInfo]
        [DependOnFunction("Read")]
        [Description("商户-读取开通的通道信息")]
        public async Task<OperationResult<UserChannelLimitOut[]>> ReadChannel()
        {
            var userid = LoginId();
            var l = ChannelsContract.UserChannels.Include(u => u.Channel)
                .Where(u => u.UserId == userid)
                .Where(u => u.Status == 1)
                .Where(u => u.Channel.Status == 1)
                .ToCacheArray(u => u.MapTo<UserChannelLimitOut>());
            return l.Length > 0
                ? new OperationResult<UserChannelLimitOut[]>(OperationResultType.Success, "获取成功", l)
                : new OperationResult<UserChannelLimitOut[]>(OperationResultType.QueryNull, "无数据", null);
        }


        /// <summary>
        /// 读取通道费率信息 有上级的 会加上上级反点
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleInfo]
        [DependOnFunction("Read")]
        [DependOnFunction("ReadChannel")]
        [Description("商户-读取通道费率信息")]
        public async Task<OperationResult<PerchenLimitOut>> ReadChannelPer(Guid channelId)
        {
            Check.NotNull(channelId, nameof(channelId));
            var userid = LoginId();
            var isPuser = MerchantContract.MerchantExtras.Where(m => m.UserId == userid).Any(m => m.PUserId != null);
            long antiPoint = 0;
            if (isPuser)
            {
                antiPoint = await ChannelsContract.Percentages.Where(d => d.UserId == userid)
                    .Where(d => d.Name == "上级反点").Select(d => d.Value).FirstOrDefaultAsync();
            }
            var per = await ChannelsContract.Percentages.Where(p => p.ChannelId == channelId).Where(p => p.UserId == userid)
                   .FirstOrDefaultAsync();
            var oOut = per.MapTo<PerchenLimitOut>();
            oOut.Value += antiPoint;
            return per == null
                ? new OperationResult<PerchenLimitOut>(OperationResultType.QueryNull, "无")
                : new OperationResult<PerchenLimitOut>(OperationResultType.Success, "成功", oOut);

        }

        [HttpPost]
        [ModuleInfo]
        [DependOnFunction("Read")]
        [Description("商户-读取银行列表")]
        public async Task<OperationResult<MerchantBankOutLimit[]>> ReadBank()
        {
            var userid = LoginId();

            var bankLists = CashMoneyContract.BankLists.Where(b => b.UserId == userid)
                .Select(b => new MerchantBankOutLimit(b))
                .ToCacheArray();
            return bankLists.Length > 0
                ? new OperationResult<MerchantBankOutLimit[]>(OperationResultType.Success, "获取成功", bankLists)
                : new OperationResult<MerchantBankOutLimit[]>(OperationResultType.QueryNull, "无");
        }


        [HttpPost]
        [ModuleInfo]
        [DependOnFunction("Read")]
        [Description("商户-增加提现记录")]
        public async Task<AjaxResult> Cash(MerchantCashInputLimit[] dtos)
        {
            Check.NotNull(dtos, nameof(dtos));
            var userid = LoginId();
            OperationResult result = await CashMoneyContract.CreateCashLogs(userid, dtos[0]);
            return result.ToAjaxResult();
        }

    }
}