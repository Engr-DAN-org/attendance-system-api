using System;
using api.Models.DTOs;

namespace api.Interfaces.Service;

public interface IAuthService
{
    public Task<TwoFactorResponseDTO> LoginAsync(LoginDTO loginDTO);
    public Task<TwoFactorResponseDTO> Resend2FAuthAsync(Resend2FACodeDTO loginDTO);
    public Task<AuthResponseDTO> Verify2FAuthAsync(TwoFactorRequestDTO twoFactorRequestDTO);
    // public Task<AuthResponseDTO> VerifyEmailAsync(VerifyEmailDTO verifyEmailDTO);
    public Task<GetProfileDTO> GetProfileAsync(string userId);
    public Task<TwoFactorResponseDTO> ForgotPasswordAsync(ForgotPasswordDTO forgotPasswordDTO);
    public Task<TwoFactorResponseDTO> ResetPasswordAsync(PasswordResetRequestDTO resetRequestDTO);
    public Task<string> ChangePasswordAsync(string id, string password, string newPassword);
}
