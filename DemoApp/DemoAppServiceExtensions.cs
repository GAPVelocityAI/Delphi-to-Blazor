using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DemoApp.Application.Services.Main;

namespace DemoApp;

/// <summary>
/// Registers every service/DbContext this module owns. Called once from the
/// Host's Program.cs — the Host never needs to know these concrete type names.
/// </summary>
public static class DemoAppServiceExtensions
{
    public static IServiceCollection AddDemoAppModule(
        this IServiceCollection services, IConfiguration configuration)
    {

        services.AddScoped<IMainService, MainService>();
        return services;
    }
}