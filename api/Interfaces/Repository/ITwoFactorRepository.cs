using System;
using api.Models.Entities;

namespace api.Interfaces.Repository;

public interface ITwoFactorRepository
{
    public Task<TwoFactorAuth?> FindByEmailAsync(string email);
    Task<TwoFactorAuth> CreateAsync(string email);
    public Task DeleteAsync(TwoFactorAuth twoFactorAuth);
}
