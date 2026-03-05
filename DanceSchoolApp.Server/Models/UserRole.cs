using System.ComponentModel.DataAnnotations;

namespace DanceSchoolApp.Server.Models
{
    public class UserRole
    {
        public int Id_User { get; set; }
        public int Id_Role { get; set; }

        public User User { get; set; }
        public Role Role { get; set; }
    }
}
