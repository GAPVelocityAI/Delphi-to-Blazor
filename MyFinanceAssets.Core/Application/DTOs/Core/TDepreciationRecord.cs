#nullable disable
using System;
using System.Collections.Generic;

namespace MyFinanceAssets.Core.Application.DTOs.Core;

/// <summary>TDepreciationRecord data transfer object (domain: Core).</summary>
public class TDepreciationRecord
{
    public int AssetId { get; set; }
    public string AssetName { get; set; } = string.Empty;
    public decimal OriginalValue { get; set; }
    public decimal CurrentValue { get; set; }
    public decimal AnnualDepreciation { get; set; }
    public int YearsRemaining { get; set; }
}
