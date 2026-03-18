using System;
using System.Collections.Generic;

namespace DanceSchoolApp.Server.Models;

public partial class ItemRequisition
{
    public int RequisitionId { get; set; }

    public int ItemVariantId { get; set; }

    public int IdParent { get; set; }

    public int Quantity { get; set; }

    public DateTime RequestedAt { get; set; }

    public DateTime? ApprovedAt { get; set; }

    public DateTime? RejectedAt { get; set; }

    public DateOnly? ExpectedReturnDate { get; set; }

    public DateTime? ReturnedAt { get; set; }

    public byte Status { get; set; }

    public virtual User IdParentNavigation { get; set; } = null!;

    public virtual ItemVariant ItemVariant { get; set; } = null!;
}
