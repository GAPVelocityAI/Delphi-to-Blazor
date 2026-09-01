#nullable disable
using System;
using System.Collections.Generic;

namespace ProvidersCore.Core.Application.DTOs.Core;

/// <summary>TProviderCategory data transfer object (domain: Core).</summary>
public class TProviderCategory
{
    public string Category { get; set; } = string.Empty;
    public int ProviderCount { get; set; }
    public int ActiveCount { get; set; }
    public double AvgRating { get; set; }
}
