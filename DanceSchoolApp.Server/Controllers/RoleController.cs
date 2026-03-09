using DanceSchoolApp.Server.DTOs;
using DanceSchoolApp.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DanceSchoolApp.Server.Controllers
{

    [ApiController]
    [Route("api/[controller]s")]
    public class RoleController : ControllerBase
    {

        private readonly RoleService _roleService;

        public RoleController(RoleService roleService)
        {
            _roleService = roleService;
        }


        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] RoleCreateRequest request)
        {
            try
            {
                var result = await _roleService.CreateRoleAsync(request);

                if (!result)
                    return BadRequest("Failed to add role");


                return Ok("Role added with success");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetRoles() 
        {
            try
            {
                var roles = await _roleService.GetRolesAsync();

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
        public async Task<IActionResult> GetRole(int id)
        {
            try
            {
                var role = await _roleService.GetRoleAsync(id);

                return Ok(role);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("add")]
        public async Task<IActionResult> AddRole([FromBody] RoleAssign request)
        {
            try
            {
                var result = await _roleService.AddRoleAsync(request);

                if(!result)
                    return BadRequest("Failed to assign role");

                return Ok("Role assigned");

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("remove")]
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
