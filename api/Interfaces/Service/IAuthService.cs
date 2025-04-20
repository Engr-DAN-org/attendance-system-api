using System;
using api.Models.DTOs;

namespace api.Interfaces.Service;

public interface IAuthService
{
    public Task<TwoFactorResponseDTO> LoginAsync(LoginDTO loginDTO);
    public Task<AuthResponseDTO> Verify2FAuthAsync(TwoFactorRequestDTO twoFactorRequestDTO);
    // public Task<AuthResponseDTO> VerifyEmailAsync(VerifyEmailDTO verifyEmailDTO);
    public Task<GetProfileDTO> GetProfileAsync(string userId);
    public Task<string> ForgotPasswordAsync(string email);
    public Task<string> ResetPasswordAsync(string email, string token, string password);
    public Task<string> ChangePasswordAsync(string email, string password, string newPassword);
}
