#nullable disable
using System;
using System.Collections.Generic;

namespace MyInventoryFoodCost.Core.Application.DTOs.Core;

/// <summary>TRecipeIngredient data transfer object (domain: Core).</summary>
public class TRecipeIngredient
{
    public string IngredientName { get; set; } = string.Empty;
    public double Quantity { get; set; }
    public string UnitMeasure { get; set; } = string.Empty;
    public decimal UnitCost { get; set; }
    public decimal LineCost { get; set; }
}
