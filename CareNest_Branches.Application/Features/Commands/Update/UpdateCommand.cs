using CareNest_Branches.Application.Interfaces.CQRS.Commands;
using CareNest_Branches.Domain.Commons.Enum;
using CareNest_Branches.Domain.Entitites;

namespace CareNest_Branches.Application.Features.Commands.Update
{
    public class UpdateCommand : ICommand<Branches>
    {
        public string Id { get; set; } = string.Empty;
        public string? ShopId { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
    }
}