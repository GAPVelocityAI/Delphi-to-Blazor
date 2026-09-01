#nullable disable
using System;
using System.Collections.Generic;

namespace MyAdmin.Domain.Entities.Core;

/// <summary>Port of <c>TAssetInfo (stored in a class-level array — the unit's table)</c>. Deterministically generated from the plan's schema.</summary>
public class Asset
{
    public int AssetId { get; set; }
    public string AssetName { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public DateTime? PurchaseDate { get; set; }
    public decimal? Value { get; set; }
    public decimal? DepreciatedValue { get; set; }
    public string Status { get; set; } = string.Empty;
}
