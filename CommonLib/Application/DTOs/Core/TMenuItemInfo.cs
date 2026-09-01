#nullable disable
using System;
using System.Collections.Generic;
namespace CommonLib.Application.DTOs.Core;

/// <summary>TMenuItemInfo data transfer object (domain: Core).</summary>
public class TMenuItemInfo
{
    public int ItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal Cost { get; set; }
    public bool Active { get; set; }
}
