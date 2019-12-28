using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using KaPai.Pay.Provide.Entities;
using Microsoft.Extensions.Logging;

namespace KaPai.Pay.Provide
{
    public abstract partial class ProvideServiceBase
    {
        public async Task<bool> ProvideInitialization(int userid, int? puserid = null, string extrastr = null)
        {

            if (PointsRepository.QueryAsNoTracking().Any(p => p.UserId == userid) &&
                ProvideExtraRepository.QueryAsNoTracking().Any(p => p.UserId == userid))
            {
                Logger.Log(LogLevel.Debug,"初始化 供应商失败,已有实体数据");
                return false;
            }

            var po = new Points {UserId = userid, Accumulative = 0, Point = 0};
            var extr = new ProvideExtra {UserId = userid, CreatedTime = DateTime.Now, PUserId = puserid};
            if(!string.IsNullOrEmpty(extrastr)) extr.Extra = JsonDocument.Parse(extrastr);


            var count = await PointsRepository.InsertAsync(po);

            if (count < 1)
            {
                Logger.Log(LogLevel.Debug, "初始化 供应商失败,PointsRepository 插入失败");
                return false;
            }

            count = await ProvideExtraRepository.InsertAsync(extr);
            if (count < 1)
            {
                Logger.Log(LogLevel.Debug, "初始化 供应商失败,ProvideExtraRepository 插入失败");
                return false;
            }
            PointsRepository.UnitOfWork.Commit();
            ProvideExtraRepository.UnitOfWork.Commit();

            return true;
        }
    }
}
