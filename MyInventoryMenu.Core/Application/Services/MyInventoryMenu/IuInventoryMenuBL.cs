using MyInventoryMenu.Core.Application.DTOs.Core;

namespace MyInventoryMenu.Core.Application.Services.MyInventoryMenu;

public interface IuInventoryMenuBL
{
    Task<TMenuCostItem[]> GetMenuCostsAsync(CancellationToken ct = default);
    Task<TMenuCategorySummary[]> GetMenuCategorySummaryAsync(CancellationToken ct = default);
    Task<TTableAvailability[]> GetTableAvailabilityAsync(CancellationToken ct = default);
    List<TMenuCostItem> GetMenuCosts();
    List<TMenuCategorySummary> GetMenuCategorySummary();
    List<TTableAvailability> GetTableAvailability();
}
