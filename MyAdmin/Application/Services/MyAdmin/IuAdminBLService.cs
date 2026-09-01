
namespace MyAdmin.Application.Services.MyAdmin;

public interface IuAdminBLService
{
    Task EnsureInitializedAsync(CancellationToken ct = default);

    Task<List<TEmployeeInfo>> GetEmployeesAsync(CancellationToken ct = default);
    Task<List<TEmployeeInfo>> GetActiveEmployeesAsync(CancellationToken ct = default);
    Task<List<TAssetInfo>> GetAssetsAsync(CancellationToken ct = default);
    Task<List<TPayrollInfo>> GetPayrollAsync(CancellationToken ct = default);

    Task AddEmployeeAsync(TEmployeeInfo employee, CancellationToken ct = default);
    Task UpdateEmployeeAsync(TEmployeeInfo employee, CancellationToken ct = default);
    Task DeleteEmployeeAsync(int employeeId, CancellationToken ct = default);

    Task AddAssetAsync(TAssetInfo asset, CancellationToken ct = default);
    Task UpdateAssetAsync(TAssetInfo asset, CancellationToken ct = default);
    Task DeleteAssetAsync(int assetId, CancellationToken ct = default);

    Task AddPayrollAsync(TPayrollInfo payroll, CancellationToken ct = default);
    Task UpdatePayrollAsync(TPayrollInfo payroll, CancellationToken ct = default);
    Task DeletePayrollAsync(int payrollId, CancellationToken ct = default);

    // Legacy-compatible synchronous-style wrappers
    void EnsureInitialized();
    List<TEmployeeInfo> GetEmployees();
    List<TEmployeeInfo> GetActiveEmployees();
    List<TAssetInfo> GetAssets();
    List<TPayrollInfo> GetPayroll();
    void AddEmployee(TEmployeeInfo employee);
    void UpdateEmployee(TEmployeeInfo employee);
    void DeleteEmployee(int employeeId);
    void AddAsset(TAssetInfo asset);
    void UpdateAsset(TAssetInfo asset);
    void DeleteAsset(int assetId);
    void AddPayroll(TPayrollInfo payroll);
    void UpdatePayroll(TPayrollInfo payroll);
    void DeletePayroll(int payrollId);
}
