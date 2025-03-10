using System;
using api.Models.Entities;

namespace api.Interfaces.Repository;

public interface IGuardianRepository
{
    public Task<Guardian> CreateGuardianAsync(Guardian guardian);
    public Task<Guardian> GetGuardianByStudentIdAsync(string studentId);
    public Task<Guardian> UpdateGuardianAsync(Guardian guardian);
    public Task<Guardian> DeleteGuardianAsync(string studentId);
}
