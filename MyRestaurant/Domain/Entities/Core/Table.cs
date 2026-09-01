#nullable disable
using System;
using System.Collections.Generic;

namespace MyRestaurant.Domain.Entities.Core;

/// <summary>Port of <c>TTableInfo (stored in a class-level array — the unit's table)</c>. Deterministically generated from the plan's schema.</summary>
public class Table
{
    public int TableId { get; set; }
    public int? TableNumber { get; set; }
    public int? Capacity { get; set; }
    public TTableStatus Status { get; set; }
    public string Zone { get; set; } = string.Empty;

    public virtual ICollection<global::MyRestaurant.Domain.Entities.Core.Order> Orders { get; set; } = new List<global::MyRestaurant.Domain.Entities.Core.Order>();
}
