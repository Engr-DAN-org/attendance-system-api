using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models.Entities;

namespace api.Models.DTOs
{
    public class GetSectionDTO(Section section)
    {
        public int Id { get; set; } = section.Id;
        public int YearLevel { get; set; } = section.YearLevel;
        public string Name { get; set; } = section.Name;
        public string Description { get; set; } = section.Description;

        public GetTeacherDTO? Teacher { get; set; } = section.Teacher != null ? new GetTeacherDTO(section.Teacher) : null;
        public GetStudentDTO[] Students { get; set; } = [.. section.Students.Select(student => new GetStudentDTO(student))];

        public GetClassScheduleDTO[] ClassSchedules { get; set; } = [.. section.ClassSchedules.Select(classSchedule => new GetClassScheduleDTO(classSchedule))];
    }

    public class CreateSectionDTO
    {
        public required int YearLevel { get; set; }
        public required string Name { get; set; }
        public string Description { get; set; } = "";
        public string? TeacherId { get; set; }
    }

}