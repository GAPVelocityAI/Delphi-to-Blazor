using MyFinancePayroll.Core.Application.DTOs.Core;

namespace MyFinancePayroll.Core.Application.Services.MyFinancePayroll;
public interface IuFinancePayrollBL
{
    List<TPayrollSummaryRecord> GetPayrollSummary();
    List<TTaxWithholdingRecord> GetTaxWithholdings();
    List<TPayrollByDeptRecord> GetPayrollByDepartment();
    Task<TPayrollSummaryRecord[]> GetPayrollSummaryAsync(CancellationToken ct = default);
    Task<TTaxWithholdingRecord[]> GetTaxWithholdingsAsync(CancellationToken ct = default);
    Task<TPayrollByDeptRecord[]> GetPayrollByDepartmentAsync(CancellationToken ct = default);
}
