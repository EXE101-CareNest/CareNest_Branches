using CareNest_Branches.Application.Interfaces.CQRS.Commands;
using CareNest_Branches.Domain.Entitites;

namespace CareNest_Branches.Application.Features.Commands.Create
{
    public class CreateCommand : ICommand<Branches>
    {
        public string? ShopId { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
