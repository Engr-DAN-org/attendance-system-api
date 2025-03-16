using api.Models.Entities;

namespace api.Models.DTOs
{
    public class GetStudentDTO
    {
        public required string IdNumber { get; set; }
        public required string Email { get; set; }
        public required string Role { get; set; }
        public required int YearLevel { get; set; }
        public required string Section { get; set; }
        public required string FullName { get; set; }
        public required List<AttendanceRecord> AttendanceRecords { get; set; }

        public GetGuardianDTO? Guardian { get; set; }
    }

}