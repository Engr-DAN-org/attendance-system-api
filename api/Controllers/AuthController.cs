using api.Interfaces.Service;
using api.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService, ILogger<AuthController> logger) : ControllerBase
    {
        private readonly IAuthService _authService = authService;
        private readonly ILogger<AuthController> _logger = logger;

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            try
            {
                var response = await _authService.LoginAsync(loginDTO);
                if (response.Success)
                    return Ok(response);

                return Unauthorized(response);
            }
            catch (Exception e)
            {
                _logger.LogError(new EventId(101, "DatabaseError"), e, "Login Attempt Failed");
                return StatusCode(500, new { message = e.Message });
            }
        }


        [HttpPost("verify-2fa")]
        public async Task<IActionResult> Verify2FAuthAsync([FromBody] TwoFactorRequestDTO twoFactorRequestDTO)
        {
            try
            {
                var result = await _authService.Verify2FAuthAsync(twoFactorRequestDTO);

                return StatusCode((int)result.StatusCode, result);
            }
            catch (Exception e)
            {
                _logger.LogError(new EventId(101, "DatabaseError"), e, "2FA Verification Attempt Failed");
                return StatusCode(500, new { message = e.Message });
            }
        }
    }
}
