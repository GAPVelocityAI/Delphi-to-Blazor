#nullable disable
using System;
using System.Collections.Generic;

namespace MyInventoryMenu.Core.Application.DTOs.Core;

/// <summary>TTableAvailability data transfer object (domain: Core).</summary>
public class TTableAvailability
{
    public int TableId { get; set; }
    public int TableNumber { get; set; }
    public int Capacity { get; set; }
    public string Zone { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public bool IsAvailable { get; set; }
}
