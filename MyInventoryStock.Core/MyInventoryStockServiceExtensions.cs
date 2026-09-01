using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyInventoryStock.Core.Application.Services.MyInventoryStock;

namespace MyInventoryStock.Core;

/// <summary>
/// Registers every service/DbContext this module owns. Called once from the
/// Host's Program.cs — the Host never needs to know these concrete type names.
/// </summary>
public static class MyInventoryStockServiceExtensions
{
    public static IServiceCollection AddMyInventoryStockModule(
        this IServiceCollection services, IConfiguration configuration)
    {

        services.AddScoped<IuInventoryStockBL, uInventoryStockBL>();
        return services;
    }
}