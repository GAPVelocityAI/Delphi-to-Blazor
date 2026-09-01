#nullable disable
using System;
using System.Collections.Generic;

namespace MyRestaurant.Domain.Entities.Core;

/// <summary>Port of <c>TBillInfo (stored in a class-level array — the unit's table)</c>. Deterministically generated from the plan's schema.</summary>
public class Bill
{
    public int BillId { get; set; }
    public int OrderId { get; set; }
    public decimal? Subtotal { get; set; }
    public decimal? Tax { get; set; }
    public decimal? Tip { get; set; }
    public decimal? Total { get; set; }
    public TPaymentMethod PaymentMethod { get; set; }
    public DateTime? PaidDate { get; set; }

    public virtual global::MyRestaurant.Domain.Entities.Core.Order Order { get; set; }
}
