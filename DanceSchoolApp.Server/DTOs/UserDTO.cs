namespace DanceSchoolApp.Server.DTOs
{
    public class UserListResponse
    {
        public int UserId { get; set; }
        public string Usarname { get; set; }
        public bool IsActive { get; set; }
        public DateOnly CreatedAt { get; set; }
        public List<RoleSummaryResponse> IdRoles { get; set; } = new();
    }

    public class UserDetailResponse
    {
        public int UserId { get; set; }

        public string Username { get; set; } = null!;

        public bool IsActive { get; set; }

        public DateOnly CreatedAt { get; set; }

        public CoachResponse? Coach { get; set; }

        public ParentResponse? Parent { get; set; }

        public StaffResponse? Staff { get; set; }

        public List<RoleSummaryResponse> IdRoles { get; set; } = new();
    }

    public class ParentResponse
    {
        public int ParentId { get; set; }
    }
    public class CoachResponse
    {
        public int CoachId { get; set; }
    }
    public class StaffResponse
    {
        public int StaffId { get; set; }
        public string? Position { get; set; }
    }
    public class UserActivationRequest
    {
        public int UserId { get; set; }
        public bool IsActive { get; set; }
    }
}
