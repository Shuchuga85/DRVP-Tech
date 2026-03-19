using BCrypt.Net;
using DanceSchoolApp.Server.Data;
using Microsoft.AspNetCore.Mvc;
using DanceSchoolApp.Server.DTOs.People;
using DanceSchoolApp.Server.Services.People;


namespace DanceSchoolApp.Server.Controllers.People
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly AuthService _authService;


        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var result = await _authService.LoginAsync(request);

                if (result is null || result.Success == false)
                    return BadRequest("User login failed");
                
                // result.Roles later for authentication roles

                return Ok("User logged in");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
