using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using KaPai.Pay.Merchant.Entities;
using Microsoft.Extensions.Logging;

namespace KaPai.Pay.Merchant
{
    public abstract partial class MerchantServiceBase
    {
        /// <summary>
        /// 初始化商户
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="puserid"></param>
        /// <returns></returns>
        public async Task<bool> MerchantInitialization(int userid, int? puserid = null,string extrastr = null)
        {
            // 判断是否拥有了数据
            if (MerchantExtraRepository.QueryAsNoTracking().Any(u => u.UserId == userid) || AmountsRepository.QueryAsNoTracking().Any(u => u.UserId == userid))
            {
                Logger.Log(LogLevel.Debug, "无法初始化商户,已有商户数据");
                return false;
            }

            var extra = new MerchantExtra
            {
                UserId = userid, CreatedTime = DateTime.Now, Key = Guid.NewGuid().ToString("N"), PUserId = puserid
            };

            if(!string.IsNullOrEmpty(extrastr)) extra.Extra = JsonDocument.Parse(extrastr);

            var amount = new Amounts {UserId = userid, Accumulative = 0, Amount = 0, CreatedTime = DateTime.Now};


            var count = await MerchantExtraRepository.InsertAsync(extra);
            if (count < 1)
            {
                Logger.Log(LogLevel.Debug, "无法初始化商户,MerchantExtraRepository 插入失败");

                return false;
            }
            count = await AmountsRepository.InsertAsync(amount);
            if (count < 1)
            {
                Logger.Log(LogLevel.Debug, "无法初始化商户,AmountsRepository 插入失败");
                return false;

            }
            MerchantExtraRepository.UnitOfWork.Commit();
            AmountsRepository.UnitOfWork.Commit();
            return true;
        }

        public async Task<bool> MerchantUpdatePUserId(Guid id, int? puserid,bool commit = false)
        {
            var Extra = MerchantExtraRepository.Query(u => u.Id == id).First();
            if (!MerchantExtraRepository.QueryAsNoTracking().Any(u => u.UserId == puserid))
            {
                return false;
            }
            Extra.PUserId = puserid;

            if (await MerchantExtraRepository.UpdateAsync(Extra) < 1)
            {
                return false;
            }
            if (commit) MerchantExtraRepository.UnitOfWork.Commit();
            return true;
        }

        public async Task<string> MerchantUpdateKey(Guid id,bool autocommit = false)
        {
          var extra =  MerchantExtraRepository.Query(u => u.Id == id).First();
          extra.Key = Guid.NewGuid().ToString("N");
          var count = await MerchantExtraRepository.UpdateAsync(extra);
          if (count < 1)
          {
              return null;
          }
          if(autocommit) MerchantExtraRepository.UnitOfWork.Commit();
          return extra.Key;
        }
    }
}
