using System.Globalization;
using Microsoft.Extensions.Logging;
using MyInventoryMenu.Core.Application.DTOs.Core;

namespace MyInventoryMenu.Core.Application.Services.MyInventoryMenu;

public class uInventoryMenuBL : IuInventoryMenuBL
{
    private readonly ILogger<uInventoryMenuBL> _logger;

    public uInventoryMenuBL(ILogger<uInventoryMenuBL> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Synchronous version ported from TInventoryMenuBL.GetMenuCosts.
    /// </summary>
    public List<TMenuCostItem> GetMenuCosts()
    {
        var result = new List<TMenuCostItem>(12);

        AddMenuItem(result, 1,  "Caesar Salad",     "Appetizer",   3.75m, 1.50m, 0.80m, 12.50m);
        AddMenuItem(result, 2,  "Bruschetta",       "Appetizer",   2.40m, 1.20m, 0.60m,  9.50m);
        AddMenuItem(result, 3,  "Soup of the Day",  "Appetizer",   2.10m, 1.00m, 0.50m,  8.00m);
        AddMenuItem(result, 4,  "Grilled Salmon",   "Main Course", 8.50m, 3.00m, 1.50m, 24.00m);
        AddMenuItem(result, 5,  "Margherita Pizza", "Main Course", 4.20m, 2.00m, 1.00m, 16.00m);
        AddMenuItem(result, 6,  "Chicken Parmesan", "Main Course", 6.80m, 2.50m, 1.20m, 19.50m);
        AddMenuItem(result, 7,  "Risotto",          "Main Course", 5.10m, 2.20m, 1.00m, 18.00m);
        AddMenuItem(result, 8,  "Seafood Pasta",    "Main Course", 9.20m, 3.00m, 1.50m, 22.00m);
        AddMenuItem(result, 9,  "Tiramisu",         "Dessert",     3.20m, 1.00m, 0.50m, 10.00m);
        AddMenuItem(result, 10, "Panna Cotta",      "Dessert",     2.80m, 0.80m, 0.40m,  9.00m);
        AddMenuItem(result, 11, "Lemonade",         "Beverage",    0.85m, 0.50m, 0.30m,  5.00m);
        AddMenuItem(result, 12, "Espresso",         "Beverage",    0.60m, 0.40m, 0.25m,  4.00m);

        _logger.LogDebug("GetMenuCosts returned {Count} items", result.Count);
        return result;
    }

    /// <summary>
    /// Synchronous version ported from TInventoryMenuBL.GetMenuCategorySummary.
    /// </summary>
    public List<TMenuCategorySummary> GetMenuCategorySummary()
    {
        var result = new List<TMenuCategorySummary>(4);

        AddCategory(result, "Appetizer",   3, 10.00m,  3.45m, 65.50m);
        AddCategory(result, "Main Course", 5, 19.90m,  9.40m, 52.76m);
        AddCategory(result, "Dessert",     2,  9.50m,  2.93m, 69.16m);
        AddCategory(result, "Beverage",    2,  4.50m,  0.97m, 78.44m);

        _logger.LogDebug("GetMenuCategorySummary returned {Count} categories", result.Count);
        return result;
    }

    /// <summary>
    /// Synchronous version ported from TInventoryMenuBL.GetTableAvailability.
    /// </summary>
    public List<TTableAvailability> GetTableAvailability()
    {
        var result = new List<TTableAvailability>(15);

        AddTable(result, 1,   1,  2,  "Indoor",  "Occupied");
        AddTable(result, 2,   2,  2,  "Indoor",  "Available");
        AddTable(result, 3,   3,  4,  "Indoor",  "Occupied");
        AddTable(result, 4,   4,  4,  "Indoor",  "Reserved");
        AddTable(result, 5,   5,  6,  "Indoor",  "Available");
        AddTable(result, 6,   6,  2,  "Bar",     "Occupied");
        AddTable(result, 7,   7,  2,  "Bar",     "Available");
        AddTable(result, 8,   8,  4,  "Bar",     "Occupied");
        AddTable(result, 9,   9,  4,  "Patio",   "Available");
        AddTable(result, 10, 10,  6,  "Patio",   "Occupied");
        AddTable(result, 11, 11,  8,  "Patio",   "Available");
        AddTable(result, 12, 12,  2,  "Patio",   "Reserved");
        AddTable(result, 13, 13,  4,  "Private", "Available");
        AddTable(result, 14, 14,  8,  "Private", "Reserved");
        AddTable(result, 15, 15, 10,  "Private", "Available");

        _logger.LogDebug("GetTableAvailability returned {Count} tables", result.Count);
        return result;
    }

    /// <summary>
    /// Async version ported from TInventoryMenuBL.GetMenuCosts — builds the full menu cost breakdown
    /// with computed TotalCost and ProfitMargin for each item.
    /// </summary>
    public async Task<TMenuCostItem[]> GetMenuCostsAsync(CancellationToken ct = default)
    {
        await Task.CompletedTask;
        return GetMenuCosts().ToArray();
    }

    /// <summary>
    /// Async version ported from TInventoryMenuBL.GetMenuCategorySummary — returns pre-computed
    /// category-level aggregates (count, avg price, avg cost, avg margin).
    /// </summary>
    public async Task<TMenuCategorySummary[]> GetMenuCategorySummaryAsync(CancellationToken ct = default)
    {
        await Task.CompletedTask;
        return GetMenuCategorySummary().ToArray();
    }

    /// <summary>
    /// Async version ported from TInventoryMenuBL.GetTableAvailability — returns the full table layout
    /// with zone, status, and computed IsAvailable flag.
    /// </summary>
    public async Task<TTableAvailability[]> GetTableAvailabilityAsync(CancellationToken ct = default)
    {
        await Task.CompletedTask;
        return GetTableAvailability().ToArray();
    }

    /// <summary>
    /// Ported from the nested AddItem procedure inside TInventoryMenuBL.GetMenuCosts.
    /// Computes TotalCost = FoodCost + LaborCost + OverheadCost and
    /// ProfitMargin = ((SellingPrice - TotalCost) / SellingPrice) * 100 when SellingPrice > 0.
    /// </summary>
    private static void AddMenuItem(
        List<TMenuCostItem> list,
        int menuItemId,
        string itemName,
        string category,
        decimal foodCost,
        decimal laborCost,
        decimal overheadCost,
        decimal sellingPrice)
    {
        decimal totalCost = foodCost + laborCost + overheadCost;

        decimal profitMargin;
        if (sellingPrice > 0m)
        {
            profitMargin = Math.Round((sellingPrice - totalCost) / sellingPrice * 100m, 2, MidpointRounding.ToEven);
        }
        else
        {
            profitMargin = 0m;
        }

        list.Add(new TMenuCostItem
        {
            MenuItemId = menuItemId,
            ItemName = itemName,
            Category = category,
            FoodCost = foodCost,
            LaborCost = laborCost,
            OverheadCost = overheadCost,
            TotalCost = totalCost,
            SellingPrice = sellingPrice,
            ProfitMargin = (double)(profitMargin)
        });
    }

    /// <summary>
    /// Ported from the nested AddCategory procedure inside TInventoryMenuBL.GetMenuCategorySummary.
    /// </summary>
    private static void AddCategory(
        List<TMenuCategorySummary> list,
        string category,
        int itemCount,
        decimal avgPrice,
        decimal avgCost,
        decimal avgMargin)
    {
        list.Add(new TMenuCategorySummary
        {
            Category = category,
            ItemCount = itemCount,
            AvgPrice = avgPrice,
            AvgCost = avgCost,
            AvgMargin = avgMargin
        });
    }

    /// <summary>
    /// Ported from the nested AddTable procedure inside TInventoryMenuBL.GetTableAvailability.
    /// IsAvailable is computed as (Status == "Available"), matching the legacy check (AStatus = 'Available').
    /// </summary>
    private static void AddTable(
        List<TTableAvailability> list,
        int tableId,
        int tableNumber,
        int capacity,
        string zone,
        string status)
    {
        list.Add(new TTableAvailability
        {
            TableId = tableId,
            TableNumber = tableNumber,
            Capacity = capacity,
            Zone = zone,
            Status = status,
            IsAvailable = string.Equals(status, "Available", StringComparison.Ordinal)
        });
    }
}
