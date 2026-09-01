#nullable disable
using System;
using System.Collections.Generic;

namespace MyRestaurant.Domain.Entities.Core;

/// <summary>Port of <c>TFoodCostInfo (stored in a class-level array — the unit's table)</c>. Deterministically generated from the plan's schema.</summary>
public class FoodCost
{
    public int RecipeId { get; set; }
    public string RecipeName { get; set; } = string.Empty;
    public int? IngredientCount { get; set; }
    public decimal? TotalCost { get; set; }
    public decimal? SellingPrice { get; set; }
    public double? CostPercentage { get; set; }
}
