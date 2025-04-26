using System;
using System.Collections.Generic;
using System.Data;
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
    public class SectionRepository(AppDbContext context) : ISectionRepository
    {
        private readonly AppDbContext _context = context;


        public async Task<Section> CreateSectionAsync(CreateSectionDTO createSectionDTO)
        {
            try
            {
                await BeginTransactionAsync();
                Console.WriteLine($"Creating Section with CourseId: {createSectionDTO.CourseId}");
                var existingSection = await _context.Sections.FirstOrDefaultAsync(sec => sec.Name == createSectionDTO.Name && sec.YearLevel == createSectionDTO.YearLevel);
                if (existingSection != null)
                    throw new DuplicateNameException("Section already exists");

                var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == createSectionDTO.CourseId) ?? throw new NotFoundException(nameof(Course));
                Console.WriteLine($"Course found: {course.Name}, ID: {course.Id}");

                createSectionDTO.CourseId = course.Id;
                var section = await _context.Sections.AddAsync(createSectionDTO.ToSection());
                await _context.SaveChangesAsync();

                await _context.ClassSchedules.AddRangeAsync(createSectionDTO.ClassSchedules.Select(cs => cs.ToClassSchedule(section.Entity.Id)));
                await _context.SaveChangesAsync();

                await CommitTransactionAsync();
                return section.Entity;
            }
            catch (System.Exception e)
            {
                await RollbackTransactionAsync();
                Console.WriteLine($"Error: {e.Message}");
                throw;
            }
        }

        public async Task<Section> UpdateSectionAsync(int sectionId, CreateSectionDTO createSectionDTO)
        {
            try
            {
                await BeginTransactionAsync();
                var section = await GetSectionByIdAsync(sectionId);

                section.YearLevel = createSectionDTO.YearLevel;
                section.Name = createSectionDTO.Name;
                section.Description = createSectionDTO.Description;
                section.TeacherId = createSectionDTO.TeacherId;

                var classSchedules = await _context.ClassSchedules.Where(cs => cs.SectionId == sectionId).ToListAsync();
                _context.ClassSchedules.RemoveRange(classSchedules);
                await _context.SaveChangesAsync();

                await _context.ClassSchedules.AddRangeAsync(createSectionDTO.ClassSchedules.Select(cs => cs.ToClassSchedule(sectionId)));
                await _context.SaveChangesAsync();

                await CommitTransactionAsync();
                return section;
            }
            catch (System.Exception)
            {
                await RollbackTransactionAsync();
                throw;
            }
        }

        public async Task DeleteSectionAsync(int id)
        {
            try
            {
                var section = await GetSectionByIdAsync(id);
                _context.Sections.Remove(section);
                await _context.SaveChangesAsync();
            }
            catch (System.Exception)
            {
                throw;
            }

        }

        public async Task<Section> GetSectionByIdAsync(int id)
        {
            return await _context.Sections
                .Include(sec => sec.ClassSchedules)
                .ThenInclude(cs => cs.SubjectTeacher)
                .ThenInclude(st => st.Subject)
                .Include(sec => sec.Course)
                .Include(sec => sec.Students)
                .FirstOrDefaultAsync(sec => sec.Id == id) ?? throw new NotFoundException(nameof(Section));
        }

        public async Task<List<Section>> GetSectionByTeacherIdAsync(string id)
        {
            return await _context.Sections.Where(sec => sec.TeacherId == id).ToListAsync() ?? throw new NotFoundException(nameof(Section));
        }

        public async Task<SectionQueryDTO> GetSectionsAsync(SectionQueryParams queryParams)
        {
            var sections = _context.Sections.AsQueryable();

            var totalCount = sections.Count();
            var totalPages = (int)Math.Ceiling((double)totalCount / queryParams.PageSize);
            var data = await sections.Skip((queryParams.Page - 1) * queryParams.PageSize)
                .Take(queryParams.PageSize)
                .Include(sec => sec.ClassSchedules)
                .Include(sec => sec.Course)
                .Include(sec => sec.Teacher)
                .Include(sec => sec.Students)
                .ToListAsync();

            return new SectionQueryDTO(totalCount, totalPages, queryParams.Page, queryParams.PageSize, data);
        }

        public async Task RollbackTransactionAsync()
        {

            await _context.Database.RollbackTransactionAsync();
        }

        public async Task BeginTransactionAsync()
        {
            await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await _context.Database.CommitTransactionAsync();
        }

    }
}