using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyAdmin.Application.Services.Assets;
using MyAdmin.Application.Services.Payroll;
using MyAdmin.Application.Services.Personnel;
using MyAdmin.Application.Services.MyAdmin;
using MyAdmin.Infrastructure.Data;

namespace MyAdmin;

/// <summary>
/// Registers every service/DbContext this module owns. Called once from the
/// Host's Program.cs — the Host never needs to know these concrete type names.
/// </summary>
public static class MyAdminServiceExtensions
{
    public static IServiceCollection AddMyAdminModule(
        this IServiceCollection services, IConfiguration configuration)
    {

        services.AddDbContextFactory<MyAdminDbContext>(options =>
            options.UseInMemoryDatabase("MyAdmin"));
        services.AddTransient(sp => sp.GetRequiredService<IDbContextFactory<MyAdminDbContext>>().CreateDbContext());
        services.AddScoped<IAssetsService, AssetsService>();
        services.AddScoped<IPayrollService, PayrollService>();
        services.AddScoped<IPersonnelService, PersonnelService>();
        services.AddScoped<IuAdminBLService, uAdminBLService>();
        return services;
    }
}