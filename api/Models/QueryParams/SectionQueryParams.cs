using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models.QueryParams
{
    public class SectionQueryParams : BaseQueryParams
    {
        public int? ClassId { get; set; }
        public int? TeacherId { get; set; }
        public int? SubjectId { get; set; }
        public int? YearLevel { get; set; }
    }
}