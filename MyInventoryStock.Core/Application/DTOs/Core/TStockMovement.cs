#nullable disable
using System;
using System.Collections.Generic;

namespace MyInventoryStock.Core.Application.DTOs.Core;

/// <summary>TStockMovement data transfer object (domain: Core).</summary>
public class TStockMovement
{
    public int MovementId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public string MovementType { get; set; } = string.Empty;
    public double Quantity { get; set; }
    public DateTime MovementDate { get; set; }
    public string Reference { get; set; } = string.Empty;
}
