using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models.QueryParams
{
    public class ClassScheduleQueryParams
    {
        public bool? Unassigned { get; set; } = null;
        public string? SubjectName { get; set; }
        public string? TeacherName { get; set; }
        public DayOfWeek? Day { get; set; }
        public string? StartTime { get; set; } // Format: "HH:mm"
        public string? EndTime { get; set; } // Format: "HH:mm"
    }

    public class ScheduleTeacherSectionQuery
    {
        public int? SectionId { get; set; }
        public string? TeacherId { get; set; }

    }
}