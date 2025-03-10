using System;
using api.Models;

namespace api.Interfaces.Repository;

public interface IUserRepository
{
    public Task<User> GetUserByEmailOrIdNoAsync(string emailOrIdNo);
    public Task<User> GetUserByIdAsync(Guid id);
    public Task<User[]> GetUsersAsync();
    public Task<User> UpdateUserAsync(User user);
    public Task<User> CreateUserAsync(User user);
    public Task<User> DeleteUserAsync(Guid id);
}
