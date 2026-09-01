#nullable disable
using System;
using System.Collections.Generic;
namespace CommonLib.Application.DTOs.Core;

/// <summary>TEmployeeInfo data transfer object (domain: Core).</summary>
public class TEmployeeInfo
{
    public int EmployeeId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public DateTime HireDate { get; set; }
    public decimal Salary { get; set; }
    public bool Active { get; set; }
}
