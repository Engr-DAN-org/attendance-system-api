using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models.DTOs;

namespace api.Interfaces.Service
{
    public interface IUserService
    {
        Task<AuthUserDTO> RegisterAsync(RegisterUserDTO registerDTO);
    }
}