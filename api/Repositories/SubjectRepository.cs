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
    public class SubjectRepository(AppDbContext context) : ISubjectRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<GetSubjectDTO> CreateSubjectAsync(CreateSubjectDTO subject)
        {
            try
            {
                var newSubject = subject.ToSubject();
                await _context.Subjects.AddAsync(newSubject);
                await _context.SaveChangesAsync();

                return new GetSubjectDTO(newSubject);
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public async Task DeleteSubjectAsync(int id)
        {
            try
            {
                var subject = await FindByIdAsync(id);
                _context.Subjects.Remove(subject);
                await _context.SaveChangesAsync();
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public async Task<GetSubjectDTO> GetSubjectByIdAsync(int id)
        {
            try
            {
                var subject = await FindByIdAsync(id);
                return new GetSubjectDTO(subject);
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public async Task<SubjectQueryDTO> QuerySubjectsAsync(SubjectQueryParams queryParams)
        {
            try
            {
                var query = _context.Subjects.AsQueryable();

                if (!string.IsNullOrEmpty(queryParams.TeacherId))
                {
                    query = query.Where(s => s.ClassSchedules.Any(sched => sched.TeacherId == queryParams.TeacherId));
                }
                if (!string.IsNullOrEmpty(queryParams.Name))
                {
                    var searchName = queryParams.Name.ToLower();
                    query = query.Where(c => c.Name.ToLower().Contains(searchName) || c.Code.ToLower().Contains(searchName));
                }

                var totalCount = query.Count();
                var totalPages = (int)Math.Ceiling((double)totalCount / queryParams.PageSize);
                var subjects = await query
                    .Skip((queryParams.Page - 1) * queryParams.PageSize)
                    .Take(queryParams.PageSize)
                    .ToListAsync();

                return new SubjectQueryDTO(totalCount, totalPages, queryParams.Page, queryParams.PageSize, subjects);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public async Task<GetSubjectDTO> UpdateSubjectAsync(int id, CreateSubjectDTO subject)
        {
            try
            {
                var existingSubject = await FindByIdAsync(id);

                var updatedSubject = subject.ToSubject(id);

                _context.Subjects.Update(updatedSubject);
                await _context.SaveChangesAsync();

                return new GetSubjectDTO(existingSubject);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        private async Task<Subject> FindByIdAsync(int id)
        {
            return await _context.Subjects.FindAsync(id) ?? throw new NotFoundException(nameof(Subject));

        }
    }
}