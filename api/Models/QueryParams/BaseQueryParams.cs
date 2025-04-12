
using api.Constants;
using api.Enums;

namespace api.Models.QueryParams
{
    public class BaseQueryParams
    {
        public string? Name { get; set; }
        public int Page { get; set; } = PageConstants.DefaultPageNumber;
        public int PageSize { get; } = PageConstants.DefaultPageSize;
        public int PageNumber => Page > 0 ? Page : PageConstants.DefaultPageNumber;
        public string Sort { get; set; } = "asc";
    }
}