using api.Constants;
using api.Enums;

namespace api.Models.QueryParams
{
    public class UsersQueryParams : BaseQueryParams
    {
        public string[] Role { get; set; } = [];
        public string[] Status { get; set; } = [];
    }
}