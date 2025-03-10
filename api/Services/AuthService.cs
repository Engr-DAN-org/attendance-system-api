using System;
using api.Interfaces.Service;
using api.Models.DTOs;

namespace api.Services;

public class AuthService : IAuthService
{
    public Task<string> ChangePasswordAsync(string email, string password, string newPassword)
    {
        throw new NotImplementedException();
    }

    public Task<string> ForgotPasswordAsync(string email)
    {
        throw new NotImplementedException();
    }

    public Task<string> LoginAsync(LoginDTO loginDTO)
    {
        throw new NotImplementedException();
    }

    public Task<string> LogoutAsync(string token)
    {
        throw new NotImplementedException();
    }

    public Task<string> ResetPasswordAsync(string email, string token, string password)
    {
        throw new NotImplementedException();
    }

    public Task<string> Verify2FAuthAsync(string emailOrId, string code)
    {
        throw new NotImplementedException();
    }

    public Task<string> VerifyEmailAsync(string email, string token)
    {
        throw new NotImplementedException();
    }
}
