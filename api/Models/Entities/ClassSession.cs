using System;
namespace api.Models.Entities;

public class ClassSession
{
    public int Id { get; set; }

    // Link to the class schedule
    public required int ClassScheduleId { get; set; }
    public bool IsRemote() => Latitude == null && Longitude == null;
    public required ClassSchedule ClassSchedule { get; set; }


    // The teacher who started the session
    public required string TeacherId { get; set; }
    public required User Teacher { get; set; }


    public string? Location { get; set; }
    // ✅ Store teacher's location at session start
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }


    // The actual date and time when the session was started
    public DateTime StartTime { get; set; } = DateTime.UtcNow;
    public DateTime? EndTime { get; set; }

    // Auto-set timestamps
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
