using CareNest_Branches.Application.Common;
using CareNest_Branches.Application.DTOs;

namespace CareNest_Branches.Application.Interfaces.Services
{
    public interface IShopService
    {
        Task<ResponseResult<ShopResponse>> GetShopById(string? id);
    }
}
