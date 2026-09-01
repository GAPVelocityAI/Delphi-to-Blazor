using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MyRestaurant.Application.Services.MyRestaurant;

public interface IuRestaurantBLService
{
    Task EnsureInitializedAsync(CancellationToken ct = default);
    Task<List<TMenuItemInfo>> GetMenuItemsAsync(CancellationToken ct = default);
    Task<List<TOrderInfo>> GetOrdersAsync(CancellationToken ct = default);
    Task<List<TOrderDetailInfo>> GetOrderDetailsAsync(int orderId, CancellationToken ct = default);
    Task<List<TBillInfo>> GetBillsAsync(CancellationToken ct = default);
    Task<List<TFoodCostInfo>> GetFoodCostsAsync(CancellationToken ct = default);
    Task AddMenuItemAsync(TMenuItemInfo item, CancellationToken ct = default);
    Task UpdateMenuItemAsync(TMenuItemInfo item, CancellationToken ct = default);
    Task DeleteMenuItemAsync(int itemId, CancellationToken ct = default);
    Task AddOrderAsync(TOrderInfo order, CancellationToken ct = default);
    Task UpdateOrderAsync(TOrderInfo order, CancellationToken ct = default);
    Task DeleteOrderAsync(int orderId, CancellationToken ct = default);
    Task AddBillAsync(TBillInfo bill, CancellationToken ct = default);
    Task UpdateBillAsync(TBillInfo bill, CancellationToken ct = default);
    Task DeleteBillAsync(int billId, CancellationToken ct = default);
    Task AddFoodCostAsync(TFoodCostInfo cost, CancellationToken ct = default);
    Task UpdateFoodCostAsync(TFoodCostInfo cost, CancellationToken ct = default);
    Task DeleteFoodCostAsync(int recipeId, CancellationToken ct = default);

    // Legacy-named synchronous-style wrappers mapped to async implementations
    Task EnsureInitialized(CancellationToken ct = default);
    Task<List<TMenuItemInfo>> GetMenuItems(CancellationToken ct = default);
    Task<List<TOrderInfo>> GetOrders(CancellationToken ct = default);
    Task<List<TOrderDetailInfo>> GetOrderDetails(int orderId, CancellationToken ct = default);
    Task<List<TBillInfo>> GetBills(CancellationToken ct = default);
    Task<List<TFoodCostInfo>> GetFoodCosts(CancellationToken ct = default);
    Task AddMenuItem(TMenuItemInfo item, CancellationToken ct = default);
    Task UpdateMenuItem(TMenuItemInfo item, CancellationToken ct = default);
    Task DeleteMenuItem(int itemId, CancellationToken ct = default);
    Task AddOrder(TOrderInfo order, CancellationToken ct = default);
    Task UpdateOrder(TOrderInfo order, CancellationToken ct = default);
    Task DeleteOrder(int orderId, CancellationToken ct = default);
    Task AddBill(TBillInfo bill, CancellationToken ct = default);
    Task UpdateBill(TBillInfo bill, CancellationToken ct = default);
    Task DeleteBill(int billId, CancellationToken ct = default);
    Task AddFoodCost(TFoodCostInfo cost, CancellationToken ct = default);
    Task UpdateFoodCost(TFoodCostInfo cost, CancellationToken ct = default);
    Task DeleteFoodCost(int recipeId, CancellationToken ct = default);
}
