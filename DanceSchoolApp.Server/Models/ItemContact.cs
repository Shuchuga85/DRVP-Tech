using System;
using System.Collections.Generic;

namespace DanceSchoolApp.Server.Models;

public partial class ItemContact
{
    public int IcontactId { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();
}
