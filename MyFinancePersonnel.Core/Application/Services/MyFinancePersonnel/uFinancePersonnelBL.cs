using Microsoft.Extensions.Logging;
using MyFinancePersonnel.Core.Application.DTOs.Core;

namespace MyFinancePersonnel.Core.Application.Services.MyFinancePersonnel;

public class uFinancePersonnelBL : IuFinancePersonnelBL
{
    private readonly ILogger<uFinancePersonnelBL> _logger;

    public uFinancePersonnelBL(ILogger<uFinancePersonnelBL> logger)
    {
        _logger = logger;
    }

    public List<TLaborCostRecord> GetLaborCostReport()
    {
        _logger.LogInformation("Generating labor cost report");

        return new List<TLaborCostRecord>
        {
            new TLaborCostRecord
            {
                EmployeeId = 9,
                Name = "Robert Johnson",
                Position = "General Manager",
                BaseSalary = 62000m,
                Benefits = 12400m,
                TotalCost = 74400m
            },
            new TLaborCostRecord
            {
                EmployeeId = 1,
                Name = "Maria Garcia",
                Position = "Head Chef",
                BaseSalary = 55000m,
                Benefits = 11000m,
                TotalCost = 66000m
            },
            new TLaborCostRecord
            {
                EmployeeId = 2,
                Name = "Carlos Reyes",
                Position = "Sous Chef",
                BaseSalary = 42000m,
                Benefits = 8400m,
                TotalCost = 50400m
            },
            new TLaborCostRecord
            {
                EmployeeId = 3,
                Name = "Ana Martinez",
                Position = "Floor Manager",
                BaseSalary = 38000m,
                Benefits = 7600m,
                TotalCost = 45600m
            },
            new TLaborCostRecord
            {
                EmployeeId = 4,
                Name = "James Wilson",
                Position = "Bartender",
                BaseSalary = 32000m,
                Benefits = 6400m,
                TotalCost = 38400m
            },
            new TLaborCostRecord
            {
                EmployeeId = 5,
                Name = "Sofia Lopez",
                Position = "Waiter",
                BaseSalary = 28000m,
                Benefits = 5600m,
                TotalCost = 33600m
            },
            new TLaborCostRecord
            {
                EmployeeId = 6,
                Name = "David Chen",
                Position = "Waiter",
                BaseSalary = 28000m,
                Benefits = 5600m,
                TotalCost = 33600m
            },
            new TLaborCostRecord
            {
                EmployeeId = 7,
                Name = "Emily Brown",
                Position = "Host",
                BaseSalary = 26000m,
                Benefits = 5200m,
                TotalCost = 31200m
            },
            new TLaborCostRecord
            {
                EmployeeId = 8,
                Name = "Miguel Torres",
                Position = "Dishwasher",
                BaseSalary = 24000m,
                Benefits = 4800m,
                TotalCost = 28800m
            }
        };
    }

    public async Task<List<TLaborCostRecord>> GetLaborCostReportAsync(CancellationToken ct = default)
    {
        return await Task.FromResult(GetLaborCostReport());
    }

    public List<TOvertimeRecord> GetOvertimeReport()
    {
        _logger.LogInformation("Generating overtime report");

        return new List<TOvertimeRecord>
        {
            new TOvertimeRecord
            {
                EmployeeId = 1,
                Name = "Maria Garcia",
                RegularHours = 160.0,
                OvertimeHours = 12.0,
                OvertimeCost = 475.96m
            },
            new TOvertimeRecord
            {
                EmployeeId = 2,
                Name = "Carlos Reyes",
                RegularHours = 160.0,
                OvertimeHours = 18.0,
                OvertimeCost = 544.23m
            },
            new TOvertimeRecord
            {
                EmployeeId = 3,
                Name = "Ana Martinez",
                RegularHours = 160.0,
                OvertimeHours = 8.0,
                OvertimeCost = 219.23m
            },
            new TOvertimeRecord
            {
                EmployeeId = 4,
                Name = "James Wilson",
                RegularHours = 160.0,
                OvertimeHours = 22.0,
                OvertimeCost = 507.69m
            },
            new TOvertimeRecord
            {
                EmployeeId = 5,
                Name = "Sofia Lopez",
                RegularHours = 152.0,
                OvertimeHours = 6.0,
                OvertimeCost = 121.15m
            },
            new TOvertimeRecord
            {
                EmployeeId = 6,
                Name = "David Chen",
                RegularHours = 160.0,
                OvertimeHours = 10.0,
                OvertimeCost = 201.92m
            },
            new TOvertimeRecord
            {
                EmployeeId = 7,
                Name = "Emily Brown",
                RegularHours = 144.0,
                OvertimeHours = 0.0,
                OvertimeCost = 0m
            },
            new TOvertimeRecord
            {
                EmployeeId = 8,
                Name = "Miguel Torres",
                RegularHours = 160.0,
                OvertimeHours = 15.0,
                OvertimeCost = 259.62m
            }
        };
    }

    public async Task<List<TOvertimeRecord>> GetOvertimeReportAsync(CancellationToken ct = default)
    {
        return await Task.FromResult(GetOvertimeReport());
    }

    public List<THeadcountRecord> GetHeadcountByPosition()
    {
        _logger.LogInformation("Generating headcount by position report");

        return new List<THeadcountRecord>
        {
            new THeadcountRecord
            {
                Position = "Waiter",
                Count = 2,
                AvgSalary = 28000m
            },
            new THeadcountRecord
            {
                Position = "Head Chef",
                Count = 1,
                AvgSalary = 55000m
            },
            new THeadcountRecord
            {
                Position = "Sous Chef",
                Count = 1,
                AvgSalary = 42000m
            },
            new THeadcountRecord
            {
                Position = "Floor Manager",
                Count = 1,
                AvgSalary = 38000m
            },
            new THeadcountRecord
            {
                Position = "Bartender",
                Count = 1,
                AvgSalary = 32000m
            },
            new THeadcountRecord
            {
                Position = "Host",
                Count = 1,
                AvgSalary = 26000m
            },
            new THeadcountRecord
            {
                Position = "Dishwasher",
                Count = 1,
                AvgSalary = 24000m
            }
        };
    }

    public async Task<List<THeadcountRecord>> GetHeadcountByPositionAsync(CancellationToken ct = default)
    {
        return await Task.FromResult(GetHeadcountByPosition());
    }
}
