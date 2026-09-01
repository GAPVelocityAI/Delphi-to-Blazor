using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyRestaurant.Application.DTOs.Food;
using MyRestaurant.Infrastructure.Data;

namespace MyRestaurant.Application.Services.Food;
public class FoodCostService : IFoodCostService
{
    private readonly IDbContextFactory<MyRestaurantDbContext> _dbContextFactory;
    private readonly ILogger<FoodCostService> _logger;

    public FoodCostService(
        IDbContextFactory<MyRestaurantDbContext> dbContextFactory,
        ILogger<FoodCostService> logger)
    {
        _dbContextFactory = dbContextFactory;
        _logger = logger;
    }

    /// <summary>
    /// Loads all food cost entries from the database asynchronously, computes the average cost percentage,
    /// and updates state.AvgCostPctCaption.
    /// Legacy: LoadFoodCosts + FormCreate grid population
    /// </summary>
    public async Task<List<TFoodCostInfoDto>> LoadFoodCostsAsync(FoodCostStateDto state, CancellationToken ct = default)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync(ct);

        var entities = await db.FoodCosts
            .AsNoTracking()
            .OrderBy(fc => fc.RecipeName)
            .ToListAsync(ct);

        return BuildFoodCostList(state, entities);
    }

    /// <summary>
    /// Synchronous wrapper that delegates to the shared builder.
    /// Legacy: LoadFoodCosts procedure — populates the grid and computes Avg Cost %.
    /// </summary>
    public List<TFoodCostInfoDto> LoadFoodCosts(FoodCostStateDto state)
    {
        using var db = _dbContextFactory.CreateDbContext();
        var entities = db.FoodCosts
            .AsNoTracking()
            .OrderBy(fc => fc.RecipeName)
            .ToList();

        return BuildFoodCostList(state, entities);
    }

    /// <summary>
    /// Prepares state for editing the selected food cost entry.
    /// Legacy: btnEditClick — looks up the record by selectedId, populates edit fields.
    /// </summary>
    public async Task<FoodCostStateDto> BtnEditClickAsync(FoodCostStateDto state, int selectedId, CancellationToken ct = default)
    {
        if (selectedId <= 0)
        {
            _logger.LogWarning("BtnEditClick called with no valid selected ID.");
            return state;
        }

        state.FSelectedId = selectedId;

        await using var db = await _dbContextFactory.CreateDbContextAsync(ct);

        var entity = await db.FoodCosts
            .AsNoTracking()
            .FirstOrDefaultAsync(fc => fc.RecipeId == selectedId, ct);

        if (entity == null)
        {
            _logger.LogWarning("FoodCost with RecipeId {RecipeId} not found for edit.", selectedId);
            return state;
        }

        // Legacy: FIsAdding := False
        state.FIsAdding = false;

        // Legacy: populate edit fields from the selected row
        state.EdtRecipeNameText = entity.RecipeName ?? string.Empty;
        state.EdtIngredientsText = (entity.IngredientCount ?? default).ToString(CultureInfo.InvariantCulture);
        state.EdtTotalCostText = (entity.TotalCost ?? default).ToString("F2", CultureInfo.InvariantCulture);
        state.EdtSellingPriceText = (entity.SellingPrice ?? default).ToString("F2", CultureInfo.InvariantCulture);

        // Legacy: pnlEdit.Visible := True — handled by the caller/page

        return state;
    }

    /// <summary>
    /// Deletes the food cost entry identified by selectedId.
    /// Legacy: btnDeleteClick — confirms then calls FRestaurantBL.DeleteFoodCost(Id).
    /// Confirmation dialog is handled by the Blazor page before calling this method.
    /// </summary>
    public async Task BtnDeleteClickAsync(FoodCostStateDto state, int selectedId, CancellationToken ct = default)
    {
        if (selectedId <= 0)
        {
            _logger.LogWarning("BtnDeleteClick called with no valid selected ID.");
            return;
        }

        state.FSelectedId = selectedId;

        await using var db = await _dbContextFactory.CreateDbContextAsync(ct);

        var entity = await db.FoodCosts
            .FirstOrDefaultAsync(fc => fc.RecipeId == selectedId, ct);

        if (entity == null)
        {
            _logger.LogWarning("FoodCost with RecipeId {RecipeId} not found for deletion.", selectedId);
            return;
        }

        db.FoodCosts.Remove(entity);
        await db.SaveChangesAsync(ct);

        _logger.LogInformation("Deleted FoodCost with RecipeId {RecipeId}.", selectedId);
    }

    /// <summary>
    /// Saves a new or updated food cost entry based on state.FIsAdding.
    /// Legacy: btnSaveClick — reads edit fields, computes CostPercentage, adds or updates.
    /// </summary>
    public async Task BtnSaveClickAsync(FoodCostStateDto state, int selectedId, CancellationToken ct = default)
    {
        state.FSelectedId = selectedId;

        // Parse edit fields exactly as legacy: StrToFloatDef / StrToIntDef with 0 default
        var recipeName = state.EdtRecipeNameText ?? string.Empty;
        int ingredientCount = int.TryParse(state.EdtIngredientsText, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsedIngredients)
            ? parsedIngredients
            : 0;
        decimal totalCost = decimal.TryParse(state.EdtTotalCostText, NumberStyles.Number, CultureInfo.InvariantCulture, out var parsedTotalCost)
            ? parsedTotalCost
            : 0m;
        decimal sellingPrice = decimal.TryParse(state.EdtSellingPriceText, NumberStyles.Number, CultureInfo.InvariantCulture, out var parsedSellingPrice)
            ? parsedSellingPrice
            : 0m;

        await using var db = await _dbContextFactory.CreateDbContextAsync(ct);

        if (state.FIsAdding)
        {
            // Legacy: AddFoodCost — computes CostPercentage = (TotalCost / SellingPrice) * 100
            decimal costPercentage = sellingPrice > 0m
                ? Math.Round(totalCost / sellingPrice * 100m, 2, MidpointRounding.ToEven)
                : 0m;

            var newEntity = new global::MyRestaurant.Domain.Entities.Core.FoodCost
            {
                RecipeName = recipeName,
                IngredientCount = ingredientCount,
                TotalCost = totalCost,
                SellingPrice = sellingPrice,
                CostPercentage = (double)(costPercentage)
            };

            db.FoodCosts.Add(newEntity);
            await db.SaveChangesAsync(ct);

            _logger.LogInformation("Added new FoodCost '{RecipeName}' with RecipeId {RecipeId}.", newEntity.RecipeName, newEntity.RecipeId);
        }
        else
        {
            // Legacy: UpdateFoodCost — sets RecipeId from FSelectedId, computes CostPercentage
            if (selectedId <= 0)
            {
                _logger.LogWarning("BtnSaveClick in update mode but selectedId is invalid.");
                return;
            }

            var existing = await db.FoodCosts
                .FirstOrDefaultAsync(fc => fc.RecipeId == selectedId, ct);

            if (existing == null)
            {
                _logger.LogWarning("FoodCost with RecipeId {RecipeId} not found for update.", selectedId);
                return;
            }

            existing.RecipeName = recipeName;
            existing.IngredientCount = ingredientCount;
            existing.TotalCost = totalCost;
            existing.SellingPrice = sellingPrice;

            // Legacy: if Cost.SellingPrice > 0 then CostPercentage := (TotalCost / SellingPrice) * 100 else 0
            if (sellingPrice > 0m)
            {
                existing.CostPercentage = (double)(Math.Round(totalCost / sellingPrice * 100m, 2, MidpointRounding.ToEven));
            }
            else
            {
                existing.CostPercentage = (double)(0m);
            }

            await db.SaveChangesAsync(ct);

            _logger.LogInformation("Updated FoodCost with RecipeId {RecipeId}.", selectedId);
        }

        // Legacy: pnlEdit.Visible := False — handled by the caller/page
        // Legacy: LoadFoodCosts — caller should re-invoke LoadFoodCostsAsync after this
    }

    /// <summary>
    /// Shared builder that maps entities to DTOs and computes summary caption.
    /// </summary>
    private static List<TFoodCostInfoDto> BuildFoodCostList(FoodCostStateDto state, List<global::MyRestaurant.Domain.Entities.Core.FoodCost> entities)
    {
        if (entities.Count == 0)
        {
            state.AvgCostPctCaption = "Avg Cost %: N/A";
            return new List<TFoodCostInfoDto>();
        }

        var result = new List<TFoodCostInfoDto>(entities.Count);
        decimal totalPct = 0m;

        foreach (var e in entities)
        {
            var dto = MapToDto(e);
            result.Add(dto);
            totalPct += (decimal)(e.CostPercentage ?? 0);
        }

        decimal avgPct = Math.Round(totalPct / entities.Count, 1, MidpointRounding.ToEven);
        state.AvgCostPctCaption = "Avg Cost %: " + (avgPct / 100m).ToString("P1", CultureInfo.InvariantCulture);

        return result;
    }

    /// <summary>
    /// Maps a FoodCost entity to a TFoodCostInfoDto.
    /// Each value is carried as its own typed field — never packed into a formatted string.
    /// </summary>
    private static TFoodCostInfoDto MapToDto(global::MyRestaurant.Domain.Entities.Core.FoodCost entity)
    {
        return new TFoodCostInfoDto
        {
            RecipeId = entity.RecipeId,
            RecipeName = entity.RecipeName ?? string.Empty,
            IngredientCount = (entity.IngredientCount) ?? 0,
            TotalCost = (entity.TotalCost) ?? 0m,
            SellingPrice = (entity.SellingPrice) ?? 0m,
            CostPercentage = (decimal)((entity.CostPercentage) ?? 0d)
        };
    }
}
