using DanceSchoolApp.Server.Models;

namespace DanceSchoolApp.Server.DTOs
{
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponse
    {
        public bool Success { get; set; }
        public List<Role> Roles { get; set; }
    }
}
