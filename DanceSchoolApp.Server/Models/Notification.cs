using System;
using System.Collections.Generic;

namespace DanceSchoolApp.Server.Models;

public partial class Notification
{
    public int NotificationId { get; set; }

    public int? IdUser { get; set; }

    public string? Title { get; set; }

    public string? Message { get; set; }

    public byte? Type { get; set; }

    public string? EntityType { get; set; }

    public int? EntityId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? ReadAt { get; set; }

    public bool IsSent { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual User? IdUserNavigation { get; set; }
}
