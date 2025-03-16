using System;
using api.Enums;
using api.Utils;
using Microsoft.AspNetCore.Identity;

namespace api.Models.Entities;

public class User : IdentityUser
{
    public required string IdNumber { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public UserRole UserRole { get; set; } = UserRole.Student;
    public DateTime CreatedAt { get; set; } = DateTimeUtils.DateTimeNow();
    public DateTime UpdatedAt { get; set; } = DateTimeUtils.DateTimeNow();

    // ✅ Only Students can be assigned to a Section
    public int? SectionId { get; set; }
    public Section? Section { get; set; }

    // ✅ Only Students can have Guardians
    public int? GuardianId { get; set; }
    public Guardian? Guardian { get; set; }

    // Only Student can have Attendance Records
    public List<AttendanceRecord> AttendanceRecords { get; set; } = [];


    // ✅ Only Teachers can have Class Schedules
    public List<ClassSchedule> ClassSchedules { get; set; } = [];
    public List<ClassSession> ClassSessions { get; set; } = [];

    public string FullName => $"{FirstName} {LastName}";
    public string Role => UserRole.ToString();

}
