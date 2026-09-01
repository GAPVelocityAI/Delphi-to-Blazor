unit uInventoryStockBL;

interface

uses
  System.SysUtils;

type
  TStockItem = record
    ItemId: Integer;
    ItemName: string;
    Category: string;
    UnitMeasure: string;
    CurrentQty: Double;
    MinStock: Double;
    UnitCost: Currency;
    NeedsReorder: Boolean;
  end;

  TStockMovement = record
    MovementId: Integer;
    ItemName: string;
    MovementType: string;
    Quantity: Double;
    MovementDate: TDateTime;
    Reference: string;
  end;

  TStockValuation = record
    Category: string;
    ItemCount: Integer;
    TotalQuantity: Double;
    TotalValue: Currency;
  end;

  TInventoryStockBL = class
  public
    function GetStockItems: TArray<TStockItem>;
    function GetStockMovements: TArray<TStockMovement>;
    function GetStockValuation: TArray<TStockValuation>;
  end;

const
  SQL_GET_STOCK_ITEMS =
    'SELECT si.ItemId, si.ItemName, si.Category, si.UnitMeasure, ' +
    '       si.CurrentQty, si.MinStock, si.UnitCost, ' +
    '       CASE WHEN si.CurrentQty <= si.MinStock THEN 1 ELSE 0 END AS NeedsReorder ' +
    'FROM StockItems si ' +
    'ORDER BY si.Category, si.ItemName';

  SQL_GET_STOCK_MOVEMENTS =
    'SELECT sm.MovementId, si.ItemName, sm.MovementType, sm.Quantity, ' +
    '       sm.MovementDate, sm.Reference ' +
    'FROM StockMovements sm ' +
    'INNER JOIN StockItems si ON si.ItemId = sm.ItemId ' +
    'ORDER BY sm.MovementDate DESC';

  SQL_GET_STOCK_VALUATION =
    'SELECT si.Category, COUNT(*) AS ItemCount, ' +
    '       SUM(si.CurrentQty) AS TotalQuantity, ' +
    '       SUM(si.CurrentQty * si.UnitCost) AS TotalValue ' +
    'FROM StockItems si ' +
    'GROUP BY si.Category ' +
    'ORDER BY TotalValue DESC';

  SQL_UPDATE_STOCK_QTY =
    'UPDATE StockItems SET CurrentQty = :NewQty, ' +
    '       LastUpdated = GETDATE() ' +
    'WHERE ItemId = :ItemId';

implementation

{ TInventoryStockBL }

function TInventoryStockBL.GetStockItems: TArray<TStockItem>;

  procedure AddItem(var Arr: TArray<TStockItem>; AId: Integer;
    const AName, ACategory, AUnit: string; AQty, AMin: Double; ACost: Currency);
  var
    Idx: Integer;
  begin
    Idx := Length(Arr);
    SetLength(Arr, Idx + 1);
    Arr[Idx].ItemId := AId;
    Arr[Idx].ItemName := AName;
    Arr[Idx].Category := ACategory;
    Arr[Idx].UnitMeasure := AUnit;
    Arr[Idx].CurrentQty := AQty;
    Arr[Idx].MinStock := AMin;
    Arr[Idx].UnitCost := ACost;
    Arr[Idx].NeedsReorder := AQty <= AMin;
  end;

begin
  Result := nil;
  AddItem(Result, 1,  'Olive Oil',     'Oils',       'liters', 12, 5,  18.50);
  AddItem(Result, 2,  'Flour',         'Dry Goods',  'kg',     45, 20,  2.80);
  AddItem(Result, 3,  'Tomatoes',      'Produce',    'kg',     30, 10,  3.20);
  AddItem(Result, 4,  'Mozzarella',    'Dairy',      'kg',     15, 5,  12.00);
  AddItem(Result, 5,  'Salmon Fillet', 'Seafood',    'kg',      8, 3,  22.00);
  AddItem(Result, 6,  'Chicken Breast','Meat',       'kg',     20, 8,   9.50);
  AddItem(Result, 7,  'Garlic',        'Produce',    'kg',      5, 2,   6.00);
  AddItem(Result, 8,  'Basil',         'Herbs',      'kg',      3, 1,  15.00);
  AddItem(Result, 9,  'Parmesan',      'Dairy',      'kg',     10, 3,  28.00);
  AddItem(Result, 10, 'Rice',          'Dry Goods',  'kg',     25, 10,  4.50);
  AddItem(Result, 11, 'Butter',        'Dairy',      'kg',      8, 3,   7.50);
  AddItem(Result, 12, 'Lemons',        'Produce',    'kg',      6, 2,   4.00);
  AddItem(Result, 13, 'Wine Vinegar',  'Condiments', 'liters',  4, 2,   8.00);
  AddItem(Result, 14, 'Black Pepper',  'Spices',     'kg',      2, 1,  35.00);
  AddItem(Result, 15, 'Heavy Cream',   'Dairy',      'liters', 10, 4,   5.50);
end;

function TInventoryStockBL.GetStockMovements: TArray<TStockMovement>;

  procedure AddMovement(var Arr: TArray<TStockMovement>; AId: Integer;
    const AName, AType: string; AQty: Double; ADate: TDateTime;
    const ARef: string);
  var
    Idx: Integer;
  begin
    Idx := Length(Arr);
    SetLength(Arr, Idx + 1);
    Arr[Idx].MovementId := AId;
    Arr[Idx].ItemName := AName;
    Arr[Idx].MovementType := AType;
    Arr[Idx].Quantity := AQty;
    Arr[Idx].MovementDate := ADate;
    Arr[Idx].Reference := ARef;
  end;

begin
  Result := nil;
  AddMovement(Result, 1,  'Olive Oil',      'IN',  24,  EncodeDate(2026, 7, 28), 'PO-2026-045');
  AddMovement(Result, 2,  'Flour',          'IN',  50,  EncodeDate(2026, 7, 27), 'PO-2026-044');
  AddMovement(Result, 3,  'Tomatoes',       'OUT', 8,   EncodeDate(2026, 8, 1),  'Order #1023');
  AddMovement(Result, 4,  'Mozzarella',     'OUT', 5,   EncodeDate(2026, 8, 1),  'Order #1024');
  AddMovement(Result, 5,  'Salmon Fillet',  'IN',  12,  EncodeDate(2026, 7, 30), 'PO-2026-046');
  AddMovement(Result, 6,  'Chicken Breast', 'OUT', 10,  EncodeDate(2026, 8, 2),  'Order #1025');
  AddMovement(Result, 7,  'Basil',          'OUT', 1.5, EncodeDate(2026, 8, 2),  'Order #1026');
  AddMovement(Result, 8,  'Parmesan',       'IN',  15,  EncodeDate(2026, 7, 25), 'PO-2026-042');
  AddMovement(Result, 9,  'Rice',           'OUT', 5,   EncodeDate(2026, 8, 3),  'Order #1027');
  AddMovement(Result, 10, 'Heavy Cream',    'IN',  20,  EncodeDate(2026, 7, 29), 'PO-2026-043');
end;

function TInventoryStockBL.GetStockValuation: TArray<TStockValuation>;

  procedure AddValuation(var Arr: TArray<TStockValuation>;
    const ACategory: string; ACount: Integer; AQty: Double; AValue: Currency);
  var
    Idx: Integer;
  begin
    Idx := Length(Arr);
    SetLength(Arr, Idx + 1);
    Arr[Idx].Category := ACategory;
    Arr[Idx].ItemCount := ACount;
    Arr[Idx].TotalQuantity := AQty;
    Arr[Idx].TotalValue := AValue;
  end;

begin
  Result := nil;
  AddValuation(Result, 'Dairy',      4, 43, 601.00);
  AddValuation(Result, 'Produce',    3, 41, 228.00);
  AddValuation(Result, 'Oils',       1, 12, 222.00);
  AddValuation(Result, 'Meat',       1, 20, 190.00);
  AddValuation(Result, 'Seafood',    1,  8, 176.00);
  AddValuation(Result, 'Dry Goods',  2, 70, 238.50);
  AddValuation(Result, 'Spices',     1,  2,  70.00);
  AddValuation(Result, 'Herbs',      1,  3,  45.00);
  AddValuation(Result, 'Condiments', 1,  4,  32.00);
end;

end.
