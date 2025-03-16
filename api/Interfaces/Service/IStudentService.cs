using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models.DTOs;
using api.Models.Entities;

namespace api.Interfaces.Service
{
    public interface IStudentService
    {
        public Task<GetStudentDTO> CreateStudentAsync(CreateStudentDTO student);
        public Task<GetStudentDTO> UpdateStudentAsync(UpdateStudentDTO student);
        public Task DeleteStudentAsync(string id);
        public Task<GetStudentDTO?> GetStudentByIdAsync(string id);
        public Task<List<GetStudentDTO>> GetStudentsAsync(StudentQueryDTO studentQueryDTO);

    }
}