namespace DanceSchoolApp.Server.DTOs
{
    public class RoleRequest
    {
        public byte RoleId { get; set; }
        public string RoleName { get; set; }
    }

    public class RoleAssign
    {
        public int UserId { get; set; }
        public byte RoleId { get; set; }
    }

    public class RoleResponse
    {
        public byte RoleId { get; set; }
        public string RoleName { get; set; } = null!;
        public List<UserResponse> Users { get; set; } = new();
    }

}
