using api.Constants;
using api.Enums;

namespace api.Models.QueryParams
{
    public class UsersQueryParams : BaseQueryParams
    {
        public string[] Role { get; set; } = [];
        public string[] Status { get; set; } = [];
        public int? SectionId { get; set; } = null;
        public bool? Paginate { get; set; } = true;
    }
}