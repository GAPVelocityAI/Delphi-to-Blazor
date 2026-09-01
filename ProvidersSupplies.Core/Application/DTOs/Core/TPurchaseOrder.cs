#nullable disable
using System;
using System.Collections.Generic;

namespace ProvidersSupplies.Core.Application.DTOs.Core;

/// <summary>TPurchaseOrder data transfer object (domain: Core).</summary>
public class TPurchaseOrder
{
    public int POId { get; set; }
    public string ProviderName { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public DateTime ExpectedDelivery { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = string.Empty;
    public int ItemCount { get; set; }
}
