using System;
using System.Collections.Generic;

namespace DanceSchoolApp.Server.Models;

public partial class BlockedPeriod
{
    public int BlockedId { get; set; }

    public DateTime StartDatetime { get; set; }

    public DateTime EndDatetime { get; set; }

    public byte Scope { get; set; }

    public int? IdCoach { get; set; }

    public int? IdStudio { get; set; }

    public string? Reason { get; set; }

    public virtual Coach? IdCoachNavigation { get; set; }

    public virtual Studio? IdStudioNavigation { get; set; }
}
