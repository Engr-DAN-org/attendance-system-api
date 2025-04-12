using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models.DTOs;
using api.Models.Entities;
using api.Models.QueryParams;

namespace api.Interfaces.Repository
{
    public interface ICourseRepository
    {
        Task<Course> CreateCourseAsync(CreateCourseDTO courseDTO);
        Task<Course> GetCourseByIdAsync(int id);
        Task<Course> UpdateCourseAsync(int id, CreateCourseDTO courseDTO);
        Task<Course> UpdateCourseIconAsync(int id, UpdateIconDTO iconDTO);
        Task DeleteCourseAsync(int id);
        Task<CourseQueryDTO> GetCourseQueryAsync(CourseQuery courseQuery); //will add query params later
    }
}