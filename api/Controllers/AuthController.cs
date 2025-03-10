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


        // public async Task<IActionResult> RegisterStudent([FromBody] RegisterStudentDTO registerStudentDTO)
        // {
        //     var result = await _authService.Register(registerDTO);
        //     if (result == null)
        //     {
        //         return BadRequest(new { message = "Email is already taken." });
        //     }
        //     return Ok(result);
        // }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO loginDTO)
        {
            return Ok(new { message = "Login successful", data = loginDTO });
        }

        [HttpGet("me")]
        // [Authorize]
        public async Task<IActionResult> Me()
        {
            // var result = await _authService.Me();
            var result = new { message = "Me" };
            return Ok(result);
        }

    }
}
