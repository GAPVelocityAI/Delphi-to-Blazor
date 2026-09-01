using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyRestaurant.Infrastructure.Data;

namespace MyRestaurant.Application.Services.MyRestaurant;

public class uRestaurantBLService : IuRestaurantBLService
{
    private readonly IDbContextFactory<MyRestaurantDbContext> _dbFactory;
    private readonly ILogger<uRestaurantBLService> _logger;

    public uRestaurantBLService(
        IDbContextFactory<MyRestaurantDbContext> dbFactory,
        ILogger<uRestaurantBLService> logger)
    {
        _dbFactory = dbFactory;
        _logger = logger;
    }

    /// <summary>
    /// Legacy EnsureInitialized seeded data into class-level arrays.
    /// In the migrated app, seed data is handled by DataSeeder at startup.
    /// This method verifies the DB has data and logs a warning if empty.
    /// </summary>
    /// <summary>
    /// Ports the seed data from legacy uRestaurantBL.EnsureInitialized. The generated version
    /// only checked for rows and warned that a DataSeeder should have run — but the DataSeeder
    /// it referred to was written outside the solution and never invoked, so every restaurant
    /// grid rendered empty. Seeding here matches how uTablesBLService already works, and each
    /// block is guarded so it is safe on every call.
    /// </summary>
    public async Task EnsureInitializedAsync(CancellationToken ct = default)
    {
        await using var db = await _dbFactory.CreateDbContextAsync(ct);
        var seeded = false;

        if (!await db.MenuItems.AnyAsync(ct))
        {
            _logger.LogInformation("Seeding MenuItems ({Count} rows).", 12);
            db.MenuItems.AddRange(
                new global::MyRestaurant.Domain.Entities.Core.MenuItem { ItemId = 1, ItemName = "Caesar Salad", Category = "Appetizer", Price = 12.50m, Cost = 3.75m, Active = true },
                new global::MyRestaurant.Domain.Entities.Core.MenuItem { ItemId = 2, ItemName = "Bruschetta", Category = "Appetizer", Price = 10.00m, Cost = 2.80m, Active = true },
                new global::MyRestaurant.Domain.Entities.Core.MenuItem { ItemId = 3, ItemName = "Soup of the Day", Category = "Appetizer", Price = 8.00m, Cost = 2.10m, Active = true },
                new global::MyRestaurant.Domain.Entities.Core.MenuItem { ItemId = 4, ItemName = "Grilled Salmon", Category = "Main Course", Price = 24.00m, Cost = 8.50m, Active = true },
                new global::MyRestaurant.Domain.Entities.Core.MenuItem { ItemId = 5, ItemName = "Margherita Pizza", Category = "Main Course", Price = 16.00m, Cost = 4.20m, Active = true },
                new global::MyRestaurant.Domain.Entities.Core.MenuItem { ItemId = 6, ItemName = "Ribeye Steak", Category = "Main Course", Price = 32.00m, Cost = 12.50m, Active = true },
                new global::MyRestaurant.Domain.Entities.Core.MenuItem { ItemId = 7, ItemName = "Chicken Alfredo", Category = "Main Course", Price = 18.50m, Cost = 5.60m, Active = true },
                new global::MyRestaurant.Domain.Entities.Core.MenuItem { ItemId = 8, ItemName = "Tiramisu", Category = "Dessert", Price = 9.00m, Cost = 2.80m, Active = true },
                new global::MyRestaurant.Domain.Entities.Core.MenuItem { ItemId = 9, ItemName = "Chocolate Lava Cake", Category = "Dessert", Price = 11.00m, Cost = 3.20m, Active = true },
                new global::MyRestaurant.Domain.Entities.Core.MenuItem { ItemId = 10, ItemName = "Craft Beer", Category = "Beverage", Price = 7.50m, Cost = 2.00m, Active = true },
                new global::MyRestaurant.Domain.Entities.Core.MenuItem { ItemId = 11, ItemName = "House Wine", Category = "Beverage", Price = 9.50m, Cost = 3.00m, Active = true },
                new global::MyRestaurant.Domain.Entities.Core.MenuItem { ItemId = 12, ItemName = "Fresh Lemonade", Category = "Beverage", Price = 5.00m, Cost = 1.20m, Active = true });
            seeded = true;
        }

        if (!await db.Orders.AnyAsync(ct))
        {
            _logger.LogInformation("Seeding Orders ({Count} rows).", 8);
            db.Orders.AddRange(
                new global::MyRestaurant.Domain.Entities.Core.Order { OrderId = 1001, TableId = 3, OrderDate = new DateTime(2026, 8, 5, 12, 30, 0, DateTimeKind.Utc), Status = TOrderStatus.Pending, TotalAmount = 45.50m },
                new global::MyRestaurant.Domain.Entities.Core.Order { OrderId = 1002, TableId = 7, OrderDate = new DateTime(2026, 8, 5, 12, 45, 0, DateTimeKind.Utc), Status = TOrderStatus.Preparing, TotalAmount = 68.00m },
                new global::MyRestaurant.Domain.Entities.Core.Order { OrderId = 1003, TableId = 1, OrderDate = new DateTime(2026, 8, 5, 11, 15, 0, DateTimeKind.Utc), Status = TOrderStatus.Served, TotalAmount = 92.50m },
                new global::MyRestaurant.Domain.Entities.Core.Order { OrderId = 1004, TableId = 12, OrderDate = new DateTime(2026, 8, 4, 19, 0, 0, DateTimeKind.Utc), Status = TOrderStatus.Paid, TotalAmount = 124.00m },
                new global::MyRestaurant.Domain.Entities.Core.Order { OrderId = 1005, TableId = 5, OrderDate = new DateTime(2026, 8, 4, 20, 30, 0, DateTimeKind.Utc), Status = TOrderStatus.Paid, TotalAmount = 56.00m },
                new global::MyRestaurant.Domain.Entities.Core.Order { OrderId = 1006, TableId = 9, OrderDate = new DateTime(2026, 8, 5, 13, 0, 0, DateTimeKind.Utc), Status = TOrderStatus.Preparing, TotalAmount = 37.50m },
                new global::MyRestaurant.Domain.Entities.Core.Order { OrderId = 1007, TableId = 2, OrderDate = new DateTime(2026, 8, 4, 18, 45, 0, DateTimeKind.Utc), Status = TOrderStatus.Cancelled, TotalAmount = 28.00m },
                new global::MyRestaurant.Domain.Entities.Core.Order { OrderId = 1008, TableId = 15, OrderDate = new DateTime(2026, 8, 5, 13, 15, 0, DateTimeKind.Utc), Status = TOrderStatus.Pending, TotalAmount = 82.00m });
            seeded = true;
        }

        if (!await db.OrderDetails.AnyAsync(ct))
        {
            _logger.LogInformation("Seeding OrderDetails ({Count} rows).", 23);
            db.OrderDetails.AddRange(
                new global::MyRestaurant.Domain.Entities.Core.OrderDetail { DetailId = 1, OrderId = 1001, ItemId = 1, ItemName = "Caesar Salad", Quantity = 1, UnitPrice = 12.50m, Subtotal = 12.50m },
                new global::MyRestaurant.Domain.Entities.Core.OrderDetail { DetailId = 2, OrderId = 1001, ItemId = 5, ItemName = "Margherita Pizza", Quantity = 1, UnitPrice = 16.00m, Subtotal = 16.00m },
                new global::MyRestaurant.Domain.Entities.Core.OrderDetail { DetailId = 3, OrderId = 1001, ItemId = 10, ItemName = "Craft Beer", Quantity = 2, UnitPrice = 7.50m, Subtotal = 15.00m },
                new global::MyRestaurant.Domain.Entities.Core.OrderDetail { DetailId = 4, OrderId = 1002, ItemId = 4, ItemName = "Grilled Salmon", Quantity = 2, UnitPrice = 24.00m, Subtotal = 48.00m },
                new global::MyRestaurant.Domain.Entities.Core.OrderDetail { DetailId = 5, OrderId = 1002, ItemId = 11, ItemName = "House Wine", Quantity = 2, UnitPrice = 9.50m, Subtotal = 19.00m },
                new global::MyRestaurant.Domain.Entities.Core.OrderDetail { DetailId = 6, OrderId = 1003, ItemId = 2, ItemName = "Bruschetta", Quantity = 1, UnitPrice = 10.00m, Subtotal = 10.00m },
                new global::MyRestaurant.Domain.Entities.Core.OrderDetail { DetailId = 7, OrderId = 1003, ItemId = 6, ItemName = "Ribeye Steak", Quantity = 2, UnitPrice = 32.00m, Subtotal = 64.00m },
                new global::MyRestaurant.Domain.Entities.Core.OrderDetail { DetailId = 8, OrderId = 1003, ItemId = 8, ItemName = "Tiramisu", Quantity = 1, UnitPrice = 9.00m, Subtotal = 9.00m },
                new global::MyRestaurant.Domain.Entities.Core.OrderDetail { DetailId = 9, OrderId = 1003, ItemId = 10, ItemName = "Craft Beer", Quantity = 1, UnitPrice = 7.50m, Subtotal = 7.50m },
                new global::MyRestaurant.Domain.Entities.Core.OrderDetail { DetailId = 10, OrderId = 1004, ItemId = 3, ItemName = "Soup of the Day", Quantity = 2, UnitPrice = 8.00m, Subtotal = 16.00m },
                new global::MyRestaurant.Domain.Entities.Core.OrderDetail { DetailId = 11, OrderId = 1004, ItemId = 7, ItemName = "Chicken Alfredo", Quantity = 3, UnitPrice = 18.50m, Subtotal = 55.50m },
                new global::MyRestaurant.Domain.Entities.Core.OrderDetail { DetailId = 12, OrderId = 1004, ItemId = 9, ItemName = "Chocolate Lava Cake", Quantity = 3, UnitPrice = 11.00m, Subtotal = 33.00m },
                new global::MyRestaurant.Domain.Entities.Core.OrderDetail { DetailId = 13, OrderId = 1005, ItemId = 5, ItemName = "Margherita Pizza", Quantity = 2, UnitPrice = 16.00m, Subtotal = 32.00m },
                new global::MyRestaurant.Domain.Entities.Core.OrderDetail { DetailId = 14, OrderId = 1005, ItemId = 12, ItemName = "Fresh Lemonade", Quantity = 4, UnitPrice = 5.00m, Subtotal = 20.00m },
                new global::MyRestaurant.Domain.Entities.Core.OrderDetail { DetailId = 15, OrderId = 1006, ItemId = 1, ItemName = "Caesar Salad", Quantity = 1, UnitPrice = 12.50m, Subtotal = 12.50m },
                new global::MyRestaurant.Domain.Entities.Core.OrderDetail { DetailId = 16, OrderId = 1006, ItemId = 7, ItemName = "Chicken Alfredo", Quantity = 1, UnitPrice = 18.50m, Subtotal = 18.50m },
                new global::MyRestaurant.Domain.Entities.Core.OrderDetail { DetailId = 17, OrderId = 1006, ItemId = 10, ItemName = "Craft Beer", Quantity = 1, UnitPrice = 7.50m, Subtotal = 7.50m },
                new global::MyRestaurant.Domain.Entities.Core.OrderDetail { DetailId = 18, OrderId = 1007, ItemId = 4, ItemName = "Grilled Salmon", Quantity = 1, UnitPrice = 24.00m, Subtotal = 24.00m },
                new global::MyRestaurant.Domain.Entities.Core.OrderDetail { DetailId = 19, OrderId = 1007, ItemId = 12, ItemName = "Fresh Lemonade", Quantity = 1, UnitPrice = 5.00m, Subtotal = 5.00m },
                new global::MyRestaurant.Domain.Entities.Core.OrderDetail { DetailId = 20, OrderId = 1008, ItemId = 2, ItemName = "Bruschetta", Quantity = 2, UnitPrice = 10.00m, Subtotal = 20.00m },
                new global::MyRestaurant.Domain.Entities.Core.OrderDetail { DetailId = 21, OrderId = 1008, ItemId = 6, ItemName = "Ribeye Steak", Quantity = 1, UnitPrice = 32.00m, Subtotal = 32.00m },
                new global::MyRestaurant.Domain.Entities.Core.OrderDetail { DetailId = 22, OrderId = 1008, ItemId = 8, ItemName = "Tiramisu", Quantity = 2, UnitPrice = 9.00m, Subtotal = 18.00m },
                new global::MyRestaurant.Domain.Entities.Core.OrderDetail { DetailId = 23, OrderId = 1008, ItemId = 11, ItemName = "House Wine", Quantity = 1, UnitPrice = 9.50m, Subtotal = 9.50m });
            seeded = true;
        }

        if (!await db.Bills.AnyAsync(ct))
        {
            _logger.LogInformation("Seeding Bills ({Count} rows).", 6);
            db.Bills.AddRange(
                new global::MyRestaurant.Domain.Entities.Core.Bill { BillId = 501, OrderId = 1003, Subtotal = 92.50m, Tax = 7.40m, Tip = 15.00m, Total = 114.90m, PaymentMethod = TPaymentMethod.CreditCard, PaidDate = new DateTime(2026, 8, 5, 13, 20, 0, DateTimeKind.Utc) },
                new global::MyRestaurant.Domain.Entities.Core.Bill { BillId = 502, OrderId = 1004, Subtotal = 124.00m, Tax = 9.92m, Tip = 20.00m, Total = 153.92m, PaymentMethod = TPaymentMethod.CreditCard, PaidDate = new DateTime(2026, 8, 4, 20, 45, 0, DateTimeKind.Utc) },
                new global::MyRestaurant.Domain.Entities.Core.Bill { BillId = 503, OrderId = 1005, Subtotal = 56.00m, Tax = 4.48m, Tip = 8.00m, Total = 68.48m, PaymentMethod = TPaymentMethod.Cash, PaidDate = new DateTime(2026, 8, 4, 21, 30, 0, DateTimeKind.Utc) },
                new global::MyRestaurant.Domain.Entities.Core.Bill { BillId = 504, OrderId = 1000, Subtotal = 78.50m, Tax = 6.28m, Tip = 12.00m, Total = 96.78m, PaymentMethod = TPaymentMethod.DebitCard, PaidDate = new DateTime(2026, 8, 3, 14, 10, 0, DateTimeKind.Utc) },
                new global::MyRestaurant.Domain.Entities.Core.Bill { BillId = 505, OrderId = 999, Subtotal = 42.00m, Tax = 3.36m, Tip = 6.00m, Total = 51.36m, PaymentMethod = TPaymentMethod.Cash, PaidDate = new DateTime(2026, 8, 3, 13, 0, 0, DateTimeKind.Utc) },
                new global::MyRestaurant.Domain.Entities.Core.Bill { BillId = 506, OrderId = 998, Subtotal = 155.00m, Tax = 12.40m, Tip = 25.00m, Total = 192.40m, PaymentMethod = TPaymentMethod.CreditCard, PaidDate = new DateTime(2026, 8, 2, 21, 15, 0, DateTimeKind.Utc) });
            seeded = true;
        }

        if (!await db.FoodCosts.AnyAsync(ct))
        {
            _logger.LogInformation("Seeding FoodCosts ({Count} rows).", 10);
            db.FoodCosts.AddRange(
                new global::MyRestaurant.Domain.Entities.Core.FoodCost { RecipeId = 1, RecipeName = "Caesar Salad", IngredientCount = 6, TotalCost = 3.75m, SellingPrice = 12.50m, CostPercentage = 30.0d },
                new global::MyRestaurant.Domain.Entities.Core.FoodCost { RecipeId = 2, RecipeName = "Bruschetta", IngredientCount = 5, TotalCost = 2.80m, SellingPrice = 10.00m, CostPercentage = 28.0d },
                new global::MyRestaurant.Domain.Entities.Core.FoodCost { RecipeId = 3, RecipeName = "Grilled Salmon", IngredientCount = 7, TotalCost = 8.50m, SellingPrice = 24.00m, CostPercentage = 35.4d },
                new global::MyRestaurant.Domain.Entities.Core.FoodCost { RecipeId = 4, RecipeName = "Margherita Pizza", IngredientCount = 5, TotalCost = 4.20m, SellingPrice = 16.00m, CostPercentage = 26.3d },
                new global::MyRestaurant.Domain.Entities.Core.FoodCost { RecipeId = 5, RecipeName = "Ribeye Steak", IngredientCount = 4, TotalCost = 12.50m, SellingPrice = 32.00m, CostPercentage = 39.1d },
                new global::MyRestaurant.Domain.Entities.Core.FoodCost { RecipeId = 6, RecipeName = "Chicken Alfredo", IngredientCount = 8, TotalCost = 5.60m, SellingPrice = 18.50m, CostPercentage = 30.3d },
                new global::MyRestaurant.Domain.Entities.Core.FoodCost { RecipeId = 7, RecipeName = "Tiramisu", IngredientCount = 7, TotalCost = 2.80m, SellingPrice = 9.00m, CostPercentage = 31.1d },
                new global::MyRestaurant.Domain.Entities.Core.FoodCost { RecipeId = 8, RecipeName = "Chocolate Lava Cake", IngredientCount = 6, TotalCost = 3.20m, SellingPrice = 11.00m, CostPercentage = 29.1d },
                new global::MyRestaurant.Domain.Entities.Core.FoodCost { RecipeId = 9, RecipeName = "Soup of the Day", IngredientCount = 9, TotalCost = 2.10m, SellingPrice = 8.00m, CostPercentage = 26.3d },
                new global::MyRestaurant.Domain.Entities.Core.FoodCost { RecipeId = 10, RecipeName = "Craft Beer", IngredientCount = 1, TotalCost = 2.00m, SellingPrice = 7.50m, CostPercentage = 26.7d });
            seeded = true;
        }

        if (seeded)
        {
            await db.SaveChangesAsync(ct);
        }
    }

    public async Task<List<TMenuItemInfo>> GetMenuItemsAsync(CancellationToken ct = default)
    {
        await EnsureInitializedAsync(ct);

        await using var db = await _dbFactory.CreateDbContextAsync(ct);

        // Legacy: Result := Copy(FMenuItems);
        // Ordered by Category, ItemName matching SQL_GET_MENU_ITEMS
        var entities = await db.MenuItems
            .AsNoTracking()
            .OrderBy(m => m.Category)
            .ThenBy(m => m.ItemName)
            .ToListAsync(ct);

        return entities.Select(e => new TMenuItemInfo
        {
            ItemId = e.ItemId,
            ItemName = e.ItemName ?? string.Empty,
            Category = e.Category ?? string.Empty,
            Price = (e.Price) ?? 0m,
            Cost = (e.Cost) ?? 0m,
            Active = (e.Active) ?? false
        }).ToList();
    }

    public async Task<List<TOrderInfo>> GetOrdersAsync(CancellationToken ct = default)
    {
        await EnsureInitializedAsync(ct);

        await using var db = await _dbFactory.CreateDbContextAsync(ct);

        // Legacy: Result := Copy(FOrders);
        // Ordered by OrderDate DESC matching SQL_GET_ORDERS
        var entities = await db.Orders
            .AsNoTracking()
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync(ct);

        return entities.Select(e => new TOrderInfo
        {
            OrderId = e.OrderId,
            TableId = e.TableId,
            OrderDate = (e.OrderDate) ?? default,
            Status = (TOrderStatus)(e.Status),
            TotalAmount = (e.TotalAmount) ?? 0m
        }).ToList();
    }

    public async Task<List<TOrderDetailInfo>> GetOrderDetailsAsync(int orderId, CancellationToken ct = default)
    {
        await EnsureInitializedAsync(ct);

        await using var db = await _dbFactory.CreateDbContextAsync(ct);

        // Legacy: filter FOrderDetails where OrderId = AOrderId
        // SQL_GET_ORDER_DETAILS joins OrderDetails with MenuItems to get ItemName
        var entities = await db.OrderDetails
            .AsNoTracking()
            .Include(d => d.MenuItem)
            .Where(d => d.OrderId == orderId)
            .ToListAsync(ct);

        return entities.Select(e => new TOrderDetailInfo
        {
            DetailId = e.DetailId,
            OrderId = e.OrderId,
            ItemId = (e.ItemId) ?? 0,
            ItemName = e.MenuItem != null ? (e.MenuItem.ItemName ?? string.Empty) : string.Empty,
            Quantity = (e.Quantity) ?? 0,
            UnitPrice = (e.UnitPrice) ?? 0m,
            Subtotal = (e.Quantity * e.UnitPrice) ?? 0m
        }).ToList();
    }

    public async Task<List<TBillInfo>> GetBillsAsync(CancellationToken ct = default)
    {
        await EnsureInitializedAsync(ct);

        await using var db = await _dbFactory.CreateDbContextAsync(ct);

        // Legacy: Result := Copy(FBills);
        // Ordered by PaidDate DESC matching SQL_GET_BILLS
        var entities = await db.Bills
            .AsNoTracking()
            .OrderByDescending(b => b.PaidDate)
            .ToListAsync(ct);

        return entities.Select(e => new TBillInfo
        {
            BillId = e.BillId,
            OrderId = e.OrderId,
            Subtotal = (e.Subtotal) ?? 0m,
            Tax = (e.Tax) ?? 0m,
            Tip = (e.Tip) ?? 0m,
            Total = (e.Total) ?? 0m,
            PaymentMethod = (TPaymentMethod)(e.PaymentMethod),
            PaidDate = (e.PaidDate) ?? default
        }).ToList();
    }

    public async Task<List<TFoodCostInfo>> GetFoodCostsAsync(CancellationToken ct = default)
    {
        await EnsureInitializedAsync(ct);

        await using var db = await _dbFactory.CreateDbContextAsync(ct);

        // Legacy: Result := Copy(FFoodCosts);
        var entities = await db.FoodCosts
            .AsNoTracking()
            .ToListAsync(ct);

        return entities.Select(e => new TFoodCostInfo
        {
            RecipeId = e.RecipeId,
            RecipeName = e.RecipeName ?? string.Empty,
            IngredientCount = (e.IngredientCount) ?? 0,
            TotalCost = (e.TotalCost) ?? 0m,
            SellingPrice = (e.SellingPrice) ?? 0m,
            CostPercentage = (decimal)((e.CostPercentage) ?? 0d)
        }).ToList();
    }

    public async Task AddMenuItemAsync(TMenuItemInfo item, CancellationToken ct = default)
    {
        // Legacy: assigned next ID (auto-increment handles this now),
        // then appended to array
        await using var db = await _dbFactory.CreateDbContextAsync(ct);

        var entity = new global::MyRestaurant.Domain.Entities.Core.MenuItem
        {
            ItemName = item.ItemName,
            Category = item.Category,
            Price = item.Price,
            Cost = item.Cost,
            Active = item.Active
        };

        db.MenuItems.Add(entity);
        await db.SaveChangesAsync(ct);

        // Reflect the DB-assigned ID back to the caller (legacy: AItem.ItemId := FNextMenuId)
        item.ItemId = entity.ItemId;
    }

    public async Task UpdateMenuItemAsync(TMenuItemInfo item, CancellationToken ct = default)
    {
        // Legacy: find by ItemId, replace entire record
        await using var db = await _dbFactory.CreateDbContextAsync(ct);

        var entity = await db.MenuItems.FindAsync(new object[] { item.ItemId }, ct);
        if (entity == null)
        {
            _logger.LogWarning("UpdateMenuItem: MenuItem with Id {ItemId} not found.", item.ItemId);
            return;
        }

        entity.ItemName = item.ItemName;
        entity.Category = item.Category;
        entity.Price = item.Price;
        entity.Cost = item.Cost;
        entity.Active = item.Active;

        await db.SaveChangesAsync(ct);
    }

    public async Task DeleteMenuItemAsync(int itemId, CancellationToken ct = default)
    {
        // Legacy: find by ItemId, remove from array
        await using var db = await _dbFactory.CreateDbContextAsync(ct);

        var entity = await db.MenuItems.FindAsync(new object[] { itemId }, ct);
        if (entity == null)
        {
            _logger.LogWarning("DeleteMenuItem: MenuItem with Id {ItemId} not found.", itemId);
            return;
        }

        db.MenuItems.Remove(entity);
        await db.SaveChangesAsync(ct);
    }

    public async Task AddOrderAsync(TOrderInfo order, CancellationToken ct = default)
    {
        // Legacy: assigned next ID (auto-increment handles this now),
        // then appended to array
        await using var db = await _dbFactory.CreateDbContextAsync(ct);

        var entity = new global::MyRestaurant.Domain.Entities.Core.Order
        {
            TableId = order.TableId,
            OrderDate = order.OrderDate,
            Status = (TOrderStatus)((int)order.Status),
            TotalAmount = order.TotalAmount
        };

        db.Orders.Add(entity);
        await db.SaveChangesAsync(ct);

        // Reflect the DB-assigned ID back to the caller (legacy: AOrder.OrderId := FNextOrderId)
        order.OrderId = entity.OrderId;
    }

    public async Task UpdateOrderAsync(TOrderInfo order, CancellationToken ct = default)
    {
        // Legacy: find by OrderId, replace entire record
        await using var db = await _dbFactory.CreateDbContextAsync(ct);

        var entity = await db.Orders.FindAsync(new object[] { order.OrderId }, ct);
        if (entity == null)
        {
            _logger.LogWarning("UpdateOrder: Order with Id {OrderId} not found.", order.OrderId);
            return;
        }

        entity.TableId = order.TableId;
        entity.OrderDate = order.OrderDate;
        entity.Status = (TOrderStatus)((int)order.Status);
        entity.TotalAmount = order.TotalAmount;

        await db.SaveChangesAsync(ct);
    }

    public async Task DeleteOrderAsync(int orderId, CancellationToken ct = default)
    {
        // Legacy: first remove all related order details, then remove the order
        await using var db = await _dbFactory.CreateDbContextAsync(ct);

        // Remove related order details first (legacy manually iterated and removed)
        // If DB has ON DELETE CASCADE this is redundant, but we replicate legacy behavior explicitly
        var relatedDetails = await db.OrderDetails
            .Where(d => d.OrderId == orderId)
            .ToListAsync(ct);

        if (relatedDetails.Count > 0)
        {
            db.OrderDetails.RemoveRange(relatedDetails);
        }

        // Remove the order itself
        var orderEntity = await db.Orders.FindAsync(new object[] { orderId }, ct);
        if (orderEntity != null)
        {
            db.Orders.Remove(orderEntity);
        }
        else
        {
            _logger.LogWarning("DeleteOrder: Order with Id {OrderId} not found.", orderId);
        }

        await db.SaveChangesAsync(ct);
    }

    public async Task AddBillAsync(TBillInfo bill, CancellationToken ct = default)
    {
        // Legacy: Tax = Subtotal * 0.08; Total = Subtotal + Tax + Tip; then append
        decimal tax = Math.Round(bill.Subtotal * 0.08m, 2, MidpointRounding.ToEven);
        decimal total = bill.Subtotal + tax + bill.Tip;

        await using var db = await _dbFactory.CreateDbContextAsync(ct);

        var entity = new global::MyRestaurant.Domain.Entities.Core.Bill
        {
            OrderId = bill.OrderId,
            Subtotal = bill.Subtotal,
            Tax = tax,
            Tip = bill.Tip,
            Total = total,
            PaymentMethod = (TPaymentMethod)((int)bill.PaymentMethod),
            PaidDate = bill.PaidDate
        };

        db.Bills.Add(entity);
        await db.SaveChangesAsync(ct);

        // Reflect computed values back to caller
        bill.BillId = entity.BillId;
        bill.Tax = tax;
        bill.Total = total;
    }

    public async Task UpdateBillAsync(TBillInfo bill, CancellationToken ct = default)
    {
        // Legacy: find by BillId, replace entire record
        await using var db = await _dbFactory.CreateDbContextAsync(ct);

        var entity = await db.Bills.FindAsync(new object[] { bill.BillId }, ct);
        if (entity == null)
        {
            _logger.LogWarning("UpdateBill: Bill with Id {BillId} not found.", bill.BillId);
            return;
        }

        entity.OrderId = bill.OrderId;
        entity.Subtotal = bill.Subtotal;
        entity.Tax = bill.Tax;
        entity.Tip = bill.Tip;
        entity.Total = bill.Total;
        entity.PaymentMethod = (TPaymentMethod)((int)bill.PaymentMethod);
        entity.PaidDate = bill.PaidDate;

        await db.SaveChangesAsync(ct);
    }

    public async Task DeleteBillAsync(int billId, CancellationToken ct = default)
    {
        // Legacy: find by BillId, remove from array
        await using var db = await _dbFactory.CreateDbContextAsync(ct);

        var entity = await db.Bills.FindAsync(new object[] { billId }, ct);
        if (entity == null)
        {
            _logger.LogWarning("DeleteBill: Bill with Id {BillId} not found.", billId);
            return;
        }

        db.Bills.Remove(entity);
        await db.SaveChangesAsync(ct);
    }

    public async Task AddFoodCostAsync(TFoodCostInfo cost, CancellationToken ct = default)
    {
        // Legacy: compute CostPercentage = (TotalCost / SellingPrice) * 100 if SellingPrice > 0
        decimal costPercentage = 0m;
        if (cost.SellingPrice > 0m)
        {
            costPercentage = Math.Round((cost.TotalCost / cost.SellingPrice) * 100m, 2, MidpointRounding.ToEven);
        }

        await using var db = await _dbFactory.CreateDbContextAsync(ct);

        var entity = new global::MyRestaurant.Domain.Entities.Core.FoodCost
        {
            RecipeName = cost.RecipeName,
            IngredientCount = cost.IngredientCount,
            TotalCost = cost.TotalCost,
            SellingPrice = cost.SellingPrice,
            CostPercentage = (double)costPercentage
        };

        db.FoodCosts.Add(entity);
        await db.SaveChangesAsync(ct);

        // Reflect DB-assigned ID and computed value back to caller
        cost.RecipeId = entity.RecipeId;
        cost.CostPercentage = (decimal)((double)costPercentage);
    }

    public async Task UpdateFoodCostAsync(TFoodCostInfo cost, CancellationToken ct = default)
    {
        // Legacy: find by RecipeId, replace entire record
        await using var db = await _dbFactory.CreateDbContextAsync(ct);

        var entity = await db.FoodCosts.FindAsync(new object[] { cost.RecipeId }, ct);
        if (entity == null)
        {
            _logger.LogWarning("UpdateFoodCost: FoodCost with Id {RecipeId} not found.", cost.RecipeId);
            return;
        }

        entity.RecipeName = cost.RecipeName;
        entity.IngredientCount = cost.IngredientCount;
        entity.TotalCost = cost.TotalCost;
        entity.SellingPrice = cost.SellingPrice;
        entity.CostPercentage = (double)(cost.CostPercentage);

        await db.SaveChangesAsync(ct);
    }

    public async Task DeleteFoodCostAsync(int recipeId, CancellationToken ct = default)
    {
        // Legacy: find by RecipeId, remove from array
        await using var db = await _dbFactory.CreateDbContextAsync(ct);

        var entity = await db.FoodCosts.FindAsync(new object[] { recipeId }, ct);
        if (entity == null)
        {
            _logger.LogWarning("DeleteFoodCost: FoodCost with Id {RecipeId} not found.", recipeId);
            return;
        }

        db.FoodCosts.Remove(entity);
        await db.SaveChangesAsync(ct);
    }

    // === Legacy-named methods delegating to Async counterparts ===

    public async Task EnsureInitialized(CancellationToken ct = default)
    {
        await EnsureInitializedAsync(ct);
    }

    public async Task<List<TMenuItemInfo>> GetMenuItems(CancellationToken ct = default)
    {
        return await GetMenuItemsAsync(ct);
    }

    public async Task<List<TOrderInfo>> GetOrders(CancellationToken ct = default)
    {
        return await GetOrdersAsync(ct);
    }

    public async Task<List<TOrderDetailInfo>> GetOrderDetails(int orderId, CancellationToken ct = default)
    {
        return await GetOrderDetailsAsync(orderId, ct);
    }

    public async Task<List<TBillInfo>> GetBills(CancellationToken ct = default)
    {
        return await GetBillsAsync(ct);
    }

    public async Task<List<TFoodCostInfo>> GetFoodCosts(CancellationToken ct = default)
    {
        return await GetFoodCostsAsync(ct);
    }

    public async Task AddMenuItem(TMenuItemInfo item, CancellationToken ct = default)
    {
        await AddMenuItemAsync(item, ct);
    }

    public async Task UpdateMenuItem(TMenuItemInfo item, CancellationToken ct = default)
    {
        await UpdateMenuItemAsync(item, ct);
    }

    public async Task DeleteMenuItem(int itemId, CancellationToken ct = default)
    {
        await DeleteMenuItemAsync(itemId, ct);
    }

    public async Task AddOrder(TOrderInfo order, CancellationToken ct = default)
    {
        await AddOrderAsync(order, ct);
    }

    public async Task UpdateOrder(TOrderInfo order, CancellationToken ct = default)
    {
        await UpdateOrderAsync(order, ct);
    }

    public async Task DeleteOrder(int orderId, CancellationToken ct = default)
    {
        await DeleteOrderAsync(orderId, ct);
    }

    public async Task AddBill(TBillInfo bill, CancellationToken ct = default)
    {
        await AddBillAsync(bill, ct);
    }

    public async Task UpdateBill(TBillInfo bill, CancellationToken ct = default)
    {
        await UpdateBillAsync(bill, ct);
    }

    public async Task DeleteBill(int billId, CancellationToken ct = default)
    {
        await DeleteBillAsync(billId, ct);
    }

    public async Task AddFoodCost(TFoodCostInfo cost, CancellationToken ct = default)
    {
        await AddFoodCostAsync(cost, ct);
    }

    public async Task UpdateFoodCost(TFoodCostInfo cost, CancellationToken ct = default)
    {
        await UpdateFoodCostAsync(cost, ct);
    }

    public async Task DeleteFoodCost(int recipeId, CancellationToken ct = default)
    {
        await DeleteFoodCostAsync(recipeId, ct);
    }
}
