using System.ComponentModel.DataAnnotations;

namespace DanceSchoolApp.Server.DTOs.People
{
    public class CreateStaffAccountRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        public string? Username { get; set; }
    }

    public class CreateStaffAccountResponse
    {
        public int UserId { get; set; }
        public string Email { get; set; } = null!;
        public string Username { get; set; } = null!;
        public bool MustChangePassword { get; set; }
    }
}