#nullable disable
using System;
using System.Collections.Generic;

namespace MyAdmin.Application.DTOs.Personnel;

/// <summary>PersonnelState data transfer object (domain: Personnel).</summary>
public class PersonnelStateDto
{
    public string LblCountCaption { get; set; } = string.Empty;
    public bool PnlEditVisible { get; set; }
    public string FAdminBL { get; set; } = string.Empty;
    public bool FIsAdding { get; set; }
    public int FSelectedId { get; set; }
    public string EdtFirstNameText { get; set; } = string.Empty;
    public string EdtLastNameText { get; set; } = string.Empty;
    public string CmbPositionText { get; set; } = string.Empty;
    public string EdtHireDateText { get; set; } = string.Empty;
    public string EdtSalaryText { get; set; } = string.Empty;
}
