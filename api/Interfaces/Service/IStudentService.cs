using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models.DTOs;
using api.Models.Entities;

namespace api.Interfaces.Service
{
    public interface IStudentService
    {
        public Task<GetStudentDTO?> GetStudentByIdAsync(string id);
        public Task<AttendanceRecord> LogAttendanceAsync(string studentId, LogAttendanceRecordDTO attendanceDTO);
        public Task<List<AttendanceRecord>> GetAttendanceRecordsAsync(string studentId);
        public Task<AttendanceRecord> FindBySessionIdAsync(string studentId, string classSessionId);
    }
}