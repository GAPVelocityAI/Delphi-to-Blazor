#nullable disable
using System;
using System.Collections.Generic;

namespace MyRestaurant.Domain.Entities.Core;

/// <summary>Port of <c>TMenuItemInfo (stored in a class-level array — the unit's table)</c>. Deterministically generated from the plan's schema.</summary>
public class MenuItem
{
    public int ItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal? Price { get; set; }
    public decimal? Cost { get; set; }
    public bool? Active { get; set; }
}
