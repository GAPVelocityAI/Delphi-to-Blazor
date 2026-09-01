#nullable disable
using System;
using System.Collections.Generic;

namespace MyFinancePayroll.Core.Application.DTOs.Core;

/// <summary>TPayrollByDeptRecord data transfer object (domain: Core).</summary>
public class TPayrollByDeptRecord
{
    public string Department { get; set; } = string.Empty;
    public int EmployeeCount { get; set; }
    public decimal TotalPayroll { get; set; }
    public decimal AvgSalary { get; set; }
}
