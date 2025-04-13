using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using api.Interfaces.Repository;
using api.Interfaces.Service;
using api.Models.DTOs;
using api.Models.Entities;
using api.Utils;

namespace api.Services
{
    public class UserService(IUserRepository userRepo, IGuardianRepository guardianRepo, IEmailService emailService) : IUserService
    {
        private readonly IUserRepository _userRepo = userRepo;
        private readonly IGuardianRepository _guardianRepo = guardianRepo;
        private readonly IEmailService _emailService = emailService;

        public async Task<AuthUserDTO> RegisterAsync(RegisterUserDTO registerDTO)
        {
            try
            {
                await _userRepo.BeginTransactionAsync();
                var idNumberUsed = await _userRepo.IsIdNumberUsedAsync(registerDTO.IdNumber);
                if (idNumberUsed)
                    throw new DuplicateNameException("User already exists with the same ID number.");

                var emailUsed = await _userRepo.IsEmailUsedAsync(registerDTO.Email);
                if (emailUsed)
                    throw new DuplicateNameException("User already exists with the same email.");

                string password = RandomCharGenerator.GenerateRandomPassword();

                var user = await _userRepo.CreateUserAsync(registerDTO, password);
                if (registerDTO.Guardian != null)
                {
                    var guardian = await _guardianRepo.UpdateGuardianAsync(user, registerDTO.Guardian);
                    user.GuardianId = guardian.Id;
                    user.Guardian = guardian;
                    await _userRepo.UpdateUserAsync(user);
                }

                await _emailService.SendRegistrationCredentialsAsync(user, password);

                await _userRepo.CommitTransactionAsync();
                return new AuthUserDTO(user);
            }
            catch (System.Exception)
            {
                await _userRepo.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<AuthUserDTO> UpdateCredentialsAsync(string id, RegisterUserDTO registerDTO)
        {
            try
            {
                await _userRepo.BeginTransactionAsync();
                var user = await _userRepo.FindByIdAsync(id);

                var idNumberUsed = await _userRepo.IsIdNumberUsedAsync(registerDTO.IdNumber, user);
                if (idNumberUsed)
                    throw new DuplicateNameException("User already exists with the same ID number.");

                var emailUsed = await _userRepo.IsEmailUsedAsync(registerDTO.Email, user);
                if (emailUsed)
                    throw new DuplicateNameException("User already exists with the same email.");

                user.FirstName = registerDTO.FirstName;
                user.LastName = registerDTO.LastName;
                user.IdNumber = registerDTO.IdNumber;
                user.Email = registerDTO.Email;
                user.PhoneNumber = registerDTO.PhoneNumber;
                await _userRepo.UpdateUserAsync(user);

                if (registerDTO.Guardian != null)
                {
                    var guardian = await _guardianRepo.UpdateGuardianAsync(user, registerDTO.Guardian);
                    user.GuardianId = guardian.Id;
                    user.Guardian = guardian;
                    await _userRepo.UpdateUserAsync(user);
                }

                await _userRepo.CommitTransactionAsync();
                return new AuthUserDTO(user);
            }
            catch (System.Exception)
            {
                await _userRepo.RollbackTransactionAsync();
                throw;
            }
        }
    }
}