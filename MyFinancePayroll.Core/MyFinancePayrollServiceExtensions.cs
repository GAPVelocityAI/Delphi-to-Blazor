using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyFinancePayroll.Core.Application.Services.MyFinancePayroll;

namespace MyFinancePayroll.Core;

/// <summary>
/// Registers every service/DbContext this module owns. Called once from the
/// Host's Program.cs — the Host never needs to know these concrete type names.
/// </summary>
public static class MyFinancePayrollServiceExtensions
{
    public static IServiceCollection AddMyFinancePayrollModule(
        this IServiceCollection services, IConfiguration configuration)
    {

        services.AddScoped<IuFinancePayrollBL, uFinancePayrollBL>();
        return services;
    }
}