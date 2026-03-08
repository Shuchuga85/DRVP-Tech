using System;
using System.Collections.Generic;

namespace DanceSchoolApp.Server.Models;

public partial class ClassValidation
{
    public int IdCoachClass { get; set; }

    public DateOnly? CoachValAt { get; set; }

    public DateOnly? StaffValAt { get; set; }

    public bool Status { get; set; }

    public virtual CoachClass IdCoachClassNavigation { get; set; } = null!;
}
