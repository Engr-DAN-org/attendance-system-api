using System;
using api.Enums;
using api.Models.DTOs;
using api.Models.Entities;
using api.Models.QueryParams;

namespace api.Interfaces.Repository;

public interface IUserRepository
{
    public Task<User> FindFirstAdmin();
    public Task<User> FindByEmailOrIdNoAsync(string emailOrIdNo);
    public Task<User> FindByIdAsync(string id);
    public Task<User> FindBySchoolIdNoAsync(string id);

    Task<bool> IsIdNumberUsedAsync(string idNumber, User? user = null);
    Task<bool> IsEmailUsedAsync(string email, User? user = null);

    public Task<UsersQueryDTO> GetUsersAsync(UsersQueryParams userQueryDTO);
    public Task<User> UpdateUserAsync(User user);
    public Task<User> CreateUserAsync(RegisterUserDTO user, string password);
    public Task<User> DeleteUserAsync(string id);

    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
