using System;
using System.Collections.Generic;

namespace DanceSchoolApp.Server.Models;

public partial class Role
{
    public byte RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public virtual ICollection<User> IdUsers { get; set; } = new List<User>();
}
