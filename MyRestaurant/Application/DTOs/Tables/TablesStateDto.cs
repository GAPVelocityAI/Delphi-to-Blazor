#nullable disable
using System;
using System.Collections.Generic;

namespace MyRestaurant.Application.DTOs.Tables;

/// <summary>TablesState data transfer object (domain: Tables).</summary>
public class TablesStateDto
{
    public int StatusIndex { get; set; }
    public string Zone { get; set; } = string.Empty;
    public string FTablesBL { get; set; } = string.Empty;
    public bool FIsAdding { get; set; }
    public int FSelectedId { get; set; }
    public string EdtNumberText { get; set; } = string.Empty;
    public string EdtCapacityText { get; set; } = string.Empty;
}
