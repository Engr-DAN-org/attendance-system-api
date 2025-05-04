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
    [Authorize(Policy = "RequireTeacherOrAdmin")]

    public class ClassSessionController(ITeacherService teacherService) : ControllerBase
    {
        private readonly ITeacherService _teacherService = teacherService;

        [HttpPost("start")]
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