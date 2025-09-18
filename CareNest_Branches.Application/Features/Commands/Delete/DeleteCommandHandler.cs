using CareNest_Branches.Application.Exceptions;
using CareNest_Branches.Application.Interfaces.CQRS.Commands;
using CareNest_Branches.Application.Interfaces.UOW;
using CareNest_Branches.Domain.Commons.Constant;
using CareNest_Branches.Domain.Entitites;

namespace CareNest_Branches.Application.Features.Commands.Delete
{
    public class DeleteCommandHandler : ICommandHandler<DeleteCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(DeleteCommand command)
        {
            // Lấy branches theo ID
            Branches? branches = await _unitOfWork.GetRepository<Branches>().GetByIdAsync(command.Id)
                                              ?? throw new BadRequestException("Id: " + MessageConstant.NotFound);

            _unitOfWork.GetRepository<Branches>().Delete(branches);

            await _unitOfWork.SaveAsync();
        }
    }
}
