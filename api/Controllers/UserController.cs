using api.Interfaces.Repository;
using api.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserRepository userRepository) : ControllerBase
    {
        private readonly IUserRepository _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));


        [Authorize(Policy = "RequireOwnerOrRole")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var user = await _userRepository.FindByIdAsync(id);

            if (user == null)
                return NotFound();
            return Ok(user);
        }

        // [Authorize(Policy = "RequireAdmin")]
        // public async Task<IActionResult> CreateStudent([FromBody] CreateUserDTO createUserDTO)
        // {
        //     var user = await _userRepository.CreateUserAsync(createUserDTO);
        //     return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        // }
    }
}
