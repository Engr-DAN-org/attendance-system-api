using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models.DTOs;
using api.Models.Entities;

namespace api.Interfaces.Repository
{
    public interface ICourseRepository
    {
        Task<Course> CreateCourseAsync(CreateCourseDTO courseDTO);
        Task<Course> GetCourseByIdAsync(int id);
        Task<Course> UpdateCourseAsync(int id, CreateCourseDTO courseDTO);
        Task DeleteCourseAsync(int id);
        Task<List<Course>> GetAllCoursesAsync(); //will add query params later
    }
}