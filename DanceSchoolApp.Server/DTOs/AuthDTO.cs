using System.ComponentModel.DataAnnotations;

namespace DanceSchoolApp.Server.DTOs
{
    public class LoginRequest
    {
        // Either username or email must be provided — validated in service.
        public string? Username { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string Password { get; set; } = null!; 
    }

    // Token is NOT returned in the response body — it is set as an
    // HttpOnly Secure cookie by the controller. This prevents JavaScript
    // from ever accessing the token, eliminating XSS token theft.
    // The client receives only the user context it needs for the UI.
    public class LoginResponse
    {
        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public List<string> Roles { get; set; } = new();
        public DateTime ExpiresAt { get; set; }
    }
}