using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models.Entities;

namespace api.Models.DTOs
{
    public class GetClassScheduleDTO(ClassSchedule classSchedule, bool includesRelation = true)
    {
        public int Id { get; set; } = classSchedule.Id;
        public int Day { get; set; } = (int)classSchedule.Day;
        public DayOfWeek DayName { get; set; } = classSchedule.Day;

        public string StartTime { get; set; } = classSchedule.StartTime;
        public string EndTime { get; set; } = classSchedule.EndTime;
        public int GracePeriod { get; set; } = classSchedule.GracePeriod;
        public int SectionId { get; set; } = classSchedule.SectionId;
        public int SubjectTeacherId { get; set; } = classSchedule.SubjectTeacherId;

        public string? SubjectCode { get; set; } = classSchedule.Subject?.Code;
        public string? SubjectName { get; set; } = classSchedule.Subject?.Name;
        public GetSectionDTO? Section { get; set; } = includesRelation && classSchedule.Section != null ? new GetSectionDTO(classSchedule.Section, false) : null;
        public GetSubjectDTO? Subject { get; set; } = includesRelation && classSchedule.Subject != null ? new GetSubjectDTO(classSchedule.Subject, false) : null;
        public GetTeacherDTO? Teacher { get; set; } = includesRelation && classSchedule.Teacher != null ? new GetTeacherDTO(classSchedule.Teacher, false) : null;
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