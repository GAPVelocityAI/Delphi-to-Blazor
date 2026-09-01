#nullable disable
using System;
using System.Collections.Generic;

namespace MyFinanceAssets.Core.Application.DTOs.Core;

/// <summary>TAssetCategoryRecord data transfer object (domain: Core).</summary>
public class TAssetCategoryRecord
{
    public string Category { get; set; } = string.Empty;
    public decimal TotalOriginal { get; set; }
    public decimal TotalCurrent { get; set; }
    public int AssetCount { get; set; }
}
