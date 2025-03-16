using api.Enums;
using api.Interfaces.Repository;
using api.Interfaces.Service;
using api.Models.DTOs;
using api.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController(ITeacherService teacherService) : ControllerBase
    {
        private readonly ITeacherService _teacherService = teacherService;

        // [HttpGet("students")]
        // [Authorize(Policy = "RequireTeacherOrAdmin")]
        // public async Task<IActionResult> GetStudents([FromQuery] StudentQueryDTO studentQueryDTO)
        // {
        //     var users = await _teacherService.GetStudentsAsync(studentQueryDTO);
        //     return Ok(users);
        // }
    }
}
