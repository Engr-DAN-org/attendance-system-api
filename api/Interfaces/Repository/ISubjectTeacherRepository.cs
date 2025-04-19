using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models.DTOs;
using api.Models.QueryParams;

namespace api.Interfaces.Repository
{
    public interface ISubjectTeacherRepository
    {
        Task<List<GetSubjectTeacherDTO>> QueryAsync(SubjectTeacherQueryParams queryParams);

        Task<GetSubjectTeacherDTO> GetByIdAsync(int id);
    }
}