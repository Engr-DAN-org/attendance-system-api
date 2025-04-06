using api.Constants;

namespace api.Models.QueryParams
{
    public class UserQueryParams : BaseQueryParams
    {
        public string? Name { get; set; }
        public string? IdNumber { get; set; }
        public string? Status { get; set; }
        public string? Role { get; set; }
    }
}