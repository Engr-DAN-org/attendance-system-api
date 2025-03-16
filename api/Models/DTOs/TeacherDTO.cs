using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models.Entities;

namespace api.Models.DTOs
{

    public class GetTeacherDTO
    {
        public required string IdNumber { get; set; }
        public required string Email { get; set; }
        public required string FullName { get; set; }
        public required string Role { get; set; }
        public required List<ClassSchedule> ClassSchedules { get; set; }
        public required List<ClassSession> ClassSessions { get; set; }
    }
}