using api.Interfaces.Repository;
using api.Interfaces.Service;
using api.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController(IStudentService studentService) : ControllerBase
    {
        private readonly IStudentService _studentService = studentService ?? throw new ArgumentNullException(nameof(studentService));

        [HttpGet]
        [Authorize(Policy = "RequireTeacherOrAdmin")]
        public async Task<IActionResult> GetStudents([FromQuery] StudentQueryDTO studentQueryDTO)
        {
            try
            {
                var users = await _studentService.GetStudentsAsync(studentQueryDTO);
                return Ok(users);
            }
            catch (System.Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }
    }
}
