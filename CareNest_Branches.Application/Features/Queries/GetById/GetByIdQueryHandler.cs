using CareNest_Branches.Application.Interfaces.CQRS.Queries;
using CareNest_Branches.Application.Interfaces.UOW;
using CareNest_Branches.Domain.Commons.Constant;
using CareNest_Branches.Domain.Entitites;

namespace CareNest_Branches.Application.Features.Queries.GetById
{
    public class GetByIdQueryHandler : IQueryHandler<GetByIdQuery, Branches>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Branches> HandleAsync(GetByIdQuery query)
        {
            Branches? branches = await _unitOfWork.GetRepository<Branches>().GetByIdAsync(query.Id);

            if (branches == null)
            {
                throw new Exception(MessageConstant.NotFound);
            }
            return branches;
        }
    }
}
