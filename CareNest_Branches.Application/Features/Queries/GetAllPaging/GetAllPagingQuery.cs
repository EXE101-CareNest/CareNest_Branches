using CareNest_Branches.Application.Common;
using CareNest_Branches.Application.Interfaces.CQRS.Queries;


namespace CareNest_Branches.Application.Features.Queries.GetAllPaging
{
    public class GetAllPagingQuery : IQuery<PageResult<BranchesResponse>>
    {
        public int Index { get; set; }
        public int PageSize { get; set; }
        public string? SortColumn { get; set; } // "Name", "Note", "CreatedAt"
        public string? SortDirection { get; set; } // "asc" or "desc"
        public string? ShopId { get; set; } // "asc" or "desc"
    }
}
