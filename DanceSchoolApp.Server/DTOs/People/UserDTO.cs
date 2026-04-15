using System.ComponentModel.DataAnnotations;

namespace DanceSchoolApp.Server.DTOs.People
{
    // ─── Responses ────────────────────────────────────────────────────────────

    public class UserListResponse
    {
        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public string? Email { get; set; }
        public bool IsActive { get; set; }
        public DateOnly CreatedAt { get; set; }
        public List<RoleSummaryResponse> Roles { get; set; } = new();
    }

    public class UserDetailResponse
    {
        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public string? Email { get; set; }
        public bool IsActive { get; set; }
        public DateOnly CreatedAt { get; set; }
        public PersonDetailResponse? PersonInfo { get; set; }
        public List<RoleSummaryResponse> Roles { get; set; } = new();
    }

    public class UserRolesResponse
    {
        public int UserId { get; set; }
        public List<RoleSummaryResponse> Roles { get; set; } = new();
    }


    // ─── Requests ─────────────────────────────────────────────────────────────

    public class UserCreateRequest
    {
        [Required]
        [MaxLength(64)]
        public string Username { get; set; } = null!;

        [Required]
        [EmailAddress]
        [MaxLength(254)]
        public string Email { get; set; } = null!;

        [Required]
        public byte? FirstRole { get; set; } = null;

        public PersonRequest? PersonInfo { get; set; } = null;
    }
}
