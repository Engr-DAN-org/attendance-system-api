using System;
using api.Models.DTOs;

namespace api.Interfaces.Service;

public interface IAuthService
{
    public Task<string> LoginAsync(LoginDTO loginDTO);
    public Task<string> Verify2FAuthAsync(string emailOrId, string code);
    public Task<string> LogoutAsync(string token);
    public Task<string> ForgotPasswordAsync(string email);
    public Task<string> ResetPasswordAsync(string email, string token, string password);
    public Task<string> VerifyEmailAsync(string email, string token);
    public Task<string> ChangePasswordAsync(string email, string password, string newPassword);
}
