
using api.Constants;

namespace api.Models.QueryParams
{
    public class BaseQueryParams
    {
        public int Page { get; set; } = PageConstants.DefaultPageNumber;
        public int PageSize { get; } = PageConstants.DefaultPageSize;
        public int PageNumber => Page > 0 ? Page : PageConstants.DefaultPageNumber;
    }
}