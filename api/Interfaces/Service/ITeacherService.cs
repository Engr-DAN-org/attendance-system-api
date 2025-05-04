using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models.DTOs;
using api.Models.Entities;

namespace api.Interfaces.Service
{
    public interface ITeacherService
    {
        Task<List<GetClassScheduleDTO>> GetClassSchedulesAsync(string teacherId);
        Task<GetClassScheduleDTO> GetScheduleByIdAsync(string teacherId, int scheduleId);

        Task<ClassSession> StartClassSessionAsync(string teacherId, CreateClassSessionDTO dto);
        Task<ClassSession> EndClassSessionAsync(string teacherId, string sessionId);
        Task<ClassSession> CancelClassSessionAsync(string teacherId, string sessionId);
        Task OverRideRecordAsync(string teacherId, OverrideAttendanceRecordDTO dto);
    }
}