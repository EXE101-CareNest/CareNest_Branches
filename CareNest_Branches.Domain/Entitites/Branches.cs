using CareNest_Branches.Domain.Commons.Base;

namespace CareNest_Branches.Domain.Entitites
{
    public class Branches : BaseEntity
    {
        public string? ShopId { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
