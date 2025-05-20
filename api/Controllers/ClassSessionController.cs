using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using api.Exceptions;
using api.Interfaces.Service;
using api.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class ClassSessionController(ITeacherService teacherService, IStudentService studentService) : ControllerBase
    {
        private readonly ITeacherService _teacherService = teacherService;
        private readonly IStudentService _studentService = studentService;

        [HttpGet("class-schedule/{scheduleId}")]
        [Authorize(Policy = "RequireTeacherOrAdmin")]

        public async Task<IActionResult> GetClassSessionsByScheduleId(int scheduleId)
        {
            try
            {
                var teacherId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(teacherId))
                    return Unauthorized();

                var classSessions = await _teacherService.GetSesssionsByScheduleIdAsync(scheduleId);
                return Ok(classSessions);
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

        [HttpGet("{sessionId}")]
        [Authorize(Policy = "RequireTeacherOrAdmin")]
        public async Task<IActionResult> GetClassSession(string sessionId)
        {
            try
            {
                var teacherId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(teacherId))
                    return Unauthorized();

                var classSession = await _teacherService.GetClassSessionByIdAsync(sessionId);
                return Ok(classSession);
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


        [HttpGet("ongoing")]
        [Authorize(Policy = "RequireTeacherOrAdmin")]
        public async Task<IActionResult> FindOngoingSession()
        {
            try
            {
                var teacherId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(teacherId))
                    return Unauthorized();

                var classSession = await _teacherService.FindOngoingSessionByTeacherId(teacherId);
                return Ok(classSession);
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

        [HttpGet("{sessionId}/attendance-record")]
        [Authorize(Policy = "RequireStudent")]
        public async Task<IActionResult> GetAttendanceRecord(string sessionId)
        {
            try
            {
                var studentId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

                var attendanceRecords = await _studentService.GetAttendanceRecordBySessionIdAsync(studentId, sessionId);
                return Ok(attendanceRecords);
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

        [HttpPost("start")]
        [Authorize(Policy = "RequireTeacherOrAdmin")]

        public async Task<IActionResult> StartClassSession([FromBody] CreateClassSessionDTO dto)
        {
            try
            {
                var teacherId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(teacherId))
                    return Unauthorized();

                var classSession = await _teacherService.StartClassSessionAsync(teacherId, dto);
                return Ok(classSession);
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

        [HttpPost("end/{id}")]
        [Authorize(Policy = "RequireTeacherOrAdmin")]

        public async Task<IActionResult> EndClassSession(string id)
        {
            try
            {
                var teacherId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(teacherId))
                    return Unauthorized();

                var classSession = await _teacherService.EndClassSessionAsync(teacherId, id);
                return Ok(classSession);
            }
            catch (System.Exception e)
            {
                return StatusCode(500, new { message = "Something went wrong.", error = e.Message });
            }
        }

        [HttpPost("cancel/{id}")]
        [Authorize(Policy = "RequireTeacherOrAdmin")]

        public async Task<IActionResult> CancelClassSession(string id)
        {
            try
            {
                var teacherId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(teacherId))
                    return Unauthorized();

                var classSession = await _teacherService.CancelClassSessionAsync(teacherId, id);
                return Ok(classSession);
            }
            catch (System.Exception e)
            {
                return StatusCode(500, new { message = "Something went wrong.", error = e.Message });
            }
        }
    }
}