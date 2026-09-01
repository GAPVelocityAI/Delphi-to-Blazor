using MyFinanceAssets.Core.Application.DTOs.Core;

namespace MyFinanceAssets.Core.Application.Services.MyFinanceAssets;
public interface IuFinanceAssetsBL
{
    Task<TDepreciationRecord[]> GetDepreciationReportAsync(CancellationToken ct = default);
    Task<TAssetCategoryRecord[]> GetAssetValueByCategoryAsync(CancellationToken ct = default);
    Task<TMaintenanceRecord[]> GetMaintenanceScheduleAsync(CancellationToken ct = default);
    List<TDepreciationRecord> GetDepreciationReport();
    List<TAssetCategoryRecord> GetAssetValueByCategory();
    List<TMaintenanceRecord> GetMaintenanceSchedule();
}
