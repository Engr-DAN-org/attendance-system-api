using System.Data;
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
    public class AuthController(IAuthService authService, IUserService userService, ILogger<AuthController> logger) : ControllerBase
    {
        private readonly IAuthService _authService = authService;
        private readonly IUserService _userService = userService;
        private readonly ILogger<AuthController> _logger = logger;

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            try
            {
                var response = await _authService.LoginAsync(loginDTO);
                return StatusCode(AuthResponseStatus.GetStatus(response.ResponseType), response);
            }
            catch (ArgumentException e)
            {
                return StatusCode(422, new { message = e.Message });
            }
            catch (Exception e)
            {
                _logger.LogError(new EventId(101, "DatabaseError"), e, "Login Attempt Failed");
                return StatusCode(500, new { message = e.Message });
            }
        }


        // [HttpPut("verify-email")]
        // public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailDTO verifyEmailDTO)
        // {
        //     try
        //     {
        //         var response = await _authService.VerifyEmailAsync(verifyEmailDTO);
        //         return StatusCode(AuthResponseStatus.GetStatus(response.ResponseType), response);
        //     }
        //     catch (Exception e)
        //     {
        //         _logger.LogError(new EventId(101, "DatabaseError"), e, "Email Verification Attempt Failed");
        //         return StatusCode(500, new { message = e.Message });
        //     }
        // }

        [HttpPost("resend-2fa")]
        public async Task<IActionResult> Resend2FAuthAsync([FromBody] Resend2FACodeDTO resend2FACodeDTO)
        {
            try
            {
                var response = await _authService.Resend2FAuthAsync(resend2FACodeDTO);
                return StatusCode(AuthResponseStatus.GetStatus(response.ResponseType), response);
            }
            catch (ArgumentException e)
            {
                return StatusCode(422, new { message = e.Message });
            }
            catch (System.Exception e)
            {
                _logger.LogError(new EventId(101, "DatabaseError"), e, "2FA Code Resend Attempt Failed");
                return StatusCode(500, new { message = e.Message });
            }
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO forgotPasswordDTO)
        {
            try
            {
                var response = await _authService.ForgotPasswordAsync(forgotPasswordDTO);
                return StatusCode(AuthResponseStatus.GetStatus(response.ResponseType), response);
            }
            catch (NotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
            catch (Exception e)
            {
                _logger.LogError(new EventId(101, "DatabaseError"), e, "Forgot Password Attempt Failed");
                return StatusCode(500, new { message = e.Message });
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] PasswordResetRequestDTO resetRequestDTO)
        {
            try
            {
                var response = await _authService.ResetPasswordAsync(resetRequestDTO);
                return StatusCode(AuthResponseStatus.GetStatus(response.ResponseType), response);
            }
            catch (System.Exception e)
            {
                _logger.LogError(new EventId(101, "DatabaseError"), e, "Reset Password Attempt Failed");
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
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

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

        [HttpPut("profile/update")]

        public async Task<IActionResult> UpdateUser([FromBody] RegisterUserDTO userDTO)
        {
            try
            {
                var id = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

                var user = await _userService.UpdateCredentialsAsync(id, userDTO);
                return Ok(user);
            }
            catch (DuplicateNameException e)
            {
                return Conflict(new { message = e.Message });
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (System.Exception e)
            {
                return StatusCode(500, new { message = e.Message });
            }
        }

        [HttpPut("profile/change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO changePasswordDTO)
        {
            try
            {
                var id = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

                await _authService.ChangePasswordAsync(id, changePasswordDTO.OldPassword, changePasswordDTO.NewPassword);
                return Ok(new { message = "Password changed successfully" });
            }
            catch (UnauthorizedAccessException e)
            {
                return Unauthorized(new { message = e.Message });
            }
            catch (NotSupportedException e)
            {
                return StatusCode(422, new { message = e.Message });
            }
            catch (System.Exception e)
            {
                _logger.LogError(new EventId(101, "DatabaseError"), e, "Change Password Attempt Failed");
                return StatusCode(500, new { message = e.Message });
            }
        }
    }
}
