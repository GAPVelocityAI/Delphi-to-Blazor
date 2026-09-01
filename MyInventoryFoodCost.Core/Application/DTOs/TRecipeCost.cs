#nullable disable
using System;
using System.Collections.Generic;
using MyInventoryFoodCost.Core.Application.DTOs.Core;

namespace MyInventoryFoodCost.Core.Application.DTOs.DTOs;

/// <summary>Ported from Delphi <c>TRecipeCost</c> in uInventoryFoodCostBL.pas.</summary>
public class TRecipeCost
{
    public int RecipeId { get; set; }
    public string RecipeName { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public List<TRecipeIngredient> Ingredients { get; set; } = new();
    public decimal TotalCost { get; set; }
    public decimal SellingPrice { get; set; }
    public double CostPercentage { get; set; }
    public bool Profitable { get; set; }
}
