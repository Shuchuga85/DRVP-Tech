using System;
using System.Collections.Generic;

namespace DanceSchoolApp.Server.Models;

public partial class AppSetting
{
    public int SettingId { get; set; }

    public string? Key { get; set; }

    public string? Value { get; set; }

    public DateOnly? UpdatedAt { get; set; }
}
