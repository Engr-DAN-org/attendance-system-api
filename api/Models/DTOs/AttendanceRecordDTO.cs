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
}