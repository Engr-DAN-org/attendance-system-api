using System.Security.Claims;
using api.Exceptions;
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
    [Authorize(Policy = "RequireStudent")]

    public class StudentController(IStudentService studentService) : ControllerBase
    {

        private readonly IStudentService _studentService = studentService ?? throw new ArgumentNullException(nameof(studentService));

        [HttpGet("section-data")]
        public async Task<IActionResult> GetSectionData()
        {
            try
            {
                var studentId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                    ?? throw new UnauthorizedAccessException();

                var users = await _studentService.GetClassSchedulesAsync(studentId);
                return Ok(users);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (UnauthorizedAccessException e)
            {
                return BadRequest(e.Message);
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(e.Message);
            }
            catch (System.Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("attendance-records")]
        public async Task<IActionResult> GetAttendanceRecords()
        {
            try
            {
                var studentId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                    ?? throw new UnauthorizedAccessException();

                var records = await _studentService.GetAttendanceRecordsAsync(studentId);
                return Ok(records);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (UnauthorizedAccessException e)
            {
                return BadRequest(e.Message);
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(e.Message);
            }
            catch (System.Exception e)
            {
                return StatusCode(500, e.Message);
            }


        }
    }
}