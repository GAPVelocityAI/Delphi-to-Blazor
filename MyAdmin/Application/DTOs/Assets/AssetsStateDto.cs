#nullable disable
using System;
using System.Collections.Generic;

namespace MyAdmin.Application.DTOs.Assets;

/// <summary>AssetsState data transfer object (domain: Assets).</summary>
public class AssetsStateDto
{
    public decimal TotalDepreciated { get; set; }
    public decimal TotalValue { get; set; }
    public string FAdminBL { get; set; } = string.Empty;
    public bool FIsAdding { get; set; }
    public int FSelectedId { get; set; }
    public string EdtAssetNameText { get; set; } = string.Empty;
    public string CmbAssetCategoryText { get; set; } = string.Empty;
    public string EdtPurchaseDateText { get; set; } = string.Empty;
    public string EdtValueText { get; set; } = string.Empty;
    public string EdtDepreciatedText { get; set; } = string.Empty;
    public string CmbAssetStatusText { get; set; } = string.Empty;
}
