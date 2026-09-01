unit uFinancePayrollBL;

interface

uses
  System.SysUtils, System.Classes;

const
  SQL_PAYROLL_SUMMARY =
    'SELECT p.Period, SUM(p.GrossPay) AS TotalGross, SUM(p.Deductions) AS TotalDeductions, ' +
    'SUM(p.NetPay) AS TotalNet, SUM(p.TaxWithholding) AS TotalTax, ' +
    'COUNT(DISTINCT p.EmployeeId) AS EmployeeCount ' +
    'FROM Payroll p GROUP BY p.Period ORDER BY p.Period DESC';

  SQL_TAX_WITHHOLDINGS =
    'SELECT p.EmployeeId, e.FirstName + '' '' + e.LastName AS Name, ' +
    'p.FederalTax, p.StateTax, p.SocialSecurity, p.Medicare ' +
    'FROM PayrollTax p INNER JOIN Employees e ON p.EmployeeId = e.EmployeeId ' +
    'WHERE p.Period = :Period ORDER BY Name';

  SQL_PAYROLL_BY_DEPARTMENT =
    'SELECT d.DepartmentName AS Department, COUNT(e.EmployeeId) AS EmployeeCount, ' +
    'SUM(p.GrossPay) AS TotalPayroll, AVG(e.Salary) AS AvgSalary ' +
    'FROM Payroll p INNER JOIN Employees e ON p.EmployeeId = e.EmployeeId ' +
    'INNER JOIN Departments d ON e.DepartmentId = d.DepartmentId ' +
    'WHERE p.Period = :Period GROUP BY d.DepartmentName ORDER BY TotalPayroll DESC';

type
  TPayrollSummaryRecord = record
    Period: string;
    TotalGross: Currency;
    TotalDeductions: Currency;
    TotalNet: Currency;
    TotalTax: Currency;
    EmployeeCount: Integer;
  end;

  TTaxWithholdingRecord = record
    EmployeeId: Integer;
    Name: string;
    FederalTax: Currency;
    StateTax: Currency;
    SocialSecurity: Currency;
    Medicare: Currency;
  end;

  TPayrollByDeptRecord = record
    Department: string;
    EmployeeCount: Integer;
    TotalPayroll: Currency;
    AvgSalary: Currency;
  end;

  TFinancePayrollBL = class
  public
    function GetPayrollSummary: TArray<TPayrollSummaryRecord>;
    function GetTaxWithholdings: TArray<TTaxWithholdingRecord>;
    function GetPayrollByDepartment: TArray<TPayrollByDeptRecord>;
  end;

implementation

{ TFinancePayrollBL }

function TFinancePayrollBL.GetPayrollSummary: TArray<TPayrollSummaryRecord>;
begin
  SetLength(Result, 6);

  Result[0].Period := '2026-07';
  Result[0].TotalGross := 30416.67;
  Result[0].TotalDeductions := 6083.33;
  Result[0].TotalNet := 24333.33;
  Result[0].TotalTax := 4562.50;
  Result[0].EmployeeCount := 10;

  Result[1].Period := '2026-06';
  Result[1].TotalGross := 30416.67;
  Result[1].TotalDeductions := 6083.33;
  Result[1].TotalNet := 24333.33;
  Result[1].TotalTax := 4562.50;
  Result[1].EmployeeCount := 10;

  Result[2].Period := '2026-05';
  Result[2].TotalGross := 28916.67;
  Result[2].TotalDeductions := 5783.33;
  Result[2].TotalNet := 23133.33;
  Result[2].TotalTax := 4337.50;
  Result[2].EmployeeCount := 9;

  Result[3].Period := '2026-04';
  Result[3].TotalGross := 28916.67;
  Result[3].TotalDeductions := 5783.33;
  Result[3].TotalNet := 23133.33;
  Result[3].TotalTax := 4337.50;
  Result[3].EmployeeCount := 9;

  Result[4].Period := '2026-03';
  Result[4].TotalGross := 28916.67;
  Result[4].TotalDeductions := 5783.33;
  Result[4].TotalNet := 23133.33;
  Result[4].TotalTax := 4337.50;
  Result[4].EmployeeCount := 9;

  Result[5].Period := '2026-02';
  Result[5].TotalGross := 26416.67;
  Result[5].TotalDeductions := 5283.33;
  Result[5].TotalNet := 21133.33;
  Result[5].TotalTax := 3962.50;
  Result[5].EmployeeCount := 8;
end;

function TFinancePayrollBL.GetTaxWithholdings: TArray<TTaxWithholdingRecord>;
begin
  SetLength(Result, 9);

  Result[0].EmployeeId := 9;
  Result[0].Name := 'Robert Johnson';
  Result[0].FederalTax := 1033.33;
  Result[0].StateTax := 310.00;
  Result[0].SocialSecurity := 320.33;
  Result[0].Medicare := 74.90;

  Result[1].EmployeeId := 1;
  Result[1].Name := 'Maria Garcia';
  Result[1].FederalTax := 916.67;
  Result[1].StateTax := 275.00;
  Result[1].SocialSecurity := 284.17;
  Result[1].Medicare := 66.46;

  Result[2].EmployeeId := 2;
  Result[2].Name := 'Carlos Reyes';
  Result[2].FederalTax := 700.00;
  Result[2].StateTax := 210.00;
  Result[2].SocialSecurity := 217.00;
  Result[2].Medicare := 50.75;

  Result[3].EmployeeId := 3;
  Result[3].Name := 'Ana Martinez';
  Result[3].FederalTax := 633.33;
  Result[3].StateTax := 190.00;
  Result[3].SocialSecurity := 196.33;
  Result[3].Medicare := 45.92;

  Result[4].EmployeeId := 4;
  Result[4].Name := 'James Wilson';
  Result[4].FederalTax := 533.33;
  Result[4].StateTax := 160.00;
  Result[4].SocialSecurity := 165.33;
  Result[4].Medicare := 38.67;

  Result[5].EmployeeId := 5;
  Result[5].Name := 'Sofia Lopez';
  Result[5].FederalTax := 466.67;
  Result[5].StateTax := 140.00;
  Result[5].SocialSecurity := 144.67;
  Result[5].Medicare := 33.83;

  Result[6].EmployeeId := 6;
  Result[6].Name := 'David Chen';
  Result[6].FederalTax := 466.67;
  Result[6].StateTax := 140.00;
  Result[6].SocialSecurity := 144.67;
  Result[6].Medicare := 33.83;

  Result[7].EmployeeId := 7;
  Result[7].Name := 'Emily Brown';
  Result[7].FederalTax := 433.33;
  Result[7].StateTax := 130.00;
  Result[7].SocialSecurity := 134.33;
  Result[7].Medicare := 31.42;

  Result[8].EmployeeId := 8;
  Result[8].Name := 'Miguel Torres';
  Result[8].FederalTax := 400.00;
  Result[8].StateTax := 120.00;
  Result[8].SocialSecurity := 124.00;
  Result[8].Medicare := 29.00;
end;

function TFinancePayrollBL.GetPayrollByDepartment: TArray<TPayrollByDeptRecord>;
begin
  SetLength(Result, 4);

  Result[0].Department := 'Kitchen';
  Result[0].EmployeeCount := 4;
  Result[0].TotalPayroll := 12583.33;
  Result[0].AvgSalary := 42750;

  Result[1].Department := 'Front of House';
  Result[1].EmployeeCount := 3;
  Result[1].TotalPayroll := 6833.33;
  Result[1].AvgSalary := 27333;

  Result[2].Department := 'Bar';
  Result[2].EmployeeCount := 1;
  Result[2].TotalPayroll := 2666.67;
  Result[2].AvgSalary := 32000;

  Result[3].Department := 'Management';
  Result[3].EmployeeCount := 2;
  Result[3].TotalPayroll := 8333.33;
  Result[3].AvgSalary := 50000;
end;

end.
