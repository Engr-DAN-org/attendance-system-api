using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Enums;
using api.Exceptions;
using api.Interfaces.Repository;
using api.Interfaces.Service;
using api.Models.DTOs;
using api.Models.Entities;

namespace api.Services
{
    public class StudentService(IUserRepository userRepository, IClassSessionRepository sessionRepository, IAttendanceRecordRepository recordRepository) : IStudentService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IAttendanceRecordRepository _recordRepository = recordRepository;
        private readonly IClassSessionRepository _sessionRepository = sessionRepository;


        public async Task<List<AttendanceRecord>> GetAttendanceRecordsAsync(string studentId)
        {
            var student = await _userRepository.FindByIdAsync(studentId);
            if (student == null || student.UserRole != UserRole.Student)
                throw new NotFoundException(nameof(User));

            return await _recordRepository.GetListByStudentIdAsync(studentId);
        }

        public async Task<AttendanceRecord> FindBySessionIdAsync(string studentId, string classSessionId)
        {
            return await _recordRepository.GetByClassSessionIdAndStudentIdAsync(classSessionId, studentId);
        }

        public async Task<GetStudentDTO?> GetStudentByIdAsync(string id)
        {
            var student = await _userRepository.FindByIdAsync(id);
            return student?.UserRole == UserRole.Student ? new GetStudentDTO(student) : null;
        }

        public async Task<AttendanceRecord> LogAttendanceAsync(string studentId, LogAttendanceRecordDTO attendanceDTO)
        {
            try
            {
                var student = await _userRepository.FindByIdAsync(studentId);
                if (student == null || student.UserRole != UserRole.Student)
                    throw new NotFoundException(nameof(User));

                var classSession = await _sessionRepository.GetByIdAsync(attendanceDTO.ClassSessionId, false);

                if (classSession.Status != ClassSessionStatus.Created && classSession.Status != ClassSessionStatus.Started)
                {
                    throw new InvalidOperationException("You may be too late. Please contact your teacher.");
                }

                return await _recordRepository.LogAttendanceAsync(classSession, attendanceDTO);
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}