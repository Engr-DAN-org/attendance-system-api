using System;
using api.Utils;

namespace api.Models.Entities;

public class ClassSchedule
{
    public int Id { get; set; }

    public DayOfWeek Day { get; set; }
    public required string StartTime { get; set; }
    public required string EndTime { get; set; }
    public int GracePeriod { get; set; } = 5;

    public DateTime CreatedAt { get; set; } = DateTimeUtils.DateTimeNow();
    public DateTime UpdatedAt { get; set; } = DateTimeUtils.DateTimeNow();


    /// Foreign Keys
    public required int SectionId { get; set; }
    public required int SubjectTeacherId { get; set; }
    //Relationships
    public Section? Section { get; set; }
    public Subject? Subject => SubjectTeacher?.Subject;
    public User? Teacher => SubjectTeacher?.Teacher;
    public SubjectTeacher? SubjectTeacher { get; set; }
}
