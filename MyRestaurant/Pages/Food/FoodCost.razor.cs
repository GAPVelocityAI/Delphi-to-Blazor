using System.Globalization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using MyRestaurant.Application.Services.Food;
using MyRestaurant.Application.Services.MyRestaurant;
using MyRestaurant.Application.DTOs.Food;

namespace MyRestaurant.Pages.Food;

public partial class FoodCost : ComponentBase, IDisposable
{
    [Inject]
    private IFoodCostService FoodCostService { get; set; } = default!;

    [Inject]
    private IuRestaurantBLService RestaurantBLService { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    private FoodCostStateDto? _state;
    private List<TFoodCostInfoDto> _foodCosts = new();
    private TFoodCostInfoDto? _selectedItem;
    private bool _editPanelVisible;
    private string _avgCostPctText = "Avg Cost %: N/A";
    private CancellationTokenSource _cts = new();
    private static readonly CultureInfo _invariantCulture = CultureInfo.InvariantCulture;

    // Corresponds to legacy FormCreate
    protected override async Task OnInitializedAsync()
    {
        _state = new FoodCostStateDto();
        await LoadFoodCosts();
    }

    // Corresponds to legacy LoadFoodCosts
    private async Task LoadFoodCosts()
    {
        var allCosts = await RestaurantBLService.GetFoodCostsAsync(_cts.Token);
        _foodCosts = allCosts.ToList();

        if (_foodCosts.Count > 0)
        {
            decimal totalPct = 0m;
            foreach (var cost in _foodCosts)
            {
                totalPct += cost.CostPercentage;
            }

            var avg = Math.Round(totalPct / _foodCosts.Count, 1, MidpointRounding.ToEven);
            _avgCostPctText = $"Avg Cost %: {avg.ToString("0.0", _invariantCulture)}%";
        }
        else
        {
            _avgCostPctText = "Avg Cost %: N/A";
        }
    }

    /// <summary>
    /// Corresponds to legacy grdFoodCostDrawCell.
    /// Returns the background color string for the Cost % column cell based on value thresholds.
    /// Legacy DrawCell colored the cell background:
    ///   > 35%  => light red ($008080FF → #FF8080)
    ///   >= 25% => light yellow ($0080FFFF → #FFFF80)
    ///   &lt; 25%  => light green ($0080FF80 → #80FF80)
    /// </summary>
    private static string GetCostPercentageCellColor(decimal costPercentage)
    {
        if (costPercentage > 35.0m)
        {
            return "#FF8080"; // Light red
        }

        if (costPercentage >= 25.0m)
        {
            return "#FFFF80"; // Light yellow
        }

        return "#80FF80"; // Light green
    }

    // Corresponds to legacy btnCloseClick
    private void BtnCloseClick()
    {
        NavigationManager.NavigateTo("/");
    }

    // Corresponds to legacy btnRefreshClick
    private async Task BtnRefreshClick()
    {
        await LoadFoodCosts();
    }

    // Corresponds to legacy btnAddClick
    private void BtnAddClick()
    {
        _state!.FIsAdding = true;
        _state.EdtRecipeNameText = string.Empty;
        _state.EdtIngredientsText = string.Empty;
        _state.EdtTotalCostText = string.Empty;
        _state.EdtSellingPriceText = string.Empty;
        _editPanelVisible = true;
    }

    // Corresponds to legacy btnEditClick
    private void BtnEditClick()
    {
        if (_selectedItem is null || _selectedItem.RecipeId == 0)
        {
            Snackbar.Add("Please select a food cost entry to edit.", Severity.Warning);
            return;
        }

        _state!.FSelectedId = _selectedItem.RecipeId;
        _state.FIsAdding = false;
        _state.EdtRecipeNameText = _selectedItem.RecipeName;
        _state.EdtIngredientsText = _selectedItem.IngredientCount.ToString(CultureInfo.InvariantCulture);
        _state.EdtTotalCostText = _selectedItem.TotalCost.ToString(CultureInfo.InvariantCulture);
        _state.EdtSellingPriceText = _selectedItem.SellingPrice.ToString(CultureInfo.InvariantCulture);
        _editPanelVisible = true;
    }

    // Corresponds to legacy btnDeleteClick
    private async Task BtnDeleteClick()
    {
        if (_selectedItem is null || _selectedItem.RecipeId == 0)
        {
            Snackbar.Add("Please select a food cost entry to delete.", Severity.Warning);
            return;
        }

        await RestaurantBLService.DeleteFoodCostAsync(_selectedItem.RecipeId, _cts.Token);

        _selectedItem = null;
        await LoadFoodCosts();
        Snackbar.Add("Food cost entry deleted.", Severity.Success);
    }

    // Corresponds to legacy btnSaveClick
    private async Task BtnSaveClick()
    {
        var recipeName = _state!.EdtRecipeNameText ?? string.Empty;

        int ingredientCount = 0;
        if (!string.IsNullOrWhiteSpace(_state.EdtIngredientsText))
        {
            int.TryParse(_state.EdtIngredientsText, NumberStyles.Integer, CultureInfo.InvariantCulture, out ingredientCount);
        }

        decimal totalCost = 0m;
        if (!string.IsNullOrWhiteSpace(_state.EdtTotalCostText))
        {
            decimal.TryParse(_state.EdtTotalCostText, NumberStyles.Number, CultureInfo.InvariantCulture, out totalCost);
        }

        decimal sellingPrice = 0m;
        if (!string.IsNullOrWhiteSpace(_state.EdtSellingPriceText))
        {
            decimal.TryParse(_state.EdtSellingPriceText, NumberStyles.Number, CultureInfo.InvariantCulture, out sellingPrice);
        }

        var costDto = new TFoodCostInfoDto
        {
            RecipeName = recipeName,
            IngredientCount = ingredientCount,
            TotalCost = totalCost,
            SellingPrice = sellingPrice
        };

        if (_state.FIsAdding)
        {
            // CostPercentage is computed by service on add
            await RestaurantBLService.AddFoodCostAsync(costDto, _cts.Token);
        }
        else
        {
            costDto.RecipeId = _state.FSelectedId;
            if (costDto.SellingPrice > 0m)
            {
                costDto.CostPercentage = Math.Round(costDto.TotalCost / costDto.SellingPrice * 100m, 1, MidpointRounding.ToEven);
            }
            else
            {
                costDto.CostPercentage = 0m;
            }

            await RestaurantBLService.UpdateFoodCostAsync(costDto, _cts.Token);
        }

        _editPanelVisible = false;
        _selectedItem = null;
        await LoadFoodCosts();
        Snackbar.Add("Food cost entry saved.", Severity.Success);
    }

    // Corresponds to legacy btnCancelClick
    private void BtnCancelClick()
    {
        _editPanelVisible = false;
    }

    private void OnSelectedItemChanged(TFoodCostInfoDto? item)
    {
        _selectedItem = item;
    }

    // Corresponds to legacy FormDestroy
    public void Dispose()
    {
        _cts.Cancel();
        _cts.Dispose();
    }
}
