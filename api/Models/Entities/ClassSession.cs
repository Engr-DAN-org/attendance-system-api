using System;
using api.Enums;
using api.Utils;
namespace api.Models.Entities;

public class ClassSession
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public ClassSessionStatus Status { get; set; } = ClassSessionStatus.Started;

    // Link to the class schedule
    public required int ClassScheduleId { get; set; }
    public bool IsRemote() => Latitude == null && Longitude == null;
    public ClassSchedule? ClassSchedule { get; set; }

    public string? Location { get; set; }
    // ✅ Store teacher's location at session start
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    //The time indicator if the atendance is not late
    public string? GraceTime { get; set; }

    // The actual date and time when the session was started
    public DateTime StartTime { get; set; } = DateTimeUtils.DateTimeNow();
    public DateTime EndTime { get; set; }

    // Auto-set timestamps
    public DateTime CreatedAt { get; set; } = DateTimeUtils.DateTimeNow();
    public DateTime? UpdatedAt { get; set; }

    public List<AttendanceRecord> AttendanceRecords { get; set; } = [];

}
