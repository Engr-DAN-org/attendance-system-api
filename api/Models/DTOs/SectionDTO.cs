using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models.Entities;

namespace api.Models.DTOs
{
    public class GetSectionDTO(Section section, bool withRelation = true, bool includesSchedules = true)
    {
        public int Id { get; set; } = section.Id;
        public int YearLevel { get; set; } = section.YearLevel;
        public string Name { get; set; } = section.Name;
        public string Description { get; set; } = section.Description;

        public int CourseId { get; set; } = section.CourseId;
        public GetCourseDTO? Course { get; set; } = section.Course != null ? new GetCourseDTO(section.Course) : null;
        public GetTeacherDTO? Teacher { get; set; } = withRelation && section.Teacher != null ? new GetTeacherDTO(section.Teacher) : null;
        public GetStudentDTO[]? Students { get; set; } = !withRelation ? [] : [.. section.Students.Select(student => new GetStudentDTO(student))];

        public GetClassScheduleDTO[] ClassSchedules { get; set; } = withRelation && includesSchedules ? [.. section.ClassSchedules.Select(classSchedule => new GetClassScheduleDTO(classSchedule, false))] : [];
    }

    public class CreateSectionDTO
    {
        public required int CourseId { get; set; }
        public required int YearLevel { get; set; }
        public required string Name { get; set; }
        public string Description { get; set; } = "";
        public string? TeacherId { get; set; }
        public required List<CreateClassScheduleDTO> ClassSchedules { get; set; }

        public Section ToSection()
        {
            return new Section()
            {
                CourseId = CourseId,
                YearLevel = YearLevel,
                Name = Name,
                Description = Description,
                TeacherId = TeacherId,
            };
        }
    }

    public class SectionQueryDTO : BaseQueryDTO<GetSectionDTO>
    {
        public SectionQueryDTO(int totalCount, int totalPages, int page, int pageSize, List<Section> data)
        {
            TotalCount = totalCount;
            TotalPages = totalPages;
            Page = page;
            PageSize = pageSize;
            Data = [.. data.Select(s => new GetSectionDTO(s, false))];
        }

    }

}