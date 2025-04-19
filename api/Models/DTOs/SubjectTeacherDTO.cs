using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models.Entities;

namespace api.Models.DTOs
{
    public class GetSubjectTeacherDTO(SubjectTeacher subjectTeacher)
    {
        public int Id { get; set; } = subjectTeacher.Id;
        public int SubjectId { get; set; } = subjectTeacher.SubjectId;

        public string SubjectCode { get; set; } = subjectTeacher.Subject?.Code ?? "";
        public string SubjectName { get; set; } = subjectTeacher.Subject?.Name ?? "";
        public string TeacherName { get; set; } = subjectTeacher.Teacher?.FullName ?? "";
        public string TeacherId { get; set; } = subjectTeacher.TeacherId;
    }

    public class CreateSubjectTeacherDTO
    {
        public required string TeacherId { get; set; }
        public int? SubjectId { get; set; }
        public SubjectTeacher ToModel(int subjectId)
        {
            return new SubjectTeacher
            {
                SubjectId = subjectId,
                TeacherId = TeacherId
            };
        }
    }
}