using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyRestaurant.Application.Services.Bill;
using MyRestaurant.Application.Services.Food;
using MyRestaurant.Application.Services.Menu;
using MyRestaurant.Application.Services.Orders;
using MyRestaurant.Application.Services.Tables;
using MyRestaurant.Application.Services.MyRestaurant;
using MyRestaurant.Infrastructure.Data;

namespace MyRestaurant;

/// <summary>
/// Registers every service/DbContext this module owns. Called once from the
/// Host's Program.cs — the Host never needs to know these concrete type names.
/// </summary>
public static class MyRestaurantServiceExtensions
{
    public static IServiceCollection AddMyRestaurantModule(
        this IServiceCollection services, IConfiguration configuration)
    {

        services.AddDbContextFactory<MyRestaurantDbContext>(options =>
            options.UseInMemoryDatabase("MyRestaurant"));
        services.AddTransient(sp => sp.GetRequiredService<IDbContextFactory<MyRestaurantDbContext>>().CreateDbContext());
        services.AddScoped<IBillCheckService, BillCheckService>();
        services.AddScoped<IFoodCostService, FoodCostService>();
        services.AddScoped<IMenuViewService, MenuViewService>();
        services.AddScoped<IOrdersService, OrdersService>();
        services.AddScoped<ITablesService, TablesService>();
        services.AddScoped<IuRestaurantBLService, uRestaurantBLService>();
        services.AddScoped<IuTablesBLService, uTablesBLService>();
        return services;
    }
}