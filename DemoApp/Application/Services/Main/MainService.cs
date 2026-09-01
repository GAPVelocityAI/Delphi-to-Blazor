using System.Globalization;
using Microsoft.Extensions.Logging;
using MyRestaurant.Pages.Tables;
using MyRestaurant.Pages.Orders;
using MyRestaurant.Pages.Bill;
using MyRestaurant.Pages.Food;
using MyAdmin.Pages.Assets;
using MyAdmin.Pages.Personnel;
using MyAdmin.Pages.Payroll;

namespace DemoApp.Application.Services.Main;

public class MainService : IMainService
{
    private readonly ILogger<MainService> _logger;

    // Navigation route map — mirrors the legacy button-to-form mapping:
    // btnTablesClick -> TfrmTables, btnOrdersClick -> TfrmOrders, etc.
    private static readonly Dictionary<string, NavigationTarget> _navigationMap = new(StringComparer.OrdinalIgnoreCase)
    {
        ["Tables"] = new NavigationTarget { Label = "Tables", Route = "/tables", Group = "Restaurant" },
        ["Orders"] = new NavigationTarget { Label = "Orders", Route = "/orders", Group = "Restaurant" },
        ["Menu"] = new NavigationTarget { Label = "Menu", Route = "/menu", Group = "Restaurant" },
        ["BillCheck"] = new NavigationTarget { Label = "Bill / Check", Route = "/billcheck", Group = "Restaurant" },
        ["FoodCost"] = new NavigationTarget { Label = "Food Cost", Route = "/foodcost", Group = "Restaurant" },
        ["Assets"] = new NavigationTarget { Label = "Assets", Route = "/assets", Group = "Administration" },
        ["Personnel"] = new NavigationTarget { Label = "Personnel", Route = "/personnel", Group = "Administration" },
        ["Payroll"] = new NavigationTarget { Label = "Payroll", Route = "/payroll", Group = "Administration" },
    };

    public MainService(ILogger<MainService> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Legacy: FormCreate
    /// StatusBar1.Panels[0].Text := 'Ready';
    /// StatusBar1.Panels[1].Text := FormatDateTime('dddd, mmmm d, yyyy', Now);
    /// </summary>
    public Task<MainStatusDto> InitializeAsync(CancellationToken ct = default)
    {
        _logger.LogInformation("Main layout initialized");

        // Mirrors legacy FormCreate exactly:
        // Panel[0] = "Ready", Panel[1] = formatted current date
        var now = DateTime.UtcNow;
        var dto = new MainStatusDto
        {
            StatusText = "Ready",
            // Legacy FormatDateTime('dddd, mmmm d, yyyy', Now) produces e.g. "Monday, January 6, 2025"
            CurrentDateText = now.ToString("dddd, MMMM d, yyyy", CultureInfo.InvariantCulture)
        };

        return Task.FromResult(dto);
    }

    /// <summary>
    /// Returns all navigation targets in the order they appear on the legacy sidebar.
    /// Restaurant group: Tables, Orders, Menu, Bill/Check, Food Cost
    /// Administration group: Assets, Personnel, Payroll
    /// 
    /// Legacy: each btnXxxClick created a modal form and showed it.
    /// In Blazor these become navigation links to routed pages.
    /// </summary>
    public Task<List<NavigationTarget>> GetNavigationTargetsAsync(CancellationToken ct = default)
    {
        // Preserve exact legacy sidebar order:
        // Restaurant section (pnlRestaurant): btnTables, btnOrders, btnMenu, btnBillCheck, btnFoodCost
        // Admin section (pnlAdmin): btnAssets, btnPersonnel, btnPayroll
        var targets = new List<NavigationTarget>
        {
            // Restaurant group — matches legacy pnlRestaurant button order
            new NavigationTarget { Label = "Tables", Route = "/tables", Group = "Restaurant" },
            new NavigationTarget { Label = "Orders", Route = "/orders", Group = "Restaurant" },
            new NavigationTarget { Label = "Menu", Route = "/menu", Group = "Restaurant" },
            new NavigationTarget { Label = "Bill / Check", Route = "/billcheck", Group = "Restaurant" },
            new NavigationTarget { Label = "Food Cost", Route = "/foodcost", Group = "Restaurant" },
            // Administration group — matches legacy pnlAdmin button order
            new NavigationTarget { Label = "Assets", Route = "/assets", Group = "Administration" },
            new NavigationTarget { Label = "Personnel", Route = "/personnel", Group = "Administration" },
            new NavigationTarget { Label = "Payroll", Route = "/payroll", Group = "Administration" },
        };

        return Task.FromResult(targets);
    }

    /// <summary>
    /// Resolves a module name to its Blazor route.
    /// 
    /// Legacy equivalences:
    ///   btnTablesClick    -> TfrmTables.ShowModal    -> /tables
    ///   btnOrdersClick    -> TfrmOrders.ShowModal    -> /orders
    ///   btnMenuClick      -> TfrmMenuView.ShowModal  -> /menu
    ///   btnBillCheckClick -> TfrmBillCheck.ShowModal  -> /billcheck
    ///   btnFoodCostClick  -> TfrmFoodCost.ShowModal   -> /foodcost
    ///   btnAssetsClick    -> TfrmAssets.ShowModal     -> /assets
    ///   btnPersonnelClick -> TfrmPersonnel.ShowModal  -> /personnel
    ///   btnPayrollClick   -> TfrmPayroll.ShowModal    -> /payroll
    /// </summary>
    public Task<string> GetRouteForModuleAsync(string moduleName, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(moduleName))
        {
            _logger.LogWarning("GetRouteForModuleAsync called with empty module name");
            return Task.FromResult("/");
        }

        if (_navigationMap.TryGetValue(moduleName, out var target))
        {
            _logger.LogInformation("Navigating to module {ModuleName} at route {Route}", moduleName, target.Route);
            return Task.FromResult(target.Route);
        }

        _logger.LogWarning("Unknown module name requested: {ModuleName}", moduleName);
        // Fall back to root — legacy would simply not open anything for an unknown button
        return Task.FromResult("/");
    }
}
