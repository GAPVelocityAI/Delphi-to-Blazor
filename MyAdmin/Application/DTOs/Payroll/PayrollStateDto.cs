#nullable disable
using System;
using System.Collections.Generic;

namespace MyAdmin.Application.DTOs.Payroll;

/// <summary>PayrollState data transfer object (domain: Payroll).</summary>
public class PayrollStateDto
{
    public string FAdminBL { get; set; } = string.Empty;
    public bool FIsAdding { get; set; }
    public int FSelectedId { get; set; }
    public string EdtEmployeeText { get; set; } = string.Empty;
    public string EdtPeriodText { get; set; } = string.Empty;
    public string EdtGrossPayText { get; set; } = string.Empty;
    public string EdtDeductionsText { get; set; } = string.Empty;
    public string EdtPayDateText { get; set; } = string.Empty;
}
