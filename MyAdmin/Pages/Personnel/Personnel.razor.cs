using Microsoft.AspNetCore.Components;
using MudBlazor;
using MyAdmin.Application.Services.Personnel;
using System.Globalization;
using MyAdmin.Application.Services.MyAdmin;

namespace MyAdmin.Pages.Personnel;

public partial class Personnel : ComponentBase, IDisposable
{
    [Inject]
    private IPersonnelService PersonnelService { get; set; } = default!;

    [Inject]
    private IuAdminBLService AdminBLService { get; set; } = default!;

    [Inject]
    private NavigationManager Navigation { get; set; } = default!;

    [Inject]
    private IDialogService DialogService { get; set; } = default!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    private PersonnelStateDto? _state;
    private List<TEmployeeInfoDto> _employees = new();
    private TEmployeeInfoDto? _selectedEmployee;
    private bool _showEditPanel;
    private bool _showActiveOnly;
    private string _employeeCountLabel = "Employees: 0";
    private string _activeSelection = "Yes";

    private CancellationTokenSource _cts = new();

    // Corresponds to legacy FormCreate
    protected override async Task OnInitializedAsync()
    {
        _state = new PersonnelStateDto();
        // Grid configuration is handled declaratively in markup (columns defined in .razor)
        // Legacy: TGridHelper.ConfigureGrid(grdPersonnel, [...], [...])
        await LoadEmployees(false);
    }

    // Corresponds to legacy btnCloseClick
    private void BtnCloseClick()
    {
        Navigation.NavigateTo("/");
    }

    // Corresponds to legacy btnRefreshClick
    private async Task BtnRefreshClick()
    {
        await LoadEmployees(false);
    }

    // Corresponds to legacy btnShowActiveClick
    private async Task BtnShowActiveClick()
    {
        _showActiveOnly = true;
        await LoadEmployees(true);
    }

    // Corresponds to legacy btnShowAllClick
    private async Task BtnShowAllClick()
    {
        _showActiveOnly = false;
        await LoadEmployees(false);
    }

    // Corresponds to legacy btnAddClick — delegates to service
    private async Task OnAddClick()
    {
        var sender = new TObject();
        await PersonnelService.BtnAddClickAsync(_state!, sender, _cts.Token);

        // Legacy: clears fields, sets cmbActive.ItemIndex := 0, shows edit panel
        _activeSelection = "Yes";
        _showEditPanel = true;
        _selectedEmployee = null;
    }

    // Corresponds to legacy btnEditClick — delegates to service
    private async Task OnEditClick()
    {
        if (_selectedEmployee == null)
            return;

        // Populate state with selected row data before calling service
        _state!.FSelectedId = _selectedEmployee.EmployeeId;
        _state.EdtFirstNameText = _selectedEmployee.FirstName ?? string.Empty;
        _state.EdtLastNameText = _selectedEmployee.LastName ?? string.Empty;
        _state.CmbPositionText = _selectedEmployee.Position ?? string.Empty;
        _state.EdtHireDateText = _selectedEmployee.HireDate.ToString("d", CultureInfo.InvariantCulture);

        // Legacy strips $ and , from salary display
        _state.EdtSalaryText = _selectedEmployee.Salary.ToString("F2", CultureInfo.InvariantCulture);

        _activeSelection = _selectedEmployee.Active ? "Yes" : "No";

        var sender = new TObject();
        _state = await PersonnelService.BtnEditClickAsync(_state!, sender, _cts.Token);

        _state.FIsAdding = false;
        _showEditPanel = true;
    }

    // Corresponds to legacy btnDeleteClick — delegates to service
    private async Task OnDeleteClick()
    {
        if (_selectedEmployee == null)
            return;

        var confirmed = await DialogService.ShowMessageBox(
            "Confirm Delete",
            "Are you sure you want to delete this employee?",
            yesText: "Yes",
            cancelText: "No");

        if (confirmed == true)
        {
            _state!.FSelectedId = _selectedEmployee.EmployeeId;
            var sender = new TObject();
            await PersonnelService.BtnDeleteClickAsync(_state!, sender, _cts.Token);
            _selectedEmployee = null;
            await LoadEmployees(_showActiveOnly);
        }
    }

    // Corresponds to legacy btnSaveClick — delegates to service
    private async Task OnSaveClick()
    {
        // Sync active selection back to state before save
        // Legacy: Employee.Active := (cmbActive.ItemIndex = 0)
        _state!.FAdminBL = _activeSelection == "Yes" ? "True" : "False";

        var sender = new TObject();
        await PersonnelService.BtnSaveClickAsync(_state!, sender, _cts.Token);

        _showEditPanel = false;
        await LoadEmployees(_showActiveOnly);
    }

    // Corresponds to legacy btnCancelClick
    private void BtnCancelClick()
    {
        // Legacy: pnlEdit.Visible := False
        _showEditPanel = false;
    }

    // Corresponds to legacy LoadEmployees(AActiveOnly: Boolean)
    private async Task LoadEmployees(bool activeOnly)
    {
        _employees = await PersonnelService.LoadEmployeesAsync(_state!, activeOnly, _cts.Token);
        _employeeCountLabel = $"Employees: {_employees.Count.ToString(CultureInfo.InvariantCulture)}";
    }

    private void OnSelectedItemChanged(TEmployeeInfoDto? item)
    {
        _selectedEmployee = item;
    }

    // Corresponds to legacy FormDestroy
    public void Dispose()
    {
        _cts.Cancel();
        _cts.Dispose();
    }
}
