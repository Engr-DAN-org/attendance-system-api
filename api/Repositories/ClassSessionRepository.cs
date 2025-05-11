using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Enums;
using api.Exceptions;
using api.Interfaces.Repository;
using api.Interfaces.Service;
using api.Models.DTOs;
using api.Models.Entities;
using api.Utils;
using Microsoft.EntityFrameworkCore;
using MimeKit.Utils;

namespace api.Repositories
{
    public class ClassSessionRepository(AppDbContext context) : IClassSessionRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<ClassSession> CancelClassSessionAsync(string id)
        {
            try
            {
                var classSession = await GetByIdAsync(id, true);
                // Check if the class session is already canceled or ended
                if (classSession.Status == ClassSessionStatus.Canceled || classSession.Status == ClassSessionStatus.Ended)
                {
                    throw new InvalidOperationException("Class session is already canceled or ended.");
                }

                var now = DateTimeUtils.DateTimeNow();

                classSession.Status = ClassSessionStatus.Canceled;
                classSession.UpdatedAt = now;
                _context.ClassSessions.Update(classSession);
                await _context.SaveChangesAsync();

                return classSession;
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public async Task<ClassSession> CreateAsync(ClassSchedule classSchedule, CreateClassSessionDTO dto)
        {
            try
            {
                var sessionDTO = dto.ToClassSession(classSchedule);
                var newSession = await _context.ClassSessions.AddAsync(sessionDTO);


                await _context.SaveChangesAsync();

                return newSession.Entity;
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public async Task<ClassSession> EndClassSessionAsync(string id)
        {
            try
            {
                var classSession = await GetByIdAsync(id, true);
                var now = DateTimeUtils.DateTimeNow();

                // Check if the class session is already canceled or ended
                if (classSession.Status == ClassSessionStatus.Canceled || classSession.Status == ClassSessionStatus.Ended)
                {
                    throw new InvalidOperationException("Class session is already canceled or ended.");
                }

                classSession.Status = ClassSessionStatus.Ended;
                classSession.EndTime = now;
                classSession.UpdatedAt = now;
                _context.ClassSessions.Update(classSession);
                await _context.SaveChangesAsync();

                return classSession;
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public async Task<List<ClassSession>> GetListByClassScheduleIdAsync(int classScheduleId)
        {
            var query = _context.ClassSessions.Where(x => x.ClassScheduleId == classScheduleId)
                .AsQueryable()
                .Include(x => x.AttendanceRecords);

            return await query.ToListAsync();
        }

        public async Task<ClassSession> GetByIdAsync(string id, bool? includesRelation = false)
        {
            var query = _context.ClassSessions
                .AsQueryable();
            if (includesRelation == true)
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                query = query.Include(cs => cs.AttendanceRecords)
                        .ThenInclude(ar => ar.Student)
                    .Include(cs => cs.ClassSchedule)
                        .ThenInclude(cs => cs.SubjectTeacher)
                            .ThenInclude(st => st.Subject)
                    .Include(cs => cs.ClassSchedule)
                        .ThenInclude(cs => cs.SubjectTeacher)
                            .ThenInclude(st => st.Teacher);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            }

            return await query.FirstOrDefaultAsync(x => x.Id == id) ?? throw new NotFoundException(nameof(ClassSession));
        }

        public async Task BeginTransactionAsync()
        {
            await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await _context.Database.CommitTransactionAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            await _context.Database.RollbackTransactionAsync();
        }
    }
}