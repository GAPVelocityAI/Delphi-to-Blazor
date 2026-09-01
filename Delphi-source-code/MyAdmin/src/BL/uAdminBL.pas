unit uAdminBL;

interface

uses
  System.SysUtils, System.Classes, uCommonTypes;

const
  SQL_GET_EMPLOYEES =
    'SELECT EmployeeId, FirstName, LastName, Position, HireDate, Salary, Active ' +
    'FROM Employees ORDER BY LastName, FirstName';

  SQL_GET_ASSETS =
    'SELECT AssetId, AssetName, Category, PurchaseDate, Value, DepreciatedValue, Status ' +
    'FROM Assets ORDER BY AssetName';

  SQL_GET_PAYROLL =
    'SELECT PayrollId, EmployeeId, EmployeeName, Period, GrossPay, Deductions, NetPay, PayDate ' +
    'FROM Payroll WHERE Period = :Period ORDER BY EmployeeName';

  SQL_INSERT_EMPLOYEE =
    'INSERT INTO Employees (FirstName, LastName, Position, HireDate, Salary, Active) ' +
    'VALUES (:FirstName, :LastName, :Position, :HireDate, :Salary, :Active)';

  SQL_UPDATE_ASSET =
    'UPDATE Assets SET AssetName = :AssetName, Category = :Category, Value = :Value, ' +
    'DepreciatedValue = :DepreciatedValue, Status = :Status WHERE AssetId = :AssetId';

type
  TAdminBL = class
  private
    class var FEmployees: TArray<TEmployeeInfo>;
    class var FAssets: TArray<TAssetInfo>;
    class var FPayroll: TArray<TPayrollInfo>;
    class var FNextEmployeeId, FNextAssetId, FNextPayrollId: Integer;
    class var FInitialized: Boolean;
    class procedure EnsureInitialized;
  public
    function GetEmployees: TArray<TEmployeeInfo>;
    function GetActiveEmployees: TArray<TEmployeeInfo>;
    function GetAssets: TArray<TAssetInfo>;
    function GetPayroll: TArray<TPayrollInfo>;

    procedure AddEmployee(var AEmployee: TEmployeeInfo);
    procedure UpdateEmployee(const AEmployee: TEmployeeInfo);
    procedure DeleteEmployee(AEmployeeId: Integer);

    procedure AddAsset(var AAsset: TAssetInfo);
    procedure UpdateAsset(const AAsset: TAssetInfo);
    procedure DeleteAsset(AAssetId: Integer);

    procedure AddPayroll(var APayroll: TPayrollInfo);
    procedure UpdatePayroll(const APayroll: TPayrollInfo);
    procedure DeletePayroll(APayrollId: Integer);
  end;

implementation

{ TAdminBL }

class procedure TAdminBL.EnsureInitialized;
var
  I: Integer;
  Gross, Deduct: Currency;
begin
  if FInitialized then
    Exit;

  FInitialized := True;

  // Initialize Employees
  SetLength(FEmployees, 10);

  FEmployees[0].EmployeeId := 1;
  FEmployees[0].FirstName := 'Maria';
  FEmployees[0].LastName := 'Garcia';
  FEmployees[0].Position := 'Head Chef';
  FEmployees[0].HireDate := EncodeDate(2019, 3, 15);
  FEmployees[0].Salary := 55000;
  FEmployees[0].Active := True;

  FEmployees[1].EmployeeId := 2;
  FEmployees[1].FirstName := 'Carlos';
  FEmployees[1].LastName := 'Reyes';
  FEmployees[1].Position := 'Sous Chef';
  FEmployees[1].HireDate := EncodeDate(2020, 6, 1);
  FEmployees[1].Salary := 42000;
  FEmployees[1].Active := True;

  FEmployees[2].EmployeeId := 3;
  FEmployees[2].FirstName := 'Ana';
  FEmployees[2].LastName := 'Martinez';
  FEmployees[2].Position := 'Floor Manager';
  FEmployees[2].HireDate := EncodeDate(2018, 11, 20);
  FEmployees[2].Salary := 38000;
  FEmployees[2].Active := True;

  FEmployees[3].EmployeeId := 4;
  FEmployees[3].FirstName := 'James';
  FEmployees[3].LastName := 'Wilson';
  FEmployees[3].Position := 'Bartender';
  FEmployees[3].HireDate := EncodeDate(2021, 1, 10);
  FEmployees[3].Salary := 32000;
  FEmployees[3].Active := True;

  FEmployees[4].EmployeeId := 5;
  FEmployees[4].FirstName := 'Sofia';
  FEmployees[4].LastName := 'Lopez';
  FEmployees[4].Position := 'Waiter';
  FEmployees[4].HireDate := EncodeDate(2022, 4, 5);
  FEmployees[4].Salary := 28000;
  FEmployees[4].Active := True;

  FEmployees[5].EmployeeId := 6;
  FEmployees[5].FirstName := 'David';
  FEmployees[5].LastName := 'Chen';
  FEmployees[5].Position := 'Waiter';
  FEmployees[5].HireDate := EncodeDate(2022, 8, 18);
  FEmployees[5].Salary := 28000;
  FEmployees[5].Active := True;

  FEmployees[6].EmployeeId := 7;
  FEmployees[6].FirstName := 'Emily';
  FEmployees[6].LastName := 'Brown';
  FEmployees[6].Position := 'Host';
  FEmployees[6].HireDate := EncodeDate(2023, 2, 14);
  FEmployees[6].Salary := 26000;
  FEmployees[6].Active := True;

  FEmployees[7].EmployeeId := 8;
  FEmployees[7].FirstName := 'Miguel';
  FEmployees[7].LastName := 'Torres';
  FEmployees[7].Position := 'Dishwasher';
  FEmployees[7].HireDate := EncodeDate(2023, 5, 1);
  FEmployees[7].Salary := 24000;
  FEmployees[7].Active := True;

  FEmployees[8].EmployeeId := 9;
  FEmployees[8].FirstName := 'Robert';
  FEmployees[8].LastName := 'Johnson';
  FEmployees[8].Position := 'General Manager';
  FEmployees[8].HireDate := EncodeDate(2017, 9, 1);
  FEmployees[8].Salary := 62000;
  FEmployees[8].Active := True;

  FEmployees[9].EmployeeId := 10;
  FEmployees[9].FirstName := 'Linda';
  FEmployees[9].LastName := 'Davis';
  FEmployees[9].Position := 'Line Cook';
  FEmployees[9].HireDate := EncodeDate(2021, 7, 22);
  FEmployees[9].Salary := 30000;
  FEmployees[9].Active := False;

  FNextEmployeeId := 11;

  // Initialize Assets
  SetLength(FAssets, 8);

  FAssets[0].AssetId := 1;
  FAssets[0].AssetName := 'Commercial Oven';
  FAssets[0].Category := 'Kitchen Equipment';
  FAssets[0].PurchaseDate := EncodeDate(2020, 1, 15);
  FAssets[0].Value := 15000;
  FAssets[0].DepreciatedValue := 12000;
  FAssets[0].Status := 'Active';

  FAssets[1].AssetId := 2;
  FAssets[1].AssetName := 'Walk-in Cooler';
  FAssets[1].Category := 'Kitchen Equipment';
  FAssets[1].PurchaseDate := EncodeDate(2019, 6, 10);
  FAssets[1].Value := 8000;
  FAssets[1].DepreciatedValue := 6500;
  FAssets[1].Status := 'Active';

  FAssets[2].AssetId := 3;
  FAssets[2].AssetName := 'POS System';
  FAssets[2].Category := 'Technology';
  FAssets[2].PurchaseDate := EncodeDate(2021, 3, 20);
  FAssets[2].Value := 3500;
  FAssets[2].DepreciatedValue := 2800;
  FAssets[2].Status := 'Active';

  FAssets[3].AssetId := 4;
  FAssets[3].AssetName := 'Dining Furniture Set';
  FAssets[3].Category := 'Furniture';
  FAssets[3].PurchaseDate := EncodeDate(2018, 11, 5);
  FAssets[3].Value := 12000;
  FAssets[3].DepreciatedValue := 7200;
  FAssets[3].Status := 'Active';

  FAssets[4].AssetId := 5;
  FAssets[4].AssetName := 'Delivery Van';
  FAssets[4].Category := 'Vehicle';
  FAssets[4].PurchaseDate := EncodeDate(2022, 2, 28);
  FAssets[4].Value := 25000;
  FAssets[4].DepreciatedValue := 21000;
  FAssets[4].Status := 'Active';

  FAssets[5].AssetId := 6;
  FAssets[5].AssetName := 'Industrial Dishwasher';
  FAssets[5].Category := 'Kitchen Equipment';
  FAssets[5].PurchaseDate := EncodeDate(2020, 8, 12);
  FAssets[5].Value := 5500;
  FAssets[5].DepreciatedValue := 4200;
  FAssets[5].Status := 'Active';

  FAssets[6].AssetId := 7;
  FAssets[6].AssetName := 'Security Camera System';
  FAssets[6].Category := 'Technology';
  FAssets[6].PurchaseDate := EncodeDate(2021, 5, 30);
  FAssets[6].Value := 2800;
  FAssets[6].DepreciatedValue := 2100;
  FAssets[6].Status := 'Active';

  FAssets[7].AssetId := 8;
  FAssets[7].AssetName := 'Bar Equipment';
  FAssets[7].Category := 'Bar';
  FAssets[7].PurchaseDate := EncodeDate(2019, 9, 15);
  FAssets[7].Value := 6000;
  FAssets[7].DepreciatedValue := 4000;
  FAssets[7].Status := 'Needs Repair';

  FNextAssetId := 9;

  // Initialize Payroll
  SetLength(FPayroll, Length(FEmployees));

  for I := 0 to High(FEmployees) do
  begin
    Gross := FEmployees[I].Salary / 12;
    Deduct := Gross * 0.20;

    FPayroll[I].PayrollId := I + 1;
    FPayroll[I].EmployeeId := FEmployees[I].EmployeeId;
    FPayroll[I].EmployeeName := FEmployees[I].FirstName + ' ' + FEmployees[I].LastName;
    FPayroll[I].Period := '2026-07';
    FPayroll[I].GrossPay := Gross;
    FPayroll[I].Deductions := Deduct;
    FPayroll[I].NetPay := Gross - Deduct;
    FPayroll[I].PayDate := EncodeDate(2026, 7, 31);
  end;

  FNextPayrollId := 11;
end;

function TAdminBL.GetEmployees: TArray<TEmployeeInfo>;
begin
  EnsureInitialized;
  Result := Copy(FEmployees);
end;

function TAdminBL.GetActiveEmployees: TArray<TEmployeeInfo>;
var
  I, Count: Integer;
begin
  EnsureInitialized;
  Count := 0;
  for I := 0 to High(FEmployees) do
    if FEmployees[I].Active then
      Inc(Count);

  SetLength(Result, Count);
  Count := 0;
  for I := 0 to High(FEmployees) do
    if FEmployees[I].Active then
    begin
      Result[Count] := FEmployees[I];
      Inc(Count);
    end;
end;

function TAdminBL.GetAssets: TArray<TAssetInfo>;
begin
  EnsureInitialized;
  Result := Copy(FAssets);
end;

function TAdminBL.GetPayroll: TArray<TPayrollInfo>;
begin
  EnsureInitialized;
  Result := Copy(FPayroll);
end;

{ Employee CRUD }

procedure TAdminBL.AddEmployee(var AEmployee: TEmployeeInfo);
begin
  EnsureInitialized;
  AEmployee.EmployeeId := FNextEmployeeId;
  Inc(FNextEmployeeId);
  SetLength(FEmployees, Length(FEmployees) + 1);
  FEmployees[High(FEmployees)] := AEmployee;
end;

procedure TAdminBL.UpdateEmployee(const AEmployee: TEmployeeInfo);
var
  I: Integer;
begin
  EnsureInitialized;
  for I := 0 to High(FEmployees) do
    if FEmployees[I].EmployeeId = AEmployee.EmployeeId then
    begin
      FEmployees[I] := AEmployee;
      Exit;
    end;
end;

procedure TAdminBL.DeleteEmployee(AEmployeeId: Integer);
var
  I, J: Integer;
begin
  EnsureInitialized;
  for I := 0 to High(FEmployees) do
    if FEmployees[I].EmployeeId = AEmployeeId then
    begin
      for J := I to High(FEmployees) - 1 do
        FEmployees[J] := FEmployees[J + 1];
      SetLength(FEmployees, Length(FEmployees) - 1);
      Exit;
    end;
end;

{ Asset CRUD }

procedure TAdminBL.AddAsset(var AAsset: TAssetInfo);
begin
  EnsureInitialized;
  AAsset.AssetId := FNextAssetId;
  Inc(FNextAssetId);
  SetLength(FAssets, Length(FAssets) + 1);
  FAssets[High(FAssets)] := AAsset;
end;

procedure TAdminBL.UpdateAsset(const AAsset: TAssetInfo);
var
  I: Integer;
begin
  EnsureInitialized;
  for I := 0 to High(FAssets) do
    if FAssets[I].AssetId = AAsset.AssetId then
    begin
      FAssets[I] := AAsset;
      Exit;
    end;
end;

procedure TAdminBL.DeleteAsset(AAssetId: Integer);
var
  I, J: Integer;
begin
  EnsureInitialized;
  for I := 0 to High(FAssets) do
    if FAssets[I].AssetId = AAssetId then
    begin
      for J := I to High(FAssets) - 1 do
        FAssets[J] := FAssets[J + 1];
      SetLength(FAssets, Length(FAssets) - 1);
      Exit;
    end;
end;

{ Payroll CRUD }

procedure TAdminBL.AddPayroll(var APayroll: TPayrollInfo);
begin
  EnsureInitialized;
  APayroll.PayrollId := FNextPayrollId;
  Inc(FNextPayrollId);
  APayroll.NetPay := APayroll.GrossPay - APayroll.Deductions;
  SetLength(FPayroll, Length(FPayroll) + 1);
  FPayroll[High(FPayroll)] := APayroll;
end;

procedure TAdminBL.UpdatePayroll(const APayroll: TPayrollInfo);
var
  I: Integer;
begin
  EnsureInitialized;
  for I := 0 to High(FPayroll) do
    if FPayroll[I].PayrollId = APayroll.PayrollId then
    begin
      FPayroll[I] := APayroll;
      Exit;
    end;
end;

procedure TAdminBL.DeletePayroll(APayrollId: Integer);
var
  I, J: Integer;
begin
  EnsureInitialized;
  for I := 0 to High(FPayroll) do
    if FPayroll[I].PayrollId = APayrollId then
    begin
      for J := I to High(FPayroll) - 1 do
        FPayroll[J] := FPayroll[J + 1];
      SetLength(FPayroll, Length(FPayroll) - 1);
      Exit;
    end;
end;

end.
