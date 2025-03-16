using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models.DTOs;
using api.Models.Entities;

namespace api.Interfaces.Service
{
    public interface ITeacherService
    {
        public Task<GetTeacherDTO> CreateTeacherAsync(CreateTeacherDTO teacher);
        public Task<GetTeacherDTO> UpdateTeacherAsync(UpdateTeacherDTO teacher);
        public Task DeleteTeacherAsync(string id);
        public Task<GetTeacherDTO?> GetTeacherByIdAsync(string id);
        public Task<List<GetTeacherDTO>> GetTeachersAsync(int page, TeacherQueryDTO teacherQueryDTO);

    }
}