using System.Globalization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using MyAdmin.Application.Services.Payroll;
using MyAdmin.Application.Services.MyAdmin;

namespace MyAdmin.Pages.Payroll;

public partial class Payroll : ComponentBase, IDisposable
{
    [Inject] private IPayrollService PayrollService { get; set; } = default!;
    [Inject] private IuAdminBLService AdminBLService { get; set; } = default!;
    [Inject] private IDialogService DialogService { get; set; } = default!;
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;

    private PayrollStateDto? _state;
    private List<TPayrollInfoDto> _payrollItems = new();
    private TPayrollInfoDto? _selectedItem;
    private bool _showEditPanel;

    private string _totalGrossLabel = "Total Gross: $0";
    private string _totalNetLabel = "Total Net: $0";
    private string _totalDeductionsLabel = "Deductions: $0";

    // FormCreate — initialise state, load grid
    protected override async Task OnInitializedAsync()
    {
        _state = new PayrollStateDto();
        await LoadPayroll();
    }

    // FormDestroy — cleanup (nothing to dispose in Blazor DI-scoped services)
    public void Dispose()
    {
        // Legacy: FAdminBL.Free — service is DI-scoped, no manual disposal needed.
    }

    // btnCloseClick — navigate away (legacy set ModalResult)
    private void BtnCloseClick()
    {
        NavigationManager.NavigateTo("/");
    }

    // btnRefreshClick — reload grid
    private async Task BtnRefreshClick()
    {
        await LoadPayroll();
    }

    // btnAddClick — show edit panel with cleared fields, set FIsAdding = true
    private void BtnAddClick()
    {
        _state!.FIsAdding = true;
        _state.EdtEmployeeText = string.Empty;
        _state.EdtPeriodText = string.Empty;
        _state.EdtGrossPayText = string.Empty;
        _state.EdtDeductionsText = string.Empty;
        _state.EdtPayDateText = string.Empty;
        _showEditPanel = true;
    }

    // btnEditClick — populate state from selected row, delegate to service
    private async Task BtnEditClick()
    {
        if (_selectedItem is null)
            return;

        _state!.FIsAdding = false;
        _state.FSelectedId = _selectedItem.PayrollId;
        _state.EdtEmployeeText = _selectedItem.EmployeeName ?? string.Empty;
        _state.EdtPeriodText = _selectedItem.Period ?? string.Empty;
        _state.EdtGrossPayText = _selectedItem.GrossPay.ToString(CultureInfo.InvariantCulture);
        _state.EdtDeductionsText = _selectedItem.Deductions.ToString(CultureInfo.InvariantCulture);
        _state.EdtPayDateText = _selectedItem.PayDate.ToString("d", CultureInfo.InvariantCulture);

        _state = await PayrollService.BtnEditClickAsync(_state, CancellationToken.None);
        _showEditPanel = true;
    }

    // btnDeleteClick — confirm then delegate to service via AdminBLService, reload grid
    private async Task BtnDeleteClick()
    {
        if (_selectedItem is null)
            return;

        var result = await DialogService.ShowMessageBox(
            "Confirm Delete",
            "Are you sure you want to delete this payroll entry?",
            yesText: "Yes",
            noText: "No");

        if (result == true)
        {
            _state!.FSelectedId = _selectedItem.PayrollId;
            await AdminBLService.DeletePayrollAsync(_selectedItem.PayrollId);
            _selectedItem = null;
            await LoadPayroll();
        }
    }

    // btnSaveClick — delegate to service via AdminBLService for add/update, hide panel, reload grid
    private async Task BtnSaveClick()
    {
        var grossPay = decimal.Parse(_state!.EdtGrossPayText ?? "0", CultureInfo.InvariantCulture);
        var deductions = decimal.Parse(_state.EdtDeductionsText ?? "0", CultureInfo.InvariantCulture);
        var payDate = DateTime.TryParse(_state.EdtPayDateText, CultureInfo.InvariantCulture,
            DateTimeStyles.None, out var parsedDate) ? parsedDate : DateTime.UtcNow;

        var payrollDto = new TPayrollInfoDto
        {
            EmployeeName = _state.EdtEmployeeText ?? string.Empty,
            Period = _state.EdtPeriodText ?? string.Empty,
            GrossPay = grossPay,
            Deductions = deductions,
            NetPay = grossPay - deductions,
            PayDate = payDate
        };

        if (_state.FIsAdding)
        {
            await AdminBLService.AddPayrollAsync(payrollDto);
        }
        else
        {
            payrollDto.PayrollId = _state.FSelectedId;
            await AdminBLService.UpdatePayrollAsync(payrollDto);
        }

        _showEditPanel = false;
        await LoadPayroll();
    }

    // btnCancelClick — hide edit panel
    private void BtnCancelClick()
    {
        _showEditPanel = false;
    }

    // LoadPayroll — call AdminBLService to get payroll data, compute summary totals for footer labels
    private async Task LoadPayroll()
    {
        _payrollItems = await AdminBLService.GetPayrollAsync();

        decimal totalGross = 0m;
        decimal totalNet = 0m;
        decimal totalDeductions = 0m;

        foreach (var item in _payrollItems)
        {
            totalGross += item.GrossPay;
            totalNet += item.NetPay;
            totalDeductions += item.Deductions;
        }

        _totalGrossLabel = "Total Gross: " + totalGross.ToString("C2", CultureInfo.InvariantCulture);
        _totalNetLabel = "Total Net: " + totalNet.ToString("C2", CultureInfo.InvariantCulture);
        _totalDeductionsLabel = "Deductions: " + totalDeductions.ToString("C2", CultureInfo.InvariantCulture);
    }

    private void OnSelectedItemChanged(TPayrollInfoDto? item)
    {
        _selectedItem = item;
    }
}
