

using System.Threading.Tasks;
using KaPai.Pay.Systems.Dtos;

namespace KaPai.Pay.Systems
{
    /// <summary>
    /// 业务契约：系统模块
    /// </summary>
    public interface ISystemsContract
    {




        Task<GlobalRegionDto[]> GetRegion(int level = 1);
    }
}