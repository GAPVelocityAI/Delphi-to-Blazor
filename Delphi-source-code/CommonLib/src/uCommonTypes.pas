unit uCommonTypes;

interface

uses
  System.SysUtils, System.Classes, Vcl.Grids;

type
  TTableStatus = (tsAvailable, tsOccupied, tsReserved, tsClosed);
  TOrderStatus = (osPending, osPreparing, osServed, osPaid, osCancelled);
  TPaymentMethod = (pmCash, pmCreditCard, pmDebitCard);

  TTableInfo = record
    TableId: Integer;
    TableNumber: Integer;
    Capacity: Integer;
    Status: TTableStatus;
    Zone: string;
  end;

  TMenuItemInfo = record
    ItemId: Integer;
    ItemName: string;
    Category: string;
    Price: Currency;
    Cost: Currency;
    Active: Boolean;
  end;

  TOrderInfo = record
    OrderId: Integer;
    TableId: Integer;
    OrderDate: TDateTime;
    Status: TOrderStatus;
    TotalAmount: Currency;
  end;

  TOrderDetailInfo = record
    DetailId: Integer;
    OrderId: Integer;
    ItemId: Integer;
    ItemName: string;
    Quantity: Integer;
    UnitPrice: Currency;
    Subtotal: Currency;
  end;

  TBillInfo = record
    BillId: Integer;
    OrderId: Integer;
    Subtotal: Currency;
    Tax: Currency;
    Tip: Currency;
    Total: Currency;
    PaymentMethod: TPaymentMethod;
    PaidDate: TDateTime;
  end;

  TEmployeeInfo = record
    EmployeeId: Integer;
    FirstName: string;
    LastName: string;
    Position: string;
    HireDate: TDateTime;
    Salary: Currency;
    Active: Boolean;
  end;

  TAssetInfo = record
    AssetId: Integer;
    AssetName: string;
    Category: string;
    PurchaseDate: TDateTime;
    Value: Currency;
    DepreciatedValue: Currency;
    Status: string;
  end;

  TPayrollInfo = record
    PayrollId: Integer;
    EmployeeId: Integer;
    EmployeeName: string;
    Period: string;
    GrossPay: Currency;
    Deductions: Currency;
    NetPay: Currency;
    PayDate: TDateTime;
  end;

  TInventoryItem = record
    InventoryId: Integer;
    ItemName: string;
    Category: string;
    Quantity: Double;
    UnitMeasure: string;
    MinStock: Double;
    UnitCost: Currency;
  end;

  TFoodCostInfo = record
    RecipeId: Integer;
    RecipeName: string;
    IngredientCount: Integer;
    TotalCost: Currency;
    SellingPrice: Currency;
    CostPercentage: Double;
  end;

  TMenuCostInfo = record
    MenuItemId: Integer;
    ItemName: string;
    Category: string;
    FoodCost: Currency;
    LaborCost: Currency;
    OverheadCost: Currency;
    TotalCost: Currency;
    Price: Currency;
    Margin: Double;
  end;

  TProviderInfo = record
    ProviderId: Integer;
    CompanyName: string;
    ContactName: string;
    Phone: string;
    Email: string;
    Category: string;
    Rating: Integer;
    Active: Boolean;
  end;

  TSupplyInfo = record
    SupplyId: Integer;
    ProviderId: Integer;
    ProviderName: string;
    ItemName: string;
    UnitCost: Currency;
    MinOrderQty: Integer;
    LeadTimeDays: Integer;
  end;

  TGridHelper = class
    class procedure ConfigureGrid(AGrid: TStringGrid; const AColumns: array of string;
      const AWidths: array of Integer);
    class procedure ClearGrid(AGrid: TStringGrid);
  end;

  TStatusHelper = record helper for TTableStatus
    function ToString: string;
  end;

  TOrderStatusHelper = record helper for TOrderStatus
    function ToString: string;
  end;

  TPaymentMethodHelper = record helper for TPaymentMethod
    function ToString: string;
  end;

implementation

{ TGridHelper }

class procedure TGridHelper.ConfigureGrid(AGrid: TStringGrid;
  const AColumns: array of string; const AWidths: array of Integer);
var
  I: Integer;
begin
  AGrid.ColCount := Length(AColumns);
  AGrid.RowCount := 2;
  AGrid.FixedRows := 1;
  AGrid.DefaultRowHeight := 22;
  AGrid.Options := AGrid.Options + [goRowSelect, goColSizing];
  for I := 0 to High(AColumns) do
  begin
    AGrid.Cells[I, 0] := AColumns[I];
    if I <= High(AWidths) then
      AGrid.ColWidths[I] := AWidths[I];
  end;
end;

class procedure TGridHelper.ClearGrid(AGrid: TStringGrid);
var
  C: Integer;
begin
  AGrid.RowCount := 2;
  for C := 0 to AGrid.ColCount - 1 do
    AGrid.Cells[C, 1] := '';
end;

{ TStatusHelper }

function TStatusHelper.ToString: string;
begin
  case Self of
    tsAvailable: Result := 'Available';
    tsOccupied:  Result := 'Occupied';
    tsReserved:  Result := 'Reserved';
    tsClosed:    Result := 'Closed';
  else
    Result := 'Unknown';
  end;
end;

{ TOrderStatusHelper }

function TOrderStatusHelper.ToString: string;
begin
  case Self of
    osPending:   Result := 'Pending';
    osPreparing: Result := 'Preparing';
    osServed:    Result := 'Served';
    osPaid:      Result := 'Paid';
    osCancelled: Result := 'Cancelled';
  else
    Result := 'Unknown';
  end;
end;

{ TPaymentMethodHelper }

function TPaymentMethodHelper.ToString: string;
begin
  case Self of
    pmCash:       Result := 'Cash';
    pmCreditCard: Result := 'Credit Card';
    pmDebitCard:  Result := 'Debit Card';
  else
    Result := 'Unknown';
  end;
end;

end.
