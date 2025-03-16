using api.Interfaces.Service;
using api.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            var response = await _authService.LoginAsync(loginDTO);
            return Ok(response);
        }


        [HttpPost("verify-2fa")]
        public async Task<IActionResult> Verify2FAuthAsync([FromBody] TwoFactorRequestDTO twoFactorRequestDTO)
        {
            var result = await _authService.Verify2FAuthAsync(twoFactorRequestDTO);

            return Ok(result);
        }
    }
}
