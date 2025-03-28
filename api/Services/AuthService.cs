using System.Net;
using api.Data;
using api.Enums;
using api.Exceptions;
using api.Interfaces.Repository;
using api.Interfaces.Service;
using api.Models.DTOs;
using api.Utils;

namespace api.Services;

public class AuthService(AppDbContext context, IUserRepository userRepository, IEmailService emailService, ITwoFactorRepository twoFactorRepository) : IAuthService
{
    private readonly AppDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
    private readonly IUserRepository _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    private readonly ITwoFactorRepository _twoFactorRepository = twoFactorRepository ?? throw new ArgumentNullException(nameof(twoFactorRepository));
    private readonly IEmailService _emailService = emailService ?? throw new ArgumentNullException(nameof(twoFactorRepository));

    public Task<string> ChangePasswordAsync(string email, string password, string newPassword)
    {
        throw new NotImplementedException();
    }

    public Task<string> ForgotPasswordAsync(string email)
    {
        throw new NotImplementedException();
    }

    public async Task<TwoFactorResponseDTO> LoginAsync(LoginDTO loginDTO)
    {
        var user = await _userRepository.FindByEmailOrIdNoAsync(loginDTO.EmailOrIdNo);
        if (user == null || user.PasswordHash == null || !CredentialUtils.VerifyPassword(loginDTO.Password, user.PasswordHash))
            return new TwoFactorResponseDTO { ResponseType = AuthResponseType.InvalidCredentials };

        if (user.Email != null)
        {
            var twoFactorEntry = await _twoFactorRepository.CreateAsync(user.Email);
            await _emailService.SendOTPEmailAsync(user.Email, twoFactorEntry.Message);
        }

        return new TwoFactorResponseDTO { Email = user.Email };
    }

    public async Task<AuthResponseDTO> Verify2FAuthAsync(TwoFactorRequestDTO twoFactorRequestDTO)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var twoFactorEntry = await _twoFactorRepository.FindByEmailAsync(twoFactorRequestDTO.Email);
            var user = await _userRepository.FindByEmailOrIdNoAsync(twoFactorRequestDTO.Email);

            if (twoFactorEntry == null || user == null)
                return new AuthResponseDTO { ResponseType = AuthResponseType.Error };
            if (twoFactorEntry.Code != twoFactorRequestDTO.Code)
                return new AuthResponseDTO { ResponseType = AuthResponseType.InvalidOTP };
            if (twoFactorEntry.IsExpired)
                return new AuthResponseDTO { ResponseType = AuthResponseType.ExpiredOTP };

            // Delete the 2FA code to prevent reuse

            var authToken = new TokenGenerator().GenerateAuthToken(user);

            await _twoFactorRepository.DeleteAsync(twoFactorEntry);

            await transaction.CommitAsync();
            return new AuthResponseDTO
            {
                Token = authToken,
                Expiry = DateTime.UtcNow.AddHours(1),
                Role = user.Role
            };

        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            return new AuthResponseDTO { ResponseType = AuthResponseType.Error };
        }
    }


    public Task<string> LogoutAsync(string token)
    {
        throw new NotImplementedException();
    }

    public Task<string> ResetPasswordAsync(string email, string token, string password)
    {
        throw new NotImplementedException();
    }



    public Task<string> VerifyEmailAsync(string email, string token)
    {
        throw new NotImplementedException();
    }

    public async Task<GetProfileDTO> GetProfileAsync(string userId)
    {
        var user = await _userRepository.FindByIdAsync(userId) ?? throw new NotFoundException("User");

        return new GetProfileDTO(user);
    }
}
