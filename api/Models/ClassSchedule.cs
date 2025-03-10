using System;

namespace api.Models;

public class ClassSchedule
{
    public int Id { get; set; }
    public int SectionId { get; set; }
    public required string SubjectId { get; set; }
    public string? TeacherId { get; set; }
    public DayOfWeek Day { get; set; }
    public required TimeOnly StartTime { get; set; }
    public int GracePeriod { get; set; } = 5;
    public required TimeOnly EndTime { get; set; }
    public required Section Section { get; set; }
    public required Subject Subject { get; set; }
    public User? Teacher { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
