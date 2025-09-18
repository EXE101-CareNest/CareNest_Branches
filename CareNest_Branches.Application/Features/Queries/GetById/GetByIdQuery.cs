using CareNest_Branches.Application.Interfaces.CQRS.Queries;
using CareNest_Branches.Domain.Entitites;

namespace CareNest_Branches.Application.Features.Queries.GetById
{
    public class GetByIdQuery : IQuery<Branches>
    {
        public required string Id { get; set; }
    }
}
