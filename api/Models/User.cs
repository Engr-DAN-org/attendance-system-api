using System;
using api.Enums;
using Microsoft.AspNetCore.Identity;

namespace api.Models;

public class User : IdentityUser
{
    public required string IdNumber { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public UserRole UserRole { get; set; } = UserRole.Student;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    // ✅ Only Students can be assigned to a Section
    public int? SectionId { get; set; }
    public Section? Section { get; set; }

    // ✅ Only Students can have Guardians
    public string? GuardianId { get; set; }
    public Guardian? Guardian { get; set; }

    // Only Student can have Attendance Records
    public List<AttendanceRecord> AttendanceRecords { get; set; } = [];


    // ✅ Only Teachers can have Subjects
    public List<ClassSchedule> ClassSchedules { get; set; } = [];

    public string FullName => $"{FirstName} {LastName}";
    public string Role => UserRole.ToString();

}
