using Microsoft.Extensions.Logging;
using MyFinancePayroll.Core.Application.DTOs.Core;

namespace MyFinancePayroll.Core.Application.Services.MyFinancePayroll;

public class uFinancePayrollBL : IuFinancePayrollBL
{
    private readonly ILogger<uFinancePayrollBL> _logger;

    public uFinancePayrollBL(ILogger<uFinancePayrollBL> logger)
    {
        _logger = logger;
    }

    public List<TPayrollSummaryRecord> GetPayrollSummary()
    {
        _logger.LogDebug("Retrieving payroll summary data (sync)");
        return BuildPayrollSummaryData();
    }

    public List<TTaxWithholdingRecord> GetTaxWithholdings()
    {
        _logger.LogDebug("Retrieving tax withholding data (sync)");
        return BuildTaxWithholdingData();
    }

    public List<TPayrollByDeptRecord> GetPayrollByDepartment()
    {
        _logger.LogDebug("Retrieving payroll by department data (sync)");
        return BuildPayrollByDepartmentData();
    }

    public async Task<TPayrollSummaryRecord[]> GetPayrollSummaryAsync(CancellationToken ct = default)
    {
        _logger.LogDebug("Retrieving payroll summary data");
        await Task.CompletedTask;
        return BuildPayrollSummaryData().ToArray();
    }

    public async Task<TTaxWithholdingRecord[]> GetTaxWithholdingsAsync(CancellationToken ct = default)
    {
        _logger.LogDebug("Retrieving tax withholding data");
        await Task.CompletedTask;
        return BuildTaxWithholdingData().ToArray();
    }

    public async Task<TPayrollByDeptRecord[]> GetPayrollByDepartmentAsync(CancellationToken ct = default)
    {
        _logger.LogDebug("Retrieving payroll by department data");
        await Task.CompletedTask;
        return BuildPayrollByDepartmentData().ToArray();
    }

    private static List<TPayrollSummaryRecord> BuildPayrollSummaryData()
    {
        return new List<TPayrollSummaryRecord>
        {
            new TPayrollSummaryRecord
            {
                Period = "2026-07",
                TotalGross = 30416.67m,
                TotalDeductions = 6083.33m,
                TotalNet = 24333.33m,
                TotalTax = 4562.50m,
                EmployeeCount = 10
            },
            new TPayrollSummaryRecord
            {
                Period = "2026-06",
                TotalGross = 30416.67m,
                TotalDeductions = 6083.33m,
                TotalNet = 24333.33m,
                TotalTax = 4562.50m,
                EmployeeCount = 10
            },
            new TPayrollSummaryRecord
            {
                Period = "2026-05",
                TotalGross = 28916.67m,
                TotalDeductions = 5783.33m,
                TotalNet = 23133.33m,
                TotalTax = 4337.50m,
                EmployeeCount = 9
            },
            new TPayrollSummaryRecord
            {
                Period = "2026-04",
                TotalGross = 28916.67m,
                TotalDeductions = 5783.33m,
                TotalNet = 23133.33m,
                TotalTax = 4337.50m,
                EmployeeCount = 9
            },
            new TPayrollSummaryRecord
            {
                Period = "2026-03",
                TotalGross = 28916.67m,
                TotalDeductions = 5783.33m,
                TotalNet = 23133.33m,
                TotalTax = 4337.50m,
                EmployeeCount = 9
            },
            new TPayrollSummaryRecord
            {
                Period = "2026-02",
                TotalGross = 26416.67m,
                TotalDeductions = 5283.33m,
                TotalNet = 21133.33m,
                TotalTax = 3962.50m,
                EmployeeCount = 8
            }
        };
    }

    private static List<TTaxWithholdingRecord> BuildTaxWithholdingData()
    {
        return new List<TTaxWithholdingRecord>
        {
            new TTaxWithholdingRecord
            {
                EmployeeId = 9,
                Name = "Robert Johnson",
                FederalTax = 1033.33m,
                StateTax = 310.00m,
                SocialSecurity = 320.33m,
                Medicare = 74.90m
            },
            new TTaxWithholdingRecord
            {
                EmployeeId = 1,
                Name = "Maria Garcia",
                FederalTax = 916.67m,
                StateTax = 275.00m,
                SocialSecurity = 284.17m,
                Medicare = 66.46m
            },
            new TTaxWithholdingRecord
            {
                EmployeeId = 2,
                Name = "Carlos Reyes",
                FederalTax = 700.00m,
                StateTax = 210.00m,
                SocialSecurity = 217.00m,
                Medicare = 50.75m
            },
            new TTaxWithholdingRecord
            {
                EmployeeId = 3,
                Name = "Ana Martinez",
                FederalTax = 633.33m,
                StateTax = 190.00m,
                SocialSecurity = 196.33m,
                Medicare = 45.92m
            },
            new TTaxWithholdingRecord
            {
                EmployeeId = 4,
                Name = "James Wilson",
                FederalTax = 533.33m,
                StateTax = 160.00m,
                SocialSecurity = 165.33m,
                Medicare = 38.67m
            },
            new TTaxWithholdingRecord
            {
                EmployeeId = 5,
                Name = "Sofia Lopez",
                FederalTax = 466.67m,
                StateTax = 140.00m,
                SocialSecurity = 144.67m,
                Medicare = 33.83m
            },
            new TTaxWithholdingRecord
            {
                EmployeeId = 6,
                Name = "David Chen",
                FederalTax = 466.67m,
                StateTax = 140.00m,
                SocialSecurity = 144.67m,
                Medicare = 33.83m
            },
            new TTaxWithholdingRecord
            {
                EmployeeId = 7,
                Name = "Emily Brown",
                FederalTax = 433.33m,
                StateTax = 130.00m,
                SocialSecurity = 134.33m,
                Medicare = 31.42m
            },
            new TTaxWithholdingRecord
            {
                EmployeeId = 8,
                Name = "Miguel Torres",
                FederalTax = 400.00m,
                StateTax = 120.00m,
                SocialSecurity = 124.00m,
                Medicare = 29.00m
            }
        };
    }

    private static List<TPayrollByDeptRecord> BuildPayrollByDepartmentData()
    {
        return new List<TPayrollByDeptRecord>
        {
            new TPayrollByDeptRecord
            {
                Department = "Kitchen",
                EmployeeCount = 4,
                TotalPayroll = 12583.33m,
                AvgSalary = 42750m
            },
            new TPayrollByDeptRecord
            {
                Department = "Front of House",
                EmployeeCount = 3,
                TotalPayroll = 6833.33m,
                AvgSalary = 27333m
            },
            new TPayrollByDeptRecord
            {
                Department = "Bar",
                EmployeeCount = 1,
                TotalPayroll = 2666.67m,
                AvgSalary = 32000m
            },
            new TPayrollByDeptRecord
            {
                Department = "Management",
                EmployeeCount = 2,
                TotalPayroll = 8333.33m,
                AvgSalary = 50000m
            }
        };
    }
}
