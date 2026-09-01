using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyAdmin.Infrastructure.Data;

namespace MyAdmin.Application.Services.Personnel;

public class PersonnelService : IPersonnelService
{
    private readonly IDbContextFactory<MyAdminDbContext> _dbFactory;
    private readonly ILogger<PersonnelService> _logger;

    public PersonnelService(
        IDbContextFactory<MyAdminDbContext> dbFactory,
        ILogger<PersonnelService> logger)
    {
        _dbFactory = dbFactory;
        _logger = logger;
    }

    /// <summary>
    /// Ported from legacy LoadEmployees(AActiveOnly: Boolean).
    /// Queries the database for employees, optionally filtering to active-only.
    /// Returns a list of DTOs for grid display.
    /// </summary>
    public async Task<List<TEmployeeInfoDto>> LoadEmployeesAsync(
        PersonnelStateDto state, bool AActiveOnly, CancellationToken ct = default)
    {
        await using var db = await _dbFactory.CreateDbContextAsync(ct);

        IQueryable<global::MyAdmin.Domain.Entities.Core.Employee> query = db.Employees
            .AsNoTracking()
            .OrderBy(e => e.LastName)
            .ThenBy(e => e.FirstName);

        if (AActiveOnly)
        {
            query = query.Where(e => (e.Active) ?? false);
        }

        var entities = await query.ToListAsync(ct);

        var result = new List<TEmployeeInfoDto>(entities.Count);
        foreach (var emp in entities)
        {
            result.Add(new TEmployeeInfoDto
            {
                EmployeeId = emp.EmployeeId,
                FirstName = emp.FirstName ?? string.Empty,
                LastName = emp.LastName ?? string.Empty,
                Position = emp.Position ?? string.Empty,
                HireDate = emp.HireDate ?? DateTime.MinValue,
                Salary = emp.Salary ?? 0m,
                Active = (emp.Active) ?? false
            });
        }

        // Legacy: lblCount.Caption := 'Employees: ' + IntToStr(Length(Employees));
        state.LblCountCaption = "Employees: " + result.Count.ToString(CultureInfo.InvariantCulture);

        _logger.LogDebug("LoadEmployeesAsync returned {Count} employees (activeOnly={ActiveOnly})",
            result.Count, AActiveOnly);

        return result;
    }

    /// <summary>
    /// Ported from legacy LoadEmployees(AActiveOnly: Boolean).
    /// Updates the state with count information after loading employees.
    /// This variant does not return the list; it updates state only.
    /// </summary>
    public async Task LoadEmployees(PersonnelStateDto state, bool AActiveOnly, CancellationToken ct = default)
    {
        await using var db = await _dbFactory.CreateDbContextAsync(ct);

        IQueryable<global::MyAdmin.Domain.Entities.Core.Employee> query = db.Employees
            .AsNoTracking();

        if (AActiveOnly)
        {
            query = query.Where(e => (e.Active) ?? false);
        }

        var count = await query.CountAsync(ct);

        // Legacy: lblCount.Caption := 'Employees: ' + IntToStr(Length(Employees));
        state.LblCountCaption = "Employees: " + count.ToString(CultureInfo.InvariantCulture);

        _logger.LogDebug("LoadEmployees updated state count to {Count} (activeOnly={ActiveOnly})",
            count, AActiveOnly);
    }

    /// <summary>
    /// Ported from legacy btnAddClick(Sender: TObject).
    /// Resets the state fields to blank for adding a new employee.
    /// Sets FIsAdding = true and clears all edit fields.
    /// </summary>
    public async Task BtnAddClickAsync(PersonnelStateDto state, object? Sender, CancellationToken ct = default)
    {
        await Task.CompletedTask;

        // Legacy: FIsAdding := True;
        state.FIsAdding = true;

        // Legacy: edtFirstName.Text := '';
        state.EdtFirstNameText = string.Empty;

        // Legacy: edtLastName.Text := '';
        state.EdtLastNameText = string.Empty;

        // Legacy: cmbPosition.ItemIndex := -1;  (no selection)
        state.CmbPositionText = string.Empty;

        // Legacy: edtHireDate.Text := '';
        state.EdtHireDateText = string.Empty;

        // Legacy: edtSalary.Text := '';
        state.EdtSalaryText = string.Empty;

        // Legacy: cmbActive.ItemIndex := 0; => defaults to "Yes" (active)
        state.FAdminBL = "Yes";

        state.FSelectedId = 0;

        // Legacy: pnlEdit.Visible := True;
        state.PnlEditVisible = true;

        _logger.LogDebug("BtnAddClickAsync: state reset for new employee entry");
    }

    /// <summary>
    /// Ported from legacy btnEditClick(Sender: TObject).
    /// Looks up the selected employee by FSelectedId from the database and
    /// populates the state DTO edit fields for editing.
    /// </summary>
    public async Task<PersonnelStateDto> BtnEditClickAsync(
        PersonnelStateDto state, object? Sender, CancellationToken ct = default)
    {
        // Legacy: Row := grdPersonnel.Row; if (Row < 1) or (Cells[0, Row] = '') then Exit;
        if (state.FSelectedId <= 0)
        {
            _logger.LogDebug("BtnEditClickAsync: no valid employee selected (FSelectedId={Id})", state.FSelectedId);
            return state;
        }

        await using var db = await _dbFactory.CreateDbContextAsync(ct);

        var employee = await db.Employees
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.EmployeeId == state.FSelectedId, ct);

        if (employee == null)
        {
            _logger.LogWarning("BtnEditClickAsync: employee with Id={Id} not found", state.FSelectedId);
            return state;
        }

        // Legacy: FIsAdding := False;
        state.FIsAdding = false;

        // Legacy: edtFirstName.Text := grdPersonnel.Cells[1, Row];
        state.EdtFirstNameText = employee.FirstName ?? string.Empty;

        // Legacy: edtLastName.Text := grdPersonnel.Cells[2, Row];
        state.EdtLastNameText = employee.LastName ?? string.Empty;

        // Legacy: cmbPosition.Text := grdPersonnel.Cells[3, Row];
        state.CmbPositionText = employee.Position ?? string.Empty;

        // Legacy: edtHireDate.Text := grdPersonnel.Cells[4, Row];
        state.EdtHireDateText = (employee.HireDate ?? DateTime.MinValue).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

        // Legacy: SalaryStr := StringReplace(SalaryStr, '$', '', [rfReplaceAll]);
        //         SalaryStr := StringReplace(SalaryStr, ',', '', [rfReplaceAll]);
        //         edtSalary.Text := SalaryStr;
        state.EdtSalaryText = (employee.Salary ?? 0m).ToString("F2", CultureInfo.InvariantCulture);

        // Legacy: if grdPersonnel.Cells[6, Row] = 'Yes' then cmbActive.ItemIndex := 0 else cmbActive.ItemIndex := 1;
        state.FAdminBL = (employee.Active ?? false) ? "Yes" : "No";

        // Legacy: pnlEdit.Visible := True;
        state.PnlEditVisible = true;

        _logger.LogDebug("BtnEditClickAsync: populated state for editing employee Id={Id}", state.FSelectedId);

        return state;
    }

    /// <summary>
    /// Ported from legacy btnDeleteClick(Sender: TObject).
    /// Deletes the employee identified by state.FSelectedId from the database.
    /// The confirmation dialog is handled by the Blazor page before calling this method.
    /// </summary>
    public async Task BtnDeleteClickAsync(
        PersonnelStateDto state, object? Sender, CancellationToken ct = default)
    {
        // Legacy: Row := grdPersonnel.Row; if (Row < 1) or (Cells[0, Row] = '') then Exit;
        if (state.FSelectedId <= 0)
        {
            _logger.LogDebug("BtnDeleteClickAsync: no valid employee selected (FSelectedId={Id})", state.FSelectedId);
            return;
        }

        // Legacy: Id := StrToIntDef(grdPersonnel.Cells[0, Row], 0);
        // Legacy: MessageDlg confirmation is handled by the page before calling this.
        // Legacy: FAdminBL.DeleteEmployee(Id);

        await using var db = await _dbFactory.CreateDbContextAsync(ct);

        var employee = await db.Employees
            .FirstOrDefaultAsync(e => e.EmployeeId == state.FSelectedId, ct);

        if (employee == null)
        {
            _logger.LogWarning("BtnDeleteClickAsync: employee with Id={Id} not found for deletion", state.FSelectedId);
            return;
        }

        db.Employees.Remove(employee);
        await db.SaveChangesAsync(ct);

        _logger.LogInformation("BtnDeleteClickAsync: deleted employee Id={Id}", state.FSelectedId);

        // Legacy: LoadEmployees(False); — the page will call LoadEmployeesAsync after this returns.
    }

    /// <summary>
    /// Ported from legacy btnSaveClick(Sender: TObject).
    /// Creates a new employee or updates an existing one based on state.FIsAdding.
    /// Parses form field values from the state DTO strings.
    /// </summary>
    public async Task BtnSaveClickAsync(
        PersonnelStateDto state, object? Sender, CancellationToken ct = default)
    {
        await using var db = await _dbFactory.CreateDbContextAsync(ct);

        // Legacy: Employee.FirstName := edtFirstName.Text;
        var firstName = state.EdtFirstNameText ?? string.Empty;

        // Legacy: Employee.LastName := edtLastName.Text;
        var lastName = state.EdtLastNameText ?? string.Empty;

        // Legacy: Employee.Position := cmbPosition.Text;
        var position = state.CmbPositionText ?? string.Empty;

        // Legacy: Employee.HireDate := StrToDateDef(edtHireDate.Text, Now);
        DateTime hireDate;
        if (!DateTime.TryParse(state.EdtHireDateText, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out hireDate))
        {
            hireDate = DateTime.UtcNow;
        }

        // Legacy: Employee.Salary := StrToCurrDef(StringReplace(StringReplace(edtSalary.Text, '$', '', ...), ',', '', ...), 0);
        var salaryStr = (state.EdtSalaryText ?? string.Empty)
            .Replace("$", "", StringComparison.Ordinal)
            .Replace(",", "", StringComparison.Ordinal);
        if (!decimal.TryParse(salaryStr, NumberStyles.Any, CultureInfo.InvariantCulture, out var salary))
        {
            salary = 0m;
        }

        // Legacy: Employee.Active := (cmbActive.ItemIndex = 0);
        // FAdminBL carries "Yes" or "No" to convey active status.
        bool active = string.Equals(state.FAdminBL, "Yes", StringComparison.OrdinalIgnoreCase);

        if (state.FIsAdding)
        {
            // Legacy: FAdminBL.AddEmployee(Employee)
            var newEmployee = new global::MyAdmin.Domain.Entities.Core.Employee
            {
                FirstName = firstName,
                LastName = lastName,
                Position = position,
                HireDate = hireDate,
                Salary = salary,
                Active = active
            };

            db.Employees.Add(newEmployee);
            await db.SaveChangesAsync(ct);

            _logger.LogInformation("BtnSaveClickAsync: added new employee '{First} {Last}' with Id={Id}",
                firstName, lastName, newEmployee.EmployeeId);
        }
        else
        {
            // Legacy: Employee.EmployeeId := FSelectedId; FAdminBL.UpdateEmployee(Employee);
            if (state.FSelectedId <= 0)
            {
                _logger.LogWarning("BtnSaveClickAsync: update requested but FSelectedId={Id} is invalid", state.FSelectedId);
                return;
            }

            var existing = await db.Employees
                .FirstOrDefaultAsync(e => e.EmployeeId == state.FSelectedId, ct);

            if (existing == null)
            {
                _logger.LogWarning("BtnSaveClickAsync: employee with Id={Id} not found for update", state.FSelectedId);
                return;
            }

            existing.FirstName = firstName;
            existing.LastName = lastName;
            existing.Position = position;
            existing.HireDate = hireDate;
            existing.Salary = salary;
            existing.Active = active;

            await db.SaveChangesAsync(ct);

            _logger.LogInformation("BtnSaveClickAsync: updated employee Id={Id}", state.FSelectedId);
        }

        // Legacy: pnlEdit.Visible := False;
        state.PnlEditVisible = false;

        // Legacy: LoadEmployees(False); — the page will call LoadEmployeesAsync after this returns.
    }
}
