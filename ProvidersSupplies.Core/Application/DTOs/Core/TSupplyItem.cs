#nullable disable
using System;
using System.Collections.Generic;

namespace ProvidersSupplies.Core.Application.DTOs.Core;

/// <summary>TSupplyItem data transfer object (domain: Core).</summary>
public class TSupplyItem
{
    public int SupplyId { get; set; }
    public int ProviderId { get; set; }
    public string ProviderName { get; set; } = string.Empty;
    public string ItemName { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal UnitCost { get; set; }
    public int MinOrderQty { get; set; }
    public int LeadTimeDays { get; set; }
    public bool InStock { get; set; }
}
