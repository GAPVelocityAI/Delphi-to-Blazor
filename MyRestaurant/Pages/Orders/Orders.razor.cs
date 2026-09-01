using System.Globalization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using MyRestaurant.Application.Services;
using MyRestaurant.Application.Services.Orders;

namespace MyRestaurant.Pages.Orders;

public partial class Orders : ComponentBase, IDisposable
{
    [Inject]
    private IOrdersService OrdersService { get; set; } = default!;

    [Inject]
    private NavigationManager Navigation { get; set; } = default!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    private List<TOrderInfoDto> _orders = new();
    private List<TOrderDetailInfoDto> _orderDetails = new();
    private TOrderInfoDto? _selectedOrder;
    private bool _editPanelVisible;
    private bool _isAdding;
    private int _selectedId;
    private string _selectedStatus = "Pending";
    private string _edtTableIdText = string.Empty;
    private string _edtTotalAmountText = string.Empty;
    private MudMessageBox _deleteConfirmBox = default!;

    private static readonly string[] StatusOptions = { "Pending", "Preparing", "Served", "Paid", "Cancelled" };

    /// <summary>
    /// FormCreate — loads initial data.
    /// Legacy: FormCreate -> TRestaurantBL.Create; LoadOrders;
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        await LoadOrdersAsync();
    }

    /// <summary>
    /// LoadOrders — fetches full order list from service and clears details grid.
    /// Legacy: LoadOrders
    /// </summary>
    private async Task LoadOrdersAsync()
    {
        var orders = await OrdersService.GetOrdersAsync();
        _orders = orders.ToList();
        _orderDetails = new List<TOrderDetailInfoDto>();
        _selectedOrder = null;
    }

    /// <summary>
    /// LoadOrderDetails — fetches detail rows for a given order.
    /// Legacy: LoadOrderDetails(AOrderId)
    /// </summary>
    private async Task LoadOrderDetailsAsync(int orderId)
    {
        if (orderId > 0)
        {
            var details = await OrdersService.GetOrderDetailsAsync(orderId);
            _orderDetails = details.ToList();
        }
        else
        {
            _orderDetails = new List<TOrderDetailInfoDto>();
        }
    }

    /// <summary>
    /// grdOrdersSelectCell — when a row is selected, load its details.
    /// Legacy: grdOrdersSelectCell
    /// </summary>
    private async Task grdOrdersSelectCell(TOrderInfoDto? order)
    {
        _selectedOrder = order;
        if (order is not null && order.OrderId > 0)
        {
            await LoadOrderDetailsAsync(order.OrderId);
        }
        else
        {
            _orderDetails = new List<TOrderDetailInfoDto>();
        }
    }

    /// <summary>
    /// btnCloseClick — navigates away (modal result equivalent).
    /// Legacy: ModalResult := mrCancel
    /// </summary>
    private void btnCloseClick()
    {
        Navigation.NavigateTo("/");
    }

    /// <summary>
    /// btnRefreshClick — reloads orders.
    /// Legacy: LoadOrders
    /// </summary>
    private async Task btnRefreshClick()
    {
        await LoadOrdersAsync();
    }

    /// <summary>
    /// btnViewDetailsClick — loads details for currently selected order row.
    /// Legacy: if grdOrders.Row > 0 then begin OrderId := ...; if OrderId > 0 then LoadOrderDetails(OrderId); end;
    /// </summary>
    private async Task btnViewDetailsClick()
    {
        if (_selectedOrder is not null && _selectedOrder.OrderId > 0)
        {
            await LoadOrderDetailsAsync(_selectedOrder.OrderId);
        }
    }

    /// <summary>
    /// btnAddClick — resets edit panel for adding a new order.
    /// Legacy: FIsAdding := True; edtTableId.Text := ''; cmbEditStatus.ItemIndex := 0;
    ///         edtTotalAmount.Text := ''; pnlEdit.Visible := True;
    /// </summary>
    private void btnAddClick()
    {
        _isAdding = true;
        _edtTableIdText = string.Empty;
        _selectedStatus = "Pending";
        _edtTotalAmountText = string.Empty;
        _editPanelVisible = true;
    }

    /// <summary>
    /// btnEditClick — populates edit panel from selected order row.
    /// Legacy: FIsAdding := False; edtTableId.Text := ...; cmbEditStatus based on StatusText; edtTotalAmount.Text := ...; pnlEdit.Visible := True;
    /// </summary>
    private void btnEditClick()
    {
        if (_selectedOrder is null || _selectedOrder.OrderId <= 0)
        {
            return;
        }

        _selectedId = _selectedOrder.OrderId;
        _isAdding = false;
        _edtTableIdText = _selectedOrder.TableId.ToString(CultureInfo.InvariantCulture);
        _edtTotalAmountText = _selectedOrder.TotalAmount.ToString(CultureInfo.InvariantCulture);

        // Legacy reads the status back off the grid cell, which was filled with
        // Status.ToString -- StatusDisplay is never populated, so it always fell
        // through to "Pending".
        var statusText = _selectedOrder.Status.ToString();
        _selectedStatus = Array.IndexOf(StatusOptions, statusText) >= 0 ? statusText : "Pending";

        _editPanelVisible = true;
    }

    /// <summary>
    /// btnDeleteClick — confirms and deletes the selected order via service.
    /// Legacy: if MessageDlg('Delete this order?', ...) = mrYes then begin
    ///           FRestaurantBL.DeleteOrder(Id); LoadOrders; end;
    /// </summary>
    private async Task btnDeleteClick()
    {
        if (_selectedOrder is null || _selectedOrder.OrderId <= 0)
        {
            return;
        }

        var confirmed = await _deleteConfirmBox.ShowAsync();
        if (confirmed != true)
        {
            return;
        }

        await OrdersService.DeleteOrderAsync(_selectedOrder.OrderId);
        Snackbar.Add("Order deleted.", Severity.Success);
        await LoadOrdersAsync();
    }

    /// <summary>
    /// btnSaveClick — saves the new or edited order via service.
    /// Legacy: Order.TableId := ...; Order.Status := TOrderStatus(cmbEditStatus.ItemIndex);
    ///         Order.TotalAmount := ...; if FIsAdding then begin Order.OrderDate := Now;
    ///         FRestaurantBL.AddOrder(Order); end else begin Order.OrderId := FSelectedId;
    ///         Order.OrderDate := Now; FRestaurantBL.UpdateOrder(Order); end;
    ///         pnlEdit.Visible := False; LoadOrders;
    /// </summary>
    private async Task btnSaveClick()
    {
        var statusIndex = Array.IndexOf(StatusOptions, _selectedStatus);
        if (statusIndex < 0)
        {
            statusIndex = 0;
        }

        if (!int.TryParse(_edtTableIdText, NumberStyles.Integer, CultureInfo.InvariantCulture, out var tableId))
        {
            tableId = 0;
        }

        if (!decimal.TryParse(_edtTotalAmountText, NumberStyles.Number, CultureInfo.InvariantCulture, out var totalAmount))
        {
            totalAmount = 0m;
        }

        var order = new TOrderInfoDto
        {
            TableId = tableId,
            StatusIndex = statusIndex,
            TotalAmount = totalAmount,
            OrderDate = DateTime.UtcNow
        };

        if (_isAdding)
        {
            await OrdersService.AddOrderAsync(order);
            Snackbar.Add("Order added.", Severity.Success);
        }
        else
        {
            order.OrderId = _selectedId;
            await OrdersService.UpdateOrderAsync(order);
            Snackbar.Add("Order updated.", Severity.Success);
        }

        _editPanelVisible = false;
        await LoadOrdersAsync();
    }

    /// <summary>
    /// btnCancelClick — hides the edit panel.
    /// Legacy: pnlEdit.Visible := False;
    /// </summary>
    private void btnCancelClick()
    {
        _editPanelVisible = false;
    }

    /// <summary>
    /// FormDestroy — cleanup (legacy: FRestaurantBL.Free).
    /// Service is DI-managed, no manual disposal needed.
    /// </summary>
    public void Dispose()
    {
        // FormDestroy: FRestaurantBL.Free — service is DI-managed, no manual disposal needed.
    }
}
