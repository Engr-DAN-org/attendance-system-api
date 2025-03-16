using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Interfaces.Repository;
using api.Interfaces.Service;

namespace api.Services
{
    public class AdminService(IUserRepository userRepository) : IAdminService
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<string> GetFirstAdminName()
        {
            var firstAdmin = await _userRepository.FindFirstAdmin();
            return firstAdmin?.FullName ?? "";
        }
    }
}