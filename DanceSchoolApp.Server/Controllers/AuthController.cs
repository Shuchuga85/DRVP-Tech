using BCrypt.Net;
using DanceSchoolApp.Server.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DanceSchoolApp.Server.Models;
using DanceSchoolApp.Server.DTOs;


namespace DanceSchoolApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _context.User
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Username == request.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password_Hash))
                return Unauthorized("Invalid username or password");

            if (!user.Is_Active)
                return Unauthorized("User is inactive");

            // For now we just return user info and roles
            var roles = user.UserRoles.Select(ur => ur.Role.Role_Name).ToList();

            return Ok(new
            {
                userId = user.User_Id,
                username = user.Username,
                roles
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(LoginRequest request)
        {
            var hash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User
            {
                Username = request.Username,
                Password_Hash = hash,
                Is_Active = true,
                Created_At = DateTime.Now
            };

            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }
    }
}
