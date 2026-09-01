unit uFrmMenuView;

interface

uses
  Winapi.Windows, Winapi.Messages, System.SysUtils, System.Variants,
  System.Classes, Vcl.Graphics, Vcl.Controls, Vcl.Forms, Vcl.Dialogs,
  Vcl.Grids, Vcl.StdCtrls, Vcl.ExtCtrls,
  uCommonTypes, uRestaurantBL;

type
  TfrmMenuView = class(TForm)
    pnlTop: TPanel;
    lblTitle: TLabel;
    btnClose: TButton;
    grdMenu: TStringGrid;
    pnlBottom: TPanel;
    btnRefresh: TButton;
    lblCategory: TLabel;
    cmbCategory: TComboBox;
    btnAdd: TButton;
    btnEdit: TButton;
    btnDelete: TButton;
    pnlEdit: TPanel;
    lblEdtName: TLabel;
    edtItemName: TEdit;
    lblEdtCategory: TLabel;
    cmbEditCategory: TComboBox;
    lblEdtPrice: TLabel;
    edtPrice: TEdit;
    lblEdtCost: TLabel;
    edtCost: TEdit;
    lblEdtActive: TLabel;
    cmbEditActive: TComboBox;
    btnSave: TButton;
    btnCancel: TButton;
    procedure FormCreate(Sender: TObject);
    procedure FormDestroy(Sender: TObject);
    procedure btnCloseClick(Sender: TObject);
    procedure btnRefreshClick(Sender: TObject);
    procedure cmbCategoryChange(Sender: TObject);
    procedure btnAddClick(Sender: TObject);
    procedure btnEditClick(Sender: TObject);
    procedure btnDeleteClick(Sender: TObject);
    procedure btnSaveClick(Sender: TObject);
    procedure btnCancelClick(Sender: TObject);
  private
    FRestaurantBL: TRestaurantBL;
    FAllMenuItems: TArray<TMenuItemInfo>;
    FIsAdding: Boolean;
    FSelectedId: Integer;
    procedure LoadMenuItems(const AItems: TArray<TMenuItemInfo>);
    procedure FilterByCategory(const ACategory: string);
  public
    { Public declarations }
  end;

var
  frmMenuView: TfrmMenuView;

implementation

{$R *.dfm}

procedure TfrmMenuView.FormCreate(Sender: TObject);
begin
  FRestaurantBL := TRestaurantBL.Create;
  TGridHelper.ConfigureGrid(grdMenu,
    ['ID', 'Name', 'Category', 'Price', 'Cost', 'Active'],
    [40, 150, 100, 70, 70, 60]);
  FAllMenuItems := FRestaurantBL.GetMenuItems;
  LoadMenuItems(FAllMenuItems);
end;

procedure TfrmMenuView.FormDestroy(Sender: TObject);
begin
  FRestaurantBL.Free;
end;

procedure TfrmMenuView.LoadMenuItems(const AItems: TArray<TMenuItemInfo>);
var
  I: Integer;
begin
  TGridHelper.ClearGrid(grdMenu);
  if Length(AItems) = 0 then
    Exit;

  grdMenu.RowCount := Length(AItems) + 1;
  for I := 0 to High(AItems) do
  begin
    grdMenu.Cells[0, I + 1] := IntToStr(AItems[I].ItemId);
    grdMenu.Cells[1, I + 1] := AItems[I].ItemName;
    grdMenu.Cells[2, I + 1] := AItems[I].Category;
    grdMenu.Cells[3, I + 1] := FormatFloat('$#,##0.00', AItems[I].Price);
    grdMenu.Cells[4, I + 1] := FormatFloat('$#,##0.00', AItems[I].Cost);
    if AItems[I].Active then
      grdMenu.Cells[5, I + 1] := 'Yes'
    else
      grdMenu.Cells[5, I + 1] := 'No';
  end;
end;

procedure TfrmMenuView.FilterByCategory(const ACategory: string);
var
  Filtered: TArray<TMenuItemInfo>;
  I, Count: Integer;
begin
  if (ACategory = '') or (ACategory = 'All') then
  begin
    LoadMenuItems(FAllMenuItems);
    Exit;
  end;

  Count := 0;
  for I := 0 to High(FAllMenuItems) do
    if SameText(FAllMenuItems[I].Category, ACategory) then
      Inc(Count);

  SetLength(Filtered, Count);
  Count := 0;
  for I := 0 to High(FAllMenuItems) do
    if SameText(FAllMenuItems[I].Category, ACategory) then
    begin
      Filtered[Count] := FAllMenuItems[I];
      Inc(Count);
    end;

  LoadMenuItems(Filtered);
end;

procedure TfrmMenuView.btnCloseClick(Sender: TObject);
begin
  ModalResult := mrCancel;
end;

procedure TfrmMenuView.btnRefreshClick(Sender: TObject);
begin
  FAllMenuItems := FRestaurantBL.GetMenuItems;
  cmbCategory.ItemIndex := 0;
  LoadMenuItems(FAllMenuItems);
end;

procedure TfrmMenuView.cmbCategoryChange(Sender: TObject);
begin
  FilterByCategory(cmbCategory.Text);
end;

procedure TfrmMenuView.btnAddClick(Sender: TObject);
begin
  FIsAdding := True;
  edtItemName.Text := '';
  cmbEditCategory.ItemIndex := 0;
  edtPrice.Text := '';
  edtCost.Text := '';
  cmbEditActive.ItemIndex := 0;
  pnlEdit.Visible := True;
end;

procedure TfrmMenuView.btnEditClick(Sender: TObject);
var
  Row: Integer;
begin
  Row := grdMenu.Row;
  if Row < 1 then
    Exit;
  FSelectedId := StrToIntDef(grdMenu.Cells[0, Row], 0);
  if FSelectedId = 0 then
    Exit;
  FIsAdding := False;
  edtItemName.Text := grdMenu.Cells[1, Row];
  cmbEditCategory.Text := grdMenu.Cells[2, Row];
  edtPrice.Text := StringReplace(grdMenu.Cells[3, Row], '$', '', [rfReplaceAll]);
  edtCost.Text := StringReplace(grdMenu.Cells[4, Row], '$', '', [rfReplaceAll]);
  if grdMenu.Cells[5, Row] = 'Yes' then
    cmbEditActive.ItemIndex := 0
  else
    cmbEditActive.ItemIndex := 1;
  pnlEdit.Visible := True;
end;

procedure TfrmMenuView.btnDeleteClick(Sender: TObject);
var
  Row, Id: Integer;
begin
  Row := grdMenu.Row;
  if Row < 1 then
    Exit;
  Id := StrToIntDef(grdMenu.Cells[0, Row], 0);
  if Id = 0 then
    Exit;
  if MessageDlg('Delete this menu item?', mtConfirmation, [mbYes, mbNo], 0) = mrYes then
  begin
    FRestaurantBL.DeleteMenuItem(Id);
    FAllMenuItems := FRestaurantBL.GetMenuItems;
    LoadMenuItems(FAllMenuItems);
  end;
end;

procedure TfrmMenuView.btnSaveClick(Sender: TObject);
var
  Item: TMenuItemInfo;
begin
  Item.ItemName := edtItemName.Text;
  Item.Category := cmbEditCategory.Text;
  Item.Price := StrToFloatDef(edtPrice.Text, 0);
  Item.Cost := StrToFloatDef(edtCost.Text, 0);
  Item.Active := (cmbEditActive.ItemIndex = 0);
  if FIsAdding then
    FRestaurantBL.AddMenuItem(Item)
  else
  begin
    Item.ItemId := FSelectedId;
    FRestaurantBL.UpdateMenuItem(Item);
  end;
  pnlEdit.Visible := False;
  FAllMenuItems := FRestaurantBL.GetMenuItems;
  LoadMenuItems(FAllMenuItems);
end;

procedure TfrmMenuView.btnCancelClick(Sender: TObject);
begin
  pnlEdit.Visible := False;
end;

end.
