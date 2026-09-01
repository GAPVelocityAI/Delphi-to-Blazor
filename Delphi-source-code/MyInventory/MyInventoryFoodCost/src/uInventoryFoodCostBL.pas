unit uInventoryFoodCostBL;

interface

uses
  System.SysUtils;

type
  TRecipeIngredient = record
    IngredientName: string;
    Quantity: Double;
    UnitMeasure: string;
    UnitCost: Currency;
    LineCost: Currency;
  end;

  TRecipeCost = record
    RecipeId: Integer;
    RecipeName: string;
    Category: string;
    Ingredients: TArray<TRecipeIngredient>;
    TotalCost: Currency;
    SellingPrice: Currency;
    CostPercentage: Double;
    Profitable: Boolean;
  end;

  TCostTrend = record
    Period: string;
    AvgCostPct: Double;
    TotalFoodCost: Currency;
    TotalRevenue: Currency;
  end;

  TInventoryFoodCostBL = class
  public
    function GetRecipeCosts: TArray<TRecipeCost>;
    function GetRecipeIngredients(ARecipeId: Integer): TArray<TRecipeIngredient>;
    function GetCostTrends: TArray<TCostTrend>;
  end;

const
  SQL_GET_RECIPE_COSTS =
    'SELECT r.RecipeId, r.RecipeName, r.Category, ' +
    '       r.TotalCost, r.SellingPrice, ' +
    '       (r.TotalCost / r.SellingPrice * 100) AS CostPercentage ' +
    'FROM Recipes r ' +
    'ORDER BY r.Category, r.RecipeName';

  SQL_GET_RECIPE_INGREDIENTS =
    'SELECT ri.IngredientName, ri.Quantity, ri.UnitMeasure, ' +
    '       ri.UnitCost, (ri.Quantity * ri.UnitCost) AS LineCost ' +
    'FROM RecipeIngredients ri ' +
    'WHERE ri.RecipeId = :RecipeId ' +
    'ORDER BY ri.IngredientName';

  SQL_GET_COST_TRENDS =
    'SELECT FORMAT(ct.PeriodDate, ''yyyy-MM'') AS Period, ' +
    '       AVG(ct.CostPercentage) AS AvgCostPct, ' +
    '       SUM(ct.FoodCost) AS TotalFoodCost, ' +
    '       SUM(ct.Revenue) AS TotalRevenue ' +
    'FROM CostTrends ct ' +
    'GROUP BY FORMAT(ct.PeriodDate, ''yyyy-MM'') ' +
    'ORDER BY Period';

implementation

{ TInventoryFoodCostBL }

function TInventoryFoodCostBL.GetRecipeCosts: TArray<TRecipeCost>;

  function MakeIngredient(const AName: string; AQty: Double;
    const AUnit: string; AUnitCost: Currency): TRecipeIngredient;
  begin
    Result.IngredientName := AName;
    Result.Quantity := AQty;
    Result.UnitMeasure := AUnit;
    Result.UnitCost := AUnitCost;
    Result.LineCost := AQty * AUnitCost;
  end;

  procedure AddRecipe(var Arr: TArray<TRecipeCost>; AId: Integer;
    const AName, ACategory: string; ACost, APrice: Currency;
    const AIngredients: TArray<TRecipeIngredient>);
  var
    Idx: Integer;
  begin
    Idx := Length(Arr);
    SetLength(Arr, Idx + 1);
    Arr[Idx].RecipeId := AId;
    Arr[Idx].RecipeName := AName;
    Arr[Idx].Category := ACategory;
    Arr[Idx].Ingredients := AIngredients;
    Arr[Idx].TotalCost := ACost;
    Arr[Idx].SellingPrice := APrice;
    if APrice > 0 then
      Arr[Idx].CostPercentage := (ACost / APrice) * 100
    else
      Arr[Idx].CostPercentage := 0;
    Arr[Idx].Profitable := Arr[Idx].CostPercentage < 35;
  end;

begin
  Result := nil;

  AddRecipe(Result, 1, 'Caesar Salad', 'Appetizer', 3.75, 12.50,
    TArray<TRecipeIngredient>.Create(
      MakeIngredient('Romaine Lettuce', 0.25, 'kg', 3.00),
      MakeIngredient('Parmesan', 0.05, 'kg', 28.00),
      MakeIngredient('Croutons', 0.10, 'kg', 5.00),
      MakeIngredient('Caesar Dressing', 0.05, 'liters', 12.00)
    ));

  AddRecipe(Result, 2, 'Grilled Salmon', 'Main Course', 8.50, 24.00,
    TArray<TRecipeIngredient>.Create(
      MakeIngredient('Salmon Fillet', 0.25, 'kg', 22.00),
      MakeIngredient('Olive Oil', 0.03, 'liters', 18.50),
      MakeIngredient('Lemon', 0.05, 'kg', 4.00),
      MakeIngredient('Asparagus', 0.15, 'kg', 8.00),
      MakeIngredient('Butter', 0.03, 'kg', 7.50)
    ));

  AddRecipe(Result, 3, 'Margherita Pizza', 'Main Course', 4.20, 16.00,
    TArray<TRecipeIngredient>.Create(
      MakeIngredient('Flour', 0.30, 'kg', 2.80),
      MakeIngredient('Mozzarella', 0.15, 'kg', 12.00),
      MakeIngredient('Tomatoes', 0.20, 'kg', 3.20),
      MakeIngredient('Basil', 0.02, 'kg', 15.00),
      MakeIngredient('Olive Oil', 0.02, 'liters', 18.50)
    ));

  AddRecipe(Result, 4, 'Chicken Parmesan', 'Main Course', 6.80, 19.50,
    TArray<TRecipeIngredient>.Create(
      MakeIngredient('Chicken Breast', 0.30, 'kg', 9.50),
      MakeIngredient('Parmesan', 0.05, 'kg', 28.00),
      MakeIngredient('Tomatoes', 0.15, 'kg', 3.20),
      MakeIngredient('Mozzarella', 0.10, 'kg', 12.00)
    ));

  AddRecipe(Result, 5, 'Risotto', 'Main Course', 5.10, 18.00,
    TArray<TRecipeIngredient>.Create(
      MakeIngredient('Rice', 0.20, 'kg', 4.50),
      MakeIngredient('Parmesan', 0.06, 'kg', 28.00),
      MakeIngredient('Butter', 0.05, 'kg', 7.50),
      MakeIngredient('Chicken Breast', 0.15, 'kg', 9.50),
      MakeIngredient('Heavy Cream', 0.08, 'liters', 5.50)
    ));

  AddRecipe(Result, 6, 'Tiramisu', 'Dessert', 3.20, 10.00,
    TArray<TRecipeIngredient>.Create(
      MakeIngredient('Heavy Cream', 0.15, 'liters', 5.50),
      MakeIngredient('Mascarpone', 0.10, 'kg', 14.00),
      MakeIngredient('Espresso', 0.05, 'liters', 8.00),
      MakeIngredient('Ladyfingers', 0.08, 'kg', 6.00)
    ));

  AddRecipe(Result, 7, 'Bruschetta', 'Appetizer', 2.40, 9.50,
    TArray<TRecipeIngredient>.Create(
      MakeIngredient('Tomatoes', 0.15, 'kg', 3.20),
      MakeIngredient('Basil', 0.02, 'kg', 15.00),
      MakeIngredient('Garlic', 0.01, 'kg', 6.00),
      MakeIngredient('Olive Oil', 0.03, 'liters', 18.50)
    ));

  AddRecipe(Result, 8, 'Panna Cotta', 'Dessert', 2.80, 9.00,
    TArray<TRecipeIngredient>.Create(
      MakeIngredient('Heavy Cream', 0.20, 'liters', 5.50),
      MakeIngredient('Vanilla Extract', 0.01, 'liters', 45.00),
      MakeIngredient('Sugar', 0.05, 'kg', 2.00),
      MakeIngredient('Gelatin', 0.01, 'kg', 35.00)
    ));

  AddRecipe(Result, 9, 'Seafood Pasta', 'Main Course', 9.20, 22.00,
    TArray<TRecipeIngredient>.Create(
      MakeIngredient('Pasta', 0.20, 'kg', 3.50),
      MakeIngredient('Salmon Fillet', 0.15, 'kg', 22.00),
      MakeIngredient('Shrimp', 0.10, 'kg', 28.00),
      MakeIngredient('Garlic', 0.01, 'kg', 6.00),
      MakeIngredient('Olive Oil', 0.03, 'liters', 18.50)
    ));

  AddRecipe(Result, 10, 'Lemonade', 'Beverage', 0.85, 5.00,
    TArray<TRecipeIngredient>.Create(
      MakeIngredient('Lemons', 0.10, 'kg', 4.00),
      MakeIngredient('Sugar', 0.05, 'kg', 2.00),
      MakeIngredient('Mint', 0.01, 'kg', 15.00)
    ));
end;

function TInventoryFoodCostBL.GetRecipeIngredients(ARecipeId: Integer): TArray<TRecipeIngredient>;
var
  Recipes: TArray<TRecipeCost>;
  I: Integer;
begin
  Result := nil;
  Recipes := GetRecipeCosts;
  for I := 0 to High(Recipes) do
  begin
    if Recipes[I].RecipeId = ARecipeId then
    begin
      Result := Recipes[I].Ingredients;
      Exit;
    end;
  end;
end;

function TInventoryFoodCostBL.GetCostTrends: TArray<TCostTrend>;

  procedure AddTrend(var Arr: TArray<TCostTrend>; const APeriod: string;
    AAvgPct: Double; AFoodCost, ARevenue: Currency);
  var
    Idx: Integer;
  begin
    Idx := Length(Arr);
    SetLength(Arr, Idx + 1);
    Arr[Idx].Period := APeriod;
    Arr[Idx].AvgCostPct := AAvgPct;
    Arr[Idx].TotalFoodCost := AFoodCost;
    Arr[Idx].TotalRevenue := ARevenue;
  end;

begin
  Result := nil;
  AddTrend(Result, '2026-02', 30.5, 12400.00, 40655.74);
  AddTrend(Result, '2026-03', 31.2, 13100.00, 41987.18);
  AddTrend(Result, '2026-04', 29.8, 12800.00, 42953.02);
  AddTrend(Result, '2026-05', 32.1, 14200.00, 44237.07);
  AddTrend(Result, '2026-06', 30.9, 13600.00, 44012.94);
  AddTrend(Result, '2026-07', 31.5, 14500.00, 46031.75);
end;

end.
