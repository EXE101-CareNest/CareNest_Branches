using CareNest_Branches.Application.Common;
using CareNest_Branches.Application.Interfaces.CQRS.Queries;
using CareNest_Branches.Application.Interfaces.UOW;
using CareNest_Branches.Domain.Entitites;

namespace CareNest_Branches.Application.Features.Queries.GetAllPaging
{
    public class GetAllPagingQueryHandler : IQueryHandler<GetAllPagingQuery, PageResult<BranchesResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllPagingQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PageResult<BranchesResponse>> HandleAsync(GetAllPagingQuery query)
        {
            var selector = ObjectMapperExtensions.CreateMapExpression<Branches, BranchesResponse>();

            var orderByFunc = GetOrderByFunc(query.SortColumn, query.SortDirection);
            // Thêm điều kiện lọc theo ShopId nếu có
            System.Linq.Expressions.Expression<Func<Branches, bool>>? predicate = null;
            if (!string.IsNullOrWhiteSpace(query.ShopId))
            {
                predicate = s => s.ShopId == query.ShopId;
            }

            IEnumerable<BranchesResponse> a = await _unitOfWork.GetRepository<Branches>().FindAsync(
                predicate: predicate,
                orderBy: orderByFunc,
                selector: selector,
                pageSize: query.PageSize,
                pageIndex: query.Index);

            return new PageResult<BranchesResponse>(a, 1, query.Index, query.PageSize);
        }


        private Func<IQueryable<Branches>, IOrderedQueryable<Branches>> GetOrderByFunc(string? sortColumn, string? sortDirection)
        {
            var ascending = string.IsNullOrWhiteSpace(sortDirection) || sortDirection.ToLower() != "desc";

            return sortColumn?.ToLower() switch
            {
                "updateat" => q => ascending ? q.OrderBy(a => a.UpdatedAt) : q.OrderByDescending(a => a.UpdatedAt),
                _ => q => q.OrderBy(a => a.CreatedAt) // fallback nếu không có sortColumn
            };
        }
    }
}
