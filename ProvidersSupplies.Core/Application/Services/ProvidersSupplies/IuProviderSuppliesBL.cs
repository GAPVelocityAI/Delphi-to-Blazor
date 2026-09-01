using ProvidersSupplies.Core.Application.DTOs.Core;

namespace ProvidersSupplies.Core.Application.Services.ProvidersSupplies;
public interface IuProviderSuppliesBL
{
    TSupplyItem[] GetSupplies();
    TPurchaseOrder[] GetPurchaseOrders();
    TSupplyPriceHistory[] GetPriceHistory();
    Task<List<TSupplyItem>> GetSuppliesAsync(CancellationToken ct = default);
    Task<List<TPurchaseOrder>> GetPurchaseOrdersAsync(CancellationToken ct = default);
    Task<List<TSupplyPriceHistory>> GetPriceHistoryAsync(CancellationToken ct = default);
}
