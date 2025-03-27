using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<IActionResult> GetSections()
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }


        [HttpPost("create")]
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> CreateSection(CreateSectionDTO createSectionDTO)
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
    }
}