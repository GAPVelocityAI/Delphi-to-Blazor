using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyAdmin.Infrastructure.Data;

namespace MyAdmin.Application.Services.Payroll;

public class PayrollService : IPayrollService
{
    private readonly IDbContextFactory<MyAdminDbContext> _dbFactory;
    private readonly ILogger<PayrollService> _logger;

    public PayrollService(
        IDbContextFactory<MyAdminDbContext> dbFactory,
        ILogger<PayrollService> logger)
    {
        _dbFactory = dbFactory;
        _logger = logger;
    }

    /// <summary>
    /// Legacy: LoadPayroll — queries all payroll records ordered by EmployeeName,
    /// maps to DTOs. Totals (TotalGross, TotalNet, TotalDeductions) are computed
    /// by the caller (page) from the returned list, matching the legacy pattern
    /// where the form iterated rows and accumulated totals.
    /// </summary>
    public async Task<List<TPayrollInfoDto>> LoadPayrollAsync(PayrollStateDto state, CancellationToken ct = default)
    {
        await using var db = await _dbFactory.CreateDbContextAsync(ct);

        // Legacy: Payroll := FAdminBL.GetPayroll;
        // The legacy GetPayroll returned all payroll records (Copy of FPayroll array).
        // Legacy SQL: SELECT ... FROM Payroll WHERE Period = :Period ORDER BY EmployeeName
        // But the in-memory implementation returned ALL records, so we match that behavior.
        var entities = await db.Payrolls
            .AsNoTracking()
            .OrderBy(p => p.EmployeeName)
            .ToListAsync(ct);

        // Map to DTOs in memory
        var result = new List<TPayrollInfoDto>(entities.Count);
        foreach (var e in entities)
        {
            result.Add(new TPayrollInfoDto
            {
                PayrollId = e.PayrollId,
                EmployeeId = e.EmployeeId,
                EmployeeName = e.EmployeeName ?? string.Empty,
                Period = e.Period ?? string.Empty,
                GrossPay = (e.GrossPay) ?? 0m,
                Deductions = (e.Deductions) ?? 0m,
                NetPay = (e.NetPay) ?? 0m,
                PayDate = (e.PayDate) ?? default
            });
        }

        return result;
    }

    /// <summary>
    /// Legacy: btnEditClick — loads selected payroll record fields into the state DTO
    /// for editing. The FSelectedId on the state identifies the row to edit.
    /// Legacy populated edit fields from the grid; here we load from DB by ID.
    /// </summary>
    public async Task<PayrollStateDto> BtnEditClickAsync(PayrollStateDto state, CancellationToken ct = default)
    {
        if (state.FSelectedId <= 0)
        {
            _logger.LogWarning("BtnEditClickAsync called with no valid FSelectedId.");
            return state;
        }

        await using var db = await _dbFactory.CreateDbContextAsync(ct);

        // Legacy: FSelectedId := StrToIntDef(grdPayroll.Cells[0, Row], 0);
        // Then populated edit fields from grid cells.
        // Here we load fresh from DB to get accurate data.
        var entity = await db.Payrolls
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.PayrollId == state.FSelectedId, ct);

        if (entity == null)
        {
            _logger.LogWarning("Payroll record with ID {PayrollId} not found.", state.FSelectedId);
            return state;
        }

        // Legacy: FIsAdding := False;
        state.FIsAdding = false;

        // Legacy: edtEmployee.Text := grdPayroll.Cells[1, Row];
        state.EdtEmployeeText = entity.EmployeeName ?? string.Empty;

        // Legacy: edtPeriod.Text := grdPayroll.Cells[2, Row];
        state.EdtPeriodText = entity.Period ?? string.Empty;

        // Legacy: edtGrossPay.Text := StringReplace(StringReplace(grdPayroll.Cells[3, Row], '$', '', ...), ',', '', ...);
        // Strip currency formatting, store raw decimal as string for the edit field
        state.EdtGrossPayText = (entity.GrossPay ?? default).ToString("F2", CultureInfo.InvariantCulture);

        // Legacy: edtDeductions.Text := StringReplace(StringReplace(grdPayroll.Cells[4, Row], '$', '', ...), ',', '', ...);
        state.EdtDeductionsText = (entity.Deductions ?? default).ToString("F2", CultureInfo.InvariantCulture);

        // Legacy: edtPayDate.Text := grdPayroll.Cells[6, Row];
        state.EdtPayDateText = (entity.PayDate ?? default).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

        return state;
    }

    /// <summary>
    /// Legacy: btnDeleteClick — deletes the payroll record identified by state.FSelectedId.
    /// The legacy code showed a confirmation dialog; that responsibility stays with the page.
    /// This method performs the actual deletion.
    /// </summary>
    public async Task BtnDeleteClickAsync(PayrollStateDto state, CancellationToken ct = default)
    {
        if (state.FSelectedId <= 0)
        {
            _logger.LogWarning("BtnDeleteClickAsync called with no valid FSelectedId.");
            return;
        }

        await using var db = await _dbFactory.CreateDbContextAsync(ct);

        // Legacy: Id := StrToIntDef(grdPayroll.Cells[0, Row], 0);
        // FAdminBL.DeletePayroll(Id);
        var entity = await db.Payrolls
            .FirstOrDefaultAsync(p => p.PayrollId == state.FSelectedId, ct);

        if (entity == null)
        {
            _logger.LogWarning("Payroll record with ID {PayrollId} not found for deletion.", state.FSelectedId);
            return;
        }

        db.Payrolls.Remove(entity);
        await db.SaveChangesAsync(ct);

        _logger.LogInformation("Deleted payroll record with ID {PayrollId}.", state.FSelectedId);
    }

    /// <summary>
    /// Legacy: btnSaveClick — creates or updates a payroll record based on state.FIsAdding.
    /// Parses edit field values from state, computes NetPay = GrossPay - Deductions,
    /// then persists to DB.
    /// </summary>
    public async Task BtnSaveClickAsync(PayrollStateDto state, CancellationToken ct = default)
    {
        await using var db = await _dbFactory.CreateDbContextAsync(ct);

        // Legacy: Payroll.EmployeeName := edtEmployee.Text;
        var employeeName = state.EdtEmployeeText ?? string.Empty;

        // Legacy: Payroll.Period := edtPeriod.Text;
        var period = state.EdtPeriodText ?? string.Empty;

        // Legacy: Payroll.GrossPay := StrToCurrDef(StringReplace(StringReplace(edtGrossPay.Text, '$', '', ...), ',', '', ...), 0);
        var grossPayText = (state.EdtGrossPayText ?? string.Empty)
            .Replace("$", "", StringComparison.Ordinal)
            .Replace(",", "", StringComparison.Ordinal);
        decimal grossPay = decimal.TryParse(grossPayText, NumberStyles.Any, CultureInfo.InvariantCulture, out var gp) ? gp : 0m;

        // Legacy: Payroll.Deductions := StrToCurrDef(StringReplace(StringReplace(edtDeductions.Text, '$', '', ...), ',', '', ...), 0);
        var deductionsText = (state.EdtDeductionsText ?? string.Empty)
            .Replace("$", "", StringComparison.Ordinal)
            .Replace(",", "", StringComparison.Ordinal);
        decimal deductions = decimal.TryParse(deductionsText, NumberStyles.Any, CultureInfo.InvariantCulture, out var dd) ? dd : 0m;

        // Legacy: Payroll.PayDate := StrToDateDef(edtPayDate.Text, Now);
        DateTime payDate;
        if (!DateTime.TryParse(state.EdtPayDateText, CultureInfo.InvariantCulture, DateTimeStyles.None, out payDate))
        {
            payDate = DateTime.UtcNow;
        }

        // Legacy: NetPay := GrossPay - Deductions (computed in both AddPayroll and UpdatePayroll paths)
        decimal netPay = grossPay - deductions;

        if (state.FIsAdding)
        {
            // Legacy: FAdminBL.AddPayroll(Payroll)
            // ID assigned by database (identity column), not manual MAX+1
            var newEntity = new global::MyAdmin.Domain.Entities.Core.Payroll
            {
                EmployeeId = 0, // Legacy AddPayroll did not set EmployeeId from UI; it was left as default
                EmployeeName = employeeName,
                Period = period,
                GrossPay = grossPay,
                Deductions = deductions,
                NetPay = netPay,
                PayDate = payDate
            };

            db.Payrolls.Add(newEntity);
            await db.SaveChangesAsync(ct);

            _logger.LogInformation("Added new payroll record with ID {PayrollId}.", newEntity.PayrollId);
        }
        else
        {
            // Legacy: Payroll.PayrollId := FSelectedId;
            // Payroll.NetPay := Payroll.GrossPay - Payroll.Deductions;
            // FAdminBL.UpdatePayroll(Payroll);
            var existing = await db.Payrolls
                .FirstOrDefaultAsync(p => p.PayrollId == state.FSelectedId, ct);

            if (existing == null)
            {
                _logger.LogWarning("Payroll record with ID {PayrollId} not found for update.", state.FSelectedId);
                return;
            }

            existing.EmployeeName = employeeName;
            existing.Period = period;
            existing.GrossPay = grossPay;
            existing.Deductions = deductions;
            existing.NetPay = netPay;
            existing.PayDate = payDate;

            await db.SaveChangesAsync(ct);

            _logger.LogInformation("Updated payroll record with ID {PayrollId}.", state.FSelectedId);
        }
    }
}
