using System.Security.Claims;
using api.Enums;
using api.Exceptions;
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
    public class TeacherController(ITeacherService teacherService, IClassScheduleRepository scheduleRepository) : ControllerBase
    {
        private readonly ITeacherService _teacherService = teacherService;
        private readonly IClassScheduleRepository _scheduleRepo = scheduleRepository;

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

                var classSchedule = await _scheduleRepo.GetScheduleByIdAsync(id, false);

                if (classSchedule?.SubjectTeacher?.TeacherId != teacherId)
                    return NotFound();

                return Ok(new GetClassScheduleDTO(classSchedule, true));

            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (System.Exception e)
            {
                return StatusCode(500, new { message = "Something went wrong.", error = e.Message });
            }
        }

        // [HttpGet("students")]
        // [Authorize(Policy = "RequireTeacherOrAdmin")]
        // public async Task<IActionResult> GetStudents([FromQuery] StudentQueryDTO studentQueryDTO)
        // {
        //     var users = await _teacherService.GetStudentsAsync(studentQueryDTO);
        //     return Ok(users);
        // }
    }
}
