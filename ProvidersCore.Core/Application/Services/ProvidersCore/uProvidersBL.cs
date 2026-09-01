using Microsoft.Extensions.Logging;
using ProvidersCore.Core.Application.DTOs.Core;

namespace ProvidersCore.Core.Application.Services.ProvidersCore;

public class uProvidersBL : IuProvidersBL
{
    private readonly ILogger<uProvidersBL> _logger;

    public uProvidersBL(ILogger<uProvidersBL> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Returns all providers — synchronous version ported from TProvidersBL.GetProviders.
    /// </summary>
    public TProviderRecord[] GetProviders()
    {
        _logger.LogDebug("GetProviders called");
        return BuildProviders();
    }

    /// <summary>
    /// Returns all providers — async version ported from TProvidersBL.GetProviders.
    /// Legacy code builds an in-memory array of seed records; reproduced exactly.
    /// </summary>
    public async Task<List<TProviderRecord>> GetProvidersAsync(CancellationToken ct = default)
    {
        _logger.LogDebug("GetProvidersAsync called");
        await Task.CompletedTask;
        return new List<TProviderRecord>(BuildProviders());
    }

    /// <summary>
    /// Returns provider performance data — synchronous version ported from TProvidersBL.GetProviderPerformance.
    /// </summary>
    public TProviderPerformance[] GetProviderPerformance()
    {
        _logger.LogDebug("GetProviderPerformance called");
        return BuildProviderPerformance();
    }

    /// <summary>
    /// Returns provider performance data — async version ported from TProvidersBL.GetProviderPerformance.
    /// Legacy code builds an in-memory array ordered by QualityScore DESC; reproduced exactly.
    /// </summary>
    public async Task<List<TProviderPerformance>> GetProviderPerformanceAsync(CancellationToken ct = default)
    {
        _logger.LogDebug("GetProviderPerformanceAsync called");
        await Task.CompletedTask;
        return new List<TProviderPerformance>(BuildProviderPerformance());
    }

    /// <summary>
    /// Returns provider category summary — synchronous version ported from TProvidersBL.GetProviderCategories.
    /// </summary>
    public TProviderCategory[] GetProviderCategories()
    {
        _logger.LogDebug("GetProviderCategories called");
        return BuildProviderCategories();
    }

    /// <summary>
    /// Returns provider category summary — async version ported from TProvidersBL.GetProviderCategories.
    /// Legacy code builds an in-memory array of category aggregates ordered by Category ASC.
    /// </summary>
    public async Task<List<TProviderCategory>> GetProviderCategoriesAsync(CancellationToken ct = default)
    {
        _logger.LogDebug("GetProviderCategoriesAsync called");
        await Task.CompletedTask;
        return new List<TProviderCategory>(BuildProviderCategories());
    }

    private static TProviderRecord[] BuildProviders()
    {
        var result = new TProviderRecord[]
        {
            new TProviderRecord
            {
                ProviderId = 1,
                CompanyName = "Fresh Farms Co",
                ContactName = "Maria Lopez",
                Phone = "(555) 100-2001",
                Email = "maria@freshfarms.com",
                Category = "Produce",
                Rating = 5,
                Active = true,
                ContractExpiry = new DateTime(2027, 3, 15, 0, 0, 0, DateTimeKind.Utc)
            },
            new TProviderRecord
            {
                ProviderId = 2,
                CompanyName = "Ocean Direct",
                ContactName = "James Chen",
                Phone = "(555) 100-2002",
                Email = "james@oceandirect.com",
                Category = "Seafood",
                Rating = 4,
                Active = true,
                ContractExpiry = new DateTime(2027, 1, 20, 0, 0, 0, DateTimeKind.Utc)
            },
            new TProviderRecord
            {
                ProviderId = 3,
                CompanyName = "Valley Meats",
                ContactName = "Robert Miller",
                Phone = "(555) 100-2003",
                Email = "robert@valleymeats.com",
                Category = "Meat",
                Rating = 5,
                Active = true,
                ContractExpiry = new DateTime(2026, 12, 31, 0, 0, 0, DateTimeKind.Utc)
            },
            new TProviderRecord
            {
                ProviderId = 4,
                CompanyName = "Dairy Delights",
                ContactName = "Susan White",
                Phone = "(555) 100-2004",
                Email = "susan@dairydelights.com",
                Category = "Dairy",
                Rating = 3,
                Active = true,
                ContractExpiry = new DateTime(2026, 11, 30, 0, 0, 0, DateTimeKind.Utc)
            },
            new TProviderRecord
            {
                ProviderId = 5,
                CompanyName = "Golden Grain Supply",
                ContactName = "David Brown",
                Phone = "(555) 100-2005",
                Email = "david@goldengrain.com",
                Category = "Dry Goods",
                Rating = 4,
                Active = true,
                ContractExpiry = new DateTime(2027, 6, 15, 0, 0, 0, DateTimeKind.Utc)
            },
            new TProviderRecord
            {
                ProviderId = 6,
                CompanyName = "Mediterranean Imports",
                ContactName = "Sofia Romano",
                Phone = "(555) 100-2006",
                Email = "sofia@medimports.com",
                Category = "Oils",
                Rating = 5,
                Active = true,
                ContractExpiry = new DateTime(2027, 2, 28, 0, 0, 0, DateTimeKind.Utc)
            },
            new TProviderRecord
            {
                ProviderId = 7,
                CompanyName = "Herb Garden Direct",
                ContactName = "Emily Green",
                Phone = "(555) 100-2007",
                Email = "emily@herbgarden.com",
                Category = "Herbs",
                Rating = 4,
                Active = true,
                ContractExpiry = new DateTime(2026, 10, 15, 0, 0, 0, DateTimeKind.Utc)
            },
            new TProviderRecord
            {
                ProviderId = 8,
                CompanyName = "Pacific Seafood Inc",
                ContactName = "Tom Nakamura",
                Phone = "(555) 100-2008",
                Email = "tom@pacificseafood.com",
                Category = "Seafood",
                Rating = 3,
                Active = false,
                ContractExpiry = new DateTime(2026, 8, 31, 0, 0, 0, DateTimeKind.Utc)
            }
        };

        // Legacy ORDER BY p.Category, p.CompanyName
        Array.Sort(result, (a, b) =>
        {
            int cmp = string.Compare(a.Category, b.Category, StringComparison.OrdinalIgnoreCase);
            if (cmp != 0) return cmp;
            return string.Compare(a.CompanyName, b.CompanyName, StringComparison.OrdinalIgnoreCase);
        });

        return result;
    }

    private static TProviderPerformance[] BuildProviderPerformance()
    {
        var result = new TProviderPerformance[]
        {
            new TProviderPerformance
            {
                ProviderId = 1,
                CompanyName = "Fresh Farms Co",
                OnTimeDeliveryPct = 98.5,
                QualityScore = 95,
                TotalOrders = 124,
                AvgLeadTimeDays = 1.5
            },
            new TProviderPerformance
            {
                ProviderId = 3,
                CompanyName = "Valley Meats",
                OnTimeDeliveryPct = 97.0,
                QualityScore = 94,
                TotalOrders = 98,
                AvgLeadTimeDays = 2.0
            },
            new TProviderPerformance
            {
                ProviderId = 6,
                CompanyName = "Mediterranean Imports",
                OnTimeDeliveryPct = 96.2,
                QualityScore = 92,
                TotalOrders = 56,
                AvgLeadTimeDays = 5.0
            },
            new TProviderPerformance
            {
                ProviderId = 2,
                CompanyName = "Ocean Direct",
                OnTimeDeliveryPct = 94.8,
                QualityScore = 90,
                TotalOrders = 87,
                AvgLeadTimeDays = 2.5
            },
            new TProviderPerformance
            {
                ProviderId = 5,
                CompanyName = "Golden Grain Supply",
                OnTimeDeliveryPct = 93.5,
                QualityScore = 89,
                TotalOrders = 72,
                AvgLeadTimeDays = 3.0
            },
            new TProviderPerformance
            {
                ProviderId = 7,
                CompanyName = "Herb Garden Direct",
                OnTimeDeliveryPct = 91.0,
                QualityScore = 88,
                TotalOrders = 45,
                AvgLeadTimeDays = 2.0
            },
            new TProviderPerformance
            {
                ProviderId = 4,
                CompanyName = "Dairy Delights",
                OnTimeDeliveryPct = 85.2,
                QualityScore = 78,
                TotalOrders = 110,
                AvgLeadTimeDays = 1.5
            },
            new TProviderPerformance
            {
                ProviderId = 8,
                CompanyName = "Pacific Seafood Inc",
                OnTimeDeliveryPct = 80.0,
                QualityScore = 72,
                TotalOrders = 34,
                AvgLeadTimeDays = 4.0
            }
        };

        // Legacy ORDER BY pp.QualityScore DESC — data is already inserted in that order,
        // but sort explicitly to match the SQL semantics.
        Array.Sort(result, (a, b) => b.QualityScore.CompareTo(a.QualityScore));

        return result;
    }

    private static TProviderCategory[] BuildProviderCategories()
    {
        var result = new TProviderCategory[]
        {
            new TProviderCategory
            {
                Category = "Dairy",
                ProviderCount = 1,
                ActiveCount = 1,
                AvgRating = 3.0
            },
            new TProviderCategory
            {
                Category = "Dry Goods",
                ProviderCount = 1,
                ActiveCount = 1,
                AvgRating = 4.0
            },
            new TProviderCategory
            {
                Category = "Herbs",
                ProviderCount = 1,
                ActiveCount = 1,
                AvgRating = 4.0
            },
            new TProviderCategory
            {
                Category = "Meat",
                ProviderCount = 1,
                ActiveCount = 1,
                AvgRating = 5.0
            },
            new TProviderCategory
            {
                Category = "Oils",
                ProviderCount = 1,
                ActiveCount = 1,
                AvgRating = 5.0
            },
            new TProviderCategory
            {
                Category = "Produce",
                ProviderCount = 1,
                ActiveCount = 1,
                AvgRating = 5.0
            },
            new TProviderCategory
            {
                Category = "Seafood",
                ProviderCount = 2,
                ActiveCount = 1,
                AvgRating = 3.5
            }
        };

        // Legacy ORDER BY p.Category — data is already in alphabetical order,
        // but sort explicitly to match the SQL semantics.
        Array.Sort(result, (a, b) => string.Compare(a.Category, b.Category, StringComparison.OrdinalIgnoreCase));

        return result;
    }
}
