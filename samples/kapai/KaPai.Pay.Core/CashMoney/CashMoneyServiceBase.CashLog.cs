using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hangfire.States;
using KaPai.Pay.CashMoney.Entities;
using KaPai.Pay.Merchant;
using Microsoft.Extensions.DependencyInjection;
using OSharp.Data;

namespace KaPai.Pay.CashMoney
{
    public abstract partial class CashMoneyServiceBase
    {
        protected IMerchantContract MerchantContract => ServiceProvider.GetService<IMerchantContract>();


        /// <summary>
        /// 增加提现记录
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="dtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> CreateCashLogs(int userid, MerchantCashInputLimit dto,bool commit = true)
        {

            var Min = long.MinValue;
            var Max = long.MaxValue;
            
            // todo: 系统设置 最小提款额度 和 最大提款额度
            if (dto.Point < Min)
            {
                return new OperationResult(OperationResultType.Error, $"最小提款额度为:{Min}");
            }
            if (dto.Point > Max)
            {
                return new OperationResult(OperationResultType.Error, $"最大提款额度为:{Max}");
            }


            var bank = BankListRepository.QueryAsNoTracking(b => b.Id == dto.Bank)
                .FirstOrDefault(b => b.UserId == userid);
            if (bank == null)
            {
                return new OperationResult(OperationResultType.Error, "未找到收款信息");
            }
            // 获取可提现 积分
            var amount = await MerchantContract.MerchantGetAvailable(userid);
            if (amount < dto.Point)
            {
                return new OperationResult(OperationResultType.Error, "余额不足");
            }
            var log = new CashLog
            {
                BankName = bank.BankName,
                Account = bank.Account,
                BankType = bank.BankType,
                Name = bank.Name,
                Status = 0,
                Remarks = dto.Remarks,
                Amount = dto.Point
            };
            var count = await CashLogRepository.InsertAsync(log);
            if (count < 1)
            {
                return new OperationResult(OperationResultType.Error, "数据增加失败");
            }
            // 把此次积分 转移到冻结积分
            await MerchantContract.MerchantFreezeAmountChange(userid, log.Amount);
            if(commit) CashLogRepository.UnitOfWork.Commit();
            return new OperationResult(OperationResultType.Success,"提现记录增加成功");
        }


    }
}