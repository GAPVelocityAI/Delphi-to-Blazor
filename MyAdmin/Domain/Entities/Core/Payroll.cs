#nullable disable
using System;
using System.Collections.Generic;

namespace MyAdmin.Domain.Entities.Core;

/// <summary>Port of <c>TPayrollInfo (stored in a class-level array — the unit's table)</c>. Deterministically generated from the plan's schema.</summary>
public class Payroll
{
    public int PayrollId { get; set; }
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public string Period { get; set; } = string.Empty;
    public decimal? GrossPay { get; set; }
    public decimal? Deductions { get; set; }
    public decimal? NetPay { get; set; }
    public DateTime? PayDate { get; set; }

    public virtual global::MyAdmin.Domain.Entities.Core.Employee Employee { get; set; }
}
