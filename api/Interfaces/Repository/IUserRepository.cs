using System;
using api.Enums;
using api.Models.DTOs;
using api.Models.Entities;

namespace api.Interfaces.Repository;

public interface IUserRepository
{
    public Task<User?> FindFirstAdmin();
    public Task<User?> FindByEmailOrIdNoAsync(string emailOrIdNo);
    public Task<User?> FindByIdAsync(string id);
    public Task<User?> FindBySchoolIdNoAsync(string id);

    public Task<List<T>> GetUsersAsync<T>(UserRole userRole, int page, Action<IQueryable<User>>? queryCallback = null, Func<IQueryable<User>, IQueryable<T>>? selectCallback = null);
    public Task<User> UpdateUserAsync(User user);
    public Task<User> CreateUserAsync(User user);
    public Task<User> DeleteUserAsync(string id);

    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
