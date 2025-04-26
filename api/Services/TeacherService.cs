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
using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class TeacherService(AppDbContext context) : ITeacherService
    {
        private readonly AppDbContext _context = context;

        public async Task<List<GetClassScheduleDTO>> GetClassSchedulesAsync(string teacherId)
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var data = await _context.ClassSchedules
                 .Include(cs => cs.SubjectTeacher)
                 .ThenInclude(st => st.Subject)
                 .Include(cs => cs.Section)
                 .ThenInclude(s => s.Course)
                 .Where(cs => cs.SubjectTeacher != null && cs.SubjectTeacher.TeacherId == teacherId)
                 .ToListAsync();

            return [.. data.Select(cs => new GetClassScheduleDTO(cs))];
        }
    }
}
