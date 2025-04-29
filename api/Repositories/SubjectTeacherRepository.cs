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
    public class SubjectTeacherRepository(AppDbContext dbContext) : ISubjectTeacherRepository
    {
        private readonly AppDbContext _dbContext = dbContext;

        public async Task<GetSubjectTeacherDTO> GetByIdAsync(int id)
        {
            var data = await _dbContext.SubjectTeachers
                .Include(st => st.Subject)
                .Include(st => st.Teacher)
                .FirstOrDefaultAsync(st => st.Id == id) ?? throw new NotFoundException(nameof(SubjectTeacher));
            return new GetSubjectTeacherDTO(data);
        }

        public async Task<List<GetSubjectTeacherDTO>> QueryAsync(SubjectTeacherQueryParams queryParams)
        {
            var query = _dbContext.SubjectTeachers
                .Include(st => st.Subject)
                .Include(st => st.Teacher)
                .AsQueryable();

            if (!string.IsNullOrEmpty(queryParams.Name))
            {
                var searchName = queryParams.Name.ToLower();

                query = query.Where(st =>
                    (st.Subject != null &&
                        ((st.Subject.Name != null && st.Subject.Name.ToLower().Contains(searchName)) ||
                         (st.Subject.Code != null && st.Subject.Code.ToLower().Contains(searchName)))
                    ) ||
                    (st.Teacher != null &&
                        ((st.Teacher.FirstName != null ? st.Teacher.FirstName : "") + " " +
                        (st.Teacher.LastName != null ? st.Teacher.LastName : "")).ToLower().Contains(searchName)
                    )
                );
            }
            if (!string.IsNullOrEmpty(queryParams.TeacherId))
            {
                query = query.Where(st => st.TeacherId == queryParams.TeacherId);
            }
            if (queryParams.SubjectId.HasValue)
            {
                query = query.Where(st => st.SubjectId == queryParams.SubjectId);
            }
            if (queryParams.Limit is > 0)
            {
                query = query.Take(queryParams.Limit.Value);
            }


            return await query
                .Select(st => new GetSubjectTeacherDTO(st))
                .ToListAsync();
        }
    }
}