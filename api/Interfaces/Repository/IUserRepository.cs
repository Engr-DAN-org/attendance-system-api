using System;
using api.Models.DTOs;
using api.Models.Entities;

namespace api.Interfaces.Repository;

public interface IUserRepository
{
    public Task<User?> GetUserByEmailOrIdNoAsync(string emailOrIdNo);
    public Task<User?> GetUserByIdAsync(Guid id);
    public Task<List<User>> GetUsersAsync();
    public Task<List<GetStudentDTO>> GetStudentsAsync(StudentQueryDTO studentQueryDTO);

    public Task<User> UpdateUserAsync(User user);
    public Task<User> CreateUserAsync(User user);
    public Task<User> DeleteUserAsync(Guid id);
}
