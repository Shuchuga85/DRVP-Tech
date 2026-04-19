using System;
using System.Collections.Generic;

namespace DanceSchoolApp.Server.Models;

public partial class AppSetting
{
    public int SettingId { get; set; }

    public string? SettingKey { get; set; }

    public string? SettingValue { get; set; }

    public DateOnly? UpdatedAt { get; set; }
}
