#nullable disable
using System;
using System.Collections.Generic;
namespace CommonLib.Application.DTOs.Core;

/// <summary>TFoodCostInfo data transfer object (domain: Core).</summary>
public class TFoodCostInfo
{
    public int RecipeId { get; set; }
    public string RecipeName { get; set; } = string.Empty;
    public int IngredientCount { get; set; }
    public decimal TotalCost { get; set; }
    public decimal SellingPrice { get; set; }
    public decimal CostPercentage { get; set; }
}
