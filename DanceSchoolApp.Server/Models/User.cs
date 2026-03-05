using System.ComponentModel.DataAnnotations;

namespace DanceSchoolApp.Server.Models
{
    public class User
    {
        [Key]
        public int User_Id { get; set; }
        public string Username { get; set; }
        public string Password_Hash { get; set; }
        public bool Is_Active { get; set; }
        public DateTime Created_At { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }

    }
}
