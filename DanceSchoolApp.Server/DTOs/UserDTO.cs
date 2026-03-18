namespace DanceSchoolApp.Server.DTOs
{
    public class UserListResponse
    {
        public int UserId { get; set; }
        public string Usarname { get; set; }
        public string? Email { get; set; }
        public bool IsActive { get; set; }
        public DateOnly CreatedAt { get; set; }
        public List<RoleSummaryResponse> IdRoles { get; set; } = new();
    }

    public class UserDetailResponse
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string? Email { get; set; }
        public bool IsActive { get; set; }
        public DateOnly CreatedAt { get; set; }
        public PersonDetailResponse? PersonInfo { get; set; }
        public List<RoleSummaryResponse> IdRoles { get; set; } = new();
    }
    public class UserCreateRequest
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int? FirstRole { get; set; } = null;
        public PersonRequest? PersonInfo { get; set; } = null;
    }

    public class UserActivationRequest
    {
        public int UserId { get; set; }
        public bool IsActive { get; set; }
    }
}
