using System;
using System.Collections.Generic;

namespace DanceSchoolApp.Server.Models;

public partial class NewsPost
{
    public int PostId { get; set; }

    public string Title { get; set; } = null!;

    public string? Subtitle { get; set; }

    public string? Description { get; set; }

    public string? ImageUrl { get; set; }

    public DateOnly? CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public virtual User? CreatedByNavigation { get; set; }
}
