#nullable disable
using System;
using System.Collections.Generic;
namespace CommonLib.Application.DTOs.Core;

/// <summary>TTableInfo data transfer object (domain: Core).</summary>
public class TTableInfo
{
    public int StatusIndex { get; set; }
    public string StatusText { get; set; } = string.Empty;
    public int TableId { get; set; }
    public int TableNumber { get; set; }
    public int Capacity { get; set; }
    public TTableStatus Status { get; set; }
    public string Zone { get; set; } = string.Empty;
}
