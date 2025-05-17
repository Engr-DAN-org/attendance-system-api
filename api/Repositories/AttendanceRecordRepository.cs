using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Enums;
using api.Exceptions;
using api.Interfaces.Repository;
using api.Models.DTOs;
using api.Models.Entities;
using api.Models.QueryParams;
using api.Utils;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class AttendanceRecordRepository(AppDbContext context) : IAttendanceRecordRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<AttendanceRecord> CreateAsync(string classSessionId, string studentId)
        {
            var record = await _context.AttendanceRecords.AddAsync(new AttendanceRecord
            {
                ClassSessionId = classSessionId,
                StudentId = studentId,
            });
            await _context.SaveChangesAsync();
            return record.Entity;
        }

        public async Task FinalizeAsAbsentAsync(AttendanceRecord attendanceRecord)
        {
            attendanceRecord.Status = AttendanceStatus.Absent;
            attendanceRecord.UpdatedAt = DateTimeUtils.DateTimeNow();

            _context.AttendanceRecords.Update(attendanceRecord);
            await _context.SaveChangesAsync();
        }

        public async Task<AttendanceRecord> GetByClassSessionIdAndStudentIdAsync(string classSessionId, string studentId, bool? includesRelation = false)
        {
            var query = _context.AttendanceRecords.AsQueryable()
                .Include(x => x.ClassSession)
                .Include(x => x.Student)
                .Where(x => x.ClassSessionId == classSessionId && x.StudentId == studentId);
            return await query.FirstOrDefaultAsync() ?? throw new NotFoundException(nameof(AttendanceRecord));
        }

        public async Task<AttendanceRecord> GetByIdAsync(string id)
        {
            return await _context.AttendanceRecords.AsQueryable()
                .Include(x => x.ClassSession)
                .Include(x => x.Student)
                .FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new NotFoundException(nameof(AttendanceRecord));
        }

        public async Task<List<AttendanceRecord>> GetListByClassSessionIdAsync(string classSessionId)
        {
            var query = _context.AttendanceRecords.AsQueryable()
                .Include(x => x.ClassSession)
                .Include(x => x.Student)
                .Where(x => x.ClassSessionId == classSessionId);
            return await query.ToListAsync();
        }

        public async Task<List<AttendanceRecord>> GetListByStudentIdAsync(string studentId)
        {
            var query = _context.AttendanceRecords.AsQueryable()
                .Include(x => x.ClassSession)
                .Include(x => x.Student)
                .Where(x => x.StudentId == studentId);
            return await query.ToListAsync();
        }

        public async Task<AttendanceRecord> LogAttendanceAsync(string studentId, ClassSession classSession, LogAttendanceRecordDTO recordDTO)
        {
            var attendanceRecord = await _context.AttendanceRecords
                    .FirstOrDefaultAsync(x => x.ClassSessionId == recordDTO.ClassSessionId && x.StudentId == studentId)
                    ?? throw new NotFoundException(nameof(AttendanceRecord));

            if (attendanceRecord.Status != AttendanceStatus.Unmarked)
                throw new InvalidOperationException("Attendance already marked. Contact your teacher if this is a mistake.");

            var currentTime = DateTimeUtils.DateTimeNow();

            if (classSession.GraceTime != null && DateTime.TryParse(classSession.GraceTime, out var graceTime) && graceTime < currentTime)
                attendanceRecord.Status = AttendanceStatus.Late;
            else
                attendanceRecord.Status = AttendanceStatus.Present;

            attendanceRecord.Location = recordDTO.Location;
            attendanceRecord.Latitude = recordDTO.Latitude;
            attendanceRecord.Longitude = recordDTO.Longitude;
            attendanceRecord.Distance = recordDTO.Distance;
            attendanceRecord.UpdatedAt = currentTime;
            attendanceRecord.ClockInRecord = currentTime;

            var updatedLog = _context.AttendanceRecords.Update(attendanceRecord);
            await _context.SaveChangesAsync();
            return updatedLog.Entity;
        }


        public async Task OverrideRecord(string teacherId, AttendanceRecord attendanceRecord, AttendanceStatus status)
        {
            attendanceRecord.Status = status;
            attendanceRecord.IsOverRidden = true;
            attendanceRecord.UpdatedAt = DateTimeUtils.DateTimeNow();
            attendanceRecord.OverriddenAt = DateTimeUtils.DateTimeNow();
            attendanceRecord.OverriddenBy = teacherId;

            _context.AttendanceRecords.Update(attendanceRecord);
            await _context.SaveChangesAsync();
        }

        public async Task<List<GetAttendanceRecordDTO>> QueryAsync(AttendanceRecordQueryParams queryParams)
        {
            var query = _context.AttendanceRecords
                .Include(x => x.ClassSession)
                .Include(x => x.Student)
                .AsQueryable();

            if (!string.IsNullOrEmpty(queryParams.ClassSessionId))
                query = query.Where(x => x.ClassSessionId == queryParams.ClassSessionId);
            if (!string.IsNullOrEmpty(queryParams.StudentIdNumber))
                query = query.Where(x => x.Student!.IdNumber == queryParams.StudentIdNumber);
            if (queryParams.ClassScheduleId != null)
                query = query.Where(x => x.ClassSession!.ClassScheduleId == queryParams.ClassScheduleId);
            if (queryParams.Status.Length > 0)
                query = query.Where(x => queryParams.Status.Contains(x.Status));
            if (queryParams.Paginate == true)
            {
                query = query
                    .Skip((queryParams.Page - 1) * queryParams.PageSize)
                    .Take(queryParams.PageSize);
            }

            var records = await query.ToListAsync();

            return [.. records.Select(x => new GetAttendanceRecordDTO(x, false))];

        }
    }
}