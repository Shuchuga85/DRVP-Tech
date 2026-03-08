using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DanceSchoolApp.Server.DTOs;
using DanceSchoolApp.Server.Services;

namespace DanceSchoolApp.Server.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {

        private readonly RoleService _roleService;

        public RoleController(RoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddRole([FromBody] RoleRequest request)
        {
            try
            {
                var result = await _roleService.AddRoleAsync(request);

                if (!result)
                    return BadRequest("Failed to add role");


                return Ok("Role added with success");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> AllRoles() 
        {
            try
            {
                var roles = await _roleService.AllRolesAsync();

                if (roles is null || !roles.Any()) 
                {
                    return NotFound();
                }

                return Ok(roles);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoles(int id)
        {
            try
            {
                var role = await _roleService.GetRolesAsync(id);

                return Ok(role);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("assign")]
        public async Task<IActionResult> AssignRole([FromBody] RoleAssign request)
        {
            try
            {
                var result = await _roleService.AssignRoleAsync(request);

                if(!result)
                    return BadRequest("Failed to assign role");

                return Ok("Role assigned");

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("remove")]
        public async Task<IActionResult> RemoveRole([FromBody] RoleAssign request)
        {
            try
            {
                var result = await _roleService.RemoveRoleAsync(request);
                if (!result)
                    return BadRequest("Failed to remove role");

                return Ok("Role Removed");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            
        }
    }
}
