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

    public async Task<string> ChangePasswordAsync(string id, string password, string newPassword)
    {
        try
        {
            var user = _userRepository.FindByIdAsync(id).Result ?? throw new NotFoundException("User");

            if (!CredentialUtils.VerifyPassword(password, user.PasswordHash!))
                throw new NotSupportedException("Current password is incorrect.");

            user.PasswordHash = CredentialUtils.HashPassword(newPassword);
            var updatedUser = await _userRepository.UpdateUserAsync(user);
            return updatedUser.Id;
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    public async Task<TwoFactorResponseDTO> LoginAsync(LoginDTO loginDTO)
    {
        try
        {
            await _context.Database.BeginTransactionAsync();

            var user = await _userRepository.FindByEmailOrIdNoAsync(loginDTO.EmailOrIdNo);

            if (string.IsNullOrEmpty(user.Email))
                throw new ArgumentException("User Email required for Two-Factor Authentication is not set. Please contact the Administrator.");


            if (user == null || user.PasswordHash == null || !CredentialUtils.VerifyPassword(loginDTO.Password, user.PasswordHash))
                return new TwoFactorResponseDTO { ResponseType = AuthResponseType.InvalidCredentials };

            var twoFactorEntry = await _twoFactorRepository.CreateAsync(user.Email);

            await _emailService.SendOTPEmailAsync(user.Email, twoFactorEntry.Message);

            await _context.Database.CommitTransactionAsync();
            return new TwoFactorResponseDTO { Email = user.Email };
        }
        catch (Exception)
        {
            await _context.Database.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<TwoFactorResponseDTO> Resend2FAuthAsync(Resend2FACodeDTO resend2FACodeDTO)
    {
        try
        {
            await _context.Database.BeginTransactionAsync();
            var user = await _userRepository.FindByEmailOrIdNoAsync(resend2FACodeDTO.Email);

            if (string.IsNullOrEmpty(user.Email))
                throw new ArgumentException("User Email required for Two-Factor Authentication is not set. Please contact the Administrator.");

            var twoFactorEntry = await _twoFactorRepository.CreateAsync(user.Email);


            await _emailService.SendOTPEmailAsync(user.Email, twoFactorEntry.Message);

            await _context.Database.CommitTransactionAsync();
            return new TwoFactorResponseDTO { Email = user.Email };
        }
        catch (System.Exception)
        {
            await _context.Database.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<TwoFactorResponseDTO> ForgotPasswordAsync(ForgotPasswordDTO forgotPasswordDTO)
    {
        try
        {
            await _context.Database.BeginTransactionAsync();

            var user = await _userRepository.FindByEmailOrIdNoAsync(forgotPasswordDTO.Email);

            if (string.IsNullOrEmpty(user.Email))
                throw new ArgumentException("User Email required for Two-Factor Authentication is not set. Please contact the Administrator.");

            var twoFactorEntry = await _twoFactorRepository.CreateAsync(user.Email);

            await _emailService.SendForgotPasswordOTPAsync(user.Email, twoFactorEntry.Code);

            await _context.Database.CommitTransactionAsync();
            return new TwoFactorResponseDTO { Email = user.Email };

        }
        catch (System.Exception)
        {
            await _context.Database.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<TwoFactorResponseDTO> ResetPasswordAsync(PasswordResetRequestDTO resetRequestDTO)
    {
        try
        {
            await _context.Database.BeginTransactionAsync();

            var twoFactorEntry = await _twoFactorRepository.FindByEmailAsync(resetRequestDTO.Email);
            var user = await _userRepository.FindByEmailOrIdNoAsync(resetRequestDTO.Email);

            if (twoFactorEntry == null || user == null)
                return new TwoFactorResponseDTO { ResponseType = AuthResponseType.Error };
            if (twoFactorEntry.Code != resetRequestDTO.Code)
                return new TwoFactorResponseDTO { ResponseType = AuthResponseType.InvalidOTP };
            if (twoFactorEntry.IsExpired)
                return new TwoFactorResponseDTO { ResponseType = AuthResponseType.ExpiredOTP };

            string password = RandomCharGenerator.GenerateRandomPassword();

            user.PasswordHash = CredentialUtils.HashPassword(password);
            await _context.SaveChangesAsync();

            await _emailService.SendPasswordResetEmailAsync(user.Email!, password);

            await _context.Database.CommitTransactionAsync();
            return new TwoFactorResponseDTO { Email = user.Email, ResponseType = AuthResponseType.PasswordResetSuccess };
        }
        catch (System.Exception)
        {
            await _context.Database.RollbackTransactionAsync();
            throw;
        }
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

            if (user.EmailConfirmed == false)
            { // If First time login, set the email as confirmed
                user.EmailConfirmed = true;
                await _userRepository.UpdateUserAsync(user);
            }

            var authToken = new TokenGenerator().GenerateAuthToken(user);
            // Delete the 2FA code to prevent reuse
            await _twoFactorRepository.DeleteAsync(twoFactorEntry);

            await transaction.CommitAsync();
            return new AuthResponseDTO
            {
                Token = authToken,
                Expiry = DateTime.UtcNow.AddHours(1),
                User = new AuthUserDTO(user),
            };

        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }


    public Task<string> LogoutAsync(string token)
    {
        throw new NotImplementedException();
    }


    // public async Task<AuthResponseDTO> VerifyEmailAsync(VerifyEmailDTO verifyEmailDTO)
    // {
    //     try
    //     {
    //         await _userRepository.BeginTransactionAsync();
    //         var user = await _userRepository.FindByIdAsync(verifyEmailDTO.Id);
    //         if (user.EmailConfirmed == true) throw new InvalidOperationException("User is already verified. Please proceed to the login page.");

    //         user.EmailConfirmed = true;
    //         // user.Status = UserStatus.Active;
    //         user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(verifyEmailDTO.Password);

    //         var authToken = new TokenGenerator().GenerateAuthToken(user);

    //         await _userRepository.UpdateUserAsync(user);
    //         await _userRepository.CommitTransactionAsync();
    //         return new AuthResponseDTO
    //         {
    //             Token = authToken,
    //             Expiry = DateTime.UtcNow.AddHours(1),
    //             User = new AuthUserDTO(user),
    //         };
    //     }
    //     catch (Exception)
    //     {
    //         await _userRepository.RollbackTransactionAsync();
    //         throw;
    //     }

    // }

    public async Task<GetProfileDTO> GetProfileAsync(string userId)
    {
        var user = await _userRepository.FindByIdAsync(userId) ?? throw new NotFoundException("User");

        return new GetProfileDTO(user);
    }

}
