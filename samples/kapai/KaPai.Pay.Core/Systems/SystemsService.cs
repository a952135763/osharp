

using System.Threading.Tasks;
using KaPai.Pay.Systems.Dtos;
using KaPai.Pay.Systems.Entities;
using OSharp.Caching;
using OSharp.Entity;
using OSharp.Mapping;

namespace KaPai.Pay.Systems
{
    /// <summary>
    /// 业务实现：系统模块
    /// </summary>
    public class SystemsService: ISystemsContract
    {
        protected readonly IRepository<GlobalRegion, int> Repository;

       public SystemsService(IRepository<GlobalRegion, int> repository)
        {
            Repository = repository;
        }

        public async Task<GlobalRegionDto[]> GetRegion(int level = 1)
        {
            var resArray = Repository.QueryAsNoTracking(d => d.Level == level)
                .ToCacheArray(d => d.MapTo<GlobalRegionDto>());
            return resArray;
        }
    }
}