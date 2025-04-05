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
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class CourseRepository(AppDbContext context) : ICourseRepository
    {
        private readonly AppDbContext _context = context;
        public async Task<Course> CreateCourseAsync(CreateCourseDTO courseDTO)
        {
            try
            {
                var existingCourseName = await _context.Courses.AnyAsync(
                    c => string.Equals(c.Name, courseDTO.Name));
                if (existingCourseName)
                    throw new DuplicateNameException("Course with this name already exists.");

                var existingCourseCode = await _context.Courses.AnyAsync(
                    c => string.Equals(c.Code, courseDTO.Code));
                if (existingCourseName)
                    throw new DuplicateNameException("Course with this code already exists.");

                var course = await _context.Courses.AddAsync(new Course
                {
                    Name = courseDTO.Name,
                    Code = courseDTO.Code,
                    Years = courseDTO.Years,
                    Description = courseDTO.Description,
                });
                await _context.SaveChangesAsync();
                return course.Entity;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteCourseAsync(int id)
        {
            try
            {
                var course = await GetCourseByIdAsync(id);
                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public async Task<List<Course>> GetAllCoursesAsync()
        {
            try
            {
                return await _context.Courses.ToListAsync();
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public async Task<Course> GetCourseByIdAsync(int id)
        {
            return await _context.Courses.FindAsync(id) ?? throw new NotFoundException(typeof(Course).Name);
        }

        public async Task<Course> UpdateCourseAsync(int id, CreateCourseDTO courseDTO)
        {
            try
            {
                var course = await GetCourseByIdAsync(id);
                course.Name = courseDTO.Name;
                course.Code = courseDTO.Code;
                course.Years = courseDTO.Years;
                course.Description = courseDTO.Description;
                _context.Courses.Update(course);
                await _context.SaveChangesAsync();

                return course;
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}