using CareNest_Branches.Application.Common;
using CareNest_Branches.Application.DTOs;
using CareNest_Branches.Application.Interfaces.Services;
using CareNest_Branches.Domain.Commons.Base;
using CareNest_Branches.Domain.Commons.Constant;
using CareNest_Branches.Infrastructure.ApiEndpoints;

namespace CareNest_Branches.Infrastructure.Services
{
    public class ShopService : IShopService
    {
        private readonly IAPIService _apiService;

        public ShopService(IAPIService apiService)
        {
            _apiService = apiService;
        }
        public async Task<ResponseResult<ShopResponse>> GetShopById(string? id)
        {
            var shop = await _apiService.GetAsync<ShopResponse>("shop", ShopEndpoint.GetById(id));
            if (!shop.IsSuccess)
            {
                throw BaseException.BadRequestBadRequestResponse("Shop Id : " + MessageConstant.NotFound);
            }
            return shop;
        }
    }
}
