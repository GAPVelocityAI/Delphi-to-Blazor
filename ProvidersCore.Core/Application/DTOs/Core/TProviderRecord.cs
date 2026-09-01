#nullable disable
using System;
using System.Collections.Generic;

namespace ProvidersCore.Core.Application.DTOs.Core;

/// <summary>TProviderRecord data transfer object (domain: Core).</summary>
public class TProviderRecord
{
    public int ProviderId { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string ContactName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int Rating { get; set; }
    public bool Active { get; set; }
    public DateTime ContractExpiry { get; set; }
}
