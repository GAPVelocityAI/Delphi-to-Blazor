#nullable disable
using System;
using System.Collections.Generic;

namespace MyInventoryMenu.Core.Application.DTOs.Core;

/// <summary>TMenuCostItem data transfer object (domain: Core).</summary>
public class TMenuCostItem
{
    public int MenuItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal FoodCost { get; set; }
    public decimal LaborCost { get; set; }
    public decimal OverheadCost { get; set; }
    public decimal TotalCost { get; set; }
    public decimal SellingPrice { get; set; }
    public double ProfitMargin { get; set; }
}
