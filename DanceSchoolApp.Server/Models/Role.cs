using System.ComponentModel.DataAnnotations;

namespace DanceSchoolApp.Server.Models
{
    public class Role
    {
        [Key]
        public int Role_Id { get; set; }
        public string Role_Name { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }
    }
}
