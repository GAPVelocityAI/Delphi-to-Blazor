using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyFinanceAssets.Core.Application.Services.MyFinanceAssets;

namespace MyFinanceAssets.Core;

/// <summary>
/// Registers every service/DbContext this module owns. Called once from the
/// Host's Program.cs — the Host never needs to know these concrete type names.
/// </summary>
public static class MyFinanceAssetsServiceExtensions
{
    public static IServiceCollection AddMyFinanceAssetsModule(
        this IServiceCollection services, IConfiguration configuration)
    {

        services.AddScoped<IuFinanceAssetsBL, uFinanceAssetsBL>();
        return services;
    }
}