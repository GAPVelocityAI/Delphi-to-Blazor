using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyInventoryFoodCost.Core.Application.Services.MyInventoryFoodCost;

namespace MyInventoryFoodCost.Core;

/// <summary>
/// Registers every service/DbContext this module owns. Called once from the
/// Host's Program.cs — the Host never needs to know these concrete type names.
/// </summary>
public static class MyInventoryFoodCostServiceExtensions
{
    public static IServiceCollection AddMyInventoryFoodCostModule(
        this IServiceCollection services, IConfiguration configuration)
    {

        services.AddScoped<IuInventoryFoodCostBL, uInventoryFoodCostBL>();
        return services;
    }
}