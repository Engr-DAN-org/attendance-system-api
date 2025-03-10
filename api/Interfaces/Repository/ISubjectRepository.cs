using System;
using api.Models.Entities;

namespace api.Interfaces.Repository;

public interface ISubjectRepository
{
    public Task<Subject> GetSubjectByIdAsync(string id);
    public Task<Subject> GetSubjectByNameAsync(string name);
    public Task<Subject[]> GetSubjectsAsync();
    public Task<Subject> UpdateSubjectAsync(Subject subject);
    public Task<Subject> CreateSubjectAsync(Subject subject);
    public Task<Subject> DeleteSubjectAsync(string id);

}
