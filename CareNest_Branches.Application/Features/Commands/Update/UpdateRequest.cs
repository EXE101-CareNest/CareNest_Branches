using CareNest_Branches.Domain.Commons.Enum;

namespace CareNest_Branches.Application.Features.Commands.Update
{
    public class UpdateRequest
    {
        public string? ShopId { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
