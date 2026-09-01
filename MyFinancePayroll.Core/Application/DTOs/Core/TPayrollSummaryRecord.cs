#nullable disable
using System;
using System.Collections.Generic;

namespace MyFinancePayroll.Core.Application.DTOs.Core;

/// <summary>TPayrollSummaryRecord data transfer object (domain: Core).</summary>
public class TPayrollSummaryRecord
{
    public string Period { get; set; } = string.Empty;
    public decimal TotalGross { get; set; }
    public decimal TotalDeductions { get; set; }
    public decimal TotalNet { get; set; }
    public decimal TotalTax { get; set; }
    public int EmployeeCount { get; set; }
}
