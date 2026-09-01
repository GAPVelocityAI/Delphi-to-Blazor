unit uRestaurantBL;

interface

uses
  System.SysUtils, System.Classes, uCommonTypes;

const
  SQL_GET_MENU_ITEMS =
    'SELECT ItemId, ItemName, Category, Price, Cost, Active ' +
    'FROM MenuItems ORDER BY Category, ItemName';

  SQL_GET_ORDERS =
    'SELECT o.OrderId, o.TableId, o.OrderDate, o.Status, o.TotalAmount ' +
    'FROM Orders o ORDER BY o.OrderDate DESC';

  SQL_GET_ORDER_DETAILS =
    'SELECT d.DetailId, d.OrderId, d.ItemId, m.ItemName, d.Quantity, ' +
    'd.UnitPrice, (d.Quantity * d.UnitPrice) AS Subtotal ' +
    'FROM OrderDetails d INNER JOIN MenuItems m ON d.ItemId = m.ItemId ' +
    'WHERE d.OrderId = :OrderId';

  SQL_GET_BILLS =
    'SELECT b.BillId, b.OrderId, b.Subtotal, b.Tax, b.Tip, b.Total, ' +
    'b.PaymentMethod, b.PaidDate FROM Bills b ORDER BY b.PaidDate DESC';

  SQL_INSERT_ORDER =
    'INSERT INTO Orders (TableId, OrderDate, Status, TotalAmount) ' +
    'VALUES (:TableId, :OrderDate, :Status, :TotalAmount)';

  SQL_UPDATE_ORDER_STATUS =
    'UPDATE Orders SET Status = :Status WHERE OrderId = :OrderId';

type
  TRestaurantBL = class
  private
    class var FMenuItems: TArray<TMenuItemInfo>;
    class var FOrders: TArray<TOrderInfo>;
    class var FOrderDetails: TArray<TOrderDetailInfo>;
    class var FBills: TArray<TBillInfo>;
    class var FFoodCosts: TArray<TFoodCostInfo>;
    class var FNextMenuId, FNextOrderId, FNextDetailId, FNextBillId, FNextRecipeId: Integer;
    class var FInitialized: Boolean;
    class procedure EnsureInitialized;
  public
    function GetMenuItems: TArray<TMenuItemInfo>;
    function GetOrders: TArray<TOrderInfo>;
    function GetOrderDetails(AOrderId: Integer): TArray<TOrderDetailInfo>;
    function GetBills: TArray<TBillInfo>;
    function GetFoodCosts: TArray<TFoodCostInfo>;

    procedure AddMenuItem(var AItem: TMenuItemInfo);
    procedure UpdateMenuItem(const AItem: TMenuItemInfo);
    procedure DeleteMenuItem(AItemId: Integer);

    procedure AddOrder(var AOrder: TOrderInfo);
    procedure UpdateOrder(const AOrder: TOrderInfo);
    procedure DeleteOrder(AOrderId: Integer);

    procedure AddBill(var ABill: TBillInfo);
    procedure UpdateBill(const ABill: TBillInfo);
    procedure DeleteBill(ABillId: Integer);

    procedure AddFoodCost(var ACost: TFoodCostInfo);
    procedure UpdateFoodCost(const ACost: TFoodCostInfo);
    procedure DeleteFoodCost(ARecipeId: Integer);
  end;

implementation

{ TRestaurantBL }

class procedure TRestaurantBL.EnsureInitialized;
begin
  if FInitialized then
    Exit;
  FInitialized := True;

  FNextMenuId := 13;
  FNextOrderId := 1009;
  FNextDetailId := 24;
  FNextBillId := 507;
  FNextRecipeId := 11;

  // Menu Items
  SetLength(FMenuItems, 12);

  FMenuItems[0].ItemId := 1;
  FMenuItems[0].ItemName := 'Caesar Salad';
  FMenuItems[0].Category := 'Appetizer';
  FMenuItems[0].Price := 12.50;
  FMenuItems[0].Cost := 3.75;
  FMenuItems[0].Active := True;

  FMenuItems[1].ItemId := 2;
  FMenuItems[1].ItemName := 'Bruschetta';
  FMenuItems[1].Category := 'Appetizer';
  FMenuItems[1].Price := 10.00;
  FMenuItems[1].Cost := 2.80;
  FMenuItems[1].Active := True;

  FMenuItems[2].ItemId := 3;
  FMenuItems[2].ItemName := 'Soup of the Day';
  FMenuItems[2].Category := 'Appetizer';
  FMenuItems[2].Price := 8.00;
  FMenuItems[2].Cost := 2.10;
  FMenuItems[2].Active := True;

  FMenuItems[3].ItemId := 4;
  FMenuItems[3].ItemName := 'Grilled Salmon';
  FMenuItems[3].Category := 'Main Course';
  FMenuItems[3].Price := 24.00;
  FMenuItems[3].Cost := 8.50;
  FMenuItems[3].Active := True;

  FMenuItems[4].ItemId := 5;
  FMenuItems[4].ItemName := 'Margherita Pizza';
  FMenuItems[4].Category := 'Main Course';
  FMenuItems[4].Price := 16.00;
  FMenuItems[4].Cost := 4.20;
  FMenuItems[4].Active := True;

  FMenuItems[5].ItemId := 6;
  FMenuItems[5].ItemName := 'Ribeye Steak';
  FMenuItems[5].Category := 'Main Course';
  FMenuItems[5].Price := 32.00;
  FMenuItems[5].Cost := 12.50;
  FMenuItems[5].Active := True;

  FMenuItems[6].ItemId := 7;
  FMenuItems[6].ItemName := 'Chicken Alfredo';
  FMenuItems[6].Category := 'Main Course';
  FMenuItems[6].Price := 18.50;
  FMenuItems[6].Cost := 5.60;
  FMenuItems[6].Active := True;

  FMenuItems[7].ItemId := 8;
  FMenuItems[7].ItemName := 'Tiramisu';
  FMenuItems[7].Category := 'Dessert';
  FMenuItems[7].Price := 9.00;
  FMenuItems[7].Cost := 2.80;
  FMenuItems[7].Active := True;

  FMenuItems[8].ItemId := 9;
  FMenuItems[8].ItemName := 'Chocolate Lava Cake';
  FMenuItems[8].Category := 'Dessert';
  FMenuItems[8].Price := 11.00;
  FMenuItems[8].Cost := 3.20;
  FMenuItems[8].Active := True;

  FMenuItems[9].ItemId := 10;
  FMenuItems[9].ItemName := 'Craft Beer';
  FMenuItems[9].Category := 'Beverage';
  FMenuItems[9].Price := 7.50;
  FMenuItems[9].Cost := 2.00;
  FMenuItems[9].Active := True;

  FMenuItems[10].ItemId := 11;
  FMenuItems[10].ItemName := 'House Wine';
  FMenuItems[10].Category := 'Beverage';
  FMenuItems[10].Price := 9.50;
  FMenuItems[10].Cost := 3.00;
  FMenuItems[10].Active := True;

  FMenuItems[11].ItemId := 12;
  FMenuItems[11].ItemName := 'Fresh Lemonade';
  FMenuItems[11].Category := 'Beverage';
  FMenuItems[11].Price := 5.00;
  FMenuItems[11].Cost := 1.20;
  FMenuItems[11].Active := True;

  // Orders
  SetLength(FOrders, 8);

  FOrders[0].OrderId := 1001;
  FOrders[0].TableId := 3;
  FOrders[0].OrderDate := EncodeDate(2026, 8, 5) + EncodeTime(12, 30, 0, 0);
  FOrders[0].Status := osPending;
  FOrders[0].TotalAmount := 45.50;

  FOrders[1].OrderId := 1002;
  FOrders[1].TableId := 7;
  FOrders[1].OrderDate := EncodeDate(2026, 8, 5) + EncodeTime(12, 45, 0, 0);
  FOrders[1].Status := osPreparing;
  FOrders[1].TotalAmount := 68.00;

  FOrders[2].OrderId := 1003;
  FOrders[2].TableId := 1;
  FOrders[2].OrderDate := EncodeDate(2026, 8, 5) + EncodeTime(11, 15, 0, 0);
  FOrders[2].Status := osServed;
  FOrders[2].TotalAmount := 92.50;

  FOrders[3].OrderId := 1004;
  FOrders[3].TableId := 12;
  FOrders[3].OrderDate := EncodeDate(2026, 8, 4) + EncodeTime(19, 00, 0, 0);
  FOrders[3].Status := osPaid;
  FOrders[3].TotalAmount := 124.00;

  FOrders[4].OrderId := 1005;
  FOrders[4].TableId := 5;
  FOrders[4].OrderDate := EncodeDate(2026, 8, 4) + EncodeTime(20, 30, 0, 0);
  FOrders[4].Status := osPaid;
  FOrders[4].TotalAmount := 56.00;

  FOrders[5].OrderId := 1006;
  FOrders[5].TableId := 9;
  FOrders[5].OrderDate := EncodeDate(2026, 8, 5) + EncodeTime(13, 00, 0, 0);
  FOrders[5].Status := osPreparing;
  FOrders[5].TotalAmount := 37.50;

  FOrders[6].OrderId := 1007;
  FOrders[6].TableId := 2;
  FOrders[6].OrderDate := EncodeDate(2026, 8, 4) + EncodeTime(18, 45, 0, 0);
  FOrders[6].Status := osCancelled;
  FOrders[6].TotalAmount := 28.00;

  FOrders[7].OrderId := 1008;
  FOrders[7].TableId := 15;
  FOrders[7].OrderDate := EncodeDate(2026, 8, 5) + EncodeTime(13, 15, 0, 0);
  FOrders[7].Status := osPending;
  FOrders[7].TotalAmount := 82.00;

  // Order Details (all orders 1001-1008)
  SetLength(FOrderDetails, 23);

  // Order 1001
  FOrderDetails[0].DetailId := 1; FOrderDetails[0].OrderId := 1001; FOrderDetails[0].ItemId := 1;
  FOrderDetails[0].ItemName := 'Caesar Salad'; FOrderDetails[0].Quantity := 1;
  FOrderDetails[0].UnitPrice := 12.50; FOrderDetails[0].Subtotal := 12.50;

  FOrderDetails[1].DetailId := 2; FOrderDetails[1].OrderId := 1001; FOrderDetails[1].ItemId := 5;
  FOrderDetails[1].ItemName := 'Margherita Pizza'; FOrderDetails[1].Quantity := 1;
  FOrderDetails[1].UnitPrice := 16.00; FOrderDetails[1].Subtotal := 16.00;

  FOrderDetails[2].DetailId := 3; FOrderDetails[2].OrderId := 1001; FOrderDetails[2].ItemId := 10;
  FOrderDetails[2].ItemName := 'Craft Beer'; FOrderDetails[2].Quantity := 2;
  FOrderDetails[2].UnitPrice := 7.50; FOrderDetails[2].Subtotal := 15.00;

  // Order 1002
  FOrderDetails[3].DetailId := 4; FOrderDetails[3].OrderId := 1002; FOrderDetails[3].ItemId := 4;
  FOrderDetails[3].ItemName := 'Grilled Salmon'; FOrderDetails[3].Quantity := 2;
  FOrderDetails[3].UnitPrice := 24.00; FOrderDetails[3].Subtotal := 48.00;

  FOrderDetails[4].DetailId := 5; FOrderDetails[4].OrderId := 1002; FOrderDetails[4].ItemId := 11;
  FOrderDetails[4].ItemName := 'House Wine'; FOrderDetails[4].Quantity := 2;
  FOrderDetails[4].UnitPrice := 9.50; FOrderDetails[4].Subtotal := 19.00;

  // Order 1003
  FOrderDetails[5].DetailId := 6; FOrderDetails[5].OrderId := 1003; FOrderDetails[5].ItemId := 2;
  FOrderDetails[5].ItemName := 'Bruschetta'; FOrderDetails[5].Quantity := 1;
  FOrderDetails[5].UnitPrice := 10.00; FOrderDetails[5].Subtotal := 10.00;

  FOrderDetails[6].DetailId := 7; FOrderDetails[6].OrderId := 1003; FOrderDetails[6].ItemId := 6;
  FOrderDetails[6].ItemName := 'Ribeye Steak'; FOrderDetails[6].Quantity := 2;
  FOrderDetails[6].UnitPrice := 32.00; FOrderDetails[6].Subtotal := 64.00;

  FOrderDetails[7].DetailId := 8; FOrderDetails[7].OrderId := 1003; FOrderDetails[7].ItemId := 8;
  FOrderDetails[7].ItemName := 'Tiramisu'; FOrderDetails[7].Quantity := 1;
  FOrderDetails[7].UnitPrice := 9.00; FOrderDetails[7].Subtotal := 9.00;

  FOrderDetails[8].DetailId := 9; FOrderDetails[8].OrderId := 1003; FOrderDetails[8].ItemId := 10;
  FOrderDetails[8].ItemName := 'Craft Beer'; FOrderDetails[8].Quantity := 1;
  FOrderDetails[8].UnitPrice := 7.50; FOrderDetails[8].Subtotal := 7.50;

  // Order 1004
  FOrderDetails[9].DetailId := 10; FOrderDetails[9].OrderId := 1004; FOrderDetails[9].ItemId := 3;
  FOrderDetails[9].ItemName := 'Soup of the Day'; FOrderDetails[9].Quantity := 2;
  FOrderDetails[9].UnitPrice := 8.00; FOrderDetails[9].Subtotal := 16.00;

  FOrderDetails[10].DetailId := 11; FOrderDetails[10].OrderId := 1004; FOrderDetails[10].ItemId := 7;
  FOrderDetails[10].ItemName := 'Chicken Alfredo'; FOrderDetails[10].Quantity := 3;
  FOrderDetails[10].UnitPrice := 18.50; FOrderDetails[10].Subtotal := 55.50;

  FOrderDetails[11].DetailId := 12; FOrderDetails[11].OrderId := 1004; FOrderDetails[11].ItemId := 9;
  FOrderDetails[11].ItemName := 'Chocolate Lava Cake'; FOrderDetails[11].Quantity := 3;
  FOrderDetails[11].UnitPrice := 11.00; FOrderDetails[11].Subtotal := 33.00;

  // Order 1005
  FOrderDetails[12].DetailId := 13; FOrderDetails[12].OrderId := 1005; FOrderDetails[12].ItemId := 5;
  FOrderDetails[12].ItemName := 'Margherita Pizza'; FOrderDetails[12].Quantity := 2;
  FOrderDetails[12].UnitPrice := 16.00; FOrderDetails[12].Subtotal := 32.00;

  FOrderDetails[13].DetailId := 14; FOrderDetails[13].OrderId := 1005; FOrderDetails[13].ItemId := 12;
  FOrderDetails[13].ItemName := 'Fresh Lemonade'; FOrderDetails[13].Quantity := 4;
  FOrderDetails[13].UnitPrice := 5.00; FOrderDetails[13].Subtotal := 20.00;

  // Order 1006
  FOrderDetails[14].DetailId := 15; FOrderDetails[14].OrderId := 1006; FOrderDetails[14].ItemId := 1;
  FOrderDetails[14].ItemName := 'Caesar Salad'; FOrderDetails[14].Quantity := 1;
  FOrderDetails[14].UnitPrice := 12.50; FOrderDetails[14].Subtotal := 12.50;

  FOrderDetails[15].DetailId := 16; FOrderDetails[15].OrderId := 1006; FOrderDetails[15].ItemId := 7;
  FOrderDetails[15].ItemName := 'Chicken Alfredo'; FOrderDetails[15].Quantity := 1;
  FOrderDetails[15].UnitPrice := 18.50; FOrderDetails[15].Subtotal := 18.50;

  FOrderDetails[16].DetailId := 17; FOrderDetails[16].OrderId := 1006; FOrderDetails[16].ItemId := 10;
  FOrderDetails[16].ItemName := 'Craft Beer'; FOrderDetails[16].Quantity := 1;
  FOrderDetails[16].UnitPrice := 7.50; FOrderDetails[16].Subtotal := 7.50;

  // Order 1007
  FOrderDetails[17].DetailId := 18; FOrderDetails[17].OrderId := 1007; FOrderDetails[17].ItemId := 4;
  FOrderDetails[17].ItemName := 'Grilled Salmon'; FOrderDetails[17].Quantity := 1;
  FOrderDetails[17].UnitPrice := 24.00; FOrderDetails[17].Subtotal := 24.00;

  FOrderDetails[18].DetailId := 19; FOrderDetails[18].OrderId := 1007; FOrderDetails[18].ItemId := 12;
  FOrderDetails[18].ItemName := 'Fresh Lemonade'; FOrderDetails[18].Quantity := 1;
  FOrderDetails[18].UnitPrice := 5.00; FOrderDetails[18].Subtotal := 5.00;

  // Order 1008
  FOrderDetails[19].DetailId := 20; FOrderDetails[19].OrderId := 1008; FOrderDetails[19].ItemId := 2;
  FOrderDetails[19].ItemName := 'Bruschetta'; FOrderDetails[19].Quantity := 2;
  FOrderDetails[19].UnitPrice := 10.00; FOrderDetails[19].Subtotal := 20.00;

  FOrderDetails[20].DetailId := 21; FOrderDetails[20].OrderId := 1008; FOrderDetails[20].ItemId := 6;
  FOrderDetails[20].ItemName := 'Ribeye Steak'; FOrderDetails[20].Quantity := 1;
  FOrderDetails[20].UnitPrice := 32.00; FOrderDetails[20].Subtotal := 32.00;

  FOrderDetails[21].DetailId := 22; FOrderDetails[21].OrderId := 1008; FOrderDetails[21].ItemId := 8;
  FOrderDetails[21].ItemName := 'Tiramisu'; FOrderDetails[21].Quantity := 2;
  FOrderDetails[21].UnitPrice := 9.00; FOrderDetails[21].Subtotal := 18.00;

  FOrderDetails[22].DetailId := 23; FOrderDetails[22].OrderId := 1008; FOrderDetails[22].ItemId := 11;
  FOrderDetails[22].ItemName := 'House Wine'; FOrderDetails[22].Quantity := 1;
  FOrderDetails[22].UnitPrice := 9.50; FOrderDetails[22].Subtotal := 9.50;

  // Bills
  SetLength(FBills, 6);

  FBills[0].BillId := 501;
  FBills[0].OrderId := 1003;
  FBills[0].Subtotal := 92.50;
  FBills[0].Tax := 7.40;
  FBills[0].Tip := 15.00;
  FBills[0].Total := 114.90;
  FBills[0].PaymentMethod := pmCreditCard;
  FBills[0].PaidDate := EncodeDate(2026, 8, 5) + EncodeTime(13, 20, 0, 0);

  FBills[1].BillId := 502;
  FBills[1].OrderId := 1004;
  FBills[1].Subtotal := 124.00;
  FBills[1].Tax := 9.92;
  FBills[1].Tip := 20.00;
  FBills[1].Total := 153.92;
  FBills[1].PaymentMethod := pmCreditCard;
  FBills[1].PaidDate := EncodeDate(2026, 8, 4) + EncodeTime(20, 45, 0, 0);

  FBills[2].BillId := 503;
  FBills[2].OrderId := 1005;
  FBills[2].Subtotal := 56.00;
  FBills[2].Tax := 4.48;
  FBills[2].Tip := 8.00;
  FBills[2].Total := 68.48;
  FBills[2].PaymentMethod := pmCash;
  FBills[2].PaidDate := EncodeDate(2026, 8, 4) + EncodeTime(21, 30, 0, 0);

  FBills[3].BillId := 504;
  FBills[3].OrderId := 1000;
  FBills[3].Subtotal := 78.50;
  FBills[3].Tax := 6.28;
  FBills[3].Tip := 12.00;
  FBills[3].Total := 96.78;
  FBills[3].PaymentMethod := pmDebitCard;
  FBills[3].PaidDate := EncodeDate(2026, 8, 3) + EncodeTime(14, 10, 0, 0);

  FBills[4].BillId := 505;
  FBills[4].OrderId := 999;
  FBills[4].Subtotal := 42.00;
  FBills[4].Tax := 3.36;
  FBills[4].Tip := 6.00;
  FBills[4].Total := 51.36;
  FBills[4].PaymentMethod := pmCash;
  FBills[4].PaidDate := EncodeDate(2026, 8, 3) + EncodeTime(13, 00, 0, 0);

  FBills[5].BillId := 506;
  FBills[5].OrderId := 998;
  FBills[5].Subtotal := 155.00;
  FBills[5].Tax := 12.40;
  FBills[5].Tip := 25.00;
  FBills[5].Total := 192.40;
  FBills[5].PaymentMethod := pmCreditCard;
  FBills[5].PaidDate := EncodeDate(2026, 8, 2) + EncodeTime(21, 15, 0, 0);

  // Food Costs
  SetLength(FFoodCosts, 10);

  FFoodCosts[0].RecipeId := 1;
  FFoodCosts[0].RecipeName := 'Caesar Salad';
  FFoodCosts[0].IngredientCount := 6;
  FFoodCosts[0].TotalCost := 3.75;
  FFoodCosts[0].SellingPrice := 12.50;
  FFoodCosts[0].CostPercentage := 30.0;

  FFoodCosts[1].RecipeId := 2;
  FFoodCosts[1].RecipeName := 'Bruschetta';
  FFoodCosts[1].IngredientCount := 5;
  FFoodCosts[1].TotalCost := 2.80;
  FFoodCosts[1].SellingPrice := 10.00;
  FFoodCosts[1].CostPercentage := 28.0;

  FFoodCosts[2].RecipeId := 3;
  FFoodCosts[2].RecipeName := 'Grilled Salmon';
  FFoodCosts[2].IngredientCount := 7;
  FFoodCosts[2].TotalCost := 8.50;
  FFoodCosts[2].SellingPrice := 24.00;
  FFoodCosts[2].CostPercentage := 35.4;

  FFoodCosts[3].RecipeId := 4;
  FFoodCosts[3].RecipeName := 'Margherita Pizza';
  FFoodCosts[3].IngredientCount := 5;
  FFoodCosts[3].TotalCost := 4.20;
  FFoodCosts[3].SellingPrice := 16.00;
  FFoodCosts[3].CostPercentage := 26.3;

  FFoodCosts[4].RecipeId := 5;
  FFoodCosts[4].RecipeName := 'Ribeye Steak';
  FFoodCosts[4].IngredientCount := 4;
  FFoodCosts[4].TotalCost := 12.50;
  FFoodCosts[4].SellingPrice := 32.00;
  FFoodCosts[4].CostPercentage := 39.1;

  FFoodCosts[5].RecipeId := 6;
  FFoodCosts[5].RecipeName := 'Chicken Alfredo';
  FFoodCosts[5].IngredientCount := 8;
  FFoodCosts[5].TotalCost := 5.60;
  FFoodCosts[5].SellingPrice := 18.50;
  FFoodCosts[5].CostPercentage := 30.3;

  FFoodCosts[6].RecipeId := 7;
  FFoodCosts[6].RecipeName := 'Tiramisu';
  FFoodCosts[6].IngredientCount := 7;
  FFoodCosts[6].TotalCost := 2.80;
  FFoodCosts[6].SellingPrice := 9.00;
  FFoodCosts[6].CostPercentage := 31.1;

  FFoodCosts[7].RecipeId := 8;
  FFoodCosts[7].RecipeName := 'Chocolate Lava Cake';
  FFoodCosts[7].IngredientCount := 6;
  FFoodCosts[7].TotalCost := 3.20;
  FFoodCosts[7].SellingPrice := 11.00;
  FFoodCosts[7].CostPercentage := 29.1;

  FFoodCosts[8].RecipeId := 9;
  FFoodCosts[8].RecipeName := 'Soup of the Day';
  FFoodCosts[8].IngredientCount := 9;
  FFoodCosts[8].TotalCost := 2.10;
  FFoodCosts[8].SellingPrice := 8.00;
  FFoodCosts[8].CostPercentage := 26.3;

  FFoodCosts[9].RecipeId := 10;
  FFoodCosts[9].RecipeName := 'Craft Beer';
  FFoodCosts[9].IngredientCount := 1;
  FFoodCosts[9].TotalCost := 2.00;
  FFoodCosts[9].SellingPrice := 7.50;
  FFoodCosts[9].CostPercentage := 26.7;
end;

function TRestaurantBL.GetMenuItems: TArray<TMenuItemInfo>;
begin
  EnsureInitialized;
  Result := Copy(FMenuItems);
end;

function TRestaurantBL.GetOrders: TArray<TOrderInfo>;
begin
  EnsureInitialized;
  Result := Copy(FOrders);
end;

function TRestaurantBL.GetOrderDetails(AOrderId: Integer): TArray<TOrderDetailInfo>;
var
  I, Count: Integer;
begin
  EnsureInitialized;
  Count := 0;
  for I := 0 to High(FOrderDetails) do
    if FOrderDetails[I].OrderId = AOrderId then
      Inc(Count);
  SetLength(Result, Count);
  Count := 0;
  for I := 0 to High(FOrderDetails) do
    if FOrderDetails[I].OrderId = AOrderId then
    begin
      Result[Count] := FOrderDetails[I];
      Inc(Count);
    end;
end;

function TRestaurantBL.GetBills: TArray<TBillInfo>;
begin
  EnsureInitialized;
  Result := Copy(FBills);
end;

function TRestaurantBL.GetFoodCosts: TArray<TFoodCostInfo>;
begin
  EnsureInitialized;
  Result := Copy(FFoodCosts);
end;

{ Menu Item CRUD }

procedure TRestaurantBL.AddMenuItem(var AItem: TMenuItemInfo);
begin
  EnsureInitialized;
  AItem.ItemId := FNextMenuId;
  Inc(FNextMenuId);
  SetLength(FMenuItems, Length(FMenuItems) + 1);
  FMenuItems[High(FMenuItems)] := AItem;
end;

procedure TRestaurantBL.UpdateMenuItem(const AItem: TMenuItemInfo);
var
  I: Integer;
begin
  EnsureInitialized;
  for I := 0 to High(FMenuItems) do
    if FMenuItems[I].ItemId = AItem.ItemId then
    begin
      FMenuItems[I] := AItem;
      Exit;
    end;
end;

procedure TRestaurantBL.DeleteMenuItem(AItemId: Integer);
var
  I, J: Integer;
begin
  EnsureInitialized;
  for I := 0 to High(FMenuItems) do
    if FMenuItems[I].ItemId = AItemId then
    begin
      for J := I to High(FMenuItems) - 1 do
        FMenuItems[J] := FMenuItems[J + 1];
      SetLength(FMenuItems, Length(FMenuItems) - 1);
      Exit;
    end;
end;

{ Order CRUD }

procedure TRestaurantBL.AddOrder(var AOrder: TOrderInfo);
begin
  EnsureInitialized;
  AOrder.OrderId := FNextOrderId;
  Inc(FNextOrderId);
  SetLength(FOrders, Length(FOrders) + 1);
  FOrders[High(FOrders)] := AOrder;
end;

procedure TRestaurantBL.UpdateOrder(const AOrder: TOrderInfo);
var
  I: Integer;
begin
  EnsureInitialized;
  for I := 0 to High(FOrders) do
    if FOrders[I].OrderId = AOrder.OrderId then
    begin
      FOrders[I] := AOrder;
      Exit;
    end;
end;

procedure TRestaurantBL.DeleteOrder(AOrderId: Integer);
var
  I, J: Integer;
begin
  EnsureInitialized;
  // Remove related order details first
  I := 0;
  while I <= High(FOrderDetails) do
  begin
    if FOrderDetails[I].OrderId = AOrderId then
    begin
      for J := I to High(FOrderDetails) - 1 do
        FOrderDetails[J] := FOrderDetails[J + 1];
      SetLength(FOrderDetails, Length(FOrderDetails) - 1);
    end
    else
      Inc(I);
  end;
  // Remove the order
  for I := 0 to High(FOrders) do
    if FOrders[I].OrderId = AOrderId then
    begin
      for J := I to High(FOrders) - 1 do
        FOrders[J] := FOrders[J + 1];
      SetLength(FOrders, Length(FOrders) - 1);
      Exit;
    end;
end;

{ Bill CRUD }

procedure TRestaurantBL.AddBill(var ABill: TBillInfo);
begin
  EnsureInitialized;
  ABill.BillId := FNextBillId;
  Inc(FNextBillId);
  ABill.Tax := ABill.Subtotal * 0.08;
  ABill.Total := ABill.Subtotal + ABill.Tax + ABill.Tip;
  SetLength(FBills, Length(FBills) + 1);
  FBills[High(FBills)] := ABill;
end;

procedure TRestaurantBL.UpdateBill(const ABill: TBillInfo);
var
  I: Integer;
begin
  EnsureInitialized;
  for I := 0 to High(FBills) do
    if FBills[I].BillId = ABill.BillId then
    begin
      FBills[I] := ABill;
      Exit;
    end;
end;

procedure TRestaurantBL.DeleteBill(ABillId: Integer);
var
  I, J: Integer;
begin
  EnsureInitialized;
  for I := 0 to High(FBills) do
    if FBills[I].BillId = ABillId then
    begin
      for J := I to High(FBills) - 1 do
        FBills[J] := FBills[J + 1];
      SetLength(FBills, Length(FBills) - 1);
      Exit;
    end;
end;

{ Food Cost CRUD }

procedure TRestaurantBL.AddFoodCost(var ACost: TFoodCostInfo);
begin
  EnsureInitialized;
  ACost.RecipeId := FNextRecipeId;
  Inc(FNextRecipeId);
  if ACost.SellingPrice > 0 then
    ACost.CostPercentage := (ACost.TotalCost / ACost.SellingPrice) * 100;
  SetLength(FFoodCosts, Length(FFoodCosts) + 1);
  FFoodCosts[High(FFoodCosts)] := ACost;
end;

procedure TRestaurantBL.UpdateFoodCost(const ACost: TFoodCostInfo);
var
  I: Integer;
begin
  EnsureInitialized;
  for I := 0 to High(FFoodCosts) do
    if FFoodCosts[I].RecipeId = ACost.RecipeId then
    begin
      FFoodCosts[I] := ACost;
      Exit;
    end;
end;

procedure TRestaurantBL.DeleteFoodCost(ARecipeId: Integer);
var
  I, J: Integer;
begin
  EnsureInitialized;
  for I := 0 to High(FFoodCosts) do
    if FFoodCosts[I].RecipeId = ARecipeId then
    begin
      for J := I to High(FFoodCosts) - 1 do
        FFoodCosts[J] := FFoodCosts[J + 1];
      SetLength(FFoodCosts, Length(FFoodCosts) - 1);
      Exit;
    end;
end;

end.
