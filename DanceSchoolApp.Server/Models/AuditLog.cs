using System;
using System.Collections.Generic;

namespace DanceSchoolApp.Server.Models;

public partial class AuditLog
{
    public int LogId { get; set; }

    public int? IdUser { get; set; }

    public string? Action { get; set; }

    public string? EntityType { get; set; }

    public int? EntityId { get; set; }

    public string? Description { get; set; }

    public string? IpAddress { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User? IdUserNavigation { get; set; }
}
