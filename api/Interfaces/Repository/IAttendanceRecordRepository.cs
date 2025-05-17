using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Enums;
using api.Models.DTOs;
using api.Models.Entities;
using api.Models.QueryParams;

namespace api.Interfaces.Repository
{
    public interface IAttendanceRecordRepository
    {
        Task<AttendanceRecord> CreateAsync(string classSessionId, string studentId);
        Task<AttendanceRecord> GetByIdAsync(string id);
        Task<AttendanceRecord> GetByClassSessionIdAndStudentIdAsync(string classSessionId, string studentId, bool? includesRelation = false);
        Task<AttendanceRecord> LogAttendanceAsync(string studentId, ClassSession classSession, LogAttendanceRecordDTO recordDTO);

        Task<List<GetAttendanceRecordDTO>> QueryAsync(AttendanceRecordQueryParams queryParams);
        Task<List<AttendanceRecord>> GetListByClassSessionIdAsync(string classSessionId);
        Task<List<AttendanceRecord>> GetListByStudentIdAsync(string studentId);
        Task FinalizeAsAbsentAsync(AttendanceRecord attendanceRecord);
        Task OverrideRecord(string teacherId, AttendanceRecord attendanceRecord, AttendanceStatus status);
    }

}