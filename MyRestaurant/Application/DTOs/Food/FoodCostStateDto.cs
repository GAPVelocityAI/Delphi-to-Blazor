#nullable disable
using System;
using System.Collections.Generic;

namespace MyRestaurant.Application.DTOs.Food;

/// <summary>FoodCostState data transfer object (domain: Food).</summary>
public class FoodCostStateDto
{
    public string AvgCostPctCaption { get; set; } = string.Empty;
    public string FRestaurantBL { get; set; } = string.Empty;
    public bool FIsAdding { get; set; }
    public int FSelectedId { get; set; }
    public string EdtRecipeNameText { get; set; } = string.Empty;
    public string EdtIngredientsText { get; set; } = string.Empty;
    public string EdtTotalCostText { get; set; } = string.Empty;
    public string EdtSellingPriceText { get; set; } = string.Empty;
}
