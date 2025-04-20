using System;
using api.Models.DTOs;
using api.Models.Entities;
using api.Models.QueryParams;

namespace api.Interfaces.Repository;

public interface ISubjectRepository
{
    Task<GetSubjectDTO> GetSubjectByIdAsync(int id);
    Task<SubjectQueryDTO> QuerySubjectsAsync(SubjectQueryParams queryParams);
    Task<GetSubjectDTO> UpdateSubjectAsync(int id, CreateSubjectDTO subject);
    Task<GetSubjectDTO> CreateSubjectAsync(CreateSubjectDTO subject);
    Task DeleteSubjectAsync(int id);

}
