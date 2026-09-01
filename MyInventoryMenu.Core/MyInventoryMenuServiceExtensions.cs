using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyInventoryMenu.Core.Application.Services.MyInventoryMenu;

namespace MyInventoryMenu.Core;

/// <summary>
/// Registers every service/DbContext this module owns. Called once from the
/// Host's Program.cs — the Host never needs to know these concrete type names.
/// </summary>
public static class MyInventoryMenuServiceExtensions
{
    public static IServiceCollection AddMyInventoryMenuModule(
        this IServiceCollection services, IConfiguration configuration)
    {

        services.AddScoped<IuInventoryMenuBL, uInventoryMenuBL>();
        return services;
    }
}