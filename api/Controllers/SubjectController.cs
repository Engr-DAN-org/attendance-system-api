using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using api.Exceptions;
using api.Interfaces.Repository;
using api.Models.DTOs;
using api.Models.QueryParams;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubjectController(ISubjectRepository subjectRepository) : ControllerBase
    {

        private readonly ISubjectRepository _subjectRepository = subjectRepository;

        [HttpPost]
        public async Task<IActionResult> CreateSubject([FromBody] CreateSubjectDTO subjectDTO)
        {
            try
            {
                var subject = await _subjectRepository.CreateSubjectAsync(subjectDTO);
                return CreatedAtAction(nameof(GetSubjectById), new { id = subject.Id }, subject);
            }
            catch (DuplicateNameException e)
            {
                return Conflict(new { message = e.Message });
            }
            catch (System.Exception e)
            {
                return StatusCode(500, new { message = e.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> QuerySubjects([FromQuery] SubjectQueryParams queryParams)
        {
            try
            {
                var subjects = await _subjectRepository.QuerySubjectsAsync(queryParams);
                return Ok(subjects);
            }
            catch (System.Exception e)
            {
                return StatusCode(500, new { message = e.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSubjectById(int id)
        {
            try
            {
                var subject = await _subjectRepository.GetSubjectByIdAsync(id);
                return Ok(subject);

            }
            catch (NotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
            catch (System.Exception e)
            {
                return StatusCode(500, new { message = e.Message });

            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubject(int id)
        {
            try
            {
                await _subjectRepository.DeleteSubjectAsync(id);
                return NoContent();

            }
            catch (NotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
            catch (System.Exception e)
            {
                return StatusCode(500, new { message = e.Message });

            }
        }

    }
}