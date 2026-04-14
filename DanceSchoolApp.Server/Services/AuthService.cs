using DanceSchoolApp.Server.Data;
using DanceSchoolApp.Server.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DanceSchoolApp.Server.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;

        public AuthService(AppDbContext context)
        {
            _context = context;
        }

        // Returns both the raw token string (for the cookie) and the
        // LoginResponse (for the response body). The controller handles
        // setting the cookie — services should not touch HttpContext.
        public async Task<(string Token, LoginResponse Response)> LoginAsync(
            LoginRequest request)
        {
            // ── 1. At least one identifier must be provided ────────────────────
            if (string.IsNullOrWhiteSpace(request.Username) &&
                string.IsNullOrWhiteSpace(request.Email))
            {
                throw new ArgumentException(
                    "Either username or email is required.");
            }

            // ── 2. Find user by username or email ─────────────────────────────
            var user = await _context.Users
                .Include(u => u.IdRoles)
                .FirstOrDefaultAsync(u =>
                    (!string.IsNullOrWhiteSpace(request.Username) &&
                      u.Username == request.Username) ||
                    (!string.IsNullOrWhiteSpace(request.Email) &&
                      u.Email == request.Email));

            // Generic message — do not reveal which field was wrong.
            if (user is null)
                throw new UnauthorizedAccessException("Invalid credentials.");

            // ── 3. Check account is active ────────────────────────────────────
            if (!user.IsActive)
                throw new UnauthorizedAccessException("This account is inactive.");

            // ── 4. Verify password ────────────────────────────────────────────
            bool passwordValid = BCrypt.Net.BCrypt.Verify(
                request.Password, user.PasswordHash);

            if (!passwordValid)
                throw new UnauthorizedAccessException("Invalid credentials.");

            // ── 5. Build JWT ───────────────────────────────────────────────────
            var roles = user.IdRoles.Select(r => r.RoleName).ToList();
            var expiresAt = DateTime.UtcNow.AddDays(7);
            var token = GenerateToken(user.UserId, user.Username, roles, expiresAt);

            var response = new LoginResponse
            {
                UserId = user.UserId,
                Username = user.Username,
                Roles = roles,
                ExpiresAt = expiresAt
            };

            return (token, response);
        }

        private static string GenerateToken(
            int userId, string username, List<string> roles, DateTime expiresAt)
        {
            var secret = Environment.GetEnvironmentVariable("DanceSchoolApp_JWT_Secret");

            if (string.IsNullOrWhiteSpace(secret))
                throw new InvalidOperationException(
                    "JWT secret is not configured. " +
                    "Set the 'DanceSchoolApp_JWT_Secret' environment variable.");

            if (secret.Length < 32)
                throw new InvalidOperationException(
                    "JWT secret must be at least 32 characters long.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,        userId.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, username),
                new Claim(JwtRegisteredClaimNames.Jti,        Guid.NewGuid().ToString())
            };

            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var token = new JwtSecurityToken(
                issuer: "DanceSchoolApp",
                audience: "DanceSchoolApp",
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}