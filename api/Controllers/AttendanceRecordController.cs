using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using api.Exceptions;
using api.Interfaces.Repository;
using api.Interfaces.Service;
using api.Models.DTOs;
using api.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttendanceRecordController(IAttendanceRecordRepository recordRepository, ITeacherService teacherService, IStudentService studentService) : ControllerBase
    {

        private readonly IAttendanceRecordRepository _recordRepository = recordRepository;
        private readonly IStudentService _studentService = studentService;
        private readonly ITeacherService _teacherService = teacherService;

        [HttpPost("log")]
        [Authorize(Policy = "RequireStudent")]
        public async Task<IActionResult> LogAttendance([FromBody] LogAttendanceRecordDTO recordDTO)
        {
            try
            {
                var studentId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(studentId))
                    return Unauthorized();


                var attendanceRecord = await _studentService.LogAttendanceAsync(studentId, recordDTO);
                return Ok(attendanceRecord);
            }
            catch (NotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
            catch (UnauthorizedAccessException e)
            {
                return BadRequest(new { message = e.Message });
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(new { message = e.Message });
            }
            catch (System.Exception e)
            {
                return StatusCode(500, new { message = "Something went wrong.", error = e.Message });
            }
        }

        [HttpPost("override")]
        [Authorize(Policy = "RequireTeacherOrAdmin")]
        public async Task<IActionResult> OverrideAttendanceRecord([FromBody] OverrideAttendanceRecordDTO dto)
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
                return StatusCode(500, new { message = "Something went wrong.", error = e.Message });
            }
        }
    }
}