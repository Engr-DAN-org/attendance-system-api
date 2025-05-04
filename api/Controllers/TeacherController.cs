using System.Security.Claims;
using api.Enums;
using api.Exceptions;
using api.Interfaces.Repository;
using api.Interfaces.Service;
using api.Models.DTOs;
using api.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController(ITeacherService teacherService) : ControllerBase
    {
        private readonly ITeacherService _teacherService = teacherService;

        [HttpGet("class-schedules")]
        [Authorize(Policy = "RequireTeacherOrAdmin")]
        public async Task<IActionResult> GetClassSchedules()
        {
            try
            {
                var teacherId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(teacherId))
                    return Unauthorized();

                var classSchedules = await _teacherService.GetClassSchedulesAsync(teacherId);
                return Ok(classSchedules);
            }
            catch (System.Exception e)
            {

                return StatusCode(500, new { message = "Something went wrong.", error = e.Message });
            }
        }

        [HttpGet("class-schedules/{id}")]
        [Authorize(Policy = "RequireTeacherOrAdmin")]
        public async Task<IActionResult> GetScheduleById(int id)
        {
            try
            {
                var teacherId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(teacherId))
                    return Unauthorized();
                var classSchedule = await _teacherService.GetScheduleByIdAsync(teacherId, id);

                return Ok(classSchedule);

            }
            catch (NotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
            catch (UnauthorizedAccessException e)
            {
                return BadRequest(new { message = e.Message });
            }
            catch (System.Exception e)
            {
                return StatusCode(500, new { message = "Something went wrong.", error = e.Message });
            }
        }
    }
}
