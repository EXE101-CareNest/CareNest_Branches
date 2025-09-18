using CareNest_Branches.Domain.Entitites;
using CareNest_Branches.Application.Exceptions;
using CareNest_Branches.Application.Exceptions.Validators;
using CareNest_Branches.Application.Interfaces.CQRS.Commands;
using CareNest_Branches.Application.Interfaces.UOW;
using CareNest_Branches.Domain.Commons.Constant;
using Shared.Helper;

namespace CareNest_Branches.Application.Features.Commands.Update
{
    public class UpdateCommandHandler : ICommandHandler<UpdateCommand, Branches>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Branches> HandleAsync(UpdateCommand command)
        {
            // Gọi validator để kiểm tra dữ liệu
            Validate.ValidateUpdate(command);

            // Tìm để cập nhật
            Branches? branches = await _unitOfWork.GetRepository<Branches>().GetByIdAsync(command.Id)
               ?? throw new BadRequestException("Id: " + MessageConstant.NotFound);

            branches.Address = command.Address ?? branches.Address;
            branches.PhoneNumber = command.PhoneNumber ?? branches.PhoneNumber;
            branches.ShopId = command.ShopId ?? branches.ShopId;
            branches.UpdatedAt = TimeHelper.GetUtcNow();

            _unitOfWork.GetRepository<Branches>().Update(branches);
            await _unitOfWork.SaveAsync();
            return branches;

        }
    }
}
