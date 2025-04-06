using System.Data;
using api.Exceptions;
using api.Interfaces.Repository;
using api.Models.DTOs;
using api.Models.QueryParams;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize(Policy = "RequireAdmin")]
    public class CourseController(ICourseRepository courseRepository) : ControllerBase
    {
        private readonly ICourseRepository _courseRepository = courseRepository;

        [HttpGet]
        public async Task<IActionResult> GetCourses([FromQuery] CourseQuery courseQuery)
        {
            try
            {
                var courses = await _courseRepository.GetCourseQueryAsync(courseQuery);
                return Ok(courses);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourseById(int id)
        {
            try
            {
                var course = await _courseRepository.GetCourseByIdAsync(id);
                return Ok(course);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateCourse([FromBody] CreateCourseDTO createCourseDTO)
        {
            try
            {
                var course = await _courseRepository.CreateCourseAsync(createCourseDTO);
                return Ok(course);
            }
            catch (DuplicateNameException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(int id, [FromBody] CreateCourseDTO createCourseDTO)
        {
            try
            {
                var course = await _courseRepository.UpdateCourseAsync(id, createCourseDTO);
                return Ok(course);
            }
            catch (NotFoundException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            try
            {
                await _courseRepository.DeleteCourseAsync(id);
                return NoContent();
            }
            catch (NotFoundException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

    }
}