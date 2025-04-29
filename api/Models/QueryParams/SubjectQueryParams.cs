using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models.QueryParams
{
    public class SubjectQueryParams : BaseQueryParams
    {
        public string? TeacherId { get; set; }
    }

    public class SubjectTeacherQueryParams
    {
        public string? Name { get; set; }
        public string? TeacherId { get; set; }
        public int? SubjectId { get; set; }
        public int? Limit { get; set; }
    }
}