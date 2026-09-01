#nullable disable
using System;
using System.Collections.Generic;

namespace MyAdmin.Domain.Entities.Core;

/// <summary>Port of <c>TEmployeeInfo (stored in a class-level array — the unit's table)</c>. Deterministically generated from the plan's schema.</summary>
public class Employee
{
    public int EmployeeId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public DateTime? HireDate { get; set; }
    public decimal? Salary { get; set; }
    public bool? Active { get; set; }

    public virtual ICollection<global::MyAdmin.Domain.Entities.Core.Payroll> Payrolls { get; set; } = new List<global::MyAdmin.Domain.Entities.Core.Payroll>();
}
