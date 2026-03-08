using System;
using System.Collections.Generic;

namespace DanceSchoolApp.Server.Models;

public partial class Studio
{
    public int StudioId { get; set; }

    public string Name { get; set; } = null!;

    public int Capacity { get; set; }

    public string? Address { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<BlockedPeriod> BlockedPeriods { get; set; } = new List<BlockedPeriod>();

    public virtual ICollection<CoachClass> CoachClasses { get; set; } = new List<CoachClass>();

    public virtual ICollection<Modality> IdModalities { get; set; } = new List<Modality>();
}
