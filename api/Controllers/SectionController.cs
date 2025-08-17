using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Exceptions;
using api.Interfaces.Service;
using api.Models.DTOs;
using api.Models.QueryParams;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SectionController(ISectionService sectionService, ILogger<SectionController> logger) : ControllerBase
    {
        private readonly ISectionService _sectionService = sectionService;
        private readonly ILogger<SectionController> _logger = logger;

        [HttpGet]
        // [Authorize(Policy = "RequireTeacherOrAdmin")]
        public async Task<IActionResult> GetSections([FromQuery] SectionQueryParams queryParams)
        {
            try
            {
                var sections = await _sectionService.GetSectionsAsync(queryParams);
                return Ok(sections);
            }
            catch (Exception e)
            {
                _logger.LogError(new EventId(101, "DatabaseError"), e, "Get Sections Failed");
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
                _logger.LogError(new EventId(101, "DatabaseError"), e, "Create Section Failed");
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
                _logger.LogError(new EventId(101, "DatabaseError"), e, "Update Section Failed");
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
                _logger.LogError(new EventId(101, "DatabaseError"), e, "Get Section by Id Failed");
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
                _logger.LogError(new EventId(101, "DatabaseError"), e, "Delete Section Failed");
                return StatusCode(500, e.Message);
            }
        }
    }
}