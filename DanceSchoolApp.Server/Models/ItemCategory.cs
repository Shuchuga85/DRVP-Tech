using System;
using System.Collections.Generic;

namespace DanceSchoolApp.Server.Models;

public partial class ItemCategory
{
    public int CategoryId { get; set; }

    public string? CatgName { get; set; }

    public bool IsActive { get; set; }


    public virtual ICollection<Item> Items { get; set; } = new List<Item>();
}
