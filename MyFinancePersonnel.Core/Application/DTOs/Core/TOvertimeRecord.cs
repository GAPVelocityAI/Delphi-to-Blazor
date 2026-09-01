#nullable disable
using System;
using System.Collections.Generic;

namespace MyFinancePersonnel.Core.Application.DTOs.Core;

/// <summary>TOvertimeRecord data transfer object (domain: Core).</summary>
public class TOvertimeRecord
{
    public int EmployeeId { get; set; }
    public string Name { get; set; } = string.Empty;
    public double RegularHours { get; set; }
    public double OvertimeHours { get; set; }
    public decimal OvertimeCost { get; set; }
}
