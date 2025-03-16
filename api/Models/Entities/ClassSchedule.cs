using System;
using api.Utils;

namespace api.Models.Entities;

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
    public DateTime CreatedAt { get; set; } = DateTimeUtils.DateTimeNow();
    public DateTime UpdatedAt { get; set; } = DateTimeUtils.DateTimeNow();
}
