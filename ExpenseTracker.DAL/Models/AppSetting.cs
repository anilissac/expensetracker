using System;
using System.Collections.Generic;

namespace ExpenseTracker.DAL.Models;

/// <summary>
/// Common setting values for application
/// </summary>
public partial class AppSetting
{
    /// <summary>
    /// Primary Key Identifier of this table.
    /// </summary>
    public byte AppSettingID { get; set; }

    /// <summary>
    /// Configuration Name used in the applications
    /// </summary>
    public string AppSettingName { get; set; }

    /// <summary>
    /// Configuration Values for curresponding names
    /// </summary>
    public string AppSettingValue { get; set; }

    public string DisplayName { get; set; }

    /// <summary>
    /// Description about the configuration Name and Value
    /// </summary>
    public string Description { get; set; }

    public string Category { get; set; }
}
