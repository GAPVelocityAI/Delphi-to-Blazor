using MyInventoryStock.Core.Application.DTOs.Core;

namespace MyInventoryStock.Core.Application.Services.MyInventoryStock;

public interface IuInventoryStockBL
{
    List<TStockItem> GetStockItems();
    List<TStockMovement> GetStockMovements();
    List<TStockValuation> GetStockValuation();
    Task<TStockItem[]> GetStockItemsAsync(CancellationToken ct = default);
    Task<TStockMovement[]> GetStockMovementsAsync(CancellationToken ct = default);
    Task<TStockValuation[]> GetStockValuationAsync(CancellationToken ct = default);
}
