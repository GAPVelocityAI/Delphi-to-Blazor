using System.Globalization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using MyRestaurant.Application.Services.MyRestaurant;
using MyRestaurant.Application.Services.Tables;

namespace MyRestaurant.Pages.Tables;

public partial class Tables : ComponentBase, IDisposable
{
    [Inject] private ITablesService TablesService { get; set; } = default!;
    [Inject] private IuTablesBLService TablesBLService { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] private ISnackbar Snackbar { get; set; } = default!;
    [Inject] private IDialogService DialogService { get; set; } = default!;

    private TablesStateDto? _state;
    private List<TTableInfoDto> _displayedTables = new();
    private TTableInfoDto? _selectedItem;
    private bool _editPanelVisible;
    private int _selectedStatusIndex;
    private int _selectedZoneIndex;

    private static readonly string[] ZoneNames = { "Main Hall", "Terrace", "Private", "Bar Area" };
    private static readonly string[] StatusNames = { "Available", "Occupied", "Reserved", "Closed" };

    /// <summary>
    /// Legacy: FormCreate — initializes BL, configures grid, loads all tables.
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        _state = new TablesStateDto();
        await LoadAllTablesAsync();
    }

    /// <summary>
    /// Legacy: LoadTables(const ATables: TArray&lt;TTableInfo&gt;)
    /// Delegates to service which maps TTableInfo[] to List&lt;TTableInfoDto&gt;.
    /// </summary>
    private async Task LoadTablesAsync(List<TTableInfo> tables)
    {
        _displayedTables = TablesService.LoadTables(tables);
        _selectedItem = null;
        await InvokeAsync(StateHasChanged);
    }

    private async Task LoadAllTablesAsync()
    {
        var allTables = await TablesBLService.GetTablesAsync();
        await LoadTablesAsync(allTables);
    }

    private void OnRowClick(DataGridRowClickEventArgs<TTableInfoDto> args)
    {
        _selectedItem = args.Item;
    }

    private void OnSelectedItemChanged(TTableInfoDto? item)
    {
        _selectedItem = item;
    }

    /// <summary>
    /// Legacy: btnCloseClick — sets ModalResult to mrCancel. In Blazor, navigate back.
    /// </summary>
    private void BtnCloseClick()
    {
        Navigation.NavigateTo("/");
    }

    /// <summary>
    /// Legacy: btnRefreshClick — reloads all tables from BL.
    /// </summary>
    private async Task BtnRefreshClick()
    {
        await LoadAllTablesAsync();
    }

    /// <summary>
    /// Legacy: btnFilterAvailableClick — loads only available tables.
    /// </summary>
    private async Task BtnFilterAvailableClick()
    {
        var availableTables = await TablesBLService.GetAvailableTablesAsync();
        await LoadTablesAsync(availableTables);
    }

    /// <summary>
    /// Legacy: btnShowAllClick — loads all tables.
    /// </summary>
    private async Task BtnShowAllClick()
    {
        await LoadAllTablesAsync();
    }

    /// <summary>
    /// Legacy: btnAddClick — sets adding mode, clears edit fields, shows edit panel.
    /// </summary>
    private void BtnAddClick()
    {
        _state!.FIsAdding = true;
        _state.EdtNumberText = string.Empty;
        _state.EdtCapacityText = string.Empty;
        _selectedStatusIndex = 0;
        _selectedZoneIndex = 0;
        _editPanelVisible = true;
    }

    /// <summary>
    /// Legacy: btnEditClick — populates edit fields from selected row, shows edit panel.
    /// Delegates complex logic to service.
    /// </summary>
    private async Task BtnEditClick()
    {
        if (_selectedItem == null)
        {
            Snackbar.Add("Please select a table to edit.", Severity.Warning);
            return;
        }

        _state!.FSelectedId = _selectedItem.TableId;
        _state.EdtNumberText = _selectedItem.TableNumber.ToString(CultureInfo.InvariantCulture);
        _state.EdtCapacityText = _selectedItem.Capacity.ToString(CultureInfo.InvariantCulture);

        // Legacy maps the combo index straight onto the enum ordinal
        // (TTableStatus(cmbEditStatus.ItemIndex)), so read the enum the grid binds.
        // StatusDisplay is never populated anywhere — reading it always yielded 0.
        _selectedStatusIndex = (int)_selectedItem.Status;

        var zoneText = _selectedItem.Zone ?? string.Empty;
        _selectedZoneIndex = Array.FindIndex(ZoneNames, z => string.Equals(z, zoneText, StringComparison.OrdinalIgnoreCase));
        if (_selectedZoneIndex < 0)
            _selectedZoneIndex = 0;

        _state.FIsAdding = false;

        _state = await TablesService.BtnEditClickAsync(_state, _selectedItem?.TableId ?? 0);

        _editPanelVisible = true;
    }

    /// <summary>
    /// Legacy: btnDeleteClick — confirms deletion, deletes via service, reloads grid.
    /// </summary>
    private async Task BtnDeleteClick()
    {
        if (_selectedItem == null)
        {
            Snackbar.Add("Please select a table to delete.", Severity.Warning);
            return;
        }

        var confirmed = await DialogService.ShowMessageBox(
            "Confirm Delete",
            "Are you sure you want to delete this table?",
            yesText: "Yes",
            cancelText: "No");

        if (confirmed == true)
        {
            _state!.FSelectedId = _selectedItem.TableId;
            _state = await TablesService.BtnDeleteClickAsync(_state, _selectedItem?.TableId ?? 0);
            await LoadAllTablesAsync();
        }
    }

    /// <summary>
    /// Legacy: btnSaveClick — builds TTableInfo from form fields, adds or updates via service, reloads grid.
    /// </summary>
    private async Task BtnSaveClick()
    {
        _state!.EdtNumberText = _state.EdtNumberText ?? string.Empty;
        _state.EdtCapacityText = _state.EdtCapacityText ?? string.Empty;

        // BtnSaveClickAsync reads StatusIndex and Zone off the state DTO, mirroring
        // legacy TTableStatus(cmbEditStatus.ItemIndex) and cmbEditZone.Text.
        _state.StatusIndex = _selectedStatusIndex;
        _state.Zone = (_selectedZoneIndex >= 0 && _selectedZoneIndex < ZoneNames.Length)
            ? ZoneNames[_selectedZoneIndex]
            : ZoneNames[0];

        _state = await TablesService.BtnSaveClickAsync(_state);

        _editPanelVisible = false;
        await LoadAllTablesAsync();
    }

    /// <summary>
    /// Legacy: btnCancelClick — hides edit panel.
    /// </summary>
    private void BtnCancelClick()
    {
        _editPanelVisible = false;
    }

    /// <summary>
    /// Legacy: FormDestroy — frees BL. In Blazor, handled by DI scope disposal.
    /// </summary>
    public void Dispose()
    {
        // Legacy: FormDestroy — FTablesBL.Free;
        // BL service lifetime is managed by DI container; no manual disposal needed.
    }
}
