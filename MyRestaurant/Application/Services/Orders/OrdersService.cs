using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyRestaurant.Infrastructure.Data;

namespace MyRestaurant.Application.Services.Orders;

public class OrdersService : IOrdersService
{
    private readonly IDbContextFactory<MyRestaurantDbContext> _dbFactory;
    private readonly ILogger<OrdersService> _logger;

    public OrdersService(
        IDbContextFactory<MyRestaurantDbContext> dbFactory,
        ILogger<OrdersService> logger)
    {
        _dbFactory = dbFactory;
        _logger = logger;
    }

    /// <summary>
    /// Legacy: LoadOrders — queries all orders from DB, ordered by OrderDate DESC.
    /// Returns list of TOrderInfoDto with each field carried individually.
    /// Also populates state.Orders for convenience.
    /// </summary>
    public async Task<List<TOrderInfoDto>> LoadOrdersAsync(OrdersStateDto state, CancellationToken ct = default)
    {
        await using var db = await _dbFactory.CreateDbContextAsync(ct);

        var entities = await db.Orders
            .AsNoTracking()
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync(ct);

        var result = MapOrderEntitiesToDtos(entities);

        state.Orders = result;
        state.OrderDetails = new List<TOrderDetailInfoDto>();

        return result;
    }

    /// <summary>
    /// Legacy: LoadOrders — synchronous wrapper that delegates to LoadOrdersAsync.
    /// Provided for legacy compatibility where the original Delphi code called LoadOrders directly.
    /// </summary>
    public List<TOrderInfoDto> LoadOrders(OrdersStateDto state)
    {
        return LoadOrdersAsync(state, CancellationToken.None).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Legacy: LoadOrderDetails — queries order detail rows for a given OrderId,
    /// joining with MenuItems to get ItemName. Computes Subtotal = Quantity * UnitPrice.
    /// Also populates state.OrderDetails for convenience.
    /// </summary>
    public async Task<List<TOrderDetailInfoDto>> LoadOrderDetailsAsync(OrdersStateDto state, int AOrderId, CancellationToken ct = default)
    {
        await using var db = await _dbFactory.CreateDbContextAsync(ct);

        var entities = await db.OrderDetails
            .AsNoTracking()
            .Include(d => d.MenuItem)
            .Where(d => d.OrderId == AOrderId)
            .ToListAsync(ct);

        var result = new List<TOrderDetailInfoDto>(entities.Count);
        foreach (var d in entities)
        {
            result.Add(new TOrderDetailInfoDto
            {
                DetailId = d.DetailId,
                OrderId = d.OrderId,
                ItemId = (d.ItemId) ?? 0,
                ItemName = d.MenuItem != null ? d.MenuItem.ItemName ?? string.Empty : string.Empty,
                Quantity = (d.Quantity) ?? 0,
                UnitPrice = (d.UnitPrice) ?? 0m,
                Subtotal = (d.Quantity * d.UnitPrice) ?? 0m
            });
        }

        state.OrderDetails = result;

        return result;
    }

    /// <summary>
    /// Legacy: LoadOrderDetails — synchronous wrapper that delegates to LoadOrderDetailsAsync.
    /// Provided for legacy compatibility where the original Delphi code called LoadOrderDetails directly.
    /// </summary>
    public List<TOrderDetailInfoDto> LoadOrderDetails(OrdersStateDto state, int AOrderId)
    {
        return LoadOrderDetailsAsync(state, AOrderId, CancellationToken.None).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Legacy: FormCreate — initializes the state and loads initial order list.
    /// Grid configuration is handled by the Blazor page; this method just ensures
    /// the state is properly initialized and data is ready.
    /// </summary>
    public async Task FormCreateAsync(OrdersStateDto state, object Sender, CancellationToken ct = default)
    {
        state.FIsAdding = false;
        state.FSelectedId = 0;
        state.EdtTableIdText = string.Empty;
        state.EdtTotalAmountText = string.Empty;
        state.CmbEditStatusIndex = 0;
        state.PnlEditVisible = false;

        await LoadOrdersAsync(state, ct);
    }

    /// <summary>
    /// Legacy: btnEditClick — reads the selected order by ID from DB and populates
    /// the state DTO with edit fields. Returns the updated state.
    /// Matches legacy logic: sets FIsAdding=false, populates edtTableId, cmbEditStatus,
    /// edtTotalAmount from the selected row, and makes edit panel visible.
    /// </summary>
    public async Task<OrdersStateDto> BtnEditClickAsync(OrdersStateDto state, object Sender, CancellationToken ct = default)
    {
        if (state.FSelectedId <= 0)
        {
            return state;
        }

        await using var db = await _dbFactory.CreateDbContextAsync(ct);

        var order = await db.Orders
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.OrderId == state.FSelectedId, ct);

        if (order == null)
        {
            _logger.LogWarning("Order with ID {OrderId} not found for editing.", state.FSelectedId);
            return state;
        }

        state.FIsAdding = false;
        state.EdtTableIdText = order.TableId.ToString(CultureInfo.InvariantCulture);
        state.CmbEditStatusIndex = (int)order.Status;
        state.EdtTotalAmountText = (order.TotalAmount ?? default).ToString(CultureInfo.InvariantCulture);
        state.PnlEditVisible = true;

        return state;
    }

    /// <summary>
    /// Legacy: btnDeleteClick — deletes an order and its related order details from DB.
    /// The legacy code removes order details first, then the order itself.
    /// The caller (page) handles the confirmation dialog before calling this method.
    /// </summary>
    public async Task BtnDeleteClickAsync(OrdersStateDto state, object Sender, CancellationToken ct = default)
    {
        if (state.FSelectedId <= 0)
        {
            return;
        }

        await using var db = await _dbFactory.CreateDbContextAsync(ct);

        var relatedDetails = await db.OrderDetails
            .Where(d => d.OrderId == state.FSelectedId)
            .ToListAsync(ct);

        if (relatedDetails.Count > 0)
        {
            db.OrderDetails.RemoveRange(relatedDetails);
        }

        var order = await db.Orders
            .FirstOrDefaultAsync(o => o.OrderId == state.FSelectedId, ct);

        if (order != null)
        {
            db.Orders.Remove(order);
        }

        await db.SaveChangesAsync(ct);

        _logger.LogInformation("Deleted order {OrderId} with {DetailCount} detail(s).",
            state.FSelectedId, relatedDetails.Count);
    }

    /// <summary>
    /// Legacy: btnSaveClick — adds a new order or updates an existing one based on FIsAdding.
    /// Parses TableId and TotalAmount from string fields (matching legacy StrToIntDef/StrToFloatDef).
    /// For new orders, sets OrderDate to UTC now. For updates, also sets OrderDate to UTC now
    /// (matching legacy behavior which used Now for both add and edit).
    /// </summary>
    public async Task BtnSaveClickAsync(OrdersStateDto state, object Sender, CancellationToken ct = default)
    {
        await using var db = await _dbFactory.CreateDbContextAsync(ct);

        int tableId = 0;
        if (!string.IsNullOrWhiteSpace(state.EdtTableIdText))
        {
            int.TryParse(state.EdtTableIdText, NumberStyles.Integer, CultureInfo.InvariantCulture, out tableId);
        }

        decimal totalAmount = 0m;
        if (!string.IsNullOrWhiteSpace(state.EdtTotalAmountText))
        {
            decimal.TryParse(state.EdtTotalAmountText, NumberStyles.Number, CultureInfo.InvariantCulture, out totalAmount);
        }

        var status = (TOrderStatus)state.CmbEditStatusIndex;

        if (state.FIsAdding)
        {
            var newOrder = new global::MyRestaurant.Domain.Entities.Core.Order
            {
                TableId = tableId,
                OrderDate = DateTime.UtcNow,
                Status = status,
                TotalAmount = totalAmount
            };

            db.Orders.Add(newOrder);
            await db.SaveChangesAsync(ct);

            _logger.LogInformation("Added new order {OrderId} for table {TableId}.", newOrder.OrderId, tableId);
        }
        else
        {
            var existingOrder = await db.Orders
                .FirstOrDefaultAsync(o => o.OrderId == state.FSelectedId, ct);

            if (existingOrder == null)
            {
                _logger.LogWarning("Order {OrderId} not found for update.", state.FSelectedId);
                return;
            }

            existingOrder.TableId = tableId;
            existingOrder.Status = status;
            existingOrder.TotalAmount = totalAmount;
            existingOrder.OrderDate = DateTime.UtcNow;

            await db.SaveChangesAsync(ct);

            _logger.LogInformation("Updated order {OrderId}.", state.FSelectedId);
        }

        state.PnlEditVisible = false;
    }

    /// <summary>
    /// Maps TOrderStatus enum to display text, matching legacy ToString behavior.
    /// Legacy values: Pending, Preparing, Served, Paid, Cancelled
    /// </summary>
    private static string MapStatusToText(TOrderStatus status)
    {
        return status switch
        {
            TOrderStatus.Pending => "Pending",
            TOrderStatus.Preparing => "Preparing",
            TOrderStatus.Served => "Served",
            TOrderStatus.Paid => "Paid",
            TOrderStatus.Cancelled => "Cancelled",
            _ => "Pending"
        };
    }

    /// <summary>
    /// Shared helper to map order entities to DTOs.
    /// </summary>
    private static List<TOrderInfoDto> MapOrderEntitiesToDtos(List<global::MyRestaurant.Domain.Entities.Core.Order> entities)
    {
        var result = new List<TOrderInfoDto>(entities.Count);
        foreach (var o in entities)
        {
            result.Add(new TOrderInfoDto
            {
                OrderId = o.OrderId,
                TableId = o.TableId,
                OrderDate = (o.OrderDate) ?? default,
                Status = (TOrderStatus)((int)o.Status),
                StatusText = MapStatusToText(o.Status),
                TotalAmount = (o.TotalAmount) ?? 0m
            });
        }
        return result;
    }

    public async Task<List<TOrderInfo>> GetOrdersAsync(CancellationToken ct = default)
    {
        return await LoadOrdersAsync(new OrdersStateDto(), ct);
    }

    public async Task<List<TOrderDetailInfo>> GetOrderDetailsAsync(int orderId, CancellationToken ct = default)
    {
        return await LoadOrderDetailsAsync(new OrdersStateDto(), orderId, ct);
    }

    public async Task DeleteOrderAsync(int orderId, CancellationToken ct = default)
    {
        await BtnDeleteClickAsync(new OrdersStateDto(), orderId, ct);
    }

    public async Task AddOrderAsync(TOrderInfo order, CancellationToken ct = default)
    {
        // Legacy btnSaveClick with FIsAdding = True.
        await BtnSaveClickAsync(StateFromOrder(order, isAdding: true), null, ct);
    }

    public async Task UpdateOrderAsync(TOrderInfo order, CancellationToken ct = default)
    {
        // Legacy btnSaveClick with FIsAdding = False: Order.OrderId := FSelectedId.
        await BtnSaveClickAsync(StateFromOrder(order, isAdding: false), null, ct);
    }

    /// <summary>
    /// Projects an order onto the edit-panel state that BtnSaveClickAsync reads,
    /// mirroring how the legacy form filled edtTableId/cmbEditStatus/edtTotalAmount
    /// before calling btnSaveClick.
    /// </summary>
    private static OrdersStateDto StateFromOrder(TOrderInfo order, bool isAdding)
    {
        order ??= new TOrderInfo();

        return new OrdersStateDto
        {
            FIsAdding = isAdding,
            FSelectedId = isAdding ? 0 : order.OrderId,
            EdtTableIdText = order.TableId.ToString(CultureInfo.InvariantCulture),
            EdtTotalAmountText = order.TotalAmount.ToString(CultureInfo.InvariantCulture),
            // Legacy: TOrderStatus(cmbEditStatus.ItemIndex) -- the index is the ordinal.
            CmbEditStatusIndex = (int)order.Status
        };
    }
}
