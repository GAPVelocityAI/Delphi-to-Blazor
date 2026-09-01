#nullable disable
using System;
using System.Collections.Generic;

namespace MyFinancePersonnel.Core.Application.DTOs.Core;

/// <summary>TLaborCostRecord data transfer object (domain: Core).</summary>
public class TLaborCostRecord
{
    public int EmployeeId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public decimal BaseSalary { get; set; }
    public decimal Benefits { get; set; }
    public decimal TotalCost { get; set; }
}
