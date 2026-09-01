#nullable disable
using System;
using System.Collections.Generic;

namespace ProvidersCore.Core.Application.DTOs.Core;

/// <summary>TProviderPerformance data transfer object (domain: Core).</summary>
public class TProviderPerformance
{
    public int ProviderId { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public double OnTimeDeliveryPct { get; set; }
    public int QualityScore { get; set; }
    public int TotalOrders { get; set; }
    public double AvgLeadTimeDays { get; set; }
}
