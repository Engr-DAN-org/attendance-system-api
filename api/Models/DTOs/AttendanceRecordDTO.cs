using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Enums;
using api.Models.Entities;

namespace api.Models.DTOs
{
    public class LogAttendanceRecordDTO
    {
        public required string ClassSessionId { get; set; }
        public string? Location { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public float? Distance { get; set; }
    }

    public class OverrideAttendanceRecordDTO
    {
        public string? Location { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public float? Distance { get; set; }

        public required string AttendanceRecordId { get; set; }
        public required AttendanceStatus Status { get; set; }
    }

    public class GetAttendanceRecordDTO(AttendanceRecord attendanceRecord, bool? includeStudentData = true, bool? includeClassSessionData = true)
    {
        public string Id { get; set; } = attendanceRecord.Id;
        public string ClassSessionId { get; set; } = attendanceRecord.ClassSessionId;
        public int? ClassScheduleId { get; set; } = attendanceRecord.ClassSession?.ClassScheduleId;
        public string StudentId { get; set; } = attendanceRecord.StudentId;
        public AttendanceStatus Status { get; set; } = attendanceRecord.Status;
        public string? Location { get; set; } = attendanceRecord.Location;
        public double? Latitude { get; set; } = attendanceRecord.Latitude;
        public double? Longitude { get; set; } = attendanceRecord.Longitude;
        public float? Distance { get; set; } = attendanceRecord.Distance;
        public DateTime? ClockInRecord { get; set; } = attendanceRecord.ClockInRecord;
        public string StudentName = attendanceRecord.StudentName;
        public DateTime CreatedAt { get; set; } = attendanceRecord.CreatedAt;
        public AuthUserDTO? Student { get; set; } = includeStudentData == true && attendanceRecord.Student != null ? new AuthUserDTO(attendanceRecord.Student, false) : null;
        public GetClassSessionDTO? ClassSession { get; set; } = includeClassSessionData == true && attendanceRecord.ClassSession != null ? new GetClassSessionDTO(attendanceRecord.ClassSession, false) : null;

    }

    public class AttendanceRecordQueryDTO : BaseQueryDTO<GetAttendanceRecordDTO>
    {
        public AttendanceRecordQueryDTO(int totalCount, int totalPages, int page, int pageSize, List<AttendanceRecord> data)
        {
            TotalCount = totalCount;
            TotalPages = totalPages;
            Page = page;
            PageSize = pageSize;
            Data = [.. data.Select(record => new GetAttendanceRecordDTO(record))];
        }
    }
}