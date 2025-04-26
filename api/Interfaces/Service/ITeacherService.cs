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
        public Task<List<GetClassScheduleDTO>> GetClassSchedulesAsync(string teacherId);
    }
}