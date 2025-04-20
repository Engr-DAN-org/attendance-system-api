using System;
using api.Models.DTOs;
using api.Utils;

namespace api.Models.Entities;

public class Subject
{
    public int Id { get; set; }
    public required string Code { get; set; }
    public required string Name { get; set; }
    public string Description { get; set; } = "";
    public List<ClassSchedule> ClassSchedules { get; set; } = [];
    public List<SubjectTeacher> SubjectTeachers { get; set; } = [];
    public DateTime CreatedAt { get; set; } = DateTimeUtils.DateTimeNow();
    public DateTime UpdatedAt { get; set; } = DateTimeUtils.DateTimeNow();

    public Subject UpdateSubject(CreateSubjectDTO subjectDTO)
    {
        Code = subjectDTO.Code;
        Name = subjectDTO.Name;
        Description = subjectDTO.Description ?? "";
        UpdatedAt = DateTimeUtils.DateTimeNow();
        return this;
    }
}
