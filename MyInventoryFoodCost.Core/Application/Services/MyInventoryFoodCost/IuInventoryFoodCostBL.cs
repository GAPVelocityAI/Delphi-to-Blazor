using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MyInventoryFoodCost.Core.Application.DTOs.Core;
using MyInventoryFoodCost.Core.Application.DTOs.DTOs;

namespace MyInventoryFoodCost.Core.Application.Services.MyInventoryFoodCost;

public interface IuInventoryFoodCostBL
{
    List<TRecipeCost> GetRecipeCosts();
    List<TRecipeIngredient> GetRecipeIngredients(int recipeId);
    List<TCostTrend> GetCostTrends();
    Task<List<TRecipeCost>> GetRecipeCostsAsync(CancellationToken ct = default);
    Task<List<TRecipeIngredient>> GetRecipeIngredientsAsync(int recipeId, CancellationToken ct = default);
    Task<List<TCostTrend>> GetCostTrendsAsync(CancellationToken ct = default);
}
