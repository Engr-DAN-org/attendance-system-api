using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Enums;
using api.Models.Entities;

namespace api.Models.DTOs
{
    public class CreateClassSessionDTO
    {
        public required int ClassScheduleId { get; set; }
        public DateTime StartTime { get; set; }
        public string? Location { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? GraceTime { get; set; }

        public ClassSession ToClassSession(ClassSchedule classSchedule)
        {
            return new ClassSession
            {
                ClassScheduleId = classSchedule.Id,
                Location = Location,
                Latitude = Latitude,
                Longitude = Longitude,
                GraceTime = GraceTime,
            };
        }
    }
    public class GetClassSessionDTO(ClassSession classSession)
    {
        public string Id { get; set; } = classSession.Id;
        public ClassSessionStatus Status { get; set; } = classSession.Status;
        public int ClassScheduleId { get; set; } = classSession.ClassScheduleId;
        public string? Location { get; set; } = classSession.Location;
        public double? Latitude { get; set; } = classSession.Latitude;
        public double? Longitude { get; set; } = classSession.Longitude;
        public bool IsRemote { get; set; } = classSession.IsRemote();
        public string? GraceTime { get; set; } = classSession.GraceTime;
        public DateTime StartTime { get; set; } = classSession.StartTime;
        public DateTime? EndTime { get; set; } = classSession.EndTime;
        public DateTime CreatedAt { get; set; } = classSession.CreatedAt;
        public List<GetAttendanceRecordDTO> AttendanceRecords { get; set; } = [.. classSession.AttendanceRecords.Select(x => new GetAttendanceRecordDTO(x))];
    }
}