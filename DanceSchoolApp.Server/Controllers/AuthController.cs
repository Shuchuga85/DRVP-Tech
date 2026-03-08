using BCrypt.Net;
using DanceSchoolApp.Server.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DanceSchoolApp.Server.Models;
using DanceSchoolApp.Server.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using DanceSchoolApp.Server.Services;


namespace DanceSchoolApp.Server.Controllers
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
                
                // result.Roles later

                return Ok("User logged in");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(LoginRequest request)
        {

            try
            {
                var result = await _authService.RegisterAsync(request);

                if (!result)
                    return BadRequest("User failed to register");

                return Ok("New user registered");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
    }
}
