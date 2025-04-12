using api.Constants;
using api.Enums;

namespace api.Models.QueryParams
{
    public class UsersQueryParams : BaseQueryParams
    {
        public UserRole[] Role { get; set; } = [];
        public UserStatus[] Status { get; set; } = [];
    }
}