using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Exceptions;
using api.Interfaces.Repository;
using api.Models.DTOs;
using api.Models.Entities;
using api.Models.QueryParams;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class ClassScheduleRepository(AppDbContext dbContext) : IClassScheduleRepository
    {
        private readonly AppDbContext _dbContext = dbContext;
        public async Task<GetClassScheduleDTO> CreateScheduleAsync(CreateClassScheduleDTO scheduleDTO)
        {
            var newData = scheduleDTO.ToClassSchedule();
            var classSchedule = _dbContext.ClassSchedules.Add(newData);
            await _dbContext.SaveChangesAsync();
            return new GetClassScheduleDTO(classSchedule.Entity);
        }



        public async Task<ClassSchedule> GetScheduleByIdAsync(int id, bool? includeNullSection = true)
        {
            return await _dbContext.ClassSchedules.AsQueryable()
                    .Include(cs => cs.SubjectTeacher)
                       .ThenInclude(st => st.Subject)
                   .Include(cs => cs.SubjectTeacher)
                       .ThenInclude(st => st.Teacher)
                   .Include(cs => cs.Section)
                   .Where(cs => includeNullSection == true || cs.SectionId != null)
                   .AsNoTracking()
                   .FirstOrDefaultAsync(s => s.Id == id)
                    ?? throw new NotFoundException(nameof(ClassSchedule));
        }

        public async Task<List<GetClassScheduleDTO>> QueryAsync(ClassScheduleQueryParams queryParams)
        {
            var query = _dbContext.ClassSchedules
                    .Include(cs => cs.SubjectTeacher)
                       .ThenInclude(st => st.Subject)
                   .Include(cs => cs.SubjectTeacher)
                       .ThenInclude(st => st.Teacher)
                       .AsQueryable();

            if (!string.IsNullOrEmpty(queryParams.TeacherName))
            {
                var searchName = queryParams.TeacherName.ToLower();
                query = query.Where(cs => cs.SubjectTeacher != null
                        && $"{cs.SubjectTeacher.Teacher.FirstName} {cs.SubjectTeacher.Teacher.LastName}"
                        .ToLower()
                        .Contains(searchName));
            }

            if (!string.IsNullOrEmpty(queryParams.SubjectName))
            {
                var searchName = queryParams.SubjectName.ToLower();
                query = query.Where(cs => cs.SubjectTeacher != null && (cs.SubjectTeacher.Subject.Name.ToLower().Contains(searchName) || cs.SubjectTeacher.Subject.Code.ToLower().Contains(searchName)));
            }
            if (queryParams.Unassigned == true)
            {
                query = query.Where(cs => cs.SectionId == null);
            }
            if (queryParams.Day != null)
            {
                query = query.Where(cs => cs.Day == queryParams.Day);
            }
            if (!string.IsNullOrEmpty(queryParams.StartTime))
            {
                query = query.Where(cs => cs.StartTime == queryParams.StartTime);
            }
            if (!string.IsNullOrEmpty(queryParams.EndTime))
            {
                query = query.Where(cs => cs.EndTime == queryParams.EndTime);
            }

            return await query.Include(cs => cs.Section)
                   .Select(cs => new GetClassScheduleDTO(cs, true))
                   .ToListAsync();
        }

        public async Task<List<GetClassScheduleDTO>> GetBySectionOrTeacherAsync(ScheduleTeacherSectionQuery queryParams)
        {
            Console.WriteLine($"SectionId is set: {queryParams.SectionId}");
            Console.WriteLine($"TeacherId is set: {queryParams.TeacherId}");
            var query = _dbContext.ClassSchedules
                       .AsQueryable();

            if (!string.IsNullOrEmpty(queryParams.SectionId.ToString()))
            {
                query = query.Where(cs => cs.SectionId != null && cs.SectionId == queryParams.SectionId);
            }
            if (!string.IsNullOrEmpty(queryParams.TeacherId))
            {
                query = query.Where(cs => cs.SubjectTeacher != null && cs.SubjectTeacher.TeacherId == queryParams.TeacherId);
            }
            return await query
                    .Include(cs => cs.Section)
                    .Include(cs => cs.SubjectTeacher)
                       .ThenInclude(st => st.Subject)
                    .Include(cs => cs.SubjectTeacher)
                       .ThenInclude(st => st.Teacher)
                   .Select(cs => new GetClassScheduleDTO(cs, true))
                   .ToListAsync();
        }

        public async Task<GetClassScheduleDTO> UpdateScheduleAsync(int id, CreateClassScheduleDTO scheduleDTO)
        {
            var schedule = await GetScheduleByIdAsync(id);

            schedule.StartTime = scheduleDTO.StartTime;
            schedule.EndTime = scheduleDTO.EndTime;
            schedule.SubjectTeacherId = scheduleDTO.SubjectTeacherId;
            schedule.SectionId = scheduleDTO.SectionId;
            schedule.Day = scheduleDTO.Day;
            schedule.GracePeriod = scheduleDTO.GracePeriod;
            schedule.UpdatedAt = DateTime.UtcNow;

            var updatedSchedule = _dbContext.ClassSchedules.Update(schedule);
            await _dbContext.SaveChangesAsync();
            return new GetClassScheduleDTO(updatedSchedule.Entity, true);
        }

        public async Task DeleteScheduleAsync(int id)
        {
            var schedule = await GetScheduleByIdAsync(id);
            _dbContext.ClassSchedules.Remove(schedule);
            await _dbContext.SaveChangesAsync();
        }

    }
}