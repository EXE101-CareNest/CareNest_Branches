namespace CareNest_Branches.Application.Features.Queries.GetAllPaging
{
    public class BranchesResponse
    {
        /// <summary>
        /// Id chi nhánh 
        /// </summary>
        public string? Id { get; set; }
        public string? ShopId { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
