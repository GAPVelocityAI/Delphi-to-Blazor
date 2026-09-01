#nullable disable
using System;
using System.Collections.Generic;

namespace MyInventoryMenu.Core.Application.DTOs.Core;

/// <summary>TMenuCategorySummary data transfer object (domain: Core).</summary>
public class TMenuCategorySummary
{
    public string Category { get; set; } = string.Empty;
    public int ItemCount { get; set; }
    public decimal AvgPrice { get; set; }
    public decimal AvgCost { get; set; }
    public decimal AvgMargin { get; set; }
}
