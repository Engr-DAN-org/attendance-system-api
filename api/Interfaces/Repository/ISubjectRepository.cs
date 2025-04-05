using System;
using api.Models.Entities;

namespace api.Interfaces.Repository;

public interface ISubjectRepository
{
    Task<Subject> GetSubjectByIdAsync(string id);
    Task<Subject> GetSubjectByNameAsync(string name);
    Task<Subject[]> GetSubjectsAsync();
    Task<Subject> UpdateSubjectAsync(Subject subject);
    Task<Subject> CreateSubjectAsync(Subject subject);
    Task DeleteSubjectAsync(string id);

}
