using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models.Entities;

namespace api.Models.DTOs
{
    public class GetClassScheduleDTO(ClassSchedule classSchedule)
    {
        public int Id { get; set; } = classSchedule.Id;
        public int SectionId { get; set; } = classSchedule.SectionId;
        public string SubjectId { get; set; } = classSchedule.SubjectId;
        public string? TeacherId { get; set; } = classSchedule.TeacherId;
        public DayOfWeek Day { get; set; } = classSchedule.Day;
        public TimeOnly StartTime { get; set; } = classSchedule.StartTime;
        public int GracePeriod { get; set; } = classSchedule.GracePeriod;
        public TimeOnly EndTime { get; set; } = classSchedule.EndTime;
        public Section Section { get; set; } = classSchedule.Section;
        public Subject Subject { get; set; } = classSchedule.Subject;
        public GetTeacherDTO? Teacher { get; set; } = classSchedule.Teacher != null ? new GetTeacherDTO(classSchedule.Teacher) : null;
    }
}