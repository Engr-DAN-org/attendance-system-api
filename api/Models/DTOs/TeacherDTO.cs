using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Enums;
using api.Models.Entities;

namespace api.Models.DTOs
{

    public class CreateTeacherDTO
    {
        public required string IdNumber { get; set; }
        public required string Email { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public UserRole UserRole { get; } = UserRole.Teacher;
    }

    public class GetTeacherDTO(User user)
    {
        public string IdNumber { get; set; } = user.IdNumber;
        public string Email { get; set; } = user.Email ?? "";
        public string FullName { get; set; } = user.FullName;
        public string Role { get; set; } = user.Role;
        public List<ClassSchedule> ClassSchedules { get; set; } = user.ClassSchedules;
        public List<ClassSession> ClassSessions { get; set; } = user.ClassSessions;

    }

    public class UpdateTeacherDTO
    {
        public required string IdNumber { get; set; }
        public required string Email { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
    }

    public class DeleteTeacherDTO
    {
        public required string IdNumber { get; set; }
        public UserRole UserRole { get; set; } = UserRole.Teacher;
    }
}