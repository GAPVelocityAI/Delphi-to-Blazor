#nullable disable
using System;
using System.Collections.Generic;

namespace MyInventoryStock.Core.Application.DTOs.Core;

/// <summary>TStockValuation data transfer object (domain: Core).</summary>
public class TStockValuation
{
    public string Category { get; set; } = string.Empty;
    public int ItemCount { get; set; }
    public double TotalQuantity { get; set; }
    public decimal TotalValue { get; set; }
}
