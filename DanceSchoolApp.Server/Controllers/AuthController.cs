using DanceSchoolApp.Server.DTOs;
using DanceSchoolApp.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DanceSchoolApp.Server.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private const string CookieName = "jwt";
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        // ─── POST /api/auth/login ──────────────────────────────────────────────
        // Public — no [Authorize].
        // On success sets a jwt HttpOnly Secure SameSite=Strict cookie.
        // The token never touches JavaScript — only the browser and server see it.
        // Postman handles this automatically via its cookie jar.
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var (token, response) = await _authService.LoginAsync(request);

                Response.Cookies.Append(CookieName, token, new CookieOptions
                {
                    HttpOnly = true,           // JS cannot read this cookie
                    Secure = true,           // HTTPS only
                    SameSite = SameSiteMode.Strict,
                    Expires = response.ExpiresAt,
                    Path = "/"
                });

                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // ─── POST /api/auth/logout ─────────────────────────────────────────────
        // Clears the jwt cookie by overwriting it with an expired empty cookie.
        // No server-side token invalidation needed for stateless JWT —
        // the cookie deletion is sufficient since the browser stops sending it.
        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            Response.Cookies.Append(CookieName, string.Empty, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(-1),  // immediately expired
                Path = "/"
            });

            return NoContent();
        }

        // ─── GET /api/auth/me ──────────────────────────────────────────────────
        // Returns the current authenticated user's context from JWT claims.
        // Useful for React to restore session state on page refresh without
        // a full user fetch. No DB call needed — reads directly from the token.
        [HttpGet("me")]
        [Authorize]
        public IActionResult Me()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var username = User.FindFirst(ClaimTypes.Name)?.Value;

            var roles = User.FindAll(ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            if (userIdClaim is null || username is null)
                return Unauthorized("Token claims are missing.");

            return Ok(new
            {
                UserId = int.Parse(userIdClaim),
                Username = username,
                Roles = roles
            });
        }
    }
}