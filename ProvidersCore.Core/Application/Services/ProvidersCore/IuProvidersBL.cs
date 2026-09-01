using ProvidersCore.Core.Application.DTOs.Core;

namespace ProvidersCore.Core.Application.Services.ProvidersCore;

public interface IuProvidersBL
{
    Task<List<TProviderRecord>> GetProvidersAsync(CancellationToken ct = default);
    Task<List<TProviderPerformance>> GetProviderPerformanceAsync(CancellationToken ct = default);
    Task<List<TProviderCategory>> GetProviderCategoriesAsync(CancellationToken ct = default);
    TProviderRecord[] GetProviders();
    TProviderPerformance[] GetProviderPerformance();
    TProviderCategory[] GetProviderCategories();
}
