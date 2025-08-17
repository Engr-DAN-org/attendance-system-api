using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Exceptions;
using api.Interfaces.Repository;
using api.Models.QueryParams;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubjectTeacherController(ISubjectTeacherRepository repository, ILogger<SubjectTeacherController> logger) : ControllerBase
    {

        private readonly ISubjectTeacherRepository _subjectRepository = repository;
        private readonly ILogger<SubjectTeacherController> _logger = logger;

        [HttpGet]
        public async Task<IActionResult> GetSubjectTeachers([FromQuery] SubjectTeacherQueryParams queryParams)
        {
            try
            {
                var subjectTeachers = await _subjectRepository.QueryAsync(queryParams);
                return Ok(subjectTeachers);
            }
            catch (System.Exception e)
            {
                _logger.LogError(new EventId(101, "DatabaseError"), e, "Get Subject Teachers Failed");
                return StatusCode(500, new { message = e.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSubjectTeacherById(int id)
        {
            try
            {
                var subjectTeacher = await _subjectRepository.GetByIdAsync(id);
                return Ok(subjectTeacher);
            }
            catch (NotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
            catch (System.Exception e)
            {
                _logger.LogError(new EventId(101, "DatabaseError"), e, "Get Subject Teacher by Id Failed");
                return StatusCode(500, new { message = e.Message });
            }
        }
    }
}