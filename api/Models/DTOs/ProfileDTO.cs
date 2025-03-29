using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Enums;
using api.Models.Entities;

namespace api.Models.DTOs
{
    public class GetProfileDTO(User user)
    {
        public string IdNumber { get; set; } = user.IdNumber;
        public string FirstName { get; set; } = user.FirstName;
        public string LastName { get; set; } = user.LastName;

        // ✅ Only Students can be assigned to a Section
        public int? SectionId { get; set; } = user.SectionId;
        public Section? Section { get; set; } = user.Section;

        // ✅ Only Students can have Guardians
        public int? GuardianId { get; set; } = user.GuardianId;
        public GetGuardianDTO? Guardian { get; set; } = user?.Guardian != null ? new GetGuardianDTO(user.Guardian) : null;

        // Only Student can have Attendance Records
        // public List<AttendanceRecord> AttendanceRecords { get; set; } = user.AttendanceRecords ?? [];


        // ✅ Only Teachers can have Class Schedules
        // public List<ClassSchedule> ClassSchedules { get; set; } = user.ClassSchedules ?? [];
        // public List<ClassSession> ClassSessions { get; set; } = user.ClassSessions ?? [];

        public string FullName => $"{FirstName} {LastName}";
        public string? Role = user?.UserRole.ToString();

    }


    public class UpdateProfileDTO
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}