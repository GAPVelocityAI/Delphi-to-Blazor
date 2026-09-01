unit uProviderSuppliesBL;

interface

uses
  System.SysUtils;

type
  TSupplyItem = record
    SupplyId: Integer;
    ProviderId: Integer;
    ProviderName: string;
    ItemName: string;
    Category: string;
    UnitCost: Currency;
    MinOrderQty: Integer;
    LeadTimeDays: Integer;
    InStock: Boolean;
  end;

  TPurchaseOrder = record
    POId: Integer;
    ProviderName: string;
    OrderDate: TDateTime;
    ExpectedDelivery: TDateTime;
    TotalAmount: Currency;
    Status: string;
    ItemCount: Integer;
  end;

  TSupplyPriceHistory = record
    ItemName: string;
    ProviderName: string;
    OldPrice: Currency;
    NewPrice: Currency;
    ChangeDate: TDateTime;
    ChangePct: Double;
  end;

  TProviderSuppliesBL = class
  public
    function GetSupplies: TArray<TSupplyItem>;
    function GetPurchaseOrders: TArray<TPurchaseOrder>;
    function GetPriceHistory: TArray<TSupplyPriceHistory>;
  end;

const
  SQL_GET_SUPPLIES =
    'SELECT s.SupplyId, s.ProviderId, p.CompanyName AS ProviderName, ' +
    '       s.ItemName, s.Category, s.UnitCost, s.MinOrderQty, ' +
    '       s.LeadTimeDays, s.InStock ' +
    'FROM Supplies s ' +
    'INNER JOIN Providers p ON p.ProviderId = s.ProviderId ' +
    'ORDER BY s.Category, s.ItemName';

  SQL_GET_PURCHASE_ORDERS =
    'SELECT po.POId, p.CompanyName AS ProviderName, po.OrderDate, ' +
    '       po.ExpectedDelivery, po.TotalAmount, po.Status, ' +
    '       (SELECT COUNT(*) FROM PurchaseOrderItems poi WHERE poi.POId = po.POId) AS ItemCount ' +
    'FROM PurchaseOrders po ' +
    'INNER JOIN Providers p ON p.ProviderId = po.ProviderId ' +
    'ORDER BY po.OrderDate DESC';

  SQL_GET_PRICE_HISTORY =
    'SELECT s.ItemName, p.CompanyName AS ProviderName, ' +
    '       ph.OldPrice, ph.NewPrice, ph.ChangeDate, ' +
    '       ((ph.NewPrice - ph.OldPrice) / ph.OldPrice * 100) AS ChangePct ' +
    'FROM PriceHistory ph ' +
    'INNER JOIN Supplies s ON s.SupplyId = ph.SupplyId ' +
    'INNER JOIN Providers p ON p.ProviderId = s.ProviderId ' +
    'ORDER BY ph.ChangeDate DESC';

  SQL_CREATE_PURCHASE_ORDER =
    'INSERT INTO PurchaseOrders (ProviderId, OrderDate, ExpectedDelivery, ' +
    '                            TotalAmount, Status) ' +
    'VALUES (:ProviderId, :OrderDate, :ExpectedDelivery, ' +
    '        :TotalAmount, :Status)';

implementation

{ TProviderSuppliesBL }

function TProviderSuppliesBL.GetSupplies: TArray<TSupplyItem>;

  procedure AddSupply(var Arr: TArray<TSupplyItem>; AId, AProviderId: Integer;
    const AProvider, AName, ACategory: string; ACost: Currency;
    AMinQty, ALeadDays: Integer; AInStock: Boolean);
  var
    Idx: Integer;
  begin
    Idx := Length(Arr);
    SetLength(Arr, Idx + 1);
    Arr[Idx].SupplyId := AId;
    Arr[Idx].ProviderId := AProviderId;
    Arr[Idx].ProviderName := AProvider;
    Arr[Idx].ItemName := AName;
    Arr[Idx].Category := ACategory;
    Arr[Idx].UnitCost := ACost;
    Arr[Idx].MinOrderQty := AMinQty;
    Arr[Idx].LeadTimeDays := ALeadDays;
    Arr[Idx].InStock := AInStock;
  end;

begin
  Result := nil;
  AddSupply(Result, 1,  1, 'Fresh Farms Co',       'Tomatoes',         'Produce',   3.20, 10, 1, True);
  AddSupply(Result, 2,  1, 'Fresh Farms Co',       'Garlic',           'Produce',   6.00,  5, 1, True);
  AddSupply(Result, 3,  1, 'Fresh Farms Co',       'Lemons',           'Produce',   4.00,  5, 1, True);
  AddSupply(Result, 4,  2, 'Ocean Direct',          'Salmon Fillet',    'Seafood',  22.00,  5, 2, True);
  AddSupply(Result, 5,  2, 'Ocean Direct',          'Shrimp',           'Seafood',  28.00,  3, 2, True);
  AddSupply(Result, 6,  3, 'Valley Meats',          'Chicken Breast',   'Meat',      9.50, 10, 2, True);
  AddSupply(Result, 7,  4, 'Dairy Delights',        'Mozzarella',       'Dairy',    12.00,  5, 1, True);
  AddSupply(Result, 8,  4, 'Dairy Delights',        'Butter',           'Dairy',     7.50,  5, 1, False);
  AddSupply(Result, 9,  4, 'Dairy Delights',        'Heavy Cream',      'Dairy',     5.50, 10, 1, True);
  AddSupply(Result, 10, 5, 'Golden Grain Supply',   'Flour',            'Dry Goods', 2.80, 25, 3, True);
  AddSupply(Result, 11, 5, 'Golden Grain Supply',   'Rice',             'Dry Goods', 4.50, 25, 3, True);
  AddSupply(Result, 12, 6, 'Mediterranean Imports',  'Olive Oil',        'Oils',     18.50, 10, 5, True);
end;

function TProviderSuppliesBL.GetPurchaseOrders: TArray<TPurchaseOrder>;

  procedure AddPO(var Arr: TArray<TPurchaseOrder>; AId: Integer;
    const AProvider: string; AOrderDate, ADelivery: TDateTime;
    ATotal: Currency; const AStatus: string; AItems: Integer);
  var
    Idx: Integer;
  begin
    Idx := Length(Arr);
    SetLength(Arr, Idx + 1);
    Arr[Idx].POId := AId;
    Arr[Idx].ProviderName := AProvider;
    Arr[Idx].OrderDate := AOrderDate;
    Arr[Idx].ExpectedDelivery := ADelivery;
    Arr[Idx].TotalAmount := ATotal;
    Arr[Idx].Status := AStatus;
    Arr[Idx].ItemCount := AItems;
  end;

begin
  Result := nil;
  AddPO(Result, 1045, 'Fresh Farms Co',       EncodeDate(2026, 7, 28), EncodeDate(2026, 7, 29),  320.00, 'Delivered',  3);
  AddPO(Result, 1046, 'Ocean Direct',          EncodeDate(2026, 7, 30), EncodeDate(2026, 8, 1),   610.00, 'Delivered',  2);
  AddPO(Result, 1047, 'Valley Meats',          EncodeDate(2026, 8, 1),  EncodeDate(2026, 8, 3),   475.00, 'In Transit', 1);
  AddPO(Result, 1048, 'Dairy Delights',        EncodeDate(2026, 8, 2),  EncodeDate(2026, 8, 3),   285.00, 'Pending',    3);
  AddPO(Result, 1049, 'Golden Grain Supply',   EncodeDate(2026, 8, 3),  EncodeDate(2026, 8, 6),   245.00, 'Pending',    2);
  AddPO(Result, 1050, 'Mediterranean Imports',  EncodeDate(2026, 7, 15), EncodeDate(2026, 7, 20),  370.00, 'Cancelled',  1);
end;

function TProviderSuppliesBL.GetPriceHistory: TArray<TSupplyPriceHistory>;

  procedure AddHistory(var Arr: TArray<TSupplyPriceHistory>;
    const AItem, AProvider: string; AOld, ANew: Currency;
    ADate: TDateTime; APct: Double);
  var
    Idx: Integer;
  begin
    Idx := Length(Arr);
    SetLength(Arr, Idx + 1);
    Arr[Idx].ItemName := AItem;
    Arr[Idx].ProviderName := AProvider;
    Arr[Idx].OldPrice := AOld;
    Arr[Idx].NewPrice := ANew;
    Arr[Idx].ChangeDate := ADate;
    Arr[Idx].ChangePct := APct;
  end;

begin
  Result := nil;
  AddHistory(Result, 'Salmon Fillet',   'Ocean Direct',          20.00, 22.00, EncodeDate(2026, 7, 1),   10.0);
  AddHistory(Result, 'Olive Oil',       'Mediterranean Imports',  17.00, 18.50, EncodeDate(2026, 6, 15),   8.82);
  AddHistory(Result, 'Chicken Breast',  'Valley Meats',           9.00,  9.50, EncodeDate(2026, 6, 1),    5.56);
  AddHistory(Result, 'Mozzarella',      'Dairy Delights',        11.50, 12.00, EncodeDate(2026, 5, 20),   4.35);
  AddHistory(Result, 'Flour',           'Golden Grain Supply',    3.00,  2.80, EncodeDate(2026, 5, 1),   -6.67);
  AddHistory(Result, 'Tomatoes',        'Fresh Farms Co',         3.50,  3.20, EncodeDate(2026, 4, 15),  -8.57);
  AddHistory(Result, 'Heavy Cream',     'Dairy Delights',         5.00,  5.50, EncodeDate(2026, 4, 1),   10.0);
  AddHistory(Result, 'Shrimp',          'Ocean Direct',          26.00, 28.00, EncodeDate(2026, 3, 15),   7.69);
end;

end.
