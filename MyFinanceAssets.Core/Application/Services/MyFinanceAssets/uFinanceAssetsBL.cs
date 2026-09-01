using Microsoft.Extensions.Logging;
using MyFinanceAssets.Core.Application.DTOs.Core;

namespace MyFinanceAssets.Core.Application.Services.MyFinanceAssets;

public class uFinanceAssetsBL : IuFinanceAssetsBL
{
    private readonly ILogger<uFinanceAssetsBL> _logger;

    public uFinanceAssetsBL(ILogger<uFinanceAssetsBL> logger)
    {
        _logger = logger;
    }

    public List<TDepreciationRecord> GetDepreciationReport()
    {
        _logger.LogInformation("Generating asset depreciation report (sync).");
        return BuildDepreciationData();
    }

    public async Task<TDepreciationRecord[]> GetDepreciationReportAsync(CancellationToken ct = default)
    {
        _logger.LogInformation("Generating asset depreciation report.");
        await Task.CompletedTask;
        return BuildDepreciationData().ToArray();
    }

    public List<TAssetCategoryRecord> GetAssetValueByCategory()
    {
        _logger.LogInformation("Generating asset value by category report (sync).");
        return BuildCategoryData();
    }

    public async Task<TAssetCategoryRecord[]> GetAssetValueByCategoryAsync(CancellationToken ct = default)
    {
        _logger.LogInformation("Generating asset value by category report.");
        await Task.CompletedTask;
        return BuildCategoryData().ToArray();
    }

    public List<TMaintenanceRecord> GetMaintenanceSchedule()
    {
        _logger.LogInformation("Generating asset maintenance schedule (sync).");
        return BuildMaintenanceData();
    }

    public async Task<TMaintenanceRecord[]> GetMaintenanceScheduleAsync(CancellationToken ct = default)
    {
        _logger.LogInformation("Generating asset maintenance schedule.");
        await Task.CompletedTask;
        return BuildMaintenanceData().ToArray();
    }

    private static List<TDepreciationRecord> BuildDepreciationData()
    {
        return new List<TDepreciationRecord>
        {
            new TDepreciationRecord
            {
                AssetId = 1,
                AssetName = "Commercial Oven",
                OriginalValue = 15000m,
                CurrentValue = 12000m,
                AnnualDepreciation = 1500m,
                YearsRemaining = 6
            },
            new TDepreciationRecord
            {
                AssetId = 2,
                AssetName = "Walk-in Cooler",
                OriginalValue = 8000m,
                CurrentValue = 6500m,
                AnnualDepreciation = 750m,
                YearsRemaining = 8
            },
            new TDepreciationRecord
            {
                AssetId = 3,
                AssetName = "POS System",
                OriginalValue = 3500m,
                CurrentValue = 2800m,
                AnnualDepreciation = 700m,
                YearsRemaining = 3
            },
            new TDepreciationRecord
            {
                AssetId = 4,
                AssetName = "Dining Furniture Set",
                OriginalValue = 12000m,
                CurrentValue = 7200m,
                AnnualDepreciation = 1200m,
                YearsRemaining = 4
            },
            new TDepreciationRecord
            {
                AssetId = 5,
                AssetName = "Delivery Van",
                OriginalValue = 25000m,
                CurrentValue = 21000m,
                AnnualDepreciation = 3333m,
                YearsRemaining = 5
            },
            new TDepreciationRecord
            {
                AssetId = 6,
                AssetName = "Industrial Dishwasher",
                OriginalValue = 5500m,
                CurrentValue = 4200m,
                AnnualDepreciation = 650m,
                YearsRemaining = 6
            },
            new TDepreciationRecord
            {
                AssetId = 7,
                AssetName = "Security Camera System",
                OriginalValue = 2800m,
                CurrentValue = 2100m,
                AnnualDepreciation = 560m,
                YearsRemaining = 2
            },
            new TDepreciationRecord
            {
                AssetId = 8,
                AssetName = "Bar Equipment",
                OriginalValue = 6000m,
                CurrentValue = 4000m,
                AnnualDepreciation = 857m,
                YearsRemaining = 3
            }
        };
    }

    private static List<TAssetCategoryRecord> BuildCategoryData()
    {
        return new List<TAssetCategoryRecord>
        {
            new TAssetCategoryRecord
            {
                Category = "Kitchen Equipment",
                TotalOriginal = 28500m,
                TotalCurrent = 22700m,
                AssetCount = 3
            },
            new TAssetCategoryRecord
            {
                Category = "Furniture",
                TotalOriginal = 12000m,
                TotalCurrent = 7200m,
                AssetCount = 1
            },
            new TAssetCategoryRecord
            {
                Category = "Vehicle",
                TotalOriginal = 25000m,
                TotalCurrent = 21000m,
                AssetCount = 1
            },
            new TAssetCategoryRecord
            {
                Category = "Technology",
                TotalOriginal = 6300m,
                TotalCurrent = 4900m,
                AssetCount = 2
            }
        };
    }

    private static List<TMaintenanceRecord> BuildMaintenanceData()
    {
        return new List<TMaintenanceRecord>
        {
            new TMaintenanceRecord
            {
                AssetId = 1,
                AssetName = "Commercial Oven",
                LastMaintenance = new DateTime(2026, 4, 15, 0, 0, 0, DateTimeKind.Utc),
                NextMaintenance = new DateTime(2026, 10, 15, 0, 0, 0, DateTimeKind.Utc),
                Cost = 450m
            },
            new TMaintenanceRecord
            {
                AssetId = 2,
                AssetName = "Walk-in Cooler",
                LastMaintenance = new DateTime(2026, 3, 20, 0, 0, 0, DateTimeKind.Utc),
                NextMaintenance = new DateTime(2026, 9, 20, 0, 0, 0, DateTimeKind.Utc),
                Cost = 350m
            },
            new TMaintenanceRecord
            {
                AssetId = 5,
                AssetName = "Delivery Van",
                LastMaintenance = new DateTime(2026, 5, 10, 0, 0, 0, DateTimeKind.Utc),
                NextMaintenance = new DateTime(2026, 8, 10, 0, 0, 0, DateTimeKind.Utc),
                Cost = 280m
            },
            new TMaintenanceRecord
            {
                AssetId = 6,
                AssetName = "Industrial Dishwasher",
                LastMaintenance = new DateTime(2026, 2, 28, 0, 0, 0, DateTimeKind.Utc),
                NextMaintenance = new DateTime(2026, 8, 28, 0, 0, 0, DateTimeKind.Utc),
                Cost = 200m
            },
            new TMaintenanceRecord
            {
                AssetId = 8,
                AssetName = "Bar Equipment",
                LastMaintenance = new DateTime(2026, 1, 15, 0, 0, 0, DateTimeKind.Utc),
                NextMaintenance = new DateTime(2026, 7, 15, 0, 0, 0, DateTimeKind.Utc),
                Cost = 500m
            },
            new TMaintenanceRecord
            {
                AssetId = 7,
                AssetName = "Security Camera System",
                LastMaintenance = new DateTime(2026, 6, 1, 0, 0, 0, DateTimeKind.Utc),
                NextMaintenance = new DateTime(2026, 12, 1, 0, 0, 0, DateTimeKind.Utc),
                Cost = 150m
            }
        };
    }
}
