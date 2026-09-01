using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MyAdmin.Application.Services.Payroll;

public interface IPayrollService
{
    Task<List<TPayrollInfoDto>> LoadPayrollAsync(PayrollStateDto state, CancellationToken ct = default);
    Task<PayrollStateDto> BtnEditClickAsync(PayrollStateDto state, CancellationToken ct = default);
    Task BtnDeleteClickAsync(PayrollStateDto state, CancellationToken ct = default);
    Task BtnSaveClickAsync(PayrollStateDto state, CancellationToken ct = default);
}
