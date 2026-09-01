using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using DemoApp.Application.Services.Main;
using MyRestaurant.Pages.Tables;
using MyRestaurant.Pages.Orders;
using MyAdmin.Pages.Assets;
using MyAdmin.Pages.Personnel;
using MyAdmin.Pages.Payroll;

namespace DemoApp.Pages.Main;

public partial class Main : ComponentBase
{
    [Inject]
    private IMainService MainService { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    private string StatusText { get; set; } = string.Empty;
    private string DateText { get; set; } = string.Empty;

    // Legacy: FormCreate
    protected override void OnInitialized()
    {
        StatusText = "Ready";
        DateText = DateTime.UtcNow.ToString("dddd, MMMM d, yyyy", CultureInfo.InvariantCulture);
    }

    // Legacy: btnTablesClick — navigates to the Tables page (was ShowModal in Delphi)
    private void BtnTablesClick()
    {
        NavigationManager.NavigateTo("/my-restaurant/tables");
    }

    // Legacy: btnOrdersClick — navigates to the Orders page (was ShowModal in Delphi)
    private void BtnOrdersClick()
    {
        NavigationManager.NavigateTo("/my-restaurant/orders");
    }

    // Legacy: btnMenuClick — navigates to the Menu page (was ShowModal in Delphi)
    private void BtnMenuClick()
    {
        NavigationManager.NavigateTo("/my-restaurant/menu-view");
    }

    // Legacy: btnBillCheckClick — navigates to the Bill/Check page (was ShowModal in Delphi)
    private void BtnBillCheckClick()
    {
        NavigationManager.NavigateTo("/bills");
    }

    // Legacy: btnFoodCostClick — navigates to the Food Cost page (was ShowModal in Delphi)
    private void BtnFoodCostClick()
    {
        NavigationManager.NavigateTo("/my-restaurant/food-cost");
    }

    // Legacy: btnAssetsClick — navigates to the Assets page (was ShowModal in Delphi)
    private void BtnAssetsClick()
    {
        NavigationManager.NavigateTo("/my-admin/assets");
    }

    // Legacy: btnPersonnelClick — navigates to the Personnel page (was ShowModal in Delphi)
    private void BtnPersonnelClick()
    {
        NavigationManager.NavigateTo("/my-admin/personnel");
    }

    // Legacy: btnPayrollClick — navigates to the Payroll page (was ShowModal in Delphi)
    private void BtnPayrollClick()
    {
        NavigationManager.NavigateTo("/my-admin/payroll");
    }
}
