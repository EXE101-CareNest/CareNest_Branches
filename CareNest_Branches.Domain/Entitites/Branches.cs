using CareNest_Branches.Domain.Commons;
using CareNest_Branches.Domain.Commons.Enum;

namespace CareNest_Branches.Domain.Entitites
{
    public class Branches : BaseEntity
    {
        public string? ShopId { get; set; } 
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }        
    }
}
