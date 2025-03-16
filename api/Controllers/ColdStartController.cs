using System.Threading.Tasks;
using api.Interfaces.Repository;
using api.Interfaces.Service;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ColdStartController(IAdminService adminService, ILogger<ColdStartController> logger) : ControllerBase
    {
        private readonly IAdminService _adminService = adminService;
        private readonly ILogger<ColdStartController> _logger = logger;

        [HttpGet("")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var adminName = await _adminService.GetFirstAdminName();
                return Ok(new { testData = adminName });
            }
            catch (Exception e)
            {
                _logger.LogError(new EventId(101, "DatabaseError"), e, "Database Cold Start Failed.");
                return StatusCode(500, new { message = e.Message });
            }
        }
    }
}