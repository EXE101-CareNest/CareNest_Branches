using CareNest_Branches.Application.Exceptions;
using CareNest_Branches.Application.Exceptions.Validators;
using CareNest_Branches.Application.Interfaces.CQRS.Commands;
using CareNest_Branches.Application.Interfaces.Services;
using CareNest_Branches.Application.Interfaces.UOW;
using CareNest_Branches.Domain.Commons.Constant;
using CareNest_Branches.Domain.Entitites;
using Shared.Helper;

namespace CareNest_Branches.Application.Features.Commands.Update
{
    public class UpdateCommandHandler : ICommandHandler<UpdateCommand, Branches>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IShopService _service;

        public UpdateCommandHandler(IUnitOfWork unitOfWork, IShopService service)
        {
            _unitOfWork = unitOfWork;
            _service = service;
        }

        public async Task<Branches> HandleAsync(UpdateCommand command)
        {
            // Gọi validator để kiểm tra dữ liệu
            //Validate.ValidateUpdate(command);
            var shop = await _service.GetShopById(command.ShopId);

            // Tìm để cập nhật
            Branches? branches = await _unitOfWork.GetRepository<Branches>().GetByIdAsync(command.Id)
               ?? throw new BadRequestException("Id: " + MessageConstant.NotFound);

            branches.Address = command.Address ?? branches.Address;
            branches.PhoneNumber = command.PhoneNumber ?? branches.PhoneNumber;
            branches.ShopId =  shop.Data!.Data!.Id;
            branches.UpdatedAt = TimeHelper.GetUtcNow();

            _unitOfWork.GetRepository<Branches>().Update(branches);
            await _unitOfWork.SaveAsync();
            return branches;

        }
    }
}
