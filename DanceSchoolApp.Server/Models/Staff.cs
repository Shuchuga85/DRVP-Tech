using System;
using System.Collections.Generic;

namespace DanceSchoolApp.Server.Models;

public partial class Staff
{
    public int StaffId { get; set; }

    public int PersonInfoId { get; set; }

    public string? Position { get; set; }

    public virtual PersonInfo PersonInfo { get; set; } = null!;

    public virtual User StaffNavigation { get; set; } = null!;
}
