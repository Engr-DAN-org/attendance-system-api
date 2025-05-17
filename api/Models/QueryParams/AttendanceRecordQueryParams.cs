using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Enums;

namespace api.Models.QueryParams
{
    public class AttendanceRecordQueryParams : BaseQueryParams
    {

        public string? ClassSessionId { get; set; } = null;
        public string? StudentIdNumber { get; set; } = null;
        public int? ClassScheduleId { get; set; } = null;
        public AttendanceStatus[] Status { get; set; } = [];
        public bool? Paginate { get; set; } = true;
    }
}