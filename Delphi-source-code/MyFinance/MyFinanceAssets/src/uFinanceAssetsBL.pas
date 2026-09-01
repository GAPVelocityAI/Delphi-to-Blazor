unit uFinanceAssetsBL;

interface

uses
  System.SysUtils, System.Classes;

const
  SQL_ASSET_DEPRECIATION_REPORT =
    'SELECT a.AssetId, a.AssetName, a.Value AS OriginalValue, ' +
    'a.DepreciatedValue AS CurrentValue, ' +
    '(a.Value - a.DepreciatedValue) / a.UsefulLifeYears AS AnnualDepreciation, ' +
    'a.UsefulLifeYears - DATEDIFF(YEAR, a.PurchaseDate, GETDATE()) AS YearsRemaining ' +
    'FROM Assets a ORDER BY a.AssetName';

  SQL_ASSET_VALUE_BY_CATEGORY =
    'SELECT a.Category, SUM(a.Value) AS TotalOriginal, ' +
    'SUM(a.DepreciatedValue) AS TotalCurrent, COUNT(*) AS AssetCount ' +
    'FROM Assets a GROUP BY a.Category ORDER BY TotalOriginal DESC';

  SQL_ASSET_MAINTENANCE_SCHEDULE =
    'SELECT m.AssetId, a.AssetName, m.LastMaintenanceDate, m.NextMaintenanceDate, m.EstimatedCost ' +
    'FROM AssetMaintenance m INNER JOIN Assets a ON m.AssetId = a.AssetId ' +
    'ORDER BY m.NextMaintenanceDate';

type
  TDepreciationRecord = record
    AssetId: Integer;
    AssetName: string;
    OriginalValue: Currency;
    CurrentValue: Currency;
    AnnualDepreciation: Currency;
    YearsRemaining: Integer;
  end;

  TAssetCategoryRecord = record
    Category: string;
    TotalOriginal: Currency;
    TotalCurrent: Currency;
    AssetCount: Integer;
  end;

  TMaintenanceRecord = record
    AssetId: Integer;
    AssetName: string;
    LastMaintenance: TDateTime;
    NextMaintenance: TDateTime;
    Cost: Currency;
  end;

  TFinanceAssetsBL = class
  public
    function GetDepreciationReport: TArray<TDepreciationRecord>;
    function GetAssetValueByCategory: TArray<TAssetCategoryRecord>;
    function GetMaintenanceSchedule: TArray<TMaintenanceRecord>;
  end;

implementation

{ TFinanceAssetsBL }

function TFinanceAssetsBL.GetDepreciationReport: TArray<TDepreciationRecord>;
begin
  SetLength(Result, 8);

  Result[0].AssetId := 1;
  Result[0].AssetName := 'Commercial Oven';
  Result[0].OriginalValue := 15000;
  Result[0].CurrentValue := 12000;
  Result[0].AnnualDepreciation := 1500;
  Result[0].YearsRemaining := 6;

  Result[1].AssetId := 2;
  Result[1].AssetName := 'Walk-in Cooler';
  Result[1].OriginalValue := 8000;
  Result[1].CurrentValue := 6500;
  Result[1].AnnualDepreciation := 750;
  Result[1].YearsRemaining := 8;

  Result[2].AssetId := 3;
  Result[2].AssetName := 'POS System';
  Result[2].OriginalValue := 3500;
  Result[2].CurrentValue := 2800;
  Result[2].AnnualDepreciation := 700;
  Result[2].YearsRemaining := 3;

  Result[3].AssetId := 4;
  Result[3].AssetName := 'Dining Furniture Set';
  Result[3].OriginalValue := 12000;
  Result[3].CurrentValue := 7200;
  Result[3].AnnualDepreciation := 1200;
  Result[3].YearsRemaining := 4;

  Result[4].AssetId := 5;
  Result[4].AssetName := 'Delivery Van';
  Result[4].OriginalValue := 25000;
  Result[4].CurrentValue := 21000;
  Result[4].AnnualDepreciation := 3333;
  Result[4].YearsRemaining := 5;

  Result[5].AssetId := 6;
  Result[5].AssetName := 'Industrial Dishwasher';
  Result[5].OriginalValue := 5500;
  Result[5].CurrentValue := 4200;
  Result[5].AnnualDepreciation := 650;
  Result[5].YearsRemaining := 6;

  Result[6].AssetId := 7;
  Result[6].AssetName := 'Security Camera System';
  Result[6].OriginalValue := 2800;
  Result[6].CurrentValue := 2100;
  Result[6].AnnualDepreciation := 560;
  Result[6].YearsRemaining := 2;

  Result[7].AssetId := 8;
  Result[7].AssetName := 'Bar Equipment';
  Result[7].OriginalValue := 6000;
  Result[7].CurrentValue := 4000;
  Result[7].AnnualDepreciation := 857;
  Result[7].YearsRemaining := 3;
end;

function TFinanceAssetsBL.GetAssetValueByCategory: TArray<TAssetCategoryRecord>;
begin
  SetLength(Result, 4);

  Result[0].Category := 'Kitchen Equipment';
  Result[0].TotalOriginal := 28500;
  Result[0].TotalCurrent := 22700;
  Result[0].AssetCount := 3;

  Result[1].Category := 'Furniture';
  Result[1].TotalOriginal := 12000;
  Result[1].TotalCurrent := 7200;
  Result[1].AssetCount := 1;

  Result[2].Category := 'Vehicle';
  Result[2].TotalOriginal := 25000;
  Result[2].TotalCurrent := 21000;
  Result[2].AssetCount := 1;

  Result[3].Category := 'Technology';
  Result[3].TotalOriginal := 6300;
  Result[3].TotalCurrent := 4900;
  Result[3].AssetCount := 2;
end;

function TFinanceAssetsBL.GetMaintenanceSchedule: TArray<TMaintenanceRecord>;
begin
  SetLength(Result, 6);

  Result[0].AssetId := 1;
  Result[0].AssetName := 'Commercial Oven';
  Result[0].LastMaintenance := EncodeDate(2026, 4, 15);
  Result[0].NextMaintenance := EncodeDate(2026, 10, 15);
  Result[0].Cost := 450;

  Result[1].AssetId := 2;
  Result[1].AssetName := 'Walk-in Cooler';
  Result[1].LastMaintenance := EncodeDate(2026, 3, 20);
  Result[1].NextMaintenance := EncodeDate(2026, 9, 20);
  Result[1].Cost := 350;

  Result[2].AssetId := 5;
  Result[2].AssetName := 'Delivery Van';
  Result[2].LastMaintenance := EncodeDate(2026, 5, 10);
  Result[2].NextMaintenance := EncodeDate(2026, 8, 10);
  Result[2].Cost := 280;

  Result[3].AssetId := 6;
  Result[3].AssetName := 'Industrial Dishwasher';
  Result[3].LastMaintenance := EncodeDate(2026, 2, 28);
  Result[3].NextMaintenance := EncodeDate(2026, 8, 28);
  Result[3].Cost := 200;

  Result[4].AssetId := 8;
  Result[4].AssetName := 'Bar Equipment';
  Result[4].LastMaintenance := EncodeDate(2026, 1, 15);
  Result[4].NextMaintenance := EncodeDate(2026, 7, 15);
  Result[4].Cost := 500;

  Result[5].AssetId := 7;
  Result[5].AssetName := 'Security Camera System';
  Result[5].LastMaintenance := EncodeDate(2026, 6, 1);
  Result[5].NextMaintenance := EncodeDate(2026, 12, 1);
  Result[5].Cost := 150;
end;

end.
