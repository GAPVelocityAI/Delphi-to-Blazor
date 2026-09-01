using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MyRestaurant.Application.Services.Orders;

public interface IOrdersService
{
    // Page-facing name for the legacy-shaped operation below.
    Task<List<TOrderInfo>> GetOrdersAsync(CancellationToken ct = default);
    // Page-facing name for the legacy-shaped operation below.
    Task<List<TOrderDetailInfo>> GetOrderDetailsAsync(int orderId, CancellationToken ct = default);
    // Page-facing name for the legacy-shaped operation below.
    Task DeleteOrderAsync(int orderId, CancellationToken ct = default);
    // Page-facing name for the legacy-shaped operation below.
    Task AddOrderAsync(TOrderInfo order, CancellationToken ct = default);
    // Page-facing name for the legacy-shaped operation below.
    Task UpdateOrderAsync(TOrderInfo order, CancellationToken ct = default);

    Task<List<TOrderInfoDto>> LoadOrdersAsync(OrdersStateDto state, CancellationToken ct = default);

    Task<List<TOrderDetailInfoDto>> LoadOrderDetailsAsync(OrdersStateDto state, int AOrderId, CancellationToken ct = default);

    List<TOrderInfoDto> LoadOrders(OrdersStateDto state);

    List<TOrderDetailInfoDto> LoadOrderDetails(OrdersStateDto state, int AOrderId);

    Task FormCreateAsync(OrdersStateDto state, object Sender, CancellationToken ct = default);

    Task<OrdersStateDto> BtnEditClickAsync(OrdersStateDto state, object Sender, CancellationToken ct = default);

    Task BtnDeleteClickAsync(OrdersStateDto state, object Sender, CancellationToken ct = default);

    Task BtnSaveClickAsync(OrdersStateDto state, object Sender, CancellationToken ct = default);
}
