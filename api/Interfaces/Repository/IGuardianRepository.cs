using System;
using api.Models.DTOs;
using api.Models.Entities;

namespace api.Interfaces.Repository;

public interface IGuardianRepository
{
    public Task<Guardian> CreateGuardianAsync(User student, CreateGuardianDTO guardian);
    public Task<Guardian?> GetGuardianByStudentIdAsync(string studentId);
    public Task<Guardian> UpdateGuardianAsync(User student, CreateGuardianDTO guardian);
    public Task<bool> DeleteGuardianAsync(string studentId);
}
