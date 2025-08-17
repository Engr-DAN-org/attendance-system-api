using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Exceptions;
using api.Interfaces.Repository;
using api.Models.DTOs;
using api.Models.Entities;
using api.Models.QueryParams;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClassScheduleController(IClassScheduleRepository repository, ILogger<ClassScheduleController> logger) : ControllerBase
    {
        private readonly IClassScheduleRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        private readonly ILogger<ClassScheduleController> _logger = logger;

        [HttpPost("create")]
        public async Task<IActionResult> CreateClassSchedule([FromBody] CreateClassScheduleDTO createClassScheduleDTO)
        {
            try
            {
                var classSchedule = await _repository.CreateScheduleAsync(createClassScheduleDTO);
                return CreatedAtAction(nameof(GetClassSchedule), new { id = classSchedule.Id }, classSchedule);
            }
            catch (Exception e)
            {
                _logger.LogError(new EventId(101, "DatabaseError"), e, "Create Class Schedule Failed");
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClassSchedule(int id)
        {
            try
            {
                var classSchedule = await _repository.GetScheduleByIdAsync(id);
                return Ok(classSchedule);
            }
            catch (NotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
            catch (Exception e)
            {
                _logger.LogError(new EventId(101, "DatabaseError"), e, "Get Class Schedule Failed");
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetQuery([FromQuery] ClassScheduleQueryParams queryParams)
        {
            try
            {
                var classSchedules = await _repository.QueryAsync(queryParams);
                return Ok(classSchedules);
            }
            catch (Exception e)
            {
                _logger.LogError(new EventId(101, "DatabaseError"), e, "Query Class Schedules Failed");
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("section-or-teacher")]
        public async Task<IActionResult> GetBySectionOrTeacher([FromQuery] ScheduleTeacherSectionQuery queryParams)
        {
            try
            {
                if (string.IsNullOrEmpty(queryParams.SectionId?.ToString()) && string.IsNullOrEmpty(queryParams.TeacherId?.ToString()))
                {
                    return BadRequest(new { message = "Please provide at least one of the following: SectionId or TeacherId." });
                }
                var classSchedules = await _repository.GetBySectionOrTeacherAsync(queryParams);
                return Ok(classSchedules);
            }
            catch (Exception e)
            {
                _logger.LogError(new EventId(101, "DatabaseError"), e, "Get Class Schedules by Section or Teacher Failed");
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClassSchedule(int id, [FromBody] CreateClassScheduleDTO createClassScheduleDTO)
        {
            try
            {
                var updatedClassSchedule = await _repository.UpdateScheduleAsync(id, createClassScheduleDTO);
                return Ok(updatedClassSchedule);
            }
            catch (NotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
            catch (Exception e)
            {
                _logger.LogError(new EventId(101, "DatabaseError"), e, "Update Class Schedule Failed");
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClassSchedule(int id)
        {
            try
            {
                await _repository.DeleteScheduleAsync(id);
                return NoContent();
            }
            catch (NotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
            catch (Exception e)
            {
                _logger.LogError(new EventId(101, "DatabaseError"), e, "Delete Class Schedule Failed");
                return StatusCode(500, e.Message);
            }
        }
    }
}