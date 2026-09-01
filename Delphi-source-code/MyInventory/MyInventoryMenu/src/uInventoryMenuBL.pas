unit uInventoryMenuBL;

interface

uses
  System.SysUtils;

type
  TMenuCostItem = record
    MenuItemId: Integer;
    ItemName: string;
    Category: string;
    FoodCost: Currency;
    LaborCost: Currency;
    OverheadCost: Currency;
    TotalCost: Currency;
    SellingPrice: Currency;
    ProfitMargin: Double;
  end;

  TMenuCategorySummary = record
    Category: string;
    ItemCount: Integer;
    AvgPrice: Currency;
    AvgCost: Currency;
    AvgMargin: Currency;
  end;

  TTableAvailability = record
    TableId: Integer;
    TableNumber: Integer;
    Capacity: Integer;
    Zone: string;
    Status: string;
    IsAvailable: Boolean;
  end;

  TInventoryMenuBL = class
  public
    function GetMenuCosts: TArray<TMenuCostItem>;
    function GetMenuCategorySummary: TArray<TMenuCategorySummary>;
    function GetTableAvailability: TArray<TTableAvailability>;
  end;

const
  SQL_GET_MENU_COSTS =
    'SELECT mi.MenuItemId, mi.ItemName, mi.Category, ' +
    '       mi.FoodCost, mi.LaborCost, mi.OverheadCost, ' +
    '       (mi.FoodCost + mi.LaborCost + mi.OverheadCost) AS TotalCost, ' +
    '       mi.SellingPrice, ' +
    '       ((mi.SellingPrice - (mi.FoodCost + mi.LaborCost + mi.OverheadCost)) / mi.SellingPrice * 100) AS ProfitMargin ' +
    'FROM MenuItems mi ' +
    'ORDER BY mi.Category, mi.ItemName';

  SQL_GET_MENU_CATEGORY_SUMMARY =
    'SELECT mi.Category, COUNT(*) AS ItemCount, ' +
    '       AVG(mi.SellingPrice) AS AvgPrice, ' +
    '       AVG(mi.FoodCost + mi.LaborCost + mi.OverheadCost) AS AvgCost, ' +
    '       AVG((mi.SellingPrice - (mi.FoodCost + mi.LaborCost + mi.OverheadCost)) / mi.SellingPrice * 100) AS AvgMargin ' +
    'FROM MenuItems mi ' +
    'GROUP BY mi.Category ' +
    'ORDER BY mi.Category';

  SQL_GET_TABLE_AVAILABILITY =
    'SELECT t.TableId, t.TableNumber, t.Capacity, t.Zone, t.Status, ' +
    '       CASE WHEN t.Status = ''Available'' THEN 1 ELSE 0 END AS IsAvailable ' +
    'FROM Tables t ' +
    'ORDER BY t.Zone, t.TableNumber';

implementation

{ TInventoryMenuBL }

function TInventoryMenuBL.GetMenuCosts: TArray<TMenuCostItem>;

  procedure AddItem(var Arr: TArray<TMenuCostItem>; AId: Integer;
    const AName, ACategory: string; AFood, ALabor, AOverhead, APrice: Currency);
  var
    Idx: Integer;
    Total: Currency;
  begin
    Idx := Length(Arr);
    SetLength(Arr, Idx + 1);
    Total := AFood + ALabor + AOverhead;
    Arr[Idx].MenuItemId := AId;
    Arr[Idx].ItemName := AName;
    Arr[Idx].Category := ACategory;
    Arr[Idx].FoodCost := AFood;
    Arr[Idx].LaborCost := ALabor;
    Arr[Idx].OverheadCost := AOverhead;
    Arr[Idx].TotalCost := Total;
    Arr[Idx].SellingPrice := APrice;
    if APrice > 0 then
      Arr[Idx].ProfitMargin := ((APrice - Total) / APrice) * 100
    else
      Arr[Idx].ProfitMargin := 0;
  end;

begin
  Result := nil;
  AddItem(Result, 1,  'Caesar Salad',      'Appetizer',    3.75, 1.50, 0.80, 12.50);
  AddItem(Result, 2,  'Bruschetta',        'Appetizer',    2.40, 1.20, 0.60, 9.50);
  AddItem(Result, 3,  'Soup of the Day',   'Appetizer',    2.10, 1.00, 0.50, 8.00);
  AddItem(Result, 4,  'Grilled Salmon',    'Main Course',  8.50, 3.00, 1.50, 24.00);
  AddItem(Result, 5,  'Margherita Pizza',  'Main Course',  4.20, 2.00, 1.00, 16.00);
  AddItem(Result, 6,  'Chicken Parmesan',  'Main Course',  6.80, 2.50, 1.20, 19.50);
  AddItem(Result, 7,  'Risotto',           'Main Course',  5.10, 2.20, 1.00, 18.00);
  AddItem(Result, 8,  'Seafood Pasta',     'Main Course',  9.20, 3.00, 1.50, 22.00);
  AddItem(Result, 9,  'Tiramisu',          'Dessert',      3.20, 1.00, 0.50, 10.00);
  AddItem(Result, 10, 'Panna Cotta',       'Dessert',      2.80, 0.80, 0.40, 9.00);
  AddItem(Result, 11, 'Lemonade',          'Beverage',     0.85, 0.50, 0.30, 5.00);
  AddItem(Result, 12, 'Espresso',          'Beverage',     0.60, 0.40, 0.25, 4.00);
end;

function TInventoryMenuBL.GetMenuCategorySummary: TArray<TMenuCategorySummary>;

  procedure AddCategory(var Arr: TArray<TMenuCategorySummary>;
    const ACategory: string; ACount: Integer; AAvgPrice, AAvgCost, AAvgMargin: Currency);
  var
    Idx: Integer;
  begin
    Idx := Length(Arr);
    SetLength(Arr, Idx + 1);
    Arr[Idx].Category := ACategory;
    Arr[Idx].ItemCount := ACount;
    Arr[Idx].AvgPrice := AAvgPrice;
    Arr[Idx].AvgCost := AAvgCost;
    Arr[Idx].AvgMargin := AAvgMargin;
  end;

begin
  Result := nil;
  AddCategory(Result, 'Appetizer',   3, 10.00,  3.45, 65.50);
  AddCategory(Result, 'Main Course', 5, 19.90,  9.40, 52.76);
  AddCategory(Result, 'Dessert',     2,  9.50,  2.93, 69.16);
  AddCategory(Result, 'Beverage',    2,  4.50,  0.97, 78.44);
end;

function TInventoryMenuBL.GetTableAvailability: TArray<TTableAvailability>;

  procedure AddTable(var Arr: TArray<TTableAvailability>; AId, ANumber,
    ACapacity: Integer; const AZone, AStatus: string);
  var
    Idx: Integer;
  begin
    Idx := Length(Arr);
    SetLength(Arr, Idx + 1);
    Arr[Idx].TableId := AId;
    Arr[Idx].TableNumber := ANumber;
    Arr[Idx].Capacity := ACapacity;
    Arr[Idx].Zone := AZone;
    Arr[Idx].Status := AStatus;
    Arr[Idx].IsAvailable := (AStatus = 'Available');
  end;

begin
  Result := nil;
  AddTable(Result, 1,  1,  2, 'Indoor',   'Occupied');
  AddTable(Result, 2,  2,  2, 'Indoor',   'Available');
  AddTable(Result, 3,  3,  4, 'Indoor',   'Occupied');
  AddTable(Result, 4,  4,  4, 'Indoor',   'Reserved');
  AddTable(Result, 5,  5,  6, 'Indoor',   'Available');
  AddTable(Result, 6,  6,  2, 'Bar',      'Occupied');
  AddTable(Result, 7,  7,  2, 'Bar',      'Available');
  AddTable(Result, 8,  8,  4, 'Bar',      'Occupied');
  AddTable(Result, 9,  9,  4, 'Patio',    'Available');
  AddTable(Result, 10, 10, 6, 'Patio',    'Occupied');
  AddTable(Result, 11, 11, 8, 'Patio',    'Available');
  AddTable(Result, 12, 12, 2, 'Patio',    'Reserved');
  AddTable(Result, 13, 13, 4, 'Private',  'Available');
  AddTable(Result, 14, 14, 8, 'Private',  'Reserved');
  AddTable(Result, 15, 15, 10,'Private',  'Available');
end;

end.
