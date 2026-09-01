unit uTablesBL;

interface

uses
  System.SysUtils, System.Classes, uCommonTypes;

const
  SQL_GET_TABLES =
    'SELECT TableId, TableNumber, Capacity, Status, Zone ' +
    'FROM RestaurantTables ORDER BY Zone, TableNumber';

  SQL_UPDATE_TABLE_STATUS =
    'UPDATE RestaurantTables SET Status = :Status WHERE TableId = :TableId';

  SQL_GET_FREE_TABLES =
    'SELECT TableId, TableNumber, Capacity, Status, Zone ' +
    'FROM RestaurantTables WHERE Status = 0 ORDER BY Zone, TableNumber';

type
  TTablesBL = class
  private
    class var FData: TArray<TTableInfo>;
    class var FNextId: Integer;
    class var FInitialized: Boolean;
    class procedure EnsureInitialized;
  public
    function GetTables: TArray<TTableInfo>;
    function GetAvailableTables: TArray<TTableInfo>;
    function GetTablesByZone(const AZone: string): TArray<TTableInfo>;
    procedure AddTable(var ATable: TTableInfo);
    procedure UpdateTable(const ATable: TTableInfo);
    procedure DeleteTable(ATableId: Integer);
  end;

implementation

{ TTablesBL }

class procedure TTablesBL.EnsureInitialized;
begin
  if FInitialized then Exit;

  SetLength(FData, 15);

  FData[0].TableId := 1;  FData[0].TableNumber := 1;  FData[0].Capacity := 4;
  FData[0].Status := tsOccupied;  FData[0].Zone := 'Main Hall';

  FData[1].TableId := 2;  FData[1].TableNumber := 2;  FData[1].Capacity := 4;
  FData[1].Status := tsAvailable;  FData[1].Zone := 'Main Hall';

  FData[2].TableId := 3;  FData[2].TableNumber := 3;  FData[2].Capacity := 2;
  FData[2].Status := tsOccupied;  FData[2].Zone := 'Main Hall';

  FData[3].TableId := 4;  FData[3].TableNumber := 4;  FData[3].Capacity := 6;
  FData[3].Status := tsAvailable;  FData[3].Zone := 'Main Hall';

  FData[4].TableId := 5;  FData[4].TableNumber := 5;  FData[4].Capacity := 2;
  FData[4].Status := tsReserved;  FData[4].Zone := 'Main Hall';

  FData[5].TableId := 6;  FData[5].TableNumber := 6;  FData[5].Capacity := 4;
  FData[5].Status := tsAvailable;  FData[5].Zone := 'Terrace';

  FData[6].TableId := 7;  FData[6].TableNumber := 7;  FData[6].Capacity := 6;
  FData[6].Status := tsOccupied;  FData[6].Zone := 'Terrace';

  FData[7].TableId := 8;  FData[7].TableNumber := 8;  FData[7].Capacity := 2;
  FData[7].Status := tsAvailable;  FData[7].Zone := 'Terrace';

  FData[8].TableId := 9;  FData[8].TableNumber := 9;  FData[8].Capacity := 4;
  FData[8].Status := tsOccupied;  FData[8].Zone := 'Terrace';

  FData[9].TableId := 10;  FData[9].TableNumber := 10;  FData[9].Capacity := 8;
  FData[9].Status := tsAvailable;  FData[9].Zone := 'Private';

  FData[10].TableId := 11;  FData[10].TableNumber := 11;  FData[10].Capacity := 8;
  FData[10].Status := tsReserved;  FData[10].Zone := 'Private';

  FData[11].TableId := 12;  FData[11].TableNumber := 12;  FData[11].Capacity := 6;
  FData[11].Status := tsOccupied;  FData[11].Zone := 'Private';

  FData[12].TableId := 13;  FData[12].TableNumber := 13;  FData[12].Capacity := 2;
  FData[12].Status := tsAvailable;  FData[12].Zone := 'Bar Area';

  FData[13].TableId := 14;  FData[13].TableNumber := 14;  FData[13].Capacity := 2;
  FData[13].Status := tsOccupied;  FData[13].Zone := 'Bar Area';

  FData[14].TableId := 15;  FData[14].TableNumber := 15;  FData[14].Capacity := 4;
  FData[14].Status := tsClosed;  FData[14].Zone := 'Bar Area';

  FNextId := 16;
  FInitialized := True;
end;

function TTablesBL.GetTables: TArray<TTableInfo>;
begin
  EnsureInitialized;
  Result := Copy(FData);
end;

function TTablesBL.GetAvailableTables: TArray<TTableInfo>;
var
  I, Count: Integer;
begin
  EnsureInitialized;
  Count := 0;
  for I := 0 to High(FData) do
    if FData[I].Status = tsAvailable then
      Inc(Count);

  SetLength(Result, Count);
  Count := 0;
  for I := 0 to High(FData) do
    if FData[I].Status = tsAvailable then
    begin
      Result[Count] := FData[I];
      Inc(Count);
    end;
end;

function TTablesBL.GetTablesByZone(const AZone: string): TArray<TTableInfo>;
var
  I, Count: Integer;
begin
  EnsureInitialized;
  Count := 0;
  for I := 0 to High(FData) do
    if SameText(FData[I].Zone, AZone) then
      Inc(Count);

  SetLength(Result, Count);
  Count := 0;
  for I := 0 to High(FData) do
    if SameText(FData[I].Zone, AZone) then
    begin
      Result[Count] := FData[I];
      Inc(Count);
    end;
end;

procedure TTablesBL.AddTable(var ATable: TTableInfo);
var
  Len: Integer;
begin
  EnsureInitialized;
  ATable.TableId := FNextId;
  Inc(FNextId);
  Len := Length(FData);
  SetLength(FData, Len + 1);
  FData[Len] := ATable;
end;

procedure TTablesBL.UpdateTable(const ATable: TTableInfo);
var
  I: Integer;
begin
  EnsureInitialized;
  for I := 0 to High(FData) do
    if FData[I].TableId = ATable.TableId then
    begin
      FData[I] := ATable;
      Exit;
    end;
end;

procedure TTablesBL.DeleteTable(ATableId: Integer);
var
  I, J: Integer;
begin
  EnsureInitialized;
  for I := 0 to High(FData) do
    if FData[I].TableId = ATableId then
    begin
      for J := I to High(FData) - 1 do
        FData[J] := FData[J + 1];
      SetLength(FData, Length(FData) - 1);
      Exit;
    end;
end;

end.
