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
        public int SubjectTeacherId { get; set; } = classSchedule.SubjectTeacherId;
        public DayOfWeek Day { get; set; } = classSchedule.Day;
        public string StartTime { get; set; } = classSchedule.StartTime;
        public int GracePeriod { get; set; } = classSchedule.GracePeriod;
        public string EndTime { get; set; } = classSchedule.EndTime;
        public Section? Section { get; set; } = classSchedule.Section;
        public Subject? Subject { get; set; } = classSchedule.Subject;
        public GetTeacherDTO? Teacher { get; set; } = classSchedule.Teacher != null ? new GetTeacherDTO(classSchedule.Teacher) : null;
    }

    public class CreateClassScheduleDTO
    {
        public required int SectionId { get; set; }
        public required int SubjectTeacherId { get; set; }
        public required DayOfWeek Day { get; set; }
        public required string StartTime { get; set; }
        public required string EndTime { get; set; }
        public int GracePeriod { get; set; } = 0;

        public ClassSchedule ToClassSchedule(int sectionId)
        {
            return new ClassSchedule()
            {
                SubjectTeacherId = SubjectTeacherId,
                SectionId = sectionId,
                Day = Day,
                StartTime = StartTime,
                EndTime = EndTime,
                GracePeriod = GracePeriod,
            };
        }
    }
}