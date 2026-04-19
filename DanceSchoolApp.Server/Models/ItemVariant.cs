using System;
using System.Collections.Generic;

namespace DanceSchoolApp.Server.Models;

public partial class ItemVariant
{
    public int VariantId { get; set; }

    public int IdItem { get; set; }

    public string? Color { get; set; }

    public string? Size { get; set; }

    public int Quantity { get; set; }

    public decimal? Price { get; set; }

    public bool? IsActive { get; set; }

    public virtual Item IdItemNavigation { get; set; } = null!;

    public virtual ICollection<ItemRequisition> ItemRequisitions { get; set; } = new List<ItemRequisition>();
}
