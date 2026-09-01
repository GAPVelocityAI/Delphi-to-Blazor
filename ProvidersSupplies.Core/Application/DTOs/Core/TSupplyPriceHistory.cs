#nullable disable
using System;
using System.Collections.Generic;

namespace ProvidersSupplies.Core.Application.DTOs.Core;

/// <summary>TSupplyPriceHistory data transfer object (domain: Core).</summary>
public class TSupplyPriceHistory
{
    public string ItemName { get; set; } = string.Empty;
    public string ProviderName { get; set; } = string.Empty;
    public decimal OldPrice { get; set; }
    public decimal NewPrice { get; set; }
    public DateTime ChangeDate { get; set; }
    public double ChangePct { get; set; }
}
