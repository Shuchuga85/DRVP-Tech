using System;
using System.Collections.Generic;

namespace DanceSchoolApp.Server.Models;

public partial class CoachAvailability
{
    public int CoachavId { get; set; }

    public int IdCoach { get; set; }

    public byte Weekday { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public DateOnly? ValidFrom { get; set; }

    public DateOnly? ValidUntil { get; set; }

    public virtual Coach IdCoachNavigation { get; set; } = null!;
}
