using api.Enums;
using api.Models.Entities;

namespace api.Models.DTOs
{
    public class CreateStudentDTO
    {
        public required string IdNumber { get; set; }
        public required string Email { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required int SectionId { get; set; }
        public UserRole UserRole { get; } = UserRole.Student;
        public required CreateGuardianDTO Guardian { get; set; }
    }

    public class GetStudentDTO(User user)
    {
        public string IdNumber { get; set; } = user.IdNumber;
        public string Email { get; set; } = user.Email ?? "";
        public string Role { get; set; } = user.Role;
        public int YearLevel { get; set; } = user.Section?.YearLevel ?? 0;
        public string SectionName { get; set; } = user.Section?.Name ?? "";
        public string FullName { get; set; } = user.FullName;
        public List<AttendanceRecord> AttendanceRecords { get; set; } = user.AttendanceRecords;
        public GetGuardianDTO? Guardian { get; set; } = user.Guardian != null ? new GetGuardianDTO(user.Guardian) : null;

    }
    public class UpdateStudentDTO
    {
        public required string IdNumber { get; set; }
        public required string Email { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required int YearLevel { get; set; }
        public required int SectionId { get; set; }
        public UpdateGuardianDTO? Guardian { get; set; }
    }
    public class DeleteStudentDTO
    {
        public required string IdNumber { get; set; }
        public UserRole UserRole { get; set; } = UserRole.Student;
    }

}