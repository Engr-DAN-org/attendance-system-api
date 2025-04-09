using api.Constants;
using api.Enums;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace api.Models.QueryParams
{

    public class CourseQuery : BaseQueryParams
    {
        public string? Name { get; set; }
        public string? Code { get; set; }
        public int? Years { get; set; }
        public new int PageSize { get; set; } = 9;
    }
}