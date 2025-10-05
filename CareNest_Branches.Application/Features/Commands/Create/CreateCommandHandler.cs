using CareNest_Branches.Application.Exceptions.Validators;
using CareNest_Branches.Application.Interfaces.CQRS.Commands;
using CareNest_Branches.Application.Interfaces.Services;
using CareNest_Branches.Application.Interfaces.UOW;
using CareNest_Branches.Domain.Entitites;
using Shared.Helper;

namespace CareNest_Branches.Application.Features.Commands.Create
{
    public class CreateCommandHandler : ICommandHandler<CreateCommand, Branches>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IShopService _service;


        public CreateCommandHandler(IUnitOfWork unitOfWork, IShopService shopService)
        {
            _service = shopService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Branches> HandleAsync(CreateCommand command)
        {
            //Validate.ValidateCreate(command);
            var shop = await _service.GetShopById(command.ShopId);
            Branches branches = new()
            {
                Address = command.Address,
                PhoneNumber = command.PhoneNumber,
                ShopId = shop.Data!.Data!.Id,
                CreatedAt = TimeHelper.GetUtcNow()
            };
            await _unitOfWork.GetRepository<Branches>().AddAsync(branches);
            await _unitOfWork.SaveAsync();

            return branches;
        }
    }
}
