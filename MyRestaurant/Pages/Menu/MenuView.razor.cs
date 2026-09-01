using System.Globalization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using MyRestaurant.Application.Services.Menu;
using MyRestaurant.Application.DTOs.Menu;

namespace MyRestaurant.Pages.Menu;

public partial class MenuView : ComponentBase, IDisposable
{
    [Inject]
    private IMenuViewService MenuViewService { get; set; } = default!;

    [Inject]
    private NavigationManager Navigation { get; set; } = default!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    private List<TMenuItemInfoDto> _allMenuItems = new();
    private List<TMenuItemInfoDto> _displayedItems = new();
    private TMenuItemInfoDto? _selectedItem;
    private bool _editPanelVisible;
    private bool _isAdding;
    private int _selectedId;
    private string _selectedCategory = "All";
    private string _selectedActive = "Yes";
    private string _edtItemNameText = string.Empty;
    private string _cmbEditCategoryText = "Appetizer";
    private string _edtPriceText = string.Empty;
    private string _edtCostText = string.Empty;
    private MudMessageBox _deleteConfirmBox = default!;

    private readonly string[] _categoryOptions = { "All", "Appetizer", "Main Course", "Dessert", "Beverage" };
    private readonly string[] _editCategoryOptions = { "Appetizer", "Main Course", "Dessert", "Beverage" };
    private readonly string[] _activeOptions = { "Yes", "No" };

    // Legacy: FormCreate
    protected override async Task OnInitializedAsync()
    {
        await LoadMenuItemsFromService();
    }

    // Legacy: LoadMenuItems — fetches all menu items via the service
    private async Task LoadMenuItemsFromService()
    {
        var items = await MenuViewService.GetMenuItemsAsync();
        _allMenuItems = items.ToList();
        _displayedItems = new List<TMenuItemInfoDto>(_allMenuItems);
        _selectedItem = null;
    }

    // Legacy: FilterByCategory(const ACategory: string)
    private void ApplyFilterByCategory(string category)
    {
        if (string.IsNullOrEmpty(category) || string.Equals(category, "All", StringComparison.OrdinalIgnoreCase))
        {
            _displayedItems = new List<TMenuItemInfoDto>(_allMenuItems);
        }
        else
        {
            _displayedItems = _allMenuItems
                .Where(x => string.Equals(x.Category, category, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        _selectedItem = null;
    }

    // Legacy: btnCloseClick
    private void BtnCloseClick()
    {
        Navigation.NavigateTo("/");
    }

    // Legacy: btnRefreshClick
    private async Task BtnRefreshClick()
    {
        _selectedCategory = "All";
        await LoadMenuItemsFromService();
    }

    // Legacy: cmbCategoryChange
    private void CmbCategoryChange(string value)
    {
        _selectedCategory = value;
        ApplyFilterByCategory(_selectedCategory);
    }

    // Legacy: btnAddClick
    private void BtnAddClick()
    {
        _isAdding = true;
        _edtItemNameText = string.Empty;
        _cmbEditCategoryText = _editCategoryOptions[0]; // "Appetizer"
        _edtPriceText = string.Empty;
        _edtCostText = string.Empty;
        _selectedActive = _activeOptions[0]; // "Yes"
        _editPanelVisible = true;
    }

    // Legacy: btnEditClick
    private void BtnEditClick()
    {
        if (_selectedItem == null)
            return;

        _selectedId = _selectedItem.ItemId;
        if (_selectedId == 0)
            return;

        _isAdding = false;
        _edtItemNameText = _selectedItem.ItemName;
        _cmbEditCategoryText = _selectedItem.Category;
        _edtPriceText = _selectedItem.Price.ToString(CultureInfo.InvariantCulture);
        _edtCostText = _selectedItem.Cost.ToString(CultureInfo.InvariantCulture);
        _selectedActive = _selectedItem.Active ? "Yes" : "No";

        _editPanelVisible = true;
    }

    // Legacy: btnDeleteClick
    private async Task BtnDeleteClick()
    {
        if (_selectedItem == null)
            return;

        if (_selectedItem.ItemId == 0)
            return;

        var result = await _deleteConfirmBox.ShowAsync();
        if (result != true)
            return;

        await MenuViewService.DeleteMenuItemAsync(_selectedItem.ItemId);

        await LoadMenuItemsFromService();

        // Re-apply current category filter
        if (!string.Equals(_selectedCategory, "All", StringComparison.OrdinalIgnoreCase))
        {
            ApplyFilterByCategory(_selectedCategory);
        }
    }

    // Legacy: btnSaveClick
    private async Task BtnSaveClick()
    {
        var item = new TMenuItemInfoDto
        {
            ItemName = _edtItemNameText,
            Category = _cmbEditCategoryText,
            Price = decimal.TryParse(_edtPriceText, NumberStyles.Any, CultureInfo.InvariantCulture, out var p) ? p : 0m,
            Cost = decimal.TryParse(_edtCostText, NumberStyles.Any, CultureInfo.InvariantCulture, out var c) ? c : 0m,
            Active = string.Equals(_selectedActive, "Yes", StringComparison.OrdinalIgnoreCase)
        };

        if (_isAdding)
        {
            await MenuViewService.AddMenuItemAsync(item);
        }
        else
        {
            item.ItemId = _selectedId;
            await MenuViewService.UpdateMenuItemAsync(item);
        }

        _editPanelVisible = false;

        // Reload and re-apply filter (legacy: FAllMenuItems := FRestaurantBL.GetMenuItems; LoadMenuItems)
        await LoadMenuItemsFromService();

        if (!string.Equals(_selectedCategory, "All", StringComparison.OrdinalIgnoreCase))
        {
            ApplyFilterByCategory(_selectedCategory);
        }
    }

    // Legacy: btnCancelClick
    private void BtnCancelClick()
    {
        _editPanelVisible = false;
    }

    private void OnSelectedItemChanged(TMenuItemInfoDto? item)
    {
        _selectedItem = item;
    }

    // Legacy: FormDestroy
    public void Dispose()
    {
        // Clean up any resources if needed; legacy freed FRestaurantBL here.
        // Scoped services are disposed by the DI container.
    }
}
