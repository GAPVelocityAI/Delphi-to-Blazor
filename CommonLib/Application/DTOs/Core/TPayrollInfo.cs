#nullable disable
using System;
using System.Collections.Generic;
namespace CommonLib.Application.DTOs.Core;

/// <summary>TPayrollInfo data transfer object (domain: Core).</summary>
public class TPayrollInfo
{
    public int PayrollId { get; set; }
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public string Period { get; set; } = string.Empty;
    public decimal GrossPay { get; set; }
    public decimal Deductions { get; set; }
    public decimal NetPay { get; set; }
    public DateTime PayDate { get; set; }
}
