#nullable disable
using System;
using System.Collections.Generic;

namespace MyInventoryStock.Core.Application.DTOs.Core;

/// <summary>TStockItem data transfer object (domain: Core).</summary>
public class TStockItem
{
    public int ItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string UnitMeasure { get; set; } = string.Empty;
    public double CurrentQty { get; set; }
    public double MinStock { get; set; }
    public decimal UnitCost { get; set; }
    public bool NeedsReorder { get; set; }
}
