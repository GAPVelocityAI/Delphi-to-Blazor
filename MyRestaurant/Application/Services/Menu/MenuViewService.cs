using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyRestaurant.Application.DTOs.Menu;
using MyRestaurant.Infrastructure.Data;

namespace MyRestaurant.Application.Services.Menu;

public class MenuViewService : IMenuViewService
{
    private readonly IDbContextFactory<MyRestaurantDbContext> _dbFactory;
    private readonly ILogger<MenuViewService> _logger;

    public MenuViewService(
        IDbContextFactory<MyRestaurantDbContext> dbFactory,
        ILogger<MenuViewService> logger)
    {
        _dbFactory = dbFactory;
        _logger = logger;
    }

    /// <summary>
    /// Loads all menu items from the database, ordered by Category then ItemName,
    /// and updates the state's AllMenuItems and DisplayedItems lists.
    /// Mirrors legacy: FAllMenuItems := FRestaurantBL.GetMenuItems; LoadMenuItems(FAllMenuItems);
    /// </summary>
    public async Task<List<TMenuItemInfoDto>> LoadMenuItemsAsync(MenuViewStateDto state, CancellationToken ct = default)
    {
        await using var db = await _dbFactory.CreateDbContextAsync(ct);

        var entities = await db.MenuItems
            .AsNoTracking()
            .OrderBy(m => m.Category)
            .ThenBy(m => m.ItemName)
            .ToListAsync(ct);

        var result = new List<TMenuItemInfoDto>(entities.Count);
        foreach (var e in entities)
        {
            result.Add(new TMenuItemInfoDto
            {
                ItemId = e.ItemId,
                ItemName = e.ItemName ?? string.Empty,
                Category = e.Category ?? string.Empty,
                Price = e.Price ?? 0m,
                Cost = e.Cost ?? 0m,
                Active = e.Active ?? true
            });
        }

        state.AllMenuItems = result;
        state.DisplayedItems = new List<TMenuItemInfoDto>(result);

        _logger.LogDebug("Loaded {Count} menu items", result.Count);
        return result;
    }

    /// <summary>
    /// Filters a pre-loaded list of menu items by category in-memory.
    /// Mirrors legacy: procedure LoadMenuItems(const AItems: TArray&lt;TMenuItemInfo&gt;)
    /// combined with FilterByCategory logic that filters FAllMenuItems in-place.
    /// If category is empty or "All", returns all items unfiltered.
    /// </summary>
    public List<TMenuItemInfoDto> LoadMenuItems(List<TMenuItemInfoDto> allItems, string category)
    {
        if (allItems == null)
        {
            _logger.LogDebug("LoadMenuItems called with null list");
            return new List<TMenuItemInfoDto>();
        }

        if (string.IsNullOrEmpty(category) || string.Equals(category, "All", StringComparison.OrdinalIgnoreCase))
        {
            _logger.LogDebug("LoadMenuItems returning all {Count} items (no filter)", allItems.Count);
            return new List<TMenuItemInfoDto>(allItems);
        }

        var filtered = allItems
            .Where(m => string.Equals(m.Category, category, StringComparison.OrdinalIgnoreCase))
            .ToList();

        _logger.LogDebug("LoadMenuItems filtered by '{Category}': {Count} results", category, filtered.Count);
        return filtered;
    }

    /// <summary>
    /// Filters menu items by category from the database and updates the state's DisplayedItems.
    /// If category is empty or "All", loads all items.
    /// Mirrors legacy FilterByCategory procedure.
    /// </summary>
    public async Task FilterByCategoryAsync(string category, MenuViewStateDto state, CancellationToken ct = default)
    {
        if (string.IsNullOrEmpty(category) || string.Equals(category, "All", StringComparison.OrdinalIgnoreCase))
        {
            await LoadMenuItemsAsync(state, ct);
            return;
        }

        await using var db = await _dbFactory.CreateDbContextAsync(ct);

        var entities = await db.MenuItems
            .AsNoTracking()
            .Where(m => m.Category != null && m.Category.ToLower() == category.ToLower())
            .OrderBy(m => m.Category)
            .ThenBy(m => m.ItemName)
            .ToListAsync(ct);

        var result = new List<TMenuItemInfoDto>(entities.Count);
        foreach (var e in entities)
        {
            result.Add(new TMenuItemInfoDto
            {
                ItemId = e.ItemId,
                ItemName = e.ItemName ?? string.Empty,
                Category = e.Category ?? string.Empty,
                Price = e.Price ?? 0m,
                Cost = e.Cost ?? 0m,
                Active = e.Active ?? true
            });
        }

        state.DisplayedItems = result;

        _logger.LogDebug("FilterByCategoryAsync: filtered by '{Category}': {Count} results", category, result.Count);
    }

    /// <summary>
    /// Filters by category using in-memory data from state and updates the state's displayed items list.
    /// Mirrors legacy: procedure FilterByCategory(const ACategory: string);
    /// which filtered FAllMenuItems in-place and reloaded the grid.
    /// </summary>
    public Task FilterByCategory(string category, MenuViewStateDto state, CancellationToken ct = default)
    {
        var filtered = LoadMenuItems(state.AllMenuItems, category);
        state.DisplayedItems = filtered;
        _logger.LogDebug("FilterByCategory updated state with {Count} items for category '{Category}'", filtered.Count, category);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Populates the edit state from a selected menu item ID.
    /// Mirrors legacy btnEditClick: reads the selected row's data and populates edit fields.
    /// </summary>
    public async Task<MenuViewStateDto> BtnEditClickAsync(MenuViewStateDto state, int selectedItemId, CancellationToken ct = default)
    {
        if (selectedItemId <= 0)
        {
            _logger.LogWarning("BtnEditClick called with invalid selectedItemId: {Id}", selectedItemId);
            return state;
        }

        await using var db = await _dbFactory.CreateDbContextAsync(ct);

        var entity = await db.MenuItems
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.ItemId == selectedItemId, ct);

        if (entity == null)
        {
            _logger.LogWarning("Menu item with ID {Id} not found for editing", selectedItemId);
            return state;
        }

        state.FIsAdding = false;
        state.FSelectedId = selectedItemId;
        state.EdtItemNameText = entity.ItemName ?? string.Empty;
        state.CmbEditCategoryText = entity.Category ?? string.Empty;
        state.EdtPriceText = (entity.Price ?? 0m).ToString(CultureInfo.InvariantCulture);
        state.EdtCostText = (entity.Cost ?? 0m).ToString(CultureInfo.InvariantCulture);
        state.CmbEditActiveIndex = (entity.Active ?? true) ? 0 : 1;
        state.PnlEditVisible = true;

        _logger.LogDebug("Populated edit state for menu item ID {Id}", selectedItemId);
        return state;
    }

    /// <summary>
    /// Deletes a menu item by its ID, then reloads the state's item lists.
    /// Mirrors legacy btnDeleteClick: confirms then deletes.
    /// Confirmation dialog is handled by the UI layer; this method performs the actual delete.
    /// </summary>
    public async Task BtnDeleteClickAsync(MenuViewStateDto state, int selectedItemId, CancellationToken ct = default)
    {
        if (selectedItemId <= 0)
        {
            _logger.LogWarning("BtnDeleteClick called with invalid selectedItemId: {Id}", selectedItemId);
            return;
        }

        await using var db = await _dbFactory.CreateDbContextAsync(ct);

        var entity = await db.MenuItems
            .FirstOrDefaultAsync(m => m.ItemId == selectedItemId, ct);

        if (entity == null)
        {
            _logger.LogWarning("Menu item with ID {Id} not found for deletion", selectedItemId);
            return;
        }

        db.MenuItems.Remove(entity);
        await db.SaveChangesAsync(ct);

        _logger.LogInformation("Deleted menu item ID {Id}", selectedItemId);

        // Reload items after deletion, mirroring legacy:
        // FAllMenuItems := FRestaurantBL.GetMenuItems; LoadMenuItems(FAllMenuItems);
        await LoadMenuItemsAsync(state, ct);
    }

    /// <summary>
    /// Creates or updates a menu item based on the state's FIsAdding flag.
    /// Mirrors legacy btnSaveClick.
    /// </summary>
    public async Task BtnSaveClickAsync(MenuViewStateDto state, int selectedItemId, CancellationToken ct = default)
    {
        await using var db = await _dbFactory.CreateDbContextAsync(ct);

        decimal price = 0m;
        if (!string.IsNullOrWhiteSpace(state.EdtPriceText))
        {
            decimal.TryParse(state.EdtPriceText, NumberStyles.Any, CultureInfo.InvariantCulture, out price);
        }

        decimal cost = 0m;
        if (!string.IsNullOrWhiteSpace(state.EdtCostText))
        {
            decimal.TryParse(state.EdtCostText, NumberStyles.Any, CultureInfo.InvariantCulture, out cost);
        }

        bool active = state.CmbEditActiveIndex == 0;

        if (state.FIsAdding)
        {
            var newEntity = new global::MyRestaurant.Domain.Entities.Core.MenuItem
            {
                ItemName = state.EdtItemNameText ?? string.Empty,
                Category = state.CmbEditCategoryText ?? string.Empty,
                Price = price,
                Cost = cost,
                Active = active
            };

            db.MenuItems.Add(newEntity);
            await db.SaveChangesAsync(ct);

            _logger.LogInformation("Added new menu item '{Name}' with ID {Id}", newEntity.ItemName, newEntity.ItemId);
        }
        else
        {
            int idToUpdate = selectedItemId > 0 ? selectedItemId : state.FSelectedId;

            if (idToUpdate <= 0)
            {
                _logger.LogWarning("BtnSaveClick in edit mode but no valid ID provided. selectedItemId={SelId}, FSelectedId={StateId}", selectedItemId, state.FSelectedId);
                return;
            }

            var existing = await db.MenuItems
                .FirstOrDefaultAsync(m => m.ItemId == idToUpdate, ct);

            if (existing == null)
            {
                _logger.LogWarning("Menu item with ID {Id} not found for update", idToUpdate);
                return;
            }

            existing.ItemName = state.EdtItemNameText ?? string.Empty;
            existing.Category = state.CmbEditCategoryText ?? string.Empty;
            existing.Price = price;
            existing.Cost = cost;
            existing.Active = active;

            await db.SaveChangesAsync(ct);

            _logger.LogInformation("Updated menu item ID {Id}", idToUpdate);
        }

        state.PnlEditVisible = false;

        // Reload items after save, mirroring legacy:
        // FAllMenuItems := FRestaurantBL.GetMenuItems; LoadMenuItems(FAllMenuItems);
        await LoadMenuItemsAsync(state, ct);
    }

    public async Task<List<TMenuItemInfo>> GetMenuItemsAsync(CancellationToken ct = default)
    {
        var state = new MenuViewStateDto();
        return await LoadMenuItemsAsync(state, ct);
    }

    public async Task DeleteMenuItemAsync(int id, CancellationToken ct = default)
    {
        await BtnDeleteClickAsync(new MenuViewStateDto(), id, ct);
    }

    public async Task AddMenuItemAsync(TMenuItemInfo item, CancellationToken ct = default)
    {
        // Legacy btnSaveClick with FIsAdding = True: the edit fields are the payload.
        await BtnSaveClickAsync(StateFromItem(item, isAdding: true), 0, ct);
    }

    public async Task UpdateMenuItemAsync(TMenuItemInfo item, CancellationToken ct = default)
    {
        // Legacy btnSaveClick with FIsAdding = False: Item.ItemId := FSelectedId.
        await BtnSaveClickAsync(StateFromItem(item, isAdding: false), item?.ItemId ?? 0, ct);
    }

    /// <summary>
    /// Projects a menu item onto the edit-panel state that BtnSaveClickAsync reads,
    /// mirroring how the legacy form filled edtItemName/cmbEditCategory/edtPrice/
    /// edtCost/cmbEditActive before calling btnSaveClick.
    /// </summary>
    private static MenuViewStateDto StateFromItem(TMenuItemInfo item, bool isAdding)
    {
        item ??= new TMenuItemInfo();

        return new MenuViewStateDto
        {
            FIsAdding = isAdding,
            FSelectedId = isAdding ? 0 : item.ItemId,
            EdtItemNameText = item.ItemName ?? string.Empty,
            CmbEditCategoryText = item.Category ?? string.Empty,
            EdtPriceText = item.Price.ToString(CultureInfo.InvariantCulture),
            EdtCostText = item.Cost.ToString(CultureInfo.InvariantCulture),
            // cmbEditActive: index 0 = "Yes", index 1 = "No"
            CmbEditActiveIndex = item.Active ? 0 : 1
        };
    }
}
