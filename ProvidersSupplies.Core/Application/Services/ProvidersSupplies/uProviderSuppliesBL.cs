using Microsoft.Extensions.Logging;
using ProvidersSupplies.Core.Application.DTOs.Core;

namespace ProvidersSupplies.Core.Application.Services.ProvidersSupplies;

public class uProviderSuppliesBL : IuProviderSuppliesBL
{
    private readonly ILogger<uProviderSuppliesBL> _logger;

    public uProviderSuppliesBL(ILogger<uProviderSuppliesBL> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Ported from TProviderSuppliesBL.GetSupplies — returns the full supply catalog
    /// with provider information, ordered by Category then ItemName.
    /// Legacy source: hardcoded seed data representing what SQL_GET_SUPPLIES would return.
    /// </summary>
    public TSupplyItem[] GetSupplies()
    {
        _logger.LogDebug("GetSupplies called");

        return new TSupplyItem[]
        {
            new TSupplyItem { SupplyId = 1,  ProviderId = 1, ProviderName = "Fresh Farms Co",       ItemName = "Tomatoes",       Category = "Produce",   UnitCost = 3.20m,  MinOrderQty = 10, LeadTimeDays = 1, InStock = true },
            new TSupplyItem { SupplyId = 2,  ProviderId = 1, ProviderName = "Fresh Farms Co",       ItemName = "Garlic",         Category = "Produce",   UnitCost = 6.00m,  MinOrderQty = 5,  LeadTimeDays = 1, InStock = true },
            new TSupplyItem { SupplyId = 3,  ProviderId = 1, ProviderName = "Fresh Farms Co",       ItemName = "Lemons",         Category = "Produce",   UnitCost = 4.00m,  MinOrderQty = 5,  LeadTimeDays = 1, InStock = true },
            new TSupplyItem { SupplyId = 4,  ProviderId = 2, ProviderName = "Ocean Direct",         ItemName = "Salmon Fillet",  Category = "Seafood",   UnitCost = 22.00m, MinOrderQty = 5,  LeadTimeDays = 2, InStock = true },
            new TSupplyItem { SupplyId = 5,  ProviderId = 2, ProviderName = "Ocean Direct",         ItemName = "Shrimp",         Category = "Seafood",   UnitCost = 28.00m, MinOrderQty = 3,  LeadTimeDays = 2, InStock = true },
            new TSupplyItem { SupplyId = 6,  ProviderId = 3, ProviderName = "Valley Meats",         ItemName = "Chicken Breast", Category = "Meat",      UnitCost = 9.50m,  MinOrderQty = 10, LeadTimeDays = 2, InStock = true },
            new TSupplyItem { SupplyId = 7,  ProviderId = 4, ProviderName = "Dairy Delights",       ItemName = "Mozzarella",     Category = "Dairy",     UnitCost = 12.00m, MinOrderQty = 5,  LeadTimeDays = 1, InStock = true },
            new TSupplyItem { SupplyId = 8,  ProviderId = 4, ProviderName = "Dairy Delights",       ItemName = "Butter",         Category = "Dairy",     UnitCost = 7.50m,  MinOrderQty = 5,  LeadTimeDays = 1, InStock = false },
            new TSupplyItem { SupplyId = 9,  ProviderId = 4, ProviderName = "Dairy Delights",       ItemName = "Heavy Cream",    Category = "Dairy",     UnitCost = 5.50m,  MinOrderQty = 10, LeadTimeDays = 1, InStock = true },
            new TSupplyItem { SupplyId = 10, ProviderId = 5, ProviderName = "Golden Grain Supply",  ItemName = "Flour",          Category = "Dry Goods", UnitCost = 2.80m,  MinOrderQty = 25, LeadTimeDays = 3, InStock = true },
            new TSupplyItem { SupplyId = 11, ProviderId = 5, ProviderName = "Golden Grain Supply",  ItemName = "Rice",           Category = "Dry Goods", UnitCost = 4.50m,  MinOrderQty = 25, LeadTimeDays = 3, InStock = true },
            new TSupplyItem { SupplyId = 12, ProviderId = 6, ProviderName = "Mediterranean Imports", ItemName = "Olive Oil",     Category = "Oils",      UnitCost = 18.50m, MinOrderQty = 10, LeadTimeDays = 5, InStock = true }
        };
    }

    /// <summary>
    /// Ported from TProviderSuppliesBL.GetPurchaseOrders — returns purchase orders
    /// ordered by OrderDate descending.
    /// Legacy source: hardcoded seed data representing what SQL_GET_PURCHASE_ORDERS would return.
    /// </summary>
    public TPurchaseOrder[] GetPurchaseOrders()
    {
        _logger.LogDebug("GetPurchaseOrders called");

        return new TPurchaseOrder[]
        {
            new TPurchaseOrder { POId = 1045, ProviderName = "Fresh Farms Co",       OrderDate = new DateTime(2026, 7, 28, 0, 0, 0, DateTimeKind.Utc), ExpectedDelivery = new DateTime(2026, 7, 29, 0, 0, 0, DateTimeKind.Utc), TotalAmount = 320.00m, Status = "Delivered",  ItemCount = 3 },
            new TPurchaseOrder { POId = 1046, ProviderName = "Ocean Direct",         OrderDate = new DateTime(2026, 7, 30, 0, 0, 0, DateTimeKind.Utc), ExpectedDelivery = new DateTime(2026, 8, 1,  0, 0, 0, DateTimeKind.Utc), TotalAmount = 610.00m, Status = "Delivered",  ItemCount = 2 },
            new TPurchaseOrder { POId = 1047, ProviderName = "Valley Meats",         OrderDate = new DateTime(2026, 8, 1,  0, 0, 0, DateTimeKind.Utc), ExpectedDelivery = new DateTime(2026, 8, 3,  0, 0, 0, DateTimeKind.Utc), TotalAmount = 475.00m, Status = "In Transit", ItemCount = 1 },
            new TPurchaseOrder { POId = 1048, ProviderName = "Dairy Delights",       OrderDate = new DateTime(2026, 8, 2,  0, 0, 0, DateTimeKind.Utc), ExpectedDelivery = new DateTime(2026, 8, 3,  0, 0, 0, DateTimeKind.Utc), TotalAmount = 285.00m, Status = "Pending",    ItemCount = 3 },
            new TPurchaseOrder { POId = 1049, ProviderName = "Golden Grain Supply",  OrderDate = new DateTime(2026, 8, 3,  0, 0, 0, DateTimeKind.Utc), ExpectedDelivery = new DateTime(2026, 8, 6,  0, 0, 0, DateTimeKind.Utc), TotalAmount = 245.00m, Status = "Pending",    ItemCount = 2 },
            new TPurchaseOrder { POId = 1050, ProviderName = "Mediterranean Imports", OrderDate = new DateTime(2026, 7, 15, 0, 0, 0, DateTimeKind.Utc), ExpectedDelivery = new DateTime(2026, 7, 20, 0, 0, 0, DateTimeKind.Utc), TotalAmount = 370.00m, Status = "Cancelled",  ItemCount = 1 }
        };
    }

    /// <summary>
    /// Ported from TProviderSuppliesBL.GetPriceHistory — returns supply price change history
    /// ordered by ChangeDate descending.
    /// Legacy source: hardcoded seed data representing what SQL_GET_PRICE_HISTORY would return.
    /// ChangePct is computed as ((NewPrice - OldPrice) / OldPrice * 100).
    /// </summary>
    public TSupplyPriceHistory[] GetPriceHistory()
    {
        _logger.LogDebug("GetPriceHistory called");

        return new TSupplyPriceHistory[]
        {
            new TSupplyPriceHistory { ItemName = "Salmon Fillet",  ProviderName = "Ocean Direct",         OldPrice = 20.00m, NewPrice = 22.00m, ChangeDate = new DateTime(2026, 7, 1,  0, 0, 0, DateTimeKind.Utc), ChangePct = 10.0 },
            new TSupplyPriceHistory { ItemName = "Olive Oil",      ProviderName = "Mediterranean Imports", OldPrice = 17.00m, NewPrice = 18.50m, ChangeDate = new DateTime(2026, 6, 15, 0, 0, 0, DateTimeKind.Utc), ChangePct = 8.82 },
            new TSupplyPriceHistory { ItemName = "Chicken Breast", ProviderName = "Valley Meats",         OldPrice = 9.00m,  NewPrice = 9.50m,  ChangeDate = new DateTime(2026, 6, 1,  0, 0, 0, DateTimeKind.Utc), ChangePct = 5.56 },
            new TSupplyPriceHistory { ItemName = "Mozzarella",     ProviderName = "Dairy Delights",       OldPrice = 11.50m, NewPrice = 12.00m, ChangeDate = new DateTime(2026, 5, 20, 0, 0, 0, DateTimeKind.Utc), ChangePct = 4.35 },
            new TSupplyPriceHistory { ItemName = "Flour",          ProviderName = "Golden Grain Supply",  OldPrice = 3.00m,  NewPrice = 2.80m,  ChangeDate = new DateTime(2026, 5, 1,  0, 0, 0, DateTimeKind.Utc), ChangePct = -6.67 },
            new TSupplyPriceHistory { ItemName = "Tomatoes",       ProviderName = "Fresh Farms Co",       OldPrice = 3.50m,  NewPrice = 3.20m,  ChangeDate = new DateTime(2026, 4, 15, 0, 0, 0, DateTimeKind.Utc), ChangePct = -8.57 },
            new TSupplyPriceHistory { ItemName = "Heavy Cream",    ProviderName = "Dairy Delights",       OldPrice = 5.00m,  NewPrice = 5.50m,  ChangeDate = new DateTime(2026, 4, 1,  0, 0, 0, DateTimeKind.Utc), ChangePct = 10.0 },
            new TSupplyPriceHistory { ItemName = "Shrimp",         ProviderName = "Ocean Direct",         OldPrice = 26.00m, NewPrice = 28.00m, ChangeDate = new DateTime(2026, 3, 15, 0, 0, 0, DateTimeKind.Utc), ChangePct = 7.69 }
        };
    }

    /// <summary>
    /// Async wrapper for GetSupplies.
    /// </summary>
    public async Task<List<TSupplyItem>> GetSuppliesAsync(CancellationToken ct = default)
    {
        return await Task.FromResult(new List<TSupplyItem>(GetSupplies())).ConfigureAwait(false);
    }

    /// <summary>
    /// Async wrapper for GetPurchaseOrders.
    /// </summary>
    public async Task<List<TPurchaseOrder>> GetPurchaseOrdersAsync(CancellationToken ct = default)
    {
        return await Task.FromResult(new List<TPurchaseOrder>(GetPurchaseOrders())).ConfigureAwait(false);
    }

    /// <summary>
    /// Async wrapper for GetPriceHistory.
    /// </summary>
    public async Task<List<TSupplyPriceHistory>> GetPriceHistoryAsync(CancellationToken ct = default)
    {
        return await Task.FromResult(new List<TSupplyPriceHistory>(GetPriceHistory())).ConfigureAwait(false);
    }
}
