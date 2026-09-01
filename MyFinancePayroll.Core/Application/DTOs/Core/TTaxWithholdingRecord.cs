#nullable disable
using System;
using System.Collections.Generic;

namespace MyFinancePayroll.Core.Application.DTOs.Core;

/// <summary>TTaxWithholdingRecord data transfer object (domain: Core).</summary>
public class TTaxWithholdingRecord
{
    public int EmployeeId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal FederalTax { get; set; }
    public decimal StateTax { get; set; }
    public decimal SocialSecurity { get; set; }
    public decimal Medicare { get; set; }
}
