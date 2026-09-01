#nullable disable
using System;
using System.Collections.Generic;

namespace MyRestaurant.Domain.Entities.Core;

/// <summary>Port of <c>TOrderDetailInfo (stored in a class-level array — the unit's table)</c>. Deterministically generated from the plan's schema.</summary>
public class OrderDetail
{
    public virtual global::MyRestaurant.Domain.Entities.Core.MenuItem MenuItem { get; set; }
    public int DetailId { get; set; }
    public int OrderId { get; set; }
    public int? ItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public int? Quantity { get; set; }
    public decimal? UnitPrice { get; set; }
    public decimal? Subtotal { get; set; }

    public virtual global::MyRestaurant.Domain.Entities.Core.Order Order { get; set; }
}
