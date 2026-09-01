using System.Globalization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using MyRestaurant.Application.Services.Bill;
using MyRestaurant.Application.Services.MyRestaurant;

namespace MyRestaurant.Pages.Bill;

public partial class BillCheck : ComponentBase, IDisposable
{
    [Inject] private IBillCheckService BillCheckService { get; set; } = default!;
    [Inject] private IuRestaurantBLService RestaurantBLService { get; set; } = default!;
    [Inject] private IDialogService DialogService { get; set; } = default!;
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;
    [Inject] private ISnackbar Snackbar { get; set; } = default!;

    private BillCheckStateDto? _state;
    private List<TBillInfoDto> _bills = new();
    private TBillInfoDto? _selectedBill;
    private bool _editPanelVisible;
    private string _totalBillsText = "Total Bills: 0";
    private string _totalRevenueText = "Total Revenue: $0.00";
    private string _selectedPaymentMethod = "Cash";

    private readonly string[] _paymentMethods = { "Cash", "Credit Card", "Debit Card" };

    // FormCreate — initialize state and load bills
    protected override async Task OnInitializedAsync()
    {
        _state = new BillCheckStateDto();
        await LoadBills();
    }

    // LoadBills — delegates to service, updates summary labels
    private async Task LoadBills()
    {
        var billDtos = await RestaurantBLService.GetBillsAsync(default);
        _bills = billDtos?.ToList() ?? new List<TBillInfoDto>();

        if (_bills.Count == 0)
        {
            _totalBillsText = "Total Bills: 0";
            _totalRevenueText = "Total Revenue: $0.00";
            return;
        }

        decimal totalRevenue = 0m;
        for (int i = 0; i < _bills.Count; i++)
        {
            totalRevenue += _bills[i].Total;
        }

        _totalBillsText = "Total Bills: " + _bills.Count.ToString(CultureInfo.InvariantCulture);
        _totalRevenueText = "Total Revenue: " + totalRevenue.ToString("C2", CultureInfo.InvariantCulture);
    }

    private void OnSelectedBillChanged(TBillInfoDto? bill)
    {
        _selectedBill = bill;
    }

    // btnCloseClick — navigate away (legacy: ModalResult := mrCancel)
    private void BtnCloseClick()
    {
        NavigationManager.NavigateTo("/");
    }

    // btnRefreshClick — reload bills
    private async Task BtnRefreshClick()
    {
        await LoadBills();
    }

    // btnAddClick — clear edit fields, show edit panel in add mode
    private void BtnAddClick()
    {
        _state!.FIsAdding = true;
        _state.EdtOrderIdText = string.Empty;
        _state.EdtSubtotalText = string.Empty;
        _state.EdtTipText = string.Empty;
        _selectedPaymentMethod = "Cash";
        _editPanelVisible = true;
    }

    // btnEditClick — populate edit fields from selected row, show edit panel in edit mode
    private void BtnEditClick()
    {
        if (_selectedBill == null || _selectedBill.BillId == 0)
        {
            Snackbar.Add("Please select a bill to edit.", Severity.Warning);
            return;
        }

        _state!.FSelectedId = _selectedBill.BillId;
        _state.FIsAdding = false;
        _state.EdtOrderIdText = _selectedBill.OrderId.ToString(CultureInfo.InvariantCulture);
        _state.EdtSubtotalText = _selectedBill.Subtotal.ToString(CultureInfo.InvariantCulture);
        _state.EdtTipText = _selectedBill.Tip.ToString(CultureInfo.InvariantCulture);

        var payText = _selectedBill.PaymentMethod.ToString();
        if (payText == "Cash") _selectedPaymentMethod = "Cash";
        else if (payText == "Credit Card") _selectedPaymentMethod = "Credit Card";
        else if (payText == "Debit Card") _selectedPaymentMethod = "Debit Card";
        else _selectedPaymentMethod = "Cash";

        _editPanelVisible = true;
    }

    // btnDeleteClick — confirm and delete selected bill
    private async Task BtnDeleteClick()
    {
        if (_selectedBill == null || _selectedBill.BillId == 0)
        {
            Snackbar.Add("Please select a bill to delete.", Severity.Warning);
            return;
        }

        var dialogResult = await DialogService.ShowMessageBox(
            "Confirm Delete",
            "Delete this bill?",
            yesText: "Yes",
            noText: "No");

        if (dialogResult == true)
        {
            await RestaurantBLService.DeleteBillAsync(_selectedBill.BillId, default);
            _selectedBill = null;
            await LoadBills();
        }
    }

    // btnSaveClick — build bill DTO, delegate add/update to RestaurantBLService
    private async Task BtnSaveClick()
    {
        int orderId = int.TryParse(_state!.EdtOrderIdText, NumberStyles.Integer, CultureInfo.InvariantCulture, out var oid) ? oid : 0;
        decimal subtotal = decimal.TryParse(_state.EdtSubtotalText, NumberStyles.Number, CultureInfo.InvariantCulture, out var sub) ? sub : 0m;
        decimal tip = decimal.TryParse(_state.EdtTipText, NumberStyles.Number, CultureInfo.InvariantCulture, out var t) ? t : 0m;

        var bill = new TBillInfoDto
        {
            OrderId = orderId,
            Subtotal = subtotal,
            Tip = tip,
            PaymentMethod = Enum.TryParse<TPaymentMethod>(_selectedPaymentMethod?.Replace(" ", string.Empty), out var __pm) ? __pm : TPaymentMethod.Cash,
            PaidDate = DateTime.UtcNow
        };

        if (_state.FIsAdding)
        {
            // Tax and Total computed by service (legacy: ABill.Tax := ABill.Subtotal * 0.08)
            await RestaurantBLService.AddBillAsync(bill, default);
        }
        else
        {
            bill.BillId = _state.FSelectedId;
            bill.Tax = Math.Round(subtotal * 0.08m, 2, MidpointRounding.ToEven);
            bill.Total = subtotal + bill.Tax + tip;
            await RestaurantBLService.UpdateBillAsync(bill, default);
        }

        _editPanelVisible = false;
        _selectedBill = null;
        await LoadBills();
    }

    // btnCancelClick — hide edit panel
    private void BtnCancelClick()
    {
        _editPanelVisible = false;
    }

    // FormDestroy — cleanup (legacy: FRestaurantBL.Free)
    public void Dispose()
    {
        // FormDestroy: no unmanaged resources; DI-managed services are disposed by the container.
    }
}
