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
        public required string StudentId { get; set; }
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

    public class GetAttendanceRecordDTO(AttendanceRecord attendanceRecord)
    {
        public string Id { get; set; } = attendanceRecord.Id;
        public string ClassSessionId { get; set; } = attendanceRecord.ClassSessionId;
        public string StudentId { get; set; } = attendanceRecord.StudentId;
        public AttendanceStatus Status { get; set; } = attendanceRecord.Status;
        public string? Location { get; set; } = attendanceRecord.Location;
        public double? Latitude { get; set; } = attendanceRecord.Latitude;
        public double? Longitude { get; set; } = attendanceRecord.Longitude;
        public float? Distance { get; set; } = attendanceRecord.Distance;
        public DateTime? TimeIn { get; set; } = attendanceRecord.TimeIn;
        public string StudentName = attendanceRecord.StudentName;

        public AuthUserDTO? Student { get; set; } = attendanceRecord.Student != null ? new AuthUserDTO(attendanceRecord.Student, false) : null;
    }
}