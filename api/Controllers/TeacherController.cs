using api.Enums;
using api.Interfaces.Repository;
using api.Models.DTOs;
using api.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController(IUserRepository userRepository) : ControllerBase
    {
        private readonly IUserRepository _userRepository = userRepository;

        [HttpGet("students")]
        [Authorize(Policy = "RequireTeacherOrAdmin")]
        public async Task<IActionResult> GetStudents([FromQuery] StudentQueryDTO studentQueryDTO)
        {
            var users = await _userRepository.GetStudentsAsync(studentQueryDTO);
            return Ok(users);
        }
    }
}
