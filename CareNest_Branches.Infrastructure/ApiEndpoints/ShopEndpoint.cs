namespace CareNest_Branches.Infrastructure.ApiEndpoints
{
    public class ShopEndpoint
    {
        public static string GetById(string? id) => $"/api/shop/{id}";
    }
}