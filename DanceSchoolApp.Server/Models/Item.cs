using System;
using System.Collections.Generic;

namespace DanceSchoolApp.Server.Models;

public partial class Item
{
    public int ItemId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public bool FromSchool { get; set; }

    public int? IdOwner { get; set; }

    public int? IdCategory { get; set; }

    public string? ContactPhone { get; set; }

    public string? ContactEmail { get; set; }

    public string? ContactAddress { get; set; }

    public DateOnly? CreatedAt { get; set; }

    public bool IsActive { get; set; }

    public virtual ItemCategory? IdCategoryNavigation { get; set; }

    public virtual User? IdOwnerNavigation { get; set; }

    public virtual ICollection<ItemImage> ItemImages { get; set; } = new List<ItemImage>();

    public virtual ICollection<ItemVariant> ItemVariants { get; set; } = new List<ItemVariant>();
}
