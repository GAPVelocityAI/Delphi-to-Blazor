using MyFinancePersonnel.Core.Application.DTOs.Core;

namespace MyFinancePersonnel.Core.Application.Services.MyFinancePersonnel;

public interface IuFinancePersonnelBL
{
    List<TLaborCostRecord> GetLaborCostReport();
    List<TOvertimeRecord> GetOvertimeReport();
    List<THeadcountRecord> GetHeadcountByPosition();
    async Task<List<TLaborCostRecord>> GetLaborCostReportAsync(CancellationToken ct = default)
    {
        return await Task.FromResult(GetLaborCostReport());
    }
    async Task<List<TOvertimeRecord>> GetOvertimeReportAsync(CancellationToken ct = default)
    {
        return await Task.FromResult(GetOvertimeReport());
    }
    async Task<List<THeadcountRecord>> GetHeadcountByPositionAsync(CancellationToken ct = default)
    {
        return await Task.FromResult(GetHeadcountByPosition());
    }
}
