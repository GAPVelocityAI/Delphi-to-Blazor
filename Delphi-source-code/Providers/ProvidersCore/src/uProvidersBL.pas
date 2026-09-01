unit uProvidersBL;

interface

uses
  System.SysUtils;

type
  TProviderRecord = record
    ProviderId: Integer;
    CompanyName: string;
    ContactName: string;
    Phone: string;
    Email: string;
    Category: string;
    Rating: Integer;
    Active: Boolean;
    ContractExpiry: TDateTime;
  end;

  TProviderPerformance = record
    ProviderId: Integer;
    CompanyName: string;
    OnTimeDeliveryPct: Double;
    QualityScore: Integer;
    TotalOrders: Integer;
    AvgLeadTimeDays: Double;
  end;

  TProviderCategory = record
    Category: string;
    ProviderCount: Integer;
    ActiveCount: Integer;
    AvgRating: Double;
  end;

  TProvidersBL = class
  public
    function GetProviders: TArray<TProviderRecord>;
    function GetProviderPerformance: TArray<TProviderPerformance>;
    function GetProviderCategories: TArray<TProviderCategory>;
  end;

const
  SQL_GET_PROVIDERS =
    'SELECT p.ProviderId, p.CompanyName, p.ContactName, p.Phone, p.Email, ' +
    '       p.Category, p.Rating, p.Active, p.ContractExpiry ' +
    'FROM Providers p ' +
    'ORDER BY p.Category, p.CompanyName';

  SQL_GET_PROVIDER_PERFORMANCE =
    'SELECT p.ProviderId, p.CompanyName, ' +
    '       pp.OnTimeDeliveryPct, pp.QualityScore, ' +
    '       pp.TotalOrders, pp.AvgLeadTimeDays ' +
    'FROM Providers p ' +
    'INNER JOIN ProviderPerformance pp ON pp.ProviderId = p.ProviderId ' +
    'ORDER BY pp.QualityScore DESC';

  SQL_GET_PROVIDER_CATEGORIES =
    'SELECT p.Category, COUNT(*) AS ProviderCount, ' +
    '       SUM(CASE WHEN p.Active = 1 THEN 1 ELSE 0 END) AS ActiveCount, ' +
    '       AVG(CAST(p.Rating AS FLOAT)) AS AvgRating ' +
    'FROM Providers p ' +
    'GROUP BY p.Category ' +
    'ORDER BY p.Category';

  SQL_INSERT_PROVIDER =
    'INSERT INTO Providers (CompanyName, ContactName, Phone, Email, ' +
    '                       Category, Rating, Active, ContractExpiry) ' +
    'VALUES (:CompanyName, :ContactName, :Phone, :Email, ' +
    '        :Category, :Rating, :Active, :ContractExpiry)';

implementation

{ TProvidersBL }

function TProvidersBL.GetProviders: TArray<TProviderRecord>;

  procedure AddProvider(var Arr: TArray<TProviderRecord>; AId: Integer;
    const ACompany, AContact, APhone, AEmail, ACategory: string;
    ARating: Integer; AActive: Boolean; AExpiry: TDateTime);
  var
    Idx: Integer;
  begin
    Idx := Length(Arr);
    SetLength(Arr, Idx + 1);
    Arr[Idx].ProviderId := AId;
    Arr[Idx].CompanyName := ACompany;
    Arr[Idx].ContactName := AContact;
    Arr[Idx].Phone := APhone;
    Arr[Idx].Email := AEmail;
    Arr[Idx].Category := ACategory;
    Arr[Idx].Rating := ARating;
    Arr[Idx].Active := AActive;
    Arr[Idx].ContractExpiry := AExpiry;
  end;

begin
  Result := nil;
  AddProvider(Result, 1, 'Fresh Farms Co',       'Maria Lopez',     '(555) 100-2001', 'maria@freshfarms.com',       'Produce',  5, True,  EncodeDate(2027, 3, 15));
  AddProvider(Result, 2, 'Ocean Direct',          'James Chen',      '(555) 100-2002', 'james@oceandirect.com',      'Seafood',  4, True,  EncodeDate(2027, 1, 20));
  AddProvider(Result, 3, 'Valley Meats',          'Robert Miller',   '(555) 100-2003', 'robert@valleymeats.com',     'Meat',     5, True,  EncodeDate(2026, 12, 31));
  AddProvider(Result, 4, 'Dairy Delights',        'Susan White',     '(555) 100-2004', 'susan@dairydelights.com',    'Dairy',    3, True,  EncodeDate(2026, 11, 30));
  AddProvider(Result, 5, 'Golden Grain Supply',   'David Brown',     '(555) 100-2005', 'david@goldengrain.com',      'Dry Goods',4, True,  EncodeDate(2027, 6, 15));
  AddProvider(Result, 6, 'Mediterranean Imports',  'Sofia Romano',    '(555) 100-2006', 'sofia@medimports.com',       'Oils',     5, True,  EncodeDate(2027, 2, 28));
  AddProvider(Result, 7, 'Herb Garden Direct',    'Emily Green',     '(555) 100-2007', 'emily@herbgarden.com',       'Herbs',    4, True,  EncodeDate(2026, 10, 15));
  AddProvider(Result, 8, 'Pacific Seafood Inc',   'Tom Nakamura',    '(555) 100-2008', 'tom@pacificseafood.com',     'Seafood',  3, False, EncodeDate(2026, 8, 31));
end;

function TProvidersBL.GetProviderPerformance: TArray<TProviderPerformance>;

  procedure AddPerf(var Arr: TArray<TProviderPerformance>; AId: Integer;
    const ACompany: string; AOnTime: Double; AQuality, AOrders: Integer;
    ALeadTime: Double);
  var
    Idx: Integer;
  begin
    Idx := Length(Arr);
    SetLength(Arr, Idx + 1);
    Arr[Idx].ProviderId := AId;
    Arr[Idx].CompanyName := ACompany;
    Arr[Idx].OnTimeDeliveryPct := AOnTime;
    Arr[Idx].QualityScore := AQuality;
    Arr[Idx].TotalOrders := AOrders;
    Arr[Idx].AvgLeadTimeDays := ALeadTime;
  end;

begin
  Result := nil;
  AddPerf(Result, 1, 'Fresh Farms Co',       98.5, 95, 124, 1.5);
  AddPerf(Result, 3, 'Valley Meats',          97.0, 94, 98,  2.0);
  AddPerf(Result, 6, 'Mediterranean Imports',  96.2, 92, 56,  5.0);
  AddPerf(Result, 2, 'Ocean Direct',          94.8, 90, 87,  2.5);
  AddPerf(Result, 5, 'Golden Grain Supply',   93.5, 89, 72,  3.0);
  AddPerf(Result, 7, 'Herb Garden Direct',    91.0, 88, 45,  2.0);
  AddPerf(Result, 4, 'Dairy Delights',        85.2, 78, 110, 1.5);
  AddPerf(Result, 8, 'Pacific Seafood Inc',   80.0, 72, 34,  4.0);
end;

function TProvidersBL.GetProviderCategories: TArray<TProviderCategory>;

  procedure AddCategory(var Arr: TArray<TProviderCategory>;
    const ACategory: string; ACount, AActive: Integer; AAvgRating: Double);
  var
    Idx: Integer;
  begin
    Idx := Length(Arr);
    SetLength(Arr, Idx + 1);
    Arr[Idx].Category := ACategory;
    Arr[Idx].ProviderCount := ACount;
    Arr[Idx].ActiveCount := AActive;
    Arr[Idx].AvgRating := AAvgRating;
  end;

begin
  Result := nil;
  AddCategory(Result, 'Dairy',     1, 1, 3.0);
  AddCategory(Result, 'Dry Goods', 1, 1, 4.0);
  AddCategory(Result, 'Herbs',     1, 1, 4.0);
  AddCategory(Result, 'Meat',      1, 1, 5.0);
  AddCategory(Result, 'Oils',      1, 1, 5.0);
  AddCategory(Result, 'Produce',   1, 1, 5.0);
  AddCategory(Result, 'Seafood',   2, 1, 3.5);
end;

end.
