using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Enums;

namespace api.Models.DTOs
{
    public class TeacherQueryDTO
    {
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string IdNumber { get; set; } = "";
        public UserRole UserRole { get; set; } = UserRole.Teacher;
    }

    public class StudentQueryDTO
    {
        public string Name { get; set; } = "";
        public string IdNumber { get; set; } = "";
        public int? YearLevel { get; set; }

        public string SectionId { get; set; } = "";
        public string GuardianName = "";
        public UserRole UserRole { get; set; } = UserRole.Student;
        public int Page = 1;
        public int PageSize = 10;
    }
}