#nullable disable
using System;
using System.Collections.Generic;

namespace MyRestaurant.Application.DTOs.Bill;

/// <summary>BillCheckState data transfer object (domain: Bill).</summary>
public class BillCheckStateDto
{
    public string FRestaurantBL { get; set; } = string.Empty;
    public bool FIsAdding { get; set; }
    public int FSelectedId { get; set; }
    public string EdtOrderIdText { get; set; } = string.Empty;
    public string EdtSubtotalText { get; set; } = string.Empty;
    public string EdtTipText { get; set; } = string.Empty;
}
