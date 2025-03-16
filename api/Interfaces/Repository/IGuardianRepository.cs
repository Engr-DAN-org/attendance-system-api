using System;
using api.Models.DTOs;
using api.Models.Entities;

namespace api.Interfaces.Repository;

public interface IGuardianRepository
{
    public Task<Guardian> CreateGuardianAsync(Guid studentId, CreateGuardianDTO guardian);
    public Task<Guardian?> GetGuardianByStudentIdAsync(Guid studentId);
    public Task<bool> UpdateGuardianAsync(Guid studentId, UpdateGuardianDTO guardian);
    public Task<bool> DeleteGuardianAsync(Guid studentId);
}
