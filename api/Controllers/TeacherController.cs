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
    public class TeacherController(ITeacherService teacherService, ILogger<TeacherController> logger) : ControllerBase
    {
        private readonly ITeacherService _teacherService = teacherService;
        private readonly ILogger<TeacherController> _logger = logger;

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
                _logger.LogError(new EventId(101, "DatabaseError"), e, "Get Class Schedules Failed");
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
                _logger.LogError(new EventId(101, "DatabaseError"), e, "Get Class Schedule by Id Failed");
                return StatusCode(500, new { message = "Something went wrong.", error = e.Message });
            }
        }

        [HttpPost("override-attendance")]
        [Authorize(Policy = "RequireTeacherOrAdmin")]
        public async Task<IActionResult> OverrideAttendance([FromBody] OverrideAttendanceRecordDTO dto)
        {
            try
            {
                var teacherId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(teacherId))
                    return Unauthorized();

                await _teacherService.OverRideRecordAsync(teacherId, dto);
                return Ok(new { message = "Attendance record overridden successfully." });
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
                _logger.LogError(new EventId(101, "DatabaseError"), e, "Override Attendance Record Failed");
                return StatusCode(500, new { message = "Something went wrong.", error = e.Message });
            }
        }
    }
}