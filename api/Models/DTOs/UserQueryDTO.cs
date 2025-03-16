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
        public int Page { get; set; } = 1;
    }

    public class StudentQueryDTO
    {
        public string Name { get; set; } = "";
        public string IdNumber { get; set; } = "";
        public int? YearLevel { get; set; }
        public string Email { get; set; } = "";
        public string SectionId { get; set; } = "";
        public string GuardianName = "";
        public int Page { get; set; } = 1;
    }
}