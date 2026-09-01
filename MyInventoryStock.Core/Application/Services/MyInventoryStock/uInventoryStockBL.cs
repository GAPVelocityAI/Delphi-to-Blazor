using Microsoft.Extensions.Logging;
using MyInventoryStock.Core.Application.DTOs.Core;

namespace MyInventoryStock.Core.Application.Services.MyInventoryStock;

public class uInventoryStockBL : IuInventoryStockBL
{
    private readonly ILogger<uInventoryStockBL> _logger;

    public uInventoryStockBL(ILogger<uInventoryStockBL> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Synchronous version ported from TInventoryStockBL.GetStockItems.
    /// Returns the full stock item catalog with reorder flags.
    /// </summary>
    public List<TStockItem> GetStockItems()
    {
        _logger.LogDebug("GetStockItems called");
        return BuildStockItemList();
    }

    /// <summary>
    /// Synchronous version ported from TInventoryStockBL.GetStockMovements.
    /// Returns recent stock movements (receipts and issues).
    /// </summary>
    public List<TStockMovement> GetStockMovements()
    {
        _logger.LogDebug("GetStockMovements called");
        return BuildStockMovementList();
    }

    /// <summary>
    /// Synchronous version ported from TInventoryStockBL.GetStockValuation.
    /// Returns stock valuation aggregated by category.
    /// </summary>
    public List<TStockValuation> GetStockValuation()
    {
        _logger.LogDebug("GetStockValuation called");
        return BuildStockValuationList();
    }

    /// <summary>
    /// Async version ported from TInventoryStockBL.GetStockItems.
    /// Returns the full stock item catalog with reorder flags.
    /// Legacy used hardcoded seed data; in production this would query StockItems table
    /// using SQL_GET_STOCK_ITEMS.
    /// </summary>
    public async Task<TStockItem[]> GetStockItemsAsync(CancellationToken ct = default)
    {
        _logger.LogDebug("GetStockItemsAsync called");
        await Task.CompletedTask;
        return BuildStockItemList().ToArray();
    }

    /// <summary>
    /// Async version ported from TInventoryStockBL.GetStockMovements.
    /// Returns recent stock movements (receipts and issues).
    /// Legacy used hardcoded seed data; in production this would query StockMovements
    /// joined with StockItems using SQL_GET_STOCK_MOVEMENTS.
    /// </summary>
    public async Task<TStockMovement[]> GetStockMovementsAsync(CancellationToken ct = default)
    {
        _logger.LogDebug("GetStockMovementsAsync called");
        await Task.CompletedTask;
        return BuildStockMovementList().ToArray();
    }

    /// <summary>
    /// Async version ported from TInventoryStockBL.GetStockValuation.
    /// Returns stock valuation aggregated by category.
    /// Legacy used hardcoded seed data; in production this would query StockItems
    /// with GROUP BY using SQL_GET_STOCK_VALUATION.
    /// </summary>
    public async Task<TStockValuation[]> GetStockValuationAsync(CancellationToken ct = default)
    {
        _logger.LogDebug("GetStockValuationAsync called");
        await Task.CompletedTask;
        return BuildStockValuationList().ToArray();
    }

    private static List<TStockItem> BuildStockItemList()
    {
        return new List<TStockItem>
        {
            BuildStockItem(1,  "Olive Oil",      "Oils",       "liters", 12m,  5m,  18.50m),
            BuildStockItem(2,  "Flour",           "Dry Goods",  "kg",     45m, 20m,   2.80m),
            BuildStockItem(3,  "Tomatoes",        "Produce",    "kg",     30m, 10m,   3.20m),
            BuildStockItem(4,  "Mozzarella",      "Dairy",      "kg",     15m,  5m,  12.00m),
            BuildStockItem(5,  "Salmon Fillet",   "Seafood",    "kg",      8m,  3m,  22.00m),
            BuildStockItem(6,  "Chicken Breast",  "Meat",       "kg",     20m,  8m,   9.50m),
            BuildStockItem(7,  "Garlic",          "Produce",    "kg",      5m,  2m,   6.00m),
            BuildStockItem(8,  "Basil",           "Herbs",      "kg",      3m,  1m,  15.00m),
            BuildStockItem(9,  "Parmesan",        "Dairy",      "kg",     10m,  3m,  28.00m),
            BuildStockItem(10, "Rice",            "Dry Goods",  "kg",     25m, 10m,   4.50m),
            BuildStockItem(11, "Butter",          "Dairy",      "kg",      8m,  3m,   7.50m),
            BuildStockItem(12, "Lemons",          "Produce",    "kg",      6m,  2m,   4.00m),
            BuildStockItem(13, "Wine Vinegar",    "Condiments", "liters",  4m,  2m,   8.00m),
            BuildStockItem(14, "Black Pepper",    "Spices",     "kg",      2m,  1m,  35.00m),
            BuildStockItem(15, "Heavy Cream",     "Dairy",      "liters", 10m,  4m,   5.50m),
        };
    }

    private static List<TStockMovement> BuildStockMovementList()
    {
        return new List<TStockMovement>
        {
            BuildStockMovement(1,  "Olive Oil",      "IN",  24.0m,  new DateTime(2026, 7, 28, 0, 0, 0, DateTimeKind.Utc), "PO-2026-045"),
            BuildStockMovement(2,  "Flour",           "IN",  50.0m,  new DateTime(2026, 7, 27, 0, 0, 0, DateTimeKind.Utc), "PO-2026-044"),
            BuildStockMovement(3,  "Tomatoes",        "OUT",  8.0m,  new DateTime(2026, 8, 1, 0, 0, 0, DateTimeKind.Utc),  "Order #1023"),
            BuildStockMovement(4,  "Mozzarella",      "OUT",  5.0m,  new DateTime(2026, 8, 1, 0, 0, 0, DateTimeKind.Utc),  "Order #1024"),
            BuildStockMovement(5,  "Salmon Fillet",   "IN",  12.0m,  new DateTime(2026, 7, 30, 0, 0, 0, DateTimeKind.Utc), "PO-2026-046"),
            BuildStockMovement(6,  "Chicken Breast",  "OUT", 10.0m,  new DateTime(2026, 8, 2, 0, 0, 0, DateTimeKind.Utc),  "Order #1025"),
            BuildStockMovement(7,  "Basil",           "OUT",  1.5m,  new DateTime(2026, 8, 2, 0, 0, 0, DateTimeKind.Utc),  "Order #1026"),
            BuildStockMovement(8,  "Parmesan",        "IN",  15.0m,  new DateTime(2026, 7, 25, 0, 0, 0, DateTimeKind.Utc), "PO-2026-042"),
            BuildStockMovement(9,  "Rice",            "OUT",  5.0m,  new DateTime(2026, 8, 3, 0, 0, 0, DateTimeKind.Utc),  "Order #1027"),
            BuildStockMovement(10, "Heavy Cream",     "IN",  20.0m,  new DateTime(2026, 7, 29, 0, 0, 0, DateTimeKind.Utc), "PO-2026-043"),
        };
    }

    private static List<TStockValuation> BuildStockValuationList()
    {
        return new List<TStockValuation>
        {
            BuildStockValuation("Dairy",      4, 43.0m, 601.00m),
            BuildStockValuation("Produce",    3, 41.0m, 228.00m),
            BuildStockValuation("Oils",       1, 12.0m, 222.00m),
            BuildStockValuation("Meat",       1, 20.0m, 190.00m),
            BuildStockValuation("Seafood",    1,  8.0m, 176.00m),
            BuildStockValuation("Dry Goods",  2, 70.0m, 238.50m),
            BuildStockValuation("Spices",     1,  2.0m,  70.00m),
            BuildStockValuation("Herbs",      1,  3.0m,  45.00m),
            BuildStockValuation("Condiments", 1,  4.0m,  32.00m),
        };
    }

    /// <summary>
    /// Mirrors the legacy AddItem nested procedure.
    /// NeedsReorder is true when CurrentQty &lt;= MinStock (legacy: AQty &lt;= AMin).
    /// </summary>
    private static TStockItem BuildStockItem(
        int itemId,
        string itemName,
        string category,
        string unitMeasure,
        decimal currentQty,
        decimal minStock,
        decimal unitCost)
    {
        return new TStockItem
        {
            ItemId = itemId,
            ItemName = itemName,
            Category = category,
            UnitMeasure = unitMeasure,
            CurrentQty = (double)(currentQty),
            MinStock = (double)(minStock),
            UnitCost = unitCost,
            NeedsReorder = currentQty <= minStock
        };
    }

    /// <summary>
    /// Mirrors the legacy AddMovement nested procedure.
    /// </summary>
    private static TStockMovement BuildStockMovement(
        int movementId,
        string itemName,
        string movementType,
        decimal quantity,
        DateTime movementDate,
        string reference)
    {
        return new TStockMovement
        {
            MovementId = movementId,
            ItemName = itemName,
            MovementType = movementType,
            Quantity = (double)(quantity),
            MovementDate = movementDate,
            Reference = reference
        };
    }

    /// <summary>
    /// Mirrors the legacy AddValuation nested procedure.
    /// </summary>
    private static TStockValuation BuildStockValuation(
        string category,
        int itemCount,
        decimal totalQuantity,
        decimal totalValue)
    {
        return new TStockValuation
        {
            Category = category,
            ItemCount = itemCount,
            TotalQuantity = (double)(totalQuantity),
            TotalValue = totalValue
        };
    }
}
