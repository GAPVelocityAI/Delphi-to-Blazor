#nullable disable
using System;
using System.Collections.Generic;

namespace MyRestaurant.Application.DTOs.Orders;

/// <summary>OrdersState data transfer object (domain: Orders).</summary>
public class OrdersStateDto
{
    public int CmbEditStatusIndex { get; set; }
    public List<TOrderDetailInfo> OrderDetails { get; set; } = new();
    public List<TOrderInfo> Orders { get; set; } = new();
    public bool PnlEditVisible { get; set; }
    public string FRestaurantBL { get; set; } = string.Empty;
    public bool FIsAdding { get; set; }
    public int FSelectedId { get; set; }
    public string EdtTableIdText { get; set; } = string.Empty;
    public string EdtTotalAmountText { get; set; } = string.Empty;
}
