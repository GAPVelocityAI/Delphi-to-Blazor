#nullable disable
using System;
using System.Collections.Generic;

namespace MyRestaurant.Application.DTOs.Menu;

/// <summary>MenuViewState data transfer object (domain: Menu).</summary>
public class MenuViewStateDto
{
    public List<TMenuItemInfo> AllMenuItems { get; set; } = new();
    public int CmbEditActiveIndex { get; set; }
    public List<TMenuItemInfo> DisplayedItems { get; set; } = new();
    public bool PnlEditVisible { get; set; }
    public string FRestaurantBL { get; set; } = string.Empty;
    public bool FIsAdding { get; set; }
    public int FSelectedId { get; set; }
    public string EdtItemNameText { get; set; } = string.Empty;
    public string CmbEditCategoryText { get; set; } = string.Empty;
    public string EdtPriceText { get; set; } = string.Empty;
    public string EdtCostText { get; set; } = string.Empty;
}
