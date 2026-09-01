#nullable disable
using System;
using System.Collections.Generic;

namespace MyInventoryFoodCost.Core.Application.DTOs.Core;

/// <summary>TCostTrend data transfer object (domain: Core).</summary>
public class TCostTrend
{
    public string Period { get; set; } = string.Empty;
    public double AvgCostPct { get; set; }
    public decimal TotalFoodCost { get; set; }
    public decimal TotalRevenue { get; set; }
}
