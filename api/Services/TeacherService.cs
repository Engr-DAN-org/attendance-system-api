using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Enums;
using api.Interfaces.Repository;
using api.Interfaces.Service;
using api.Models.DTOs;
using api.Models.Entities;
using api.Models.QueryParams;
using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class TeacherService(
        IClassScheduleRepository scheduleRepository,
        IClassSessionRepository sessionRepository,
        IAttendanceRecordRepository recordRepository
        ) : ITeacherService
    {
        private readonly IClassScheduleRepository _scheduleRepository = scheduleRepository;
        private readonly IClassSessionRepository _sessionRepository = sessionRepository;
        private readonly IAttendanceRecordRepository _recordRepository = recordRepository;

        public async Task<ClassSession> CancelClassSessionAsync(string teacherId, string sessionId)
        {
            try
            {
                // Start a transaction
                await _sessionRepository.BeginTransactionAsync();

                var classSession = await _sessionRepository.CancelClassSessionAsync(sessionId);

                var attendanceRecords = await _recordRepository.GetListByClassSessionIdAsync(classSession.Id);
                foreach (var record in attendanceRecords)
                {
                    await OverrideAsCanceledAsync(teacherId, record);
                }

                // Save changes to the database
                await _sessionRepository.CommitTransactionAsync();

                return classSession;
            }
            catch (System.Exception)
            {
                // Rollback the transaction in case of an error
                await _sessionRepository.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<GetClassScheduleDTO> GetScheduleByIdAsync(string teacherId, int scheduleId)
        {
            var classSchedule = await _scheduleRepository.GetScheduleByIdAsync(scheduleId, false);

            if (classSchedule?.SubjectTeacher?.TeacherId != teacherId)
                throw new UnauthorizedAccessException("You are not authorized to access this schedule.");

            return new GetClassScheduleDTO(classSchedule, true);
        }

        public async Task<ClassSession> EndClassSessionAsync(string teacherId, string sessionId)
        {
            try
            {
                // Start a transaction
                await _sessionRepository.BeginTransactionAsync();

                var classSession = await _sessionRepository.EndClassSessionAsync(sessionId);
                var attendanceRecords = await _recordRepository.GetListByClassSessionIdAsync(classSession.Id);
                foreach (var record in attendanceRecords)
                {
                    if (record.Status == AttendanceStatus.Unmarked)
                    {
                        await _recordRepository.FinalizeAsAbsentAsync(record);
                    }
                }
                // Save changes to the database
                await _sessionRepository.CommitTransactionAsync();

                return classSession;
            }
            catch (System.Exception)
            {
                // Rollback the transaction in case of an error
                await _sessionRepository.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<List<GetClassScheduleDTO>> GetClassSchedulesAsync(string teacherId)
        {
            var queryParams = new ScheduleTeacherSectionQuery
            {
                TeacherId = teacherId
            };

            return await _scheduleRepository.GetBySectionOrTeacherAsync(queryParams);
        }
        public async Task<ClassSession> StartClassSessionAsync(string teacherId, CreateClassSessionDTO dto)
        {
            try
            {
                var classSchedule = await _scheduleRepository.GetScheduleByIdAsync(dto.ClassScheduleId, false);

                if (classSchedule?.SubjectTeacher?.TeacherId != teacherId)
                    throw new UnauthorizedAccessException("You are not authorized to access this schedule.");

                await _sessionRepository.BeginTransactionAsync();

                var studentList = classSchedule.Section?.Students ?? throw new Exception("No Students in Section.");

                var classSession = await _sessionRepository.CreateAsync(classSchedule, dto);

                foreach (var student in studentList)
                {
                    await _recordRepository.CreateAsync(classSession.Id, student.Id);
                }
                await _sessionRepository.CommitTransactionAsync();
                return classSession;
            }
            catch (System.Exception)
            {
                await _sessionRepository.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task OverRideRecordAsync(string teacherId, OverrideAttendanceRecordDTO dto)
        {
            var attendanceRecord = await _recordRepository.GetByIdAsync(dto.AttendanceRecordId);
            await _recordRepository.OverrideRecord(teacherId, attendanceRecord, dto.Status);
        }

        private async Task OverrideAsCanceledAsync(string teacherId, AttendanceRecord attendanceRecord)
        {
            await _recordRepository.OverrideRecord(teacherId, attendanceRecord, AttendanceStatus.Canceled);
        }
    }
}
