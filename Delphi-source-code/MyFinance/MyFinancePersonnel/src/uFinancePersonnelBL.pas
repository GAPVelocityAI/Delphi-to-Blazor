unit uFinancePersonnelBL;

interface

uses
  System.SysUtils, System.Classes;

const
  SQL_LABOR_COST_REPORT =
    'SELECT e.EmployeeId, e.FirstName + '' '' + e.LastName AS Name, e.Position, ' +
    'e.Salary AS BaseSalary, b.BenefitsCost AS Benefits, ' +
    'e.Salary + b.BenefitsCost AS TotalCost ' +
    'FROM Employees e LEFT JOIN EmployeeBenefits b ON e.EmployeeId = b.EmployeeId ' +
    'WHERE e.Active = 1 ORDER BY TotalCost DESC';

  SQL_OVERTIME_REPORT =
    'SELECT t.EmployeeId, e.FirstName + '' '' + e.LastName AS Name, ' +
    'SUM(t.RegularHours) AS RegularHours, SUM(t.OvertimeHours) AS OvertimeHours, ' +
    'SUM(t.OvertimeHours * e.Salary / 2080 * 1.5) AS OvertimeCost ' +
    'FROM Timesheets t INNER JOIN Employees e ON t.EmployeeId = e.EmployeeId ' +
    'WHERE t.Period = :Period GROUP BY t.EmployeeId, e.FirstName, e.LastName';

  SQL_HEADCOUNT_BY_POSITION =
    'SELECT e.Position, COUNT(*) AS HeadCount, AVG(e.Salary) AS AvgSalary ' +
    'FROM Employees e WHERE e.Active = 1 ' +
    'GROUP BY e.Position ORDER BY HeadCount DESC';

type
  TLaborCostRecord = record
    EmployeeId: Integer;
    Name: string;
    Position: string;
    BaseSalary: Currency;
    Benefits: Currency;
    TotalCost: Currency;
  end;

  TOvertimeRecord = record
    EmployeeId: Integer;
    Name: string;
    RegularHours: Double;
    OvertimeHours: Double;
    OvertimeCost: Currency;
  end;

  THeadcountRecord = record
    Position: string;
    Count: Integer;
    AvgSalary: Currency;
  end;

  TFinancePersonnelBL = class
  public
    function GetLaborCostReport: TArray<TLaborCostRecord>;
    function GetOvertimeReport: TArray<TOvertimeRecord>;
    function GetHeadcountByPosition: TArray<THeadcountRecord>;
  end;

implementation

{ TFinancePersonnelBL }

function TFinancePersonnelBL.GetLaborCostReport: TArray<TLaborCostRecord>;
begin
  SetLength(Result, 9);

  Result[0].EmployeeId := 9;
  Result[0].Name := 'Robert Johnson';
  Result[0].Position := 'General Manager';
  Result[0].BaseSalary := 62000;
  Result[0].Benefits := 12400;
  Result[0].TotalCost := 74400;

  Result[1].EmployeeId := 1;
  Result[1].Name := 'Maria Garcia';
  Result[1].Position := 'Head Chef';
  Result[1].BaseSalary := 55000;
  Result[1].Benefits := 11000;
  Result[1].TotalCost := 66000;

  Result[2].EmployeeId := 2;
  Result[2].Name := 'Carlos Reyes';
  Result[2].Position := 'Sous Chef';
  Result[2].BaseSalary := 42000;
  Result[2].Benefits := 8400;
  Result[2].TotalCost := 50400;

  Result[3].EmployeeId := 3;
  Result[3].Name := 'Ana Martinez';
  Result[3].Position := 'Floor Manager';
  Result[3].BaseSalary := 38000;
  Result[3].Benefits := 7600;
  Result[3].TotalCost := 45600;

  Result[4].EmployeeId := 4;
  Result[4].Name := 'James Wilson';
  Result[4].Position := 'Bartender';
  Result[4].BaseSalary := 32000;
  Result[4].Benefits := 6400;
  Result[4].TotalCost := 38400;

  Result[5].EmployeeId := 5;
  Result[5].Name := 'Sofia Lopez';
  Result[5].Position := 'Waiter';
  Result[5].BaseSalary := 28000;
  Result[5].Benefits := 5600;
  Result[5].TotalCost := 33600;

  Result[6].EmployeeId := 6;
  Result[6].Name := 'David Chen';
  Result[6].Position := 'Waiter';
  Result[6].BaseSalary := 28000;
  Result[6].Benefits := 5600;
  Result[6].TotalCost := 33600;

  Result[7].EmployeeId := 7;
  Result[7].Name := 'Emily Brown';
  Result[7].Position := 'Host';
  Result[7].BaseSalary := 26000;
  Result[7].Benefits := 5200;
  Result[7].TotalCost := 31200;

  Result[8].EmployeeId := 8;
  Result[8].Name := 'Miguel Torres';
  Result[8].Position := 'Dishwasher';
  Result[8].BaseSalary := 24000;
  Result[8].Benefits := 4800;
  Result[8].TotalCost := 28800;
end;

function TFinancePersonnelBL.GetOvertimeReport: TArray<TOvertimeRecord>;
begin
  SetLength(Result, 8);

  Result[0].EmployeeId := 1;
  Result[0].Name := 'Maria Garcia';
  Result[0].RegularHours := 160;
  Result[0].OvertimeHours := 12;
  Result[0].OvertimeCost := 475.96;

  Result[1].EmployeeId := 2;
  Result[1].Name := 'Carlos Reyes';
  Result[1].RegularHours := 160;
  Result[1].OvertimeHours := 18;
  Result[1].OvertimeCost := 544.23;

  Result[2].EmployeeId := 3;
  Result[2].Name := 'Ana Martinez';
  Result[2].RegularHours := 160;
  Result[2].OvertimeHours := 8;
  Result[2].OvertimeCost := 219.23;

  Result[3].EmployeeId := 4;
  Result[3].Name := 'James Wilson';
  Result[3].RegularHours := 160;
  Result[3].OvertimeHours := 22;
  Result[3].OvertimeCost := 507.69;

  Result[4].EmployeeId := 5;
  Result[4].Name := 'Sofia Lopez';
  Result[4].RegularHours := 152;
  Result[4].OvertimeHours := 6;
  Result[4].OvertimeCost := 121.15;

  Result[5].EmployeeId := 6;
  Result[5].Name := 'David Chen';
  Result[5].RegularHours := 160;
  Result[5].OvertimeHours := 10;
  Result[5].OvertimeCost := 201.92;

  Result[6].EmployeeId := 7;
  Result[6].Name := 'Emily Brown';
  Result[6].RegularHours := 144;
  Result[6].OvertimeHours := 0;
  Result[6].OvertimeCost := 0;

  Result[7].EmployeeId := 8;
  Result[7].Name := 'Miguel Torres';
  Result[7].RegularHours := 160;
  Result[7].OvertimeHours := 15;
  Result[7].OvertimeCost := 259.62;
end;

function TFinancePersonnelBL.GetHeadcountByPosition: TArray<THeadcountRecord>;
begin
  SetLength(Result, 7);

  Result[0].Position := 'Waiter';
  Result[0].Count := 2;
  Result[0].AvgSalary := 28000;

  Result[1].Position := 'Head Chef';
  Result[1].Count := 1;
  Result[1].AvgSalary := 55000;

  Result[2].Position := 'Sous Chef';
  Result[2].Count := 1;
  Result[2].AvgSalary := 42000;

  Result[3].Position := 'Floor Manager';
  Result[3].Count := 1;
  Result[3].AvgSalary := 38000;

  Result[4].Position := 'Bartender';
  Result[4].Count := 1;
  Result[4].AvgSalary := 32000;

  Result[5].Position := 'Host';
  Result[5].Count := 1;
  Result[5].AvgSalary := 26000;

  Result[6].Position := 'Dishwasher';
  Result[6].Count := 1;
  Result[6].AvgSalary := 24000;
end;

end.
