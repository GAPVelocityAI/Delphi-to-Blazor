unit uFrmFoodCost;

interface

uses
  Winapi.Windows, Winapi.Messages, System.SysUtils, System.Variants,
  System.Classes, Vcl.Graphics, Vcl.Controls, Vcl.Forms, Vcl.Dialogs,
  Vcl.Grids, Vcl.StdCtrls, Vcl.ExtCtrls,
  uCommonTypes, uRestaurantBL;

type
  TfrmFoodCost = class(TForm)
    pnlTop: TPanel;
    lblTitle: TLabel;
    btnClose: TButton;
    grdFoodCost: TStringGrid;
    pnlBottom: TPanel;
    btnRefresh: TButton;
    lblAvgCostPct: TLabel;
    btnAdd: TButton;
    btnEdit: TButton;
    btnDelete: TButton;
    pnlEdit: TPanel;
    lblEdtRecipeName: TLabel;
    edtRecipeName: TEdit;
    lblEdtIngredients: TLabel;
    edtIngredients: TEdit;
    lblEdtTotalCost: TLabel;
    edtTotalCost: TEdit;
    lblEdtSellingPrice: TLabel;
    edtSellingPrice: TEdit;
    btnSave: TButton;
    btnCancel: TButton;
    procedure FormCreate(Sender: TObject);
    procedure FormDestroy(Sender: TObject);
    procedure btnCloseClick(Sender: TObject);
    procedure btnRefreshClick(Sender: TObject);
    procedure grdFoodCostDrawCell(Sender: TObject; ACol, ARow: Integer;
      Rect: TRect; State: TGridDrawState);
    procedure btnAddClick(Sender: TObject);
    procedure btnEditClick(Sender: TObject);
    procedure btnDeleteClick(Sender: TObject);
    procedure btnSaveClick(Sender: TObject);
    procedure btnCancelClick(Sender: TObject);
  private
    FRestaurantBL: TRestaurantBL;
    FIsAdding: Boolean;
    FSelectedId: Integer;
    procedure LoadFoodCosts;
  public
    { Public declarations }
  end;

var
  frmFoodCost: TfrmFoodCost;

implementation

{$R *.dfm}

procedure TfrmFoodCost.FormCreate(Sender: TObject);
begin
  FRestaurantBL := TRestaurantBL.Create;
  TGridHelper.ConfigureGrid(grdFoodCost,
    ['Recipe ID', 'Recipe', 'Ingredients', 'Cost', 'Selling Price', 'Cost %'],
    [60, 150, 80, 70, 90, 70]);
  LoadFoodCosts;
end;

procedure TfrmFoodCost.FormDestroy(Sender: TObject);
begin
  FRestaurantBL.Free;
end;

procedure TfrmFoodCost.LoadFoodCosts;
var
  Costs: TArray<TFoodCostInfo>;
  I: Integer;
  TotalPct: Double;
begin
  Costs := FRestaurantBL.GetFoodCosts;
  TGridHelper.ClearGrid(grdFoodCost);

  if Length(Costs) = 0 then
    Exit;

  TotalPct := 0;
  grdFoodCost.RowCount := Length(Costs) + 1;
  for I := 0 to High(Costs) do
  begin
    grdFoodCost.Cells[0, I + 1] := IntToStr(Costs[I].RecipeId);
    grdFoodCost.Cells[1, I + 1] := Costs[I].RecipeName;
    grdFoodCost.Cells[2, I + 1] := IntToStr(Costs[I].IngredientCount);
    grdFoodCost.Cells[3, I + 1] := FormatFloat('$#,##0.00', Costs[I].TotalCost);
    grdFoodCost.Cells[4, I + 1] := FormatFloat('$#,##0.00', Costs[I].SellingPrice);
    grdFoodCost.Cells[5, I + 1] := FormatFloat('0.0%', Costs[I].CostPercentage / 100);
    TotalPct := TotalPct + Costs[I].CostPercentage;
  end;

  if Length(Costs) > 0 then
    lblAvgCostPct.Caption := 'Avg Cost %: ' +
      FormatFloat('0.0%', (TotalPct / Length(Costs)) / 100)
  else
    lblAvgCostPct.Caption := 'Avg Cost %: N/A';
end;

procedure TfrmFoodCost.grdFoodCostDrawCell(Sender: TObject; ACol,
  ARow: Integer; Rect: TRect; State: TGridDrawState);
var
  CellText: string;
  CostPct: Double;
begin
  CellText := grdFoodCost.Cells[ACol, ARow];

  if (ARow = 0) or (gdFixed in State) then
  begin
    grdFoodCost.Canvas.Brush.Color := clBtnFace;
    grdFoodCost.Canvas.Font.Style := [fsBold];
  end
  else if (ACol = 5) and (ARow > 0) then
  begin
    CostPct := StrToFloatDef(
      StringReplace(CellText, '%', '', [rfReplaceAll]), 0) * 100;

    if CostPct > 35.0 then
      grdFoodCost.Canvas.Brush.Color := $008080FF  // Light red
    else if CostPct >= 25.0 then
      grdFoodCost.Canvas.Brush.Color := $0080FFFF  // Light yellow
    else
      grdFoodCost.Canvas.Brush.Color := $0080FF80;  // Light green

    if gdSelected in State then
      grdFoodCost.Canvas.Font.Color := clWindowText
    else
      grdFoodCost.Canvas.Font.Color := clWindowText;
  end
  else
  begin
    if gdSelected in State then
    begin
      grdFoodCost.Canvas.Brush.Color := clHighlight;
      grdFoodCost.Canvas.Font.Color := clHighlightText;
    end
    else
    begin
      grdFoodCost.Canvas.Brush.Color := clWindow;
      grdFoodCost.Canvas.Font.Color := clWindowText;
    end;
  end;

  grdFoodCost.Canvas.FillRect(Rect);
  grdFoodCost.Canvas.TextRect(Rect, Rect.Left + 4, Rect.Top + 2, CellText);
end;

procedure TfrmFoodCost.btnCloseClick(Sender: TObject);
begin
  ModalResult := mrCancel;
end;

procedure TfrmFoodCost.btnRefreshClick(Sender: TObject);
begin
  LoadFoodCosts;
end;

procedure TfrmFoodCost.btnAddClick(Sender: TObject);
begin
  FIsAdding := True;
  edtRecipeName.Text := '';
  edtIngredients.Text := '';
  edtTotalCost.Text := '';
  edtSellingPrice.Text := '';
  pnlEdit.Visible := True;
end;

procedure TfrmFoodCost.btnEditClick(Sender: TObject);
var
  Row: Integer;
begin
  Row := grdFoodCost.Row;
  if Row < 1 then
    Exit;
  FSelectedId := StrToIntDef(grdFoodCost.Cells[0, Row], 0);
  if FSelectedId = 0 then
    Exit;
  FIsAdding := False;
  edtRecipeName.Text := grdFoodCost.Cells[1, Row];
  edtIngredients.Text := grdFoodCost.Cells[2, Row];
  edtTotalCost.Text := StringReplace(grdFoodCost.Cells[3, Row], '$', '', [rfReplaceAll]);
  edtSellingPrice.Text := StringReplace(grdFoodCost.Cells[4, Row], '$', '', [rfReplaceAll]);
  pnlEdit.Visible := True;
end;

procedure TfrmFoodCost.btnDeleteClick(Sender: TObject);
var
  Row, Id: Integer;
begin
  Row := grdFoodCost.Row;
  if Row < 1 then
    Exit;
  Id := StrToIntDef(grdFoodCost.Cells[0, Row], 0);
  if Id = 0 then
    Exit;
  if MessageDlg('Delete this food cost entry?', mtConfirmation, [mbYes, mbNo], 0) = mrYes then
  begin
    FRestaurantBL.DeleteFoodCost(Id);
    LoadFoodCosts;
  end;
end;

procedure TfrmFoodCost.btnSaveClick(Sender: TObject);
var
  Cost: TFoodCostInfo;
begin
  Cost.RecipeName := edtRecipeName.Text;
  Cost.IngredientCount := StrToIntDef(edtIngredients.Text, 0);
  Cost.TotalCost := StrToFloatDef(edtTotalCost.Text, 0);
  Cost.SellingPrice := StrToFloatDef(edtSellingPrice.Text, 0);
  if FIsAdding then
    FRestaurantBL.AddFoodCost(Cost)
  else
  begin
    Cost.RecipeId := FSelectedId;
    if Cost.SellingPrice > 0 then
      Cost.CostPercentage := (Cost.TotalCost / Cost.SellingPrice) * 100
    else
      Cost.CostPercentage := 0;
    FRestaurantBL.UpdateFoodCost(Cost);
  end;
  pnlEdit.Visible := False;
  LoadFoodCosts;
end;

procedure TfrmFoodCost.btnCancelClick(Sender: TObject);
begin
  pnlEdit.Visible := False;
end;

end.
