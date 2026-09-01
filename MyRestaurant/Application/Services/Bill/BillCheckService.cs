using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyRestaurant.Infrastructure.Data;

namespace MyRestaurant.Application.Services.Bill;

public class BillCheckService : IBillCheckService
{
    private readonly IDbContextFactory<MyRestaurantDbContext> _dbFactory;
    private readonly ILogger<BillCheckService> _logger;

    public BillCheckService(
        IDbContextFactory<MyRestaurantDbContext> dbFactory,
        ILogger<BillCheckService> logger)
    {
        _dbFactory = dbFactory;
        _logger = logger;
    }

    /// <summary>
    /// Loads all bills from the database ordered by PaidDate descending.
    /// Legacy: LoadBills — queries all bills, populates grid rows, computes TotalRevenue summary.
    /// </summary>
    public async Task<List<TBillInfoDto>> LoadBillsAsync(BillCheckStateDto state, CancellationToken ct = default)
    {
        await using var db = await _dbFactory.CreateDbContextAsync(ct);

        var bills = await db.Bills
            .AsNoTracking()
            .OrderByDescending(b => b.PaidDate)
            .ToListAsync(ct);

        var result = new List<TBillInfoDto>(bills.Count);
        foreach (var b in bills)
        {
            result.Add(MapToDto(b));
        }

        _logger.LogDebug("Loaded {Count} bills", result.Count);
        return result;
    }

    /// <summary>
    /// Legacy: btnEditClick — looks up the bill identified by state.FSelectedId,
    /// populates the state DTO edit fields from the database record,
    /// sets FIsAdding = false, and returns the updated state.
    /// </summary>
    public async Task<BillCheckStateDto> BtnEditClickAsync(BillCheckStateDto state, object? sender, CancellationToken ct = default)
    {
        if (state.FSelectedId <= 0)
        {
            _logger.LogWarning("BtnEditClick called with no selected bill (FSelectedId={Id})", state.FSelectedId);
            return state;
        }

        await using var db = await _dbFactory.CreateDbContextAsync(ct);

        var bill = await db.Bills
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.BillId == state.FSelectedId, ct);

        if (bill == null)
        {
            _logger.LogWarning("Bill with ID {Id} not found for editing", state.FSelectedId);
            return state;
        }

        state.FIsAdding = false;
        state.EdtOrderIdText = bill.OrderId.ToString(CultureInfo.InvariantCulture);
        state.EdtSubtotalText = (bill.Subtotal ?? default).ToString(CultureInfo.InvariantCulture);
        state.EdtTipText = (bill.Tip ?? default).ToString(CultureInfo.InvariantCulture);

        state.FRestaurantBL = bill.PaymentMethod.ToString();

        _logger.LogDebug("Populated edit fields for BillId={Id}", state.FSelectedId);
        return state;
    }

    /// <summary>
    /// Legacy: btnDeleteClick — deletes the bill identified by state.FSelectedId.
    /// The page is responsible for showing confirmation dialog before calling this.
    /// </summary>
    public async Task BtnDeleteClickAsync(BillCheckStateDto state, object? sender, CancellationToken ct = default)
    {
        if (state.FSelectedId <= 0)
        {
            _logger.LogWarning("BtnDeleteClick called with no selected bill (FSelectedId={Id})", state.FSelectedId);
            return;
        }

        await using var db = await _dbFactory.CreateDbContextAsync(ct);

        var bill = await db.Bills
            .FirstOrDefaultAsync(b => b.BillId == state.FSelectedId, ct);

        if (bill == null)
        {
            _logger.LogWarning("Bill with ID {Id} not found for deletion", state.FSelectedId);
            return;
        }

        db.Bills.Remove(bill);
        await db.SaveChangesAsync(ct);

        _logger.LogInformation("Deleted BillId={Id}", state.FSelectedId);
    }

    /// <summary>
    /// Legacy: btnSaveClick — creates a new bill or updates an existing one.
    /// Tax = Subtotal * 0.08, Total = Subtotal + Tax + Tip.
    /// PaidDate is set to DateTime.UtcNow.
    /// </summary>
    public async Task BtnSaveClickAsync(BillCheckStateDto state, object? sender, CancellationToken ct = default)
    {
        int orderId = 0;
        if (!string.IsNullOrWhiteSpace(state.EdtOrderIdText))
        {
            int.TryParse(state.EdtOrderIdText, NumberStyles.Integer, CultureInfo.InvariantCulture, out orderId);
        }

        decimal subtotal = 0m;
        if (!string.IsNullOrWhiteSpace(state.EdtSubtotalText))
        {
            decimal.TryParse(state.EdtSubtotalText, NumberStyles.Number, CultureInfo.InvariantCulture, out subtotal);
        }

        decimal tip = 0m;
        if (!string.IsNullOrWhiteSpace(state.EdtTipText))
        {
            decimal.TryParse(state.EdtTipText, NumberStyles.Number, CultureInfo.InvariantCulture, out tip);
        }

        decimal tax = Math.Round(subtotal * 0.08m, 2, MidpointRounding.ToEven);
        decimal total = subtotal + tax + tip;

        var paymentMethod = ParsePaymentMethod(state.FRestaurantBL ?? string.Empty);

        await using var db = await _dbFactory.CreateDbContextAsync(ct);

        if (state.FIsAdding)
        {
            var newBill = new global::MyRestaurant.Domain.Entities.Core.Bill
            {
                OrderId = orderId,
                Subtotal = subtotal,
                Tax = tax,
                Tip = tip,
                Total = total,
                PaymentMethod = paymentMethod,
                PaidDate = DateTime.UtcNow
            };

            db.Bills.Add(newBill);
            await db.SaveChangesAsync(ct);

            _logger.LogInformation("Added new Bill with OrderId={OrderId}, Total={Total}", orderId, total);
        }
        else
        {
            if (state.FSelectedId <= 0)
            {
                _logger.LogWarning("BtnSaveClick in edit mode but FSelectedId={Id}", state.FSelectedId);
                return;
            }

            var existing = await db.Bills
                .FirstOrDefaultAsync(b => b.BillId == state.FSelectedId, ct);

            if (existing == null)
            {
                _logger.LogWarning("Bill with ID {Id} not found for update", state.FSelectedId);
                return;
            }

            existing.OrderId = orderId;
            existing.Subtotal = subtotal;
            existing.Tax = tax;
            existing.Tip = tip;
            existing.Total = total;
            existing.PaymentMethod = paymentMethod;
            existing.PaidDate = DateTime.UtcNow;

            await db.SaveChangesAsync(ct);

            _logger.LogInformation("Updated BillId={Id}, Total={Total}", state.FSelectedId, total);
        }
    }

    private static TBillInfoDto MapToDto(global::MyRestaurant.Domain.Entities.Core.Bill entity)
    {
        return new TBillInfoDto
        {
            BillId = entity.BillId,
            OrderId = entity.OrderId,
            Subtotal = (entity.Subtotal) ?? 0m,
            Tax = (entity.Tax) ?? 0m,
            Tip = (entity.Tip) ?? 0m,
            Total = (entity.Total) ?? 0m,
            PaymentMethod = entity.PaymentMethod,
            PaidDate = (entity.PaidDate) ?? default
        };
    }

    private static TPaymentMethod ParsePaymentMethod(string text)
    {
        return text switch
        {
            "Cash" => TPaymentMethod.Cash,
            "Credit Card" => TPaymentMethod.CreditCard,
            "Debit Card" => TPaymentMethod.DebitCard,
            _ => TPaymentMethod.Cash
        };
    }
}
