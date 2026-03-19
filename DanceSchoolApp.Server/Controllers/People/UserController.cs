using DanceSchoolApp.Server.DTOs.People;
using DanceSchoolApp.Server.Services.People;
using Microsoft.AspNetCore.Mvc;

namespace DanceSchoolApp.Server.Controllers.People
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        // ─── GET /api/users ────────────────────────────────────────────────────
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var result = await _userService.GetUsersAsync();

                if (!result.Any())
                    return NoContent();

                return Ok(result);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // ─── GET /api/users/{id} ───────────────────────────────────────────────
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            try
            {
                var result = await _userService.GetUserAsync(id);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // ─── GET /api/users/{id}/roles ──────────────────────────────────────────
        [HttpGet("{id}/roles")]
        public async Task<IActionResult> GetUserRoles(int id)
        {
            try
            {
                var result = await _userService.GetUserRolesAsync(id);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // ─── POST /api/users ───────────────────────────────────────────────────
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var newId = await _userService.CreateUserAsync(request);
                return CreatedAtAction(nameof(GetUser), new { id = newId }, new { userId = newId });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // ─── PATCH /api/users/{id}/activate ───────────────────────────────────
        [HttpPatch("{id}/activate")]
        public async Task<IActionResult> ActivateUser(int id)
        {
            try
            {
                await _userService.SetUserStateAsync(id, isActive: true);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // ─── PATCH /api/users/{id}/deactivate ─────────────────────────────────
        [HttpPatch("{id}/deactivate")]
        public async Task<IActionResult> DeactivateUser(int id)
        {
            try
            {
                await _userService.SetUserStateAsync(id, isActive: false);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
