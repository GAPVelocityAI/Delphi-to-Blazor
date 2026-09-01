using Serilog;
using MyAdmin.Application.Services.MyAdmin;
using MyRestaurant.Application.Services.MyRestaurant;
using MudBlazor.Services;
using CommonLib.Middleware;
using MyFinanceAssets.Core;
using MyFinancePayroll.Core;
using MyFinancePersonnel.Core;
using MyInventoryFoodCost.Core;
using MyInventoryMenu.Core;
using MyInventoryStock.Core;
using ProvidersCore.Core;
using ProvidersSupplies.Core;
using MyAdmin;
using MyRestaurant;
using DemoApp;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Host.UseSerilog();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();
builder.Services.AddHttpClient();
builder.Services.AddAntiforgery();

// Each module registers its own services/DbContext via its generated
// Add{ModuleName}Module extension method — the Host never needs to know
// the concrete service/DbContext type names.
builder.Services.AddMyFinanceAssetsModule(builder.Configuration);
builder.Services.AddMyFinancePayrollModule(builder.Configuration);
builder.Services.AddMyFinancePersonnelModule(builder.Configuration);
builder.Services.AddMyInventoryFoodCostModule(builder.Configuration);
builder.Services.AddMyInventoryMenuModule(builder.Configuration);
builder.Services.AddMyInventoryStockModule(builder.Configuration);
builder.Services.AddProvidersCoreModule(builder.Configuration);
builder.Services.AddProvidersSuppliesModule(builder.Configuration);
builder.Services.AddMyAdminModule(builder.Configuration);
builder.Services.AddMyRestaurantModule(builder.Configuration);
builder.Services.AddDemoAppModule(builder.Configuration);

var app = builder.Build();

// Each BL service seeds its own store on first use. Without this the seed ran lazily, on
// whichever page happened to be visited first — so a grid whose service reads the DB directly
// (MenuView, Orders) rendered empty until some other page had triggered the seed.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await services.GetRequiredService<IuTablesBLService>().EnsureInitializedAsync();
    await services.GetRequiredService<IuRestaurantBLService>().EnsureInitializedAsync();
    await services.GetRequiredService<IuAdminBLService>().EnsureInitializedAsync();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseSerilogRequestLogging();
app.UseMiddleware<SecurityHeadersMiddleware>();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();