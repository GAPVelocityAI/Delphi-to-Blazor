using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProvidersSupplies.Core.Application.Services.ProvidersSupplies;

namespace ProvidersSupplies.Core;

/// <summary>
/// Registers every service/DbContext this module owns. Called once from the
/// Host's Program.cs — the Host never needs to know these concrete type names.
/// </summary>
public static class ProvidersSuppliesServiceExtensions
{
    public static IServiceCollection AddProvidersSuppliesModule(
        this IServiceCollection services, IConfiguration configuration)
    {

        services.AddScoped<IuProviderSuppliesBL, uProviderSuppliesBL>();
        return services;
    }
}