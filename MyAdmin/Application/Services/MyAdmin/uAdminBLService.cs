using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyAdmin.Infrastructure.Data;

namespace MyAdmin.Application.Services.MyAdmin;

public class uAdminBLService : IuAdminBLService
{
    private readonly IDbContextFactory<MyAdminDbContext> _dbContextFactory;
    private readonly ILogger<uAdminBLService> _logger;

    public uAdminBLService(
        IDbContextFactory<MyAdminDbContext> dbContextFactory,
        ILogger<uAdminBLService> logger)
    {
        _dbContextFactory = dbContextFactory;
        _logger = logger;
    }

    /// <summary>
    /// Legacy: EnsureInitialized — seeds the database with initial data if tables are empty.
    /// This replaces the legacy class-level array initialization.
    /// </summary>
    public async Task EnsureInitializedAsync(CancellationToken ct = default)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync(ct);

        bool hasEmployees = await db.Employees.AnyAsync(ct);
        bool hasAssets = await db.Assets.AnyAsync(ct);

        if (hasEmployees && hasAssets)
            return;

        if (!hasEmployees)
        {
            var employees = new List<global::MyAdmin.Domain.Entities.Core.Employee>
            {
                new() { FirstName = "Maria", LastName = "Garcia", Position = "Head Chef", HireDate = new DateTime(2019, 3, 15, 0, 0, 0, DateTimeKind.Utc), Salary = 55000m, Active = true },
                new() { FirstName = "Carlos", LastName = "Reyes", Position = "Sous Chef", HireDate = new DateTime(2020, 6, 1, 0, 0, 0, DateTimeKind.Utc), Salary = 42000m, Active = true },
                new() { FirstName = "Ana", LastName = "Martinez", Position = "Floor Manager", HireDate = new DateTime(2018, 11, 20, 0, 0, 0, DateTimeKind.Utc), Salary = 38000m, Active = true },
                new() { FirstName = "James", LastName = "Wilson", Position = "Bartender", HireDate = new DateTime(2021, 1, 10, 0, 0, 0, DateTimeKind.Utc), Salary = 32000m, Active = true },
                new() { FirstName = "Sofia", LastName = "Lopez", Position = "Waiter", HireDate = new DateTime(2022, 4, 5, 0, 0, 0, DateTimeKind.Utc), Salary = 28000m, Active = true },
                new() { FirstName = "David", LastName = "Chen", Position = "Waiter", HireDate = new DateTime(2022, 8, 18, 0, 0, 0, DateTimeKind.Utc), Salary = 28000m, Active = true },
                new() { FirstName = "Emily", LastName = "Brown", Position = "Host", HireDate = new DateTime(2023, 2, 14, 0, 0, 0, DateTimeKind.Utc), Salary = 26000m, Active = true },
                new() { FirstName = "Miguel", LastName = "Torres", Position = "Dishwasher", HireDate = new DateTime(2023, 5, 1, 0, 0, 0, DateTimeKind.Utc), Salary = 24000m, Active = true },
                new() { FirstName = "Robert", LastName = "Johnson", Position = "General Manager", HireDate = new DateTime(2017, 9, 1, 0, 0, 0, DateTimeKind.Utc), Salary = 62000m, Active = true },
                new() { FirstName = "Linda", LastName = "Davis", Position = "Line Cook", HireDate = new DateTime(2021, 7, 22, 0, 0, 0, DateTimeKind.Utc), Salary = 30000m, Active = false },
            };

            db.Employees.AddRange(employees);
            await db.SaveChangesAsync(ct);

            // Initialize Payroll for each employee — legacy: GrossPay = Salary / 12, Deductions = GrossPay * 0.20
            bool hasPayroll = await db.Payrolls.AnyAsync(ct);
            if (!hasPayroll)
            {
                var savedEmployees = await db.Employees.OrderBy(e => e.EmployeeId).ToListAsync(ct);
                var payrollEntries = new List<global::MyAdmin.Domain.Entities.Core.Payroll>();

                foreach (var emp in savedEmployees)
                {
                    decimal grossPay = Math.Round((emp.Salary / 12m) ?? 0m, 2, MidpointRounding.ToEven);
                    decimal deductions = Math.Round(grossPay * 0.20m, 2, MidpointRounding.ToEven);
                    decimal netPay = grossPay - deductions;

                    payrollEntries.Add(new global::MyAdmin.Domain.Entities.Core.Payroll
                    {
                        EmployeeId = emp.EmployeeId,
                        EmployeeName = emp.FirstName + " " + emp.LastName,
                        Period = "2026-07",
                        GrossPay = grossPay,
                        Deductions = deductions,
                        NetPay = netPay,
                        PayDate = new DateTime(2026, 7, 31, 0, 0, 0, DateTimeKind.Utc),
                    });
                }

                db.Payrolls.AddRange(payrollEntries);
                await db.SaveChangesAsync(ct);
            }
        }

        if (!hasAssets)
        {
            var assets = new List<global::MyAdmin.Domain.Entities.Core.Asset>
            {
                new() { AssetName = "Commercial Oven", Category = "Kitchen Equipment", PurchaseDate = new DateTime(2020, 1, 15, 0, 0, 0, DateTimeKind.Utc), Value = 15000m, DepreciatedValue = 12000m, Status = "Active" },
                new() { AssetName = "Walk-in Cooler", Category = "Kitchen Equipment", PurchaseDate = new DateTime(2019, 6, 10, 0, 0, 0, DateTimeKind.Utc), Value = 8000m, DepreciatedValue = 6500m, Status = "Active" },
                new() { AssetName = "POS System", Category = "Technology", PurchaseDate = new DateTime(2021, 3, 20, 0, 0, 0, DateTimeKind.Utc), Value = 3500m, DepreciatedValue = 2800m, Status = "Active" },
                new() { AssetName = "Dining Furniture Set", Category = "Furniture", PurchaseDate = new DateTime(2018, 11, 5, 0, 0, 0, DateTimeKind.Utc), Value = 12000m, DepreciatedValue = 7200m, Status = "Active" },
                new() { AssetName = "Delivery Van", Category = "Vehicle", PurchaseDate = new DateTime(2022, 2, 28, 0, 0, 0, DateTimeKind.Utc), Value = 25000m, DepreciatedValue = 21000m, Status = "Active" },
                new() { AssetName = "Industrial Dishwasher", Category = "Kitchen Equipment", PurchaseDate = new DateTime(2020, 8, 12, 0, 0, 0, DateTimeKind.Utc), Value = 5500m, DepreciatedValue = 4200m, Status = "Active" },
                new() { AssetName = "Security Camera System", Category = "Technology", PurchaseDate = new DateTime(2021, 5, 30, 0, 0, 0, DateTimeKind.Utc), Value = 2800m, DepreciatedValue = 2100m, Status = "Active" },
                new() { AssetName = "Bar Equipment", Category = "Bar", PurchaseDate = new DateTime(2019, 9, 15, 0, 0, 0, DateTimeKind.Utc), Value = 6000m, DepreciatedValue = 4000m, Status = "Needs Repair" },
            };

            db.Assets.AddRange(assets);
            await db.SaveChangesAsync(ct);
        }

        _logger.LogInformation("Database seed data initialized successfully.");
    }

    /// <summary>
    /// Legacy: GetEmployees — returns all employees ordered by LastName, FirstName.
    /// </summary>
    public async Task<List<TEmployeeInfo>> GetEmployeesAsync(CancellationToken ct = default)
    {
        await EnsureInitializedAsync(ct);

        await using var db = await _dbContextFactory.CreateDbContextAsync(ct);

        var entities = await db.Employees
            .AsNoTracking()
            .OrderBy(e => e.LastName)
            .ThenBy(e => e.FirstName)
            .ToListAsync(ct);

        return entities.Select(e => MapEmployeeToDto(e)).ToList();
    }

    /// <summary>
    /// Legacy: GetActiveEmployees — returns only active employees ordered by LastName, FirstName.
    /// </summary>
    public async Task<List<TEmployeeInfo>> GetActiveEmployeesAsync(CancellationToken ct = default)
    {
        await EnsureInitializedAsync(ct);

        await using var db = await _dbContextFactory.CreateDbContextAsync(ct);

        var entities = await db.Employees
            .AsNoTracking()
            .Where(e => (e.Active) ?? false)
            .OrderBy(e => e.LastName)
            .ThenBy(e => e.FirstName)
            .ToListAsync(ct);

        return entities.Select(e => MapEmployeeToDto(e)).ToList();
    }

    /// <summary>
    /// Legacy: GetAssets — returns all assets ordered by AssetName.
    /// </summary>
    public async Task<List<TAssetInfo>> GetAssetsAsync(CancellationToken ct = default)
    {
        await EnsureInitializedAsync(ct);

        await using var db = await _dbContextFactory.CreateDbContextAsync(ct);

        var entities = await db.Assets
            .AsNoTracking()
            .OrderBy(a => a.AssetName)
            .ToListAsync(ct);

        return entities.Select(a => MapAssetToDto(a)).ToList();
    }

    /// <summary>
    /// Legacy: GetPayroll — returns all payroll entries ordered by EmployeeName.
    /// </summary>
    public async Task<List<TPayrollInfo>> GetPayrollAsync(CancellationToken ct = default)
    {
        await EnsureInitializedAsync(ct);

        await using var db = await _dbContextFactory.CreateDbContextAsync(ct);

        var entities = await db.Payrolls
            .AsNoTracking()
            .OrderBy(p => p.EmployeeName)
            .ToListAsync(ct);

        return entities.Select(p => MapPayrollToDto(p)).ToList();
    }

    /// <summary>
    /// Legacy: AddEmployee — inserts a new employee. The database assigns the ID (identity column).
    /// Updates the passed DTO's EmployeeId in-place via ref semantics on the record.
    /// </summary>
    public async Task AddEmployeeAsync(TEmployeeInfo employee, CancellationToken ct = default)
    {
        await EnsureInitializedAsync(ct);

        await using var db = await _dbContextFactory.CreateDbContextAsync(ct);

        var entity = new global::MyAdmin.Domain.Entities.Core.Employee
        {
            FirstName = employee.FirstName ?? string.Empty,
            LastName = employee.LastName ?? string.Empty,
            Position = employee.Position ?? string.Empty,
            HireDate = employee.HireDate,
            Salary = employee.Salary,
            Active = employee.Active,
        };

        db.Employees.Add(entity);
        await db.SaveChangesAsync(ct);

        employee.EmployeeId = entity.EmployeeId;

        _logger.LogInformation("Employee added with ID {EmployeeId}.", entity.EmployeeId);
    }

    /// <summary>
    /// Legacy: UpdateEmployee — updates an existing employee by EmployeeId.
    /// </summary>
    public async Task UpdateEmployeeAsync(TEmployeeInfo employee, CancellationToken ct = default)
    {
        await EnsureInitializedAsync(ct);

        await using var db = await _dbContextFactory.CreateDbContextAsync(ct);

        var entity = await db.Employees.FirstOrDefaultAsync(e => e.EmployeeId == employee.EmployeeId, ct);
        if (entity == null)
        {
            _logger.LogWarning("UpdateEmployee: Employee with ID {EmployeeId} not found.", employee.EmployeeId);
            return;
        }

        entity.FirstName = employee.FirstName ?? string.Empty;
        entity.LastName = employee.LastName ?? string.Empty;
        entity.Position = employee.Position ?? string.Empty;
        entity.HireDate = employee.HireDate;
        entity.Salary = employee.Salary;
        entity.Active = employee.Active;

        await db.SaveChangesAsync(ct);

        _logger.LogInformation("Employee with ID {EmployeeId} updated.", employee.EmployeeId);
    }

    /// <summary>
    /// Legacy: DeleteEmployee — removes employee by ID.
    /// </summary>
    public async Task DeleteEmployeeAsync(int employeeId, CancellationToken ct = default)
    {
        await EnsureInitializedAsync(ct);

        await using var db = await _dbContextFactory.CreateDbContextAsync(ct);

        var entity = await db.Employees.FirstOrDefaultAsync(e => e.EmployeeId == employeeId, ct);
        if (entity == null)
        {
            _logger.LogWarning("DeleteEmployee: Employee with ID {EmployeeId} not found.", employeeId);
            return;
        }

        db.Employees.Remove(entity);
        await db.SaveChangesAsync(ct);

        _logger.LogInformation("Employee with ID {EmployeeId} deleted.", employeeId);
    }

    /// <summary>
    /// Legacy: AddAsset — inserts a new asset. The database assigns the ID.
    /// Updates the passed DTO's AssetId in-place.
    /// </summary>
    public async Task AddAssetAsync(TAssetInfo asset, CancellationToken ct = default)
    {
        await EnsureInitializedAsync(ct);

        await using var db = await _dbContextFactory.CreateDbContextAsync(ct);

        var entity = new global::MyAdmin.Domain.Entities.Core.Asset
        {
            AssetName = asset.AssetName ?? string.Empty,
            Category = asset.Category ?? string.Empty,
            PurchaseDate = asset.PurchaseDate,
            Value = asset.Value,
            DepreciatedValue = asset.DepreciatedValue,
            Status = asset.Status ?? string.Empty,
        };

        db.Assets.Add(entity);
        await db.SaveChangesAsync(ct);

        asset.AssetId = entity.AssetId;

        _logger.LogInformation("Asset added with ID {AssetId}.", entity.AssetId);
    }

    /// <summary>
    /// Legacy: UpdateAsset — updates an existing asset by AssetId.
    /// </summary>
    public async Task UpdateAssetAsync(TAssetInfo asset, CancellationToken ct = default)
    {
        await EnsureInitializedAsync(ct);

        await using var db = await _dbContextFactory.CreateDbContextAsync(ct);

        var entity = await db.Assets.FirstOrDefaultAsync(a => a.AssetId == asset.AssetId, ct);
        if (entity == null)
        {
            _logger.LogWarning("UpdateAsset: Asset with ID {AssetId} not found.", asset.AssetId);
            return;
        }

        entity.AssetName = asset.AssetName ?? string.Empty;
        entity.Category = asset.Category ?? string.Empty;
        entity.PurchaseDate = asset.PurchaseDate;
        entity.Value = asset.Value;
        entity.DepreciatedValue = asset.DepreciatedValue;
        entity.Status = asset.Status ?? string.Empty;

        await db.SaveChangesAsync(ct);

        _logger.LogInformation("Asset with ID {AssetId} updated.", asset.AssetId);
    }

    /// <summary>
    /// Legacy: DeleteAsset — removes asset by ID.
    /// </summary>
    public async Task DeleteAssetAsync(int assetId, CancellationToken ct = default)
    {
        await EnsureInitializedAsync(ct);

        await using var db = await _dbContextFactory.CreateDbContextAsync(ct);

        var entity = await db.Assets.FirstOrDefaultAsync(a => a.AssetId == assetId, ct);
        if (entity == null)
        {
            _logger.LogWarning("DeleteAsset: Asset with ID {AssetId} not found.", assetId);
            return;
        }

        db.Assets.Remove(entity);
        await db.SaveChangesAsync(ct);

        _logger.LogInformation("Asset with ID {AssetId} deleted.", assetId);
    }

    /// <summary>
    /// Legacy: AddPayroll — inserts a new payroll entry. NetPay is computed as GrossPay - Deductions.
    /// The database assigns the ID. Updates the passed DTO's PayrollId and NetPay in-place.
    /// </summary>
    public async Task AddPayrollAsync(TPayrollInfo payroll, CancellationToken ct = default)
    {
        await EnsureInitializedAsync(ct);

        await using var db = await _dbContextFactory.CreateDbContextAsync(ct);

        // Legacy: APayroll.NetPay := APayroll.GrossPay - APayroll.Deductions;
        decimal netPay = payroll.GrossPay - payroll.Deductions;

        var entity = new global::MyAdmin.Domain.Entities.Core.Payroll
        {
            EmployeeId = payroll.EmployeeId,
            EmployeeName = payroll.EmployeeName ?? string.Empty,
            Period = payroll.Period ?? string.Empty,
            GrossPay = payroll.GrossPay,
            Deductions = payroll.Deductions,
            NetPay = netPay,
            PayDate = payroll.PayDate,
        };

        db.Payrolls.Add(entity);
        await db.SaveChangesAsync(ct);

        payroll.PayrollId = entity.PayrollId;
        payroll.NetPay = netPay;

        _logger.LogInformation("Payroll entry added with ID {PayrollId}.", entity.PayrollId);
    }

    /// <summary>
    /// Legacy: UpdatePayroll — updates an existing payroll entry by PayrollId.
    /// </summary>
    public async Task UpdatePayrollAsync(TPayrollInfo payroll, CancellationToken ct = default)
    {
        await EnsureInitializedAsync(ct);

        await using var db = await _dbContextFactory.CreateDbContextAsync(ct);

        var entity = await db.Payrolls.FirstOrDefaultAsync(p => p.PayrollId == payroll.PayrollId, ct);
        if (entity == null)
        {
            _logger.LogWarning("UpdatePayroll: Payroll entry with ID {PayrollId} not found.", payroll.PayrollId);
            return;
        }

        entity.EmployeeId = payroll.EmployeeId;
        entity.EmployeeName = payroll.EmployeeName ?? string.Empty;
        entity.Period = payroll.Period ?? string.Empty;
        entity.GrossPay = payroll.GrossPay;
        entity.Deductions = payroll.Deductions;
        entity.NetPay = payroll.NetPay;
        entity.PayDate = payroll.PayDate;

        await db.SaveChangesAsync(ct);

        _logger.LogInformation("Payroll entry with ID {PayrollId} updated.", payroll.PayrollId);
    }

    /// <summary>
    /// Legacy: DeletePayroll — removes payroll entry by ID.
    /// </summary>
    public async Task DeletePayrollAsync(int payrollId, CancellationToken ct = default)
    {
        await EnsureInitializedAsync(ct);

        await using var db = await _dbContextFactory.CreateDbContextAsync(ct);

        var entity = await db.Payrolls.FirstOrDefaultAsync(p => p.PayrollId == payrollId, ct);
        if (entity == null)
        {
            _logger.LogWarning("DeletePayroll: Payroll entry with ID {PayrollId} not found.", payrollId);
            return;
        }

        db.Payrolls.Remove(entity);
        await db.SaveChangesAsync(ct);

        _logger.LogInformation("Payroll entry with ID {PayrollId} deleted.", payrollId);
    }

    // ── Legacy-compatible synchronous wrappers ──

    /// <summary>
    /// Legacy: EnsureInitialized — synchronous wrapper for EnsureInitializedAsync.
    /// </summary>
    public void EnsureInitialized()
    {
        EnsureInitializedAsync().GetAwaiter().GetResult();
    }

    /// <summary>
    /// Legacy: GetEmployees — synchronous wrapper for GetEmployeesAsync.
    /// </summary>
    public List<TEmployeeInfo> GetEmployees()
    {
        return GetEmployeesAsync().GetAwaiter().GetResult();
    }

    /// <summary>
    /// Legacy: GetActiveEmployees — synchronous wrapper for GetActiveEmployeesAsync.
    /// </summary>
    public List<TEmployeeInfo> GetActiveEmployees()
    {
        return GetActiveEmployeesAsync().GetAwaiter().GetResult();
    }

    /// <summary>
    /// Legacy: GetAssets — synchronous wrapper for GetAssetsAsync.
    /// </summary>
    public List<TAssetInfo> GetAssets()
    {
        return GetAssetsAsync().GetAwaiter().GetResult();
    }

    /// <summary>
    /// Legacy: GetPayroll — synchronous wrapper for GetPayrollAsync.
    /// </summary>
    public List<TPayrollInfo> GetPayroll()
    {
        return GetPayrollAsync().GetAwaiter().GetResult();
    }

    /// <summary>
    /// Legacy: AddEmployee — synchronous wrapper for AddEmployeeAsync.
    /// </summary>
    public void AddEmployee(TEmployeeInfo employee)
    {
        AddEmployeeAsync(employee).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Legacy: UpdateEmployee — synchronous wrapper for UpdateEmployeeAsync.
    /// </summary>
    public void UpdateEmployee(TEmployeeInfo employee)
    {
        UpdateEmployeeAsync(employee).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Legacy: DeleteEmployee — synchronous wrapper for DeleteEmployeeAsync.
    /// </summary>
    public void DeleteEmployee(int employeeId)
    {
        DeleteEmployeeAsync(employeeId).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Legacy: AddAsset — synchronous wrapper for AddAssetAsync.
    /// </summary>
    public void AddAsset(TAssetInfo asset)
    {
        AddAssetAsync(asset).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Legacy: UpdateAsset — synchronous wrapper for UpdateAssetAsync.
    /// </summary>
    public void UpdateAsset(TAssetInfo asset)
    {
        UpdateAssetAsync(asset).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Legacy: DeleteAsset — synchronous wrapper for DeleteAssetAsync.
    /// </summary>
    public void DeleteAsset(int assetId)
    {
        DeleteAssetAsync(assetId).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Legacy: AddPayroll — synchronous wrapper for AddPayrollAsync.
    /// </summary>
    public void AddPayroll(TPayrollInfo payroll)
    {
        AddPayrollAsync(payroll).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Legacy: UpdatePayroll — synchronous wrapper for UpdatePayrollAsync.
    /// </summary>
    public void UpdatePayroll(TPayrollInfo payroll)
    {
        UpdatePayrollAsync(payroll).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Legacy: DeletePayroll — synchronous wrapper for DeletePayrollAsync.
    /// </summary>
    public void DeletePayroll(int payrollId)
    {
        DeletePayrollAsync(payrollId).GetAwaiter().GetResult();
    }

    // ── Mapping helpers ──

    private static TEmployeeInfo MapEmployeeToDto(global::MyAdmin.Domain.Entities.Core.Employee entity)
    {
        return new TEmployeeInfo
        {
            EmployeeId = entity.EmployeeId,
            FirstName = entity.FirstName ?? string.Empty,
            LastName = entity.LastName ?? string.Empty,
            Position = entity.Position ?? string.Empty,
            HireDate = (entity.HireDate) ?? default,
            Salary = (entity.Salary) ?? 0m,
            Active = (entity.Active) ?? false,
        };
    }

    private static TAssetInfo MapAssetToDto(global::MyAdmin.Domain.Entities.Core.Asset entity)
    {
        return new TAssetInfo
        {
            AssetId = entity.AssetId,
            AssetName = entity.AssetName ?? string.Empty,
            Category = entity.Category ?? string.Empty,
            PurchaseDate = (entity.PurchaseDate) ?? default,
            Value = (entity.Value) ?? 0m,
            DepreciatedValue = (entity.DepreciatedValue) ?? 0m,
            Status = entity.Status ?? string.Empty,
        };
    }

    private static TPayrollInfo MapPayrollToDto(global::MyAdmin.Domain.Entities.Core.Payroll entity)
    {
        return new TPayrollInfo
        {
            PayrollId = entity.PayrollId,
            EmployeeId = entity.EmployeeId,
            EmployeeName = entity.EmployeeName ?? string.Empty,
            Period = entity.Period ?? string.Empty,
            GrossPay = (entity.GrossPay) ?? 0m,
            Deductions = (entity.Deductions) ?? 0m,
            NetPay = (entity.NetPay) ?? 0m,
            PayDate = (entity.PayDate) ?? default,
        };
    }
}
