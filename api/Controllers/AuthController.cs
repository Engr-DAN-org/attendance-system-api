using System.Security.Claims;
using api.Enums;
using api.Exceptions;
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
                return StatusCode(AuthResponseStatus.GetStatus(response.ResponseType), response);
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
                var response = await _authService.Verify2FAuthAsync(twoFactorRequestDTO);

                return StatusCode(AuthResponseStatus.GetStatus(response.ResponseType), response);
            }
            catch (Exception e)
            {
                _logger.LogError(new EventId(101, "DatabaseError"), e, "2FA Verification Attempt Failed");
                return StatusCode(500, new { message = e.Message });
            }
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                    return StatusCode(400, new { message = "User ID is missing or invalid." });

                var response = await _authService.GetProfileAsync(userId);
                return Ok(response);
            }
            catch (NotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
            catch (Exception e)
            {
                _logger.LogError(new EventId(101, "DatabaseError"), e, "Get Profile Attempt Failed");
                return StatusCode(500, new { message = e.Message });
            }
        }
    }
}
