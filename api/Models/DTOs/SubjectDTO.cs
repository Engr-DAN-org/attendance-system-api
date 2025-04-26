using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models.Entities;

namespace api.Models.DTOs
{
    public class GetSubjectDTO(Subject subject, bool withRelation = true)
    {
        public int Id { get; set; } = subject.Id;
        public string Code { get; set; } = subject.Code;
        public string Name { get; set; } = subject.Name;
        public string? Description { get; set; } = subject.Description;

        // public List<ClassSchedule> ClassSchedules { get; set; } = subject.ClassSchedules.ToList();
        public List<GetSubjectTeacherDTO> SubjectTeachers { get; set; } = withRelation ? [.. subject.SubjectTeachers.Select(st => new GetSubjectTeacherDTO(st))] : [];
    }

    public class CreateSubjectDTO
    {
        public required string Code { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public List<CreateSubjectTeacherDTO> SubjectTeachers { get; set; } = [];

        public Subject ToSubject(int? id = null)
        {
            var subject = new Subject
            {
                Code = Code,
                Name = Name,
                Description = Description ?? string.Empty,
            };

            if (id.HasValue)
            {
                subject.Id = id.Value;
            }

            return subject;
        }

    }


    public class SubjectQueryDTO : BaseQueryDTO<GetSubjectDTO>
    {
        public SubjectQueryDTO(int totalCount, int totalPages, int page, int pageSize, List<Subject> data)
        {
            TotalCount = totalCount;
            TotalPages = totalPages;
            Page = page;
            PageSize = pageSize;
            Data = [.. data.Select(sub => new GetSubjectDTO(sub))];
        }
    }

}