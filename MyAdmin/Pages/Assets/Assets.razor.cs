using System.Globalization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using MyAdmin.Application.Services.Assets;
using MyAdmin.Application.Services.MyAdmin;

namespace MyAdmin.Pages.Assets;

public partial class Assets : ComponentBase, IDisposable
{
    [Inject]
    private IAssetsService AssetsService { get; set; } = default!;

    [Inject]
    private IuAdminBLService AdminBLService { get; set; } = default!;

    [Inject]
    private NavigationManager Navigation { get; set; } = default!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    [Inject]
    private IDialogService DialogService { get; set; } = default!;

    private AssetsStateDto? _state;
    private List<TAssetInfoDto> _assets = new();
    private TAssetInfoDto? _selectedAsset;
    private bool _loading;
    private bool _editPanelVisible;
    private string _totalValueText = "Total Value: $0.00";
    private string _totalDepreciatedText = "Total Depreciated: $0.00";
    private CancellationTokenSource _cts = new();

    /// <summary>
    /// Legacy: FormCreate — initializes state and loads assets grid.
    /// Grid configuration (columns/widths) is handled declaratively in markup.
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        _state = new AssetsStateDto();
        await LoadAssetsAsync();
    }

    /// <summary>
    /// Legacy: LoadAssets — fetches asset list from AdminBLService and computes totals.
    /// </summary>
    private async Task LoadAssetsAsync()
    {
        _loading = true;
        try
        {
            var assetArray = await AdminBLService.GetAssetsAsync(_cts.Token);
            _assets = assetArray.ToList();

            decimal totalValue = 0m;
            decimal totalDepreciated = 0m;

            foreach (var asset in _assets)
            {
                totalValue += asset.Value;
                totalDepreciated += asset.DepreciatedValue;
            }

            _totalValueText = "Total Value: " + totalValue.ToString("C2", CultureInfo.InvariantCulture);
            _totalDepreciatedText = "Total Depreciated: " + totalDepreciated.ToString("C2", CultureInfo.InvariantCulture);
        }
        finally
        {
            _loading = false;
        }
    }

    /// <summary>
    /// Legacy: btnRefreshClick — reloads the asset grid.
    /// </summary>
    private async Task btnRefreshClick()
    {
        await LoadAssetsAsync();
    }

    /// <summary>
    /// Legacy: btnAddClick — opens the edit panel in add mode with cleared fields.
    /// </summary>
    private void btnAddClick()
    {
        if (_state != null)
        {
            _state.FIsAdding = true;
            _state.EdtAssetNameText = string.Empty;
            _state.CmbAssetCategoryText = string.Empty;
            _state.EdtPurchaseDateText = string.Empty;
            _state.EdtValueText = string.Empty;
            _state.EdtDepreciatedText = string.Empty;
            _state.CmbAssetStatusText = string.Empty;
        }
        _editPanelVisible = true;
        _selectedAsset = null;
    }

    /// <summary>
    /// Legacy: btnEditClick — populates edit panel from selected grid row.
    /// </summary>
    private void btnEditClick()
    {
        if (_selectedAsset == null)
        {
            Snackbar.Add("Please select an asset to edit.", Severity.Warning);
            return;
        }

        if (_state != null)
        {
            _state.FIsAdding = false;
            _state.FSelectedId = _selectedAsset.AssetId;
            _state.EdtAssetNameText = _selectedAsset.AssetName;
            _state.CmbAssetCategoryText = _selectedAsset.Category;
            _state.EdtPurchaseDateText = _selectedAsset.PurchaseDate.ToString("d", CultureInfo.InvariantCulture);
            _state.EdtValueText = _selectedAsset.Value.ToString(CultureInfo.InvariantCulture);
            _state.EdtDepreciatedText = _selectedAsset.DepreciatedValue.ToString(CultureInfo.InvariantCulture);
            _state.CmbAssetStatusText = _selectedAsset.Status;
        }

        _editPanelVisible = true;
    }

    /// <summary>
    /// Legacy: btnDeleteClick — confirms and deletes the selected asset.
    /// </summary>
    private async Task btnDeleteClick()
    {
        if (_selectedAsset == null)
        {
            Snackbar.Add("Please select an asset to delete.", Severity.Warning);
            return;
        }

        var confirmed = await DialogService.ShowMessageBox(
            "Confirm Delete",
            "Are you sure you want to delete this asset?",
            yesText: "Yes",
            cancelText: "No");

        if (confirmed == true)
        {
            await AdminBLService.DeleteAssetAsync(_selectedAsset.AssetId, _cts.Token);
            _selectedAsset = null;
            await LoadAssetsAsync();
        }
    }

    /// <summary>
    /// Legacy: btnSaveClick — saves the new or edited asset, hides edit panel, refreshes grid.
    /// </summary>
    private async Task btnSaveClick()
    {
        if (_state == null)
            return;

        var asset = new TAssetInfoDto
        {
            AssetName = _state.EdtAssetNameText,
            Category = _state.CmbAssetCategoryText,
            PurchaseDate = DateTime.TryParse(_state.EdtPurchaseDateText, CultureInfo.InvariantCulture, DateTimeStyles.None, out var pd)
                ? pd
                : DateTime.UtcNow,
            Value = decimal.TryParse(_state.EdtValueText, NumberStyles.Any, CultureInfo.InvariantCulture, out var v) ? v : 0m,
            DepreciatedValue = decimal.TryParse(_state.EdtDepreciatedText, NumberStyles.Any, CultureInfo.InvariantCulture, out var dv) ? dv : 0m,
            Status = _state.CmbAssetStatusText
        };

        if (_state.FIsAdding)
        {
            await AdminBLService.AddAssetAsync(asset, _cts.Token);
        }
        else
        {
            asset.AssetId = _state.FSelectedId;
            await AdminBLService.UpdateAssetAsync(asset, _cts.Token);
        }

        _editPanelVisible = false;
        _state.FIsAdding = false;
        await LoadAssetsAsync();
    }

    /// <summary>
    /// Legacy: btnCancelClick — hides the edit panel without saving.
    /// </summary>
    private void btnCancelClick()
    {
        _editPanelVisible = false;
        if (_state != null)
        {
            _state.FIsAdding = false;
        }
    }

    /// <summary>
    /// Legacy: btnCloseClick — navigates away (modal result cancel equivalent).
    /// </summary>
    private void btnCloseClick()
    {
        Navigation.NavigateTo("/");
    }

    /// <summary>
    /// Tracks the selected row in the grid for edit/delete operations.
    /// </summary>
    private void OnSelectedAssetChanged(TAssetInfoDto? asset)
    {
        _selectedAsset = asset;
    }

    /// <summary>
    /// Legacy: FormDestroy — disposes cancellation token source.
    /// </summary>
    public void Dispose()
    {
        _cts.Cancel();
        _cts.Dispose();
    }
}
