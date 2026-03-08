using System;
using System.Collections.Generic;

namespace DanceSchoolApp.Server.Models;

public partial class Parent
{
    public int ParentId { get; set; }

    public int PersonInfoId { get; set; }

    public virtual User ParentNavigation { get; set; } = null!;

    public virtual PersonInfo PersonInfo { get; set; } = null!;

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
