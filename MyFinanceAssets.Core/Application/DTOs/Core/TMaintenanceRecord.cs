#nullable disable
using System;
using System.Collections.Generic;

namespace MyFinanceAssets.Core.Application.DTOs.Core;

/// <summary>TMaintenanceRecord data transfer object (domain: Core).</summary>
public class TMaintenanceRecord
{
    public int AssetId { get; set; }
    public string AssetName { get; set; } = string.Empty;
    public DateTime LastMaintenance { get; set; }
    public DateTime NextMaintenance { get; set; }
    public decimal Cost { get; set; }
}
