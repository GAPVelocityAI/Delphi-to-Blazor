#nullable disable
using System;
using System.Collections.Generic;
namespace CommonLib.Application.DTOs.Core;

/// <summary>TOrderInfo data transfer object (domain: Core).</summary>
public class TOrderInfo
{
    public int StatusIndex { get; set; }
    public string StatusText { get; set; } = string.Empty;
    public int OrderId { get; set; }
    public int TableId { get; set; }
    public DateTime OrderDate { get; set; }
    public TOrderStatus Status { get; set; }
    public decimal TotalAmount { get; set; }
}
