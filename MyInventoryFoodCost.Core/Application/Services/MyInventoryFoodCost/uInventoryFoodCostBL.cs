using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyInventoryFoodCost.Core.Application.DTOs.Core;
using MyInventoryFoodCost.Core.Application.DTOs.DTOs;

namespace MyInventoryFoodCost.Core.Application.Services.MyInventoryFoodCost;

public class uInventoryFoodCostBL : IuInventoryFoodCostBL
{
    private readonly ILogger<uInventoryFoodCostBL> _logger;

    public uInventoryFoodCostBL(ILogger<uInventoryFoodCostBL> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Returns the full list of recipe costs with their ingredients.
    /// Legacy: GetRecipeCosts
    /// </summary>
    public List<TRecipeCost> GetRecipeCosts()
    {
        return BuildRecipeCosts();
    }

    /// <summary>
    /// Returns the ingredients for a specific recipe by searching through all recipe costs.
    /// Legacy: GetRecipeIngredients
    /// </summary>
    public List<TRecipeIngredient> GetRecipeIngredients(int recipeId)
    {
        var recipes = BuildRecipeCosts();

        for (int i = 0; i < recipes.Count; i++)
        {
            if (recipes[i].RecipeId == recipeId)
            {
                return recipes[i].Ingredients;
            }
        }

        // Recipe not found — return empty list (legacy returned nil)
        return new List<TRecipeIngredient>();
    }

    /// <summary>
    /// Returns cost trend data aggregated by period.
    /// Legacy: GetCostTrends
    /// </summary>
    public List<TCostTrend> GetCostTrends()
    {
        return BuildCostTrends();
    }

    /// <summary>
    /// Async wrapper for GetRecipeCosts.
    /// Legacy: GetRecipeCosts
    /// </summary>
    public async Task<List<TRecipeCost>> GetRecipeCostsAsync(CancellationToken ct = default)
    {
        await Task.CompletedTask;
        ct.ThrowIfCancellationRequested();

        return BuildRecipeCosts();
    }

    /// <summary>
    /// Async wrapper for GetRecipeIngredients.
    /// Legacy: GetRecipeIngredients
    /// </summary>
    public async Task<List<TRecipeIngredient>> GetRecipeIngredientsAsync(int recipeId, CancellationToken ct = default)
    {
        await Task.CompletedTask;
        ct.ThrowIfCancellationRequested();

        var recipes = BuildRecipeCosts();

        for (int i = 0; i < recipes.Count; i++)
        {
            if (recipes[i].RecipeId == recipeId)
            {
                return recipes[i].Ingredients;
            }
        }

        // Recipe not found — return empty list (legacy returned nil)
        return new List<TRecipeIngredient>();
    }

    /// <summary>
    /// Async wrapper for GetCostTrends.
    /// Legacy: GetCostTrends
    /// </summary>
    public async Task<List<TCostTrend>> GetCostTrendsAsync(CancellationToken ct = default)
    {
        await Task.CompletedTask;
        ct.ThrowIfCancellationRequested();

        return BuildCostTrends();
    }

    /// <summary>
    /// Builds the full list of recipe costs with their ingredients.
    /// Legacy: GetRecipeCosts
    /// </summary>
    private List<TRecipeCost> BuildRecipeCosts()
    {
        var result = new List<TRecipeCost>();

        // Recipe 1: Caesar Salad
        AddRecipe(result, 1, "Caesar Salad", "Appetizer", 3.75m, 12.50m,
            new List<TRecipeIngredient>
            {
                MakeIngredient("Romaine Lettuce", 0.25m, "kg", 3.00m),
                MakeIngredient("Parmesan", 0.05m, "kg", 28.00m),
                MakeIngredient("Croutons", 0.10m, "kg", 5.00m),
                MakeIngredient("Caesar Dressing", 0.05m, "liters", 12.00m)
            });

        // Recipe 2: Grilled Salmon
        AddRecipe(result, 2, "Grilled Salmon", "Main Course", 8.50m, 24.00m,
            new List<TRecipeIngredient>
            {
                MakeIngredient("Salmon Fillet", 0.25m, "kg", 22.00m),
                MakeIngredient("Olive Oil", 0.03m, "liters", 18.50m),
                MakeIngredient("Lemon", 0.05m, "kg", 4.00m),
                MakeIngredient("Asparagus", 0.15m, "kg", 8.00m),
                MakeIngredient("Butter", 0.03m, "kg", 7.50m)
            });

        // Recipe 3: Margherita Pizza
        AddRecipe(result, 3, "Margherita Pizza", "Main Course", 4.20m, 16.00m,
            new List<TRecipeIngredient>
            {
                MakeIngredient("Flour", 0.30m, "kg", 2.80m),
                MakeIngredient("Mozzarella", 0.15m, "kg", 12.00m),
                MakeIngredient("Tomatoes", 0.20m, "kg", 3.20m),
                MakeIngredient("Basil", 0.02m, "kg", 15.00m),
                MakeIngredient("Olive Oil", 0.02m, "liters", 18.50m)
            });

        // Recipe 4: Chicken Parmesan
        AddRecipe(result, 4, "Chicken Parmesan", "Main Course", 6.80m, 19.50m,
            new List<TRecipeIngredient>
            {
                MakeIngredient("Chicken Breast", 0.30m, "kg", 9.50m),
                MakeIngredient("Parmesan", 0.05m, "kg", 28.00m),
                MakeIngredient("Tomatoes", 0.15m, "kg", 3.20m),
                MakeIngredient("Mozzarella", 0.10m, "kg", 12.00m)
            });

        // Recipe 5: Risotto
        AddRecipe(result, 5, "Risotto", "Main Course", 5.10m, 18.00m,
            new List<TRecipeIngredient>
            {
                MakeIngredient("Rice", 0.20m, "kg", 4.50m),
                MakeIngredient("Parmesan", 0.06m, "kg", 28.00m),
                MakeIngredient("Butter", 0.05m, "kg", 7.50m),
                MakeIngredient("Chicken Breast", 0.15m, "kg", 9.50m),
                MakeIngredient("Heavy Cream", 0.08m, "liters", 5.50m)
            });

        // Recipe 6: Tiramisu
        AddRecipe(result, 6, "Tiramisu", "Dessert", 3.20m, 10.00m,
            new List<TRecipeIngredient>
            {
                MakeIngredient("Heavy Cream", 0.15m, "liters", 5.50m),
                MakeIngredient("Mascarpone", 0.10m, "kg", 14.00m),
                MakeIngredient("Espresso", 0.05m, "liters", 8.00m),
                MakeIngredient("Ladyfingers", 0.08m, "kg", 6.00m)
            });

        // Recipe 7: Bruschetta
        AddRecipe(result, 7, "Bruschetta", "Appetizer", 2.40m, 9.50m,
            new List<TRecipeIngredient>
            {
                MakeIngredient("Tomatoes", 0.15m, "kg", 3.20m),
                MakeIngredient("Basil", 0.02m, "kg", 15.00m),
                MakeIngredient("Garlic", 0.01m, "kg", 6.00m),
                MakeIngredient("Olive Oil", 0.03m, "liters", 18.50m)
            });

        // Recipe 8: Panna Cotta
        AddRecipe(result, 8, "Panna Cotta", "Dessert", 2.80m, 9.00m,
            new List<TRecipeIngredient>
            {
                MakeIngredient("Heavy Cream", 0.20m, "liters", 5.50m),
                MakeIngredient("Vanilla Extract", 0.01m, "liters", 45.00m),
                MakeIngredient("Sugar", 0.05m, "kg", 2.00m),
                MakeIngredient("Gelatin", 0.01m, "kg", 35.00m)
            });

        // Recipe 9: Seafood Pasta
        AddRecipe(result, 9, "Seafood Pasta", "Main Course", 9.20m, 22.00m,
            new List<TRecipeIngredient>
            {
                MakeIngredient("Pasta", 0.20m, "kg", 3.50m),
                MakeIngredient("Salmon Fillet", 0.15m, "kg", 22.00m),
                MakeIngredient("Shrimp", 0.10m, "kg", 28.00m),
                MakeIngredient("Garlic", 0.01m, "kg", 6.00m),
                MakeIngredient("Olive Oil", 0.03m, "liters", 18.50m)
            });

        // Recipe 10: Lemonade
        AddRecipe(result, 10, "Lemonade", "Beverage", 0.85m, 5.00m,
            new List<TRecipeIngredient>
            {
                MakeIngredient("Lemons", 0.10m, "kg", 4.00m),
                MakeIngredient("Sugar", 0.05m, "kg", 2.00m),
                MakeIngredient("Mint", 0.01m, "kg", 15.00m)
            });

        return result;
    }

    /// <summary>
    /// Builds the full list of cost trends.
    /// Legacy: GetCostTrends
    /// </summary>
    private List<TCostTrend> BuildCostTrends()
    {
        var trends = new List<TCostTrend>
        {
            new TCostTrend
            {
                Period = "2026-02",
                AvgCostPct = (double)(30.5m),
                TotalFoodCost = 12400.00m,
                TotalRevenue = 40655.74m
            },
            new TCostTrend
            {
                Period = "2026-03",
                AvgCostPct = (double)(31.2m),
                TotalFoodCost = 13100.00m,
                TotalRevenue = 41987.18m
            },
            new TCostTrend
            {
                Period = "2026-04",
                AvgCostPct = (double)(29.8m),
                TotalFoodCost = 12800.00m,
                TotalRevenue = 42953.02m
            },
            new TCostTrend
            {
                Period = "2026-05",
                AvgCostPct = (double)(32.1m),
                TotalFoodCost = 14200.00m,
                TotalRevenue = 44237.07m
            },
            new TCostTrend
            {
                Period = "2026-06",
                AvgCostPct = (double)(30.9m),
                TotalFoodCost = 13600.00m,
                TotalRevenue = 44012.94m
            },
            new TCostTrend
            {
                Period = "2026-07",
                AvgCostPct = (double)(31.5m),
                TotalFoodCost = 14500.00m,
                TotalRevenue = 46031.75m
            }
        };

        return trends;
    }

    /// <summary>
    /// Helper: creates a TRecipeIngredient with computed LineCost = Quantity * UnitCost.
    /// Legacy: MakeIngredient (nested function in GetRecipeCosts)
    /// </summary>
    private static TRecipeIngredient MakeIngredient(string name, decimal quantity, string unitMeasure, decimal unitCost)
    {
        // LineCost = Quantity * UnitCost, matching legacy: Result.LineCost := AQty * AUnitCost
        decimal lineCost = quantity * unitCost;

        return new TRecipeIngredient
        {
            IngredientName = name,
            Quantity = (double)(quantity),
            UnitMeasure = unitMeasure,
            UnitCost = unitCost,
            LineCost = lineCost
        };
    }

    /// <summary>
    /// Helper: adds a recipe to the list, computing CostPercentage and Profitable flag.
    /// Legacy: AddRecipe (nested procedure in GetRecipeCosts)
    /// </summary>
    private static void AddRecipe(
        List<TRecipeCost> recipes,
        int recipeId,
        string recipeName,
        string category,
        decimal totalCost,
        decimal sellingPrice,
        List<TRecipeIngredient> ingredients)
    {
        decimal costPercentage;
        if (sellingPrice > 0m)
        {
            // Legacy: (ACost / APrice) * 100
            costPercentage = Math.Round(totalCost / sellingPrice * 100m, 4, MidpointRounding.ToEven);
        }
        else
        {
            costPercentage = 0m;
        }

        // Legacy: Arr[Idx].Profitable := Arr[Idx].CostPercentage < 35
        bool profitable = costPercentage < 35m;

        recipes.Add(new TRecipeCost
        {
            RecipeId = recipeId,
            RecipeName = recipeName,
            Category = category,
            Ingredients = ingredients,
            TotalCost = totalCost,
            SellingPrice = sellingPrice,
            CostPercentage = (double)(costPercentage),
            Profitable = profitable
        });
    }
}
