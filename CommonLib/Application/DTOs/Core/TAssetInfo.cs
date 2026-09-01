#nullable disable
using System;
using System.Collections.Generic;
namespace CommonLib.Application.DTOs.Core;

/// <summary>TAssetInfo data transfer object (domain: Core).</summary>
public class TAssetInfo
{
    public int AssetId { get; set; }
    public string AssetName { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public DateTime PurchaseDate { get; set; }
    public decimal Value { get; set; }
    public decimal DepreciatedValue { get; set; }
    public string Status { get; set; } = string.Empty;
}
