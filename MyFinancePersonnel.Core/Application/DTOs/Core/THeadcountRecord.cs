#nullable disable
using System;
using System.Collections.Generic;

namespace MyFinancePersonnel.Core.Application.DTOs.Core;

/// <summary>THeadcountRecord data transfer object (domain: Core).</summary>
public class THeadcountRecord
{
    public string Position { get; set; } = string.Empty;
    public int Count { get; set; }
    public decimal AvgSalary { get; set; }
}
