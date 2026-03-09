using System;
using System.Collections.Generic;

namespace DanceSchoolApp.Server.Models;

public partial class Student
{
    public int StudentId { get; set; }

    public int IdParent { get; set; }

    public int PersonInfoId { get; set; }

    public bool IsActive { get; set; }

    public virtual Parent IdParentNavigation { get; set; } = null!;

    public virtual ICollection<Participant> Participants { get; set; } = new List<Participant>();

    public virtual PersonInfo PersonInfo { get; set; } = null!;
}
