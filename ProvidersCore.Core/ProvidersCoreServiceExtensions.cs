using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProvidersCore.Core.Application.Services.ProvidersCore;

namespace ProvidersCore.Core;

/// <summary>
/// Registers every service/DbContext this module owns. Called once from the
/// Host's Program.cs — the Host never needs to know these concrete type names.
/// </summary>
public static class ProvidersCoreServiceExtensions
{
    public static IServiceCollection AddProvidersCoreModule(
        this IServiceCollection services, IConfiguration configuration)
    {

        services.AddScoped<IuProvidersBL, uProvidersBL>();
        return services;
    }
}