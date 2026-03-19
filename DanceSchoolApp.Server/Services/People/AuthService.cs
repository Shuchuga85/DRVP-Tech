using Azure.Core;
using DanceSchoolApp.Server.Data;
using DanceSchoolApp.Server.DTOs.People;
using DanceSchoolApp.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DanceSchoolApp.Server.Services.People
{
    public class AuthService
    {

        private readonly AppDbContext _context;

        public AuthService(AppDbContext context) 
        {
            _context = context;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var user = await _context.Users
               .Include(u => u.IdRoles)
               .FirstOrDefaultAsync(u => u.Username == request.Username);

            if (user == null || string.IsNullOrEmpty(user.PasswordHash) || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                throw new Exception("Invalid username or password");

            if (!user.IsActive)
                throw new Exception("User is inactive");

            var roles = user.IdRoles.ToList();

            var response = new LoginResponse
            {
                Success = true,
                Roles = roles
            };
            return response;
        }

    }
}
