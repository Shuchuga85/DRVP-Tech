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

    public class LoginResponse
    {
        public string Token { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public List<string> Roles { get; set; } = new();
    }
}