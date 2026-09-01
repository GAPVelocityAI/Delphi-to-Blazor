using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyFinancePersonnel.Core.Application.Services.MyFinancePersonnel;

namespace MyFinancePersonnel.Core;

/// <summary>
/// Registers every service/DbContext this module owns. Called once from the
/// Host's Program.cs — the Host never needs to know these concrete type names.
/// </summary>
public static class MyFinancePersonnelServiceExtensions
{
    public static IServiceCollection AddMyFinancePersonnelModule(
        this IServiceCollection services, IConfiguration configuration)
    {

        services.AddScoped<IuFinancePersonnelBL, uFinancePersonnelBL>();
        return services;
    }
}