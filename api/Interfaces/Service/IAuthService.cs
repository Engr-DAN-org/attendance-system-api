using System;

namespace api.Interfaces.Service;

public interface IAuthService
{
    public Task<string> LoginAsync(string emailOrId, string password);
    public Task<string> LogoutAsync(string token);
    public Task<string> ForgotPasswordAsync(string email);
    public Task<string> ResetPasswordAsync(string email, string token, string password);
    public Task<string> VerifyEmailAsync(string email, string token);
    public Task<string> ChangePasswordAsync(string email, string password, string newPassword);
    public Task<string> ChangeEmailAsync(string email, string newEmail, string password);

}
