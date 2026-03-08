using System;
using System.Collections.Generic;

namespace DanceSchoolApp.Server.Models;

public partial class ItemImage
{
    public int ImageId { get; set; }

    public int? IdItem { get; set; }

    public string? ImageUrl { get; set; }

    public virtual Item? IdItemNavigation { get; set; }
}
