using System;
using System.Collections.Generic;
using System.Linq;
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
    public class SectionController(ISectionService sectionService) : ControllerBase
    {
        private readonly ISectionService _sectionService = sectionService;

        [HttpGet]
        // [Authorize(Policy = "RequireTeacherOrAdmin")]
        public async Task<IActionResult> GetSections()
        {
            try
            {
                var sections = await _sectionService.GetSectionsAsync();
                return Ok(sections);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }


        [HttpPost("create")]
        // [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> CreateSection([FromBody] CreateSectionDTO createSectionDTO)
        {
            try
            {
                var section = await _sectionService.CreateSectionAsync(createSectionDTO);
                return Ok(section);
                // return CreatedAtAction(nameof(GetSectionById), new { id = section.Id }, section); 
                // //Implement once getSectionById is implemented in this controller

            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut("{id}")]
        // [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> UpdateSection([FromRoute] int id, [FromBody] CreateSectionDTO createSectionDTO)
        {
            try
            {
                var section = await _sectionService.UpdateSectionAsync(id, createSectionDTO);
                return Ok(section);
            }
            catch (NotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("{id}")]
        // [Authorize]
        public async Task<IActionResult> GetSectionById([FromRoute] int id)
        {
            try
            {
                var section = await _sectionService.GetSectionByIdAsync(id);
                return Ok(section);
            }
            catch (NotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete("{id}")]
        // [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> DeleteSection([FromRoute] int id)
        {
            try
            {
                await _sectionService.DeleteSectionAsync(id);
                return NoContent();
            }
            catch (NotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}