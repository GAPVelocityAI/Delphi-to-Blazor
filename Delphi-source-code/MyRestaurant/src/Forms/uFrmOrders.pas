unit uFrmOrders;

interface

uses
  Winapi.Windows, Winapi.Messages, System.SysUtils, System.Variants,
  System.Classes, Vcl.Graphics, Vcl.Controls, Vcl.Forms, Vcl.Dialogs,
  Vcl.Grids, Vcl.StdCtrls, Vcl.ExtCtrls,
  uCommonTypes, uRestaurantBL;

type
  TfrmOrders = class(TForm)
    pnlTop: TPanel;
    lblTitle: TLabel;
    btnClose: TButton;
    grdOrders: TStringGrid;
    grdDetails: TStringGrid;
    pnlBottom: TPanel;
    btnRefresh: TButton;
    btnViewDetails: TButton;
    btnAdd: TButton;
    btnEdit: TButton;
    btnDelete: TButton;
    pnlEdit: TPanel;
    lblEdtTableId: TLabel;
    edtTableId: TEdit;
    lblEdtStatus: TLabel;
    cmbEditStatus: TComboBox;
    lblEdtTotal: TLabel;
    edtTotalAmount: TEdit;
    btnSave: TButton;
    btnCancel: TButton;
    procedure FormCreate(Sender: TObject);
    procedure FormDestroy(Sender: TObject);
    procedure btnCloseClick(Sender: TObject);
    procedure btnRefreshClick(Sender: TObject);
    procedure btnViewDetailsClick(Sender: TObject);
    procedure grdOrdersSelectCell(Sender: TObject; ACol, ARow: Integer;
      var CanSelect: Boolean);
    procedure btnAddClick(Sender: TObject);
    procedure btnEditClick(Sender: TObject);
    procedure btnDeleteClick(Sender: TObject);
    procedure btnSaveClick(Sender: TObject);
    procedure btnCancelClick(Sender: TObject);
  private
    FRestaurantBL: TRestaurantBL;
    FIsAdding: Boolean;
    FSelectedId: Integer;
    procedure LoadOrders;
    procedure LoadOrderDetails(AOrderId: Integer);
  public
    { Public declarations }
  end;

var
  frmOrders: TfrmOrders;

implementation

{$R *.dfm}

procedure TfrmOrders.FormCreate(Sender: TObject);
begin
  FRestaurantBL := TRestaurantBL.Create;
  TGridHelper.ConfigureGrid(grdOrders,
    ['Order ID', 'Table', 'Date', 'Status', 'Total'],
    [60, 60, 120, 90, 90]);
  TGridHelper.ConfigureGrid(grdDetails,
    ['Detail ID', 'Item', 'Qty', 'Unit Price', 'Subtotal'],
    [60, 180, 50, 90, 90]);
  LoadOrders;
end;

procedure TfrmOrders.FormDestroy(Sender: TObject);
begin
  FRestaurantBL.Free;
end;

procedure TfrmOrders.LoadOrders;
var
  Orders: TArray<TOrderInfo>;
  I: Integer;
begin
  Orders := FRestaurantBL.GetOrders;
  TGridHelper.ClearGrid(grdOrders);
  TGridHelper.ClearGrid(grdDetails);

  if Length(Orders) = 0 then
    Exit;

  grdOrders.RowCount := Length(Orders) + 1;
  for I := 0 to High(Orders) do
  begin
    grdOrders.Cells[0, I + 1] := IntToStr(Orders[I].OrderId);
    grdOrders.Cells[1, I + 1] := IntToStr(Orders[I].TableId);
    grdOrders.Cells[2, I + 1] := FormatDateTime('yyyy-mm-dd hh:nn', Orders[I].OrderDate);
    grdOrders.Cells[3, I + 1] := Orders[I].Status.ToString;
    grdOrders.Cells[4, I + 1] := FormatFloat('$#,##0.00', Orders[I].TotalAmount);
  end;
end;

procedure TfrmOrders.LoadOrderDetails(AOrderId: Integer);
var
  Details: TArray<TOrderDetailInfo>;
  I: Integer;
begin
  Details := FRestaurantBL.GetOrderDetails(AOrderId);
  TGridHelper.ClearGrid(grdDetails);

  if Length(Details) = 0 then
    Exit;

  grdDetails.RowCount := Length(Details) + 1;
  for I := 0 to High(Details) do
  begin
    grdDetails.Cells[0, I + 1] := IntToStr(Details[I].DetailId);
    grdDetails.Cells[1, I + 1] := Details[I].ItemName;
    grdDetails.Cells[2, I + 1] := IntToStr(Details[I].Quantity);
    grdDetails.Cells[3, I + 1] := FormatFloat('$#,##0.00', Details[I].UnitPrice);
    grdDetails.Cells[4, I + 1] := FormatFloat('$#,##0.00', Details[I].Subtotal);
  end;
end;

procedure TfrmOrders.grdOrdersSelectCell(Sender: TObject; ACol, ARow: Integer;
  var CanSelect: Boolean);
var
  OrderId: Integer;
begin
  CanSelect := True;
  if (ARow > 0) and (grdOrders.Cells[0, ARow] <> '') then
  begin
    OrderId := StrToIntDef(grdOrders.Cells[0, ARow], 0);
    if OrderId > 0 then
      LoadOrderDetails(OrderId);
  end;
end;

procedure TfrmOrders.btnCloseClick(Sender: TObject);
begin
  ModalResult := mrCancel;
end;

procedure TfrmOrders.btnRefreshClick(Sender: TObject);
begin
  LoadOrders;
end;

procedure TfrmOrders.btnViewDetailsClick(Sender: TObject);
var
  OrderId: Integer;
begin
  if grdOrders.Row > 0 then
  begin
    OrderId := StrToIntDef(grdOrders.Cells[0, grdOrders.Row], 0);
    if OrderId > 0 then
      LoadOrderDetails(OrderId);
  end;
end;

procedure TfrmOrders.btnAddClick(Sender: TObject);
begin
  FIsAdding := True;
  edtTableId.Text := '';
  cmbEditStatus.ItemIndex := 0;
  edtTotalAmount.Text := '';
  pnlEdit.Visible := True;
end;

procedure TfrmOrders.btnEditClick(Sender: TObject);
var
  Row: Integer;
  StatusText: string;
begin
  Row := grdOrders.Row;
  if Row < 1 then
    Exit;
  FSelectedId := StrToIntDef(grdOrders.Cells[0, Row], 0);
  if FSelectedId = 0 then
    Exit;
  FIsAdding := False;
  edtTableId.Text := grdOrders.Cells[1, Row];
  StatusText := grdOrders.Cells[3, Row];
  if StatusText = 'Pending' then cmbEditStatus.ItemIndex := 0
  else if StatusText = 'Preparing' then cmbEditStatus.ItemIndex := 1
  else if StatusText = 'Served' then cmbEditStatus.ItemIndex := 2
  else if StatusText = 'Paid' then cmbEditStatus.ItemIndex := 3
  else if StatusText = 'Cancelled' then cmbEditStatus.ItemIndex := 4
  else cmbEditStatus.ItemIndex := 0;
  edtTotalAmount.Text := StringReplace(grdOrders.Cells[4, Row], '$', '', [rfReplaceAll]);
  pnlEdit.Visible := True;
end;

procedure TfrmOrders.btnDeleteClick(Sender: TObject);
var
  Row, Id: Integer;
begin
  Row := grdOrders.Row;
  if Row < 1 then
    Exit;
  Id := StrToIntDef(grdOrders.Cells[0, Row], 0);
  if Id = 0 then
    Exit;
  if MessageDlg('Delete this order?', mtConfirmation, [mbYes, mbNo], 0) = mrYes then
  begin
    FRestaurantBL.DeleteOrder(Id);
    LoadOrders;
  end;
end;

procedure TfrmOrders.btnSaveClick(Sender: TObject);
var
  Order: TOrderInfo;
begin
  Order.TableId := StrToIntDef(edtTableId.Text, 0);
  Order.Status := TOrderStatus(cmbEditStatus.ItemIndex);
  Order.TotalAmount := StrToFloatDef(edtTotalAmount.Text, 0);
  if FIsAdding then
  begin
    Order.OrderDate := Now;
    FRestaurantBL.AddOrder(Order);
  end
  else
  begin
    Order.OrderId := FSelectedId;
    // Preserve original order date from grid
    Order.OrderDate := Now;
    FRestaurantBL.UpdateOrder(Order);
  end;
  pnlEdit.Visible := False;
  LoadOrders;
end;

procedure TfrmOrders.btnCancelClick(Sender: TObject);
begin
  pnlEdit.Visible := False;
end;

end.
