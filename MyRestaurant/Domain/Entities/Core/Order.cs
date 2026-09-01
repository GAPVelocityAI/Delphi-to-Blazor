#nullable disable
using System;
using System.Collections.Generic;

namespace MyRestaurant.Domain.Entities.Core;

/// <summary>Port of <c>TOrderInfo (stored in a class-level array — the unit's table)</c>. Deterministically generated from the plan's schema.</summary>
public class Order
{
    public int OrderId { get; set; }
    public int TableId { get; set; }
    public DateTime? OrderDate { get; set; }
    public TOrderStatus Status { get; set; }
    public decimal? TotalAmount { get; set; }

    public virtual global::MyRestaurant.Domain.Entities.Core.Table Table { get; set; }
    public virtual ICollection<global::MyRestaurant.Domain.Entities.Core.Bill> Bills { get; set; } = new List<global::MyRestaurant.Domain.Entities.Core.Bill>();
    public virtual ICollection<global::MyRestaurant.Domain.Entities.Core.OrderDetail> OrderDetails { get; set; } = new List<global::MyRestaurant.Domain.Entities.Core.OrderDetail>();
}
