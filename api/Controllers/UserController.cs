using System.Data;
using api.Exceptions;
using api.Interfaces.Repository;
using api.Interfaces.Service;
using api.Models.DTOs;
using api.Models.QueryParams;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserRepository userRepository, IUserService userService) : ControllerBase
    {
        private readonly IUserRepository _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        private readonly IUserService _userService = userService ?? throw new ArgumentNullException(nameof(userService));

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] UsersQueryParams queryParams)
        {
            try
            {
                var users = await _userRepository.GetUsersAsync(queryParams);
                return Ok(users);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }


        [HttpPost("create")]
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> CreateUser([FromBody] RegisterUserDTO userDTO)
        {
            try
            {
                var user = await _userService.RegisterAsync(userDTO);
                return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
            }
            catch (DuplicateNameException e)
            {
                return Conflict(new { message = e.Message });
            }
            catch (System.Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }


        [Authorize(Policy = "RequireOwnerOrRole")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var user = await _userRepository.FindByIdAsync(id);
                return Ok(user);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (System.Exception e)
            {

                return StatusCode(500, new { message = e.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] RegisterUserDTO userDTO)
        {
            try
            {
                var user = await _userService.UpdateCredentialsAsync(id, userDTO);
                return Ok(user);
            }
            catch (DuplicateNameException e)
            {
                return Conflict(new { message = e.Message });
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (System.Exception e)
            {
                return StatusCode(500, new { message = e.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                var user = await _userRepository.DeleteUserAsync(id);
                return NoContent();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (System.Exception e)
            {
                return StatusCode(500, new { message = e.Message });
            }
        }

        [HttpGet("verify-email/{id}")]
        public async Task<IActionResult> InitializeVerification(string id)
        {
            try
            {
                var user = await _userRepository.FindByIdAsync(id);
                if (user.EmailConfirmed == true) return BadRequest(new { message = "Email is Already Verified. Proceed to the Login Page." });
                return Ok(user);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (System.Exception e)
            {
                return StatusCode(500, new { message = e.Message });
            }
        }


        // [Authorize(Policy = "RequireAdmin")]
        // public async Task<IActionResult> CreateStudent([FromBody] CreateUserDTO createUserDTO)
        // {
        //     var user = await _userRepository.CreateUserAsync(createUserDTO);
        //     return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        // }

    }
}
