unit uFrmBillCheck;

interface

uses
  Winapi.Windows, Winapi.Messages, System.SysUtils, System.Variants,
  System.Classes, Vcl.Graphics, Vcl.Controls, Vcl.Forms, Vcl.Dialogs,
  Vcl.Grids, Vcl.StdCtrls, Vcl.ExtCtrls,
  uCommonTypes, uRestaurantBL;

type
  TfrmBillCheck = class(TForm)
    pnlTop: TPanel;
    lblTitle: TLabel;
    btnClose: TButton;
    grdBills: TStringGrid;
    pnlBottom: TPanel;
    btnRefresh: TButton;
    pnlSummary: TPanel;
    lblTotalBills: TLabel;
    lblTotalRevenue: TLabel;
    btnAdd: TButton;
    btnEdit: TButton;
    btnDelete: TButton;
    pnlEdit: TPanel;
    lblEdtOrderId: TLabel;
    edtOrderId: TEdit;
    lblEdtSubtotal: TLabel;
    edtSubtotal: TEdit;
    lblEdtTip: TLabel;
    edtTip: TEdit;
    lblEdtPayment: TLabel;
    cmbEditPayment: TComboBox;
    btnSave: TButton;
    btnCancel: TButton;
    procedure FormCreate(Sender: TObject);
    procedure FormDestroy(Sender: TObject);
    procedure btnCloseClick(Sender: TObject);
    procedure btnRefreshClick(Sender: TObject);
    procedure btnAddClick(Sender: TObject);
    procedure btnEditClick(Sender: TObject);
    procedure btnDeleteClick(Sender: TObject);
    procedure btnSaveClick(Sender: TObject);
    procedure btnCancelClick(Sender: TObject);
  private
    FRestaurantBL: TRestaurantBL;
    FIsAdding: Boolean;
    FSelectedId: Integer;
    procedure LoadBills;
  public
    { Public declarations }
  end;

var
  frmBillCheck: TfrmBillCheck;

implementation

{$R *.dfm}

procedure TfrmBillCheck.FormCreate(Sender: TObject);
begin
  FRestaurantBL := TRestaurantBL.Create;
  TGridHelper.ConfigureGrid(grdBills,
    ['Bill ID', 'Order ID', 'Subtotal', 'Tax', 'Tip', 'Total', 'Payment', 'Date'],
    [50, 60, 80, 60, 60, 80, 90, 100]);
  LoadBills;
end;

procedure TfrmBillCheck.FormDestroy(Sender: TObject);
begin
  FRestaurantBL.Free;
end;

procedure TfrmBillCheck.LoadBills;
var
  Bills: TArray<TBillInfo>;
  I: Integer;
  TotalRevenue: Currency;
begin
  Bills := FRestaurantBL.GetBills;
  TGridHelper.ClearGrid(grdBills);

  if Length(Bills) = 0 then
    Exit;

  TotalRevenue := 0;
  grdBills.RowCount := Length(Bills) + 1;
  for I := 0 to High(Bills) do
  begin
    grdBills.Cells[0, I + 1] := IntToStr(Bills[I].BillId);
    grdBills.Cells[1, I + 1] := IntToStr(Bills[I].OrderId);
    grdBills.Cells[2, I + 1] := FormatFloat('$#,##0.00', Bills[I].Subtotal);
    grdBills.Cells[3, I + 1] := FormatFloat('$#,##0.00', Bills[I].Tax);
    grdBills.Cells[4, I + 1] := FormatFloat('$#,##0.00', Bills[I].Tip);
    grdBills.Cells[5, I + 1] := FormatFloat('$#,##0.00', Bills[I].Total);
    grdBills.Cells[6, I + 1] := Bills[I].PaymentMethod.ToString;
    grdBills.Cells[7, I + 1] := FormatDateTime('yyyy-mm-dd hh:nn', Bills[I].PaidDate);
    TotalRevenue := TotalRevenue + Bills[I].Total;
  end;

  lblTotalBills.Caption := 'Total Bills: ' + IntToStr(Length(Bills));
  lblTotalRevenue.Caption := 'Total Revenue: ' + FormatFloat('$#,##0.00', TotalRevenue);
end;

procedure TfrmBillCheck.btnCloseClick(Sender: TObject);
begin
  ModalResult := mrCancel;
end;

procedure TfrmBillCheck.btnRefreshClick(Sender: TObject);
begin
  LoadBills;
end;

procedure TfrmBillCheck.btnAddClick(Sender: TObject);
begin
  FIsAdding := True;
  edtOrderId.Text := '';
  edtSubtotal.Text := '';
  edtTip.Text := '';
  cmbEditPayment.ItemIndex := 0;
  pnlEdit.Visible := True;
end;

procedure TfrmBillCheck.btnEditClick(Sender: TObject);
var
  Row: Integer;
  PayText: string;
begin
  Row := grdBills.Row;
  if Row < 1 then
    Exit;
  FSelectedId := StrToIntDef(grdBills.Cells[0, Row], 0);
  if FSelectedId = 0 then
    Exit;
  FIsAdding := False;
  edtOrderId.Text := grdBills.Cells[1, Row];
  edtSubtotal.Text := StringReplace(grdBills.Cells[2, Row], '$', '', [rfReplaceAll]);
  edtTip.Text := StringReplace(grdBills.Cells[4, Row], '$', '', [rfReplaceAll]);
  PayText := grdBills.Cells[6, Row];
  if PayText = 'Cash' then cmbEditPayment.ItemIndex := 0
  else if PayText = 'Credit Card' then cmbEditPayment.ItemIndex := 1
  else if PayText = 'Debit Card' then cmbEditPayment.ItemIndex := 2
  else cmbEditPayment.ItemIndex := 0;
  pnlEdit.Visible := True;
end;

procedure TfrmBillCheck.btnDeleteClick(Sender: TObject);
var
  Row, Id: Integer;
begin
  Row := grdBills.Row;
  if Row < 1 then
    Exit;
  Id := StrToIntDef(grdBills.Cells[0, Row], 0);
  if Id = 0 then
    Exit;
  if MessageDlg('Delete this bill?', mtConfirmation, [mbYes, mbNo], 0) = mrYes then
  begin
    FRestaurantBL.DeleteBill(Id);
    LoadBills;
  end;
end;

procedure TfrmBillCheck.btnSaveClick(Sender: TObject);
var
  Bill: TBillInfo;
begin
  Bill.OrderId := StrToIntDef(edtOrderId.Text, 0);
  Bill.Subtotal := StrToFloatDef(edtSubtotal.Text, 0);
  Bill.Tip := StrToFloatDef(edtTip.Text, 0);
  Bill.PaymentMethod := TPaymentMethod(cmbEditPayment.ItemIndex);
  Bill.PaidDate := Now;
  if FIsAdding then
    FRestaurantBL.AddBill(Bill)
  else
  begin
    Bill.BillId := FSelectedId;
    Bill.Tax := Bill.Subtotal * 0.08;
    Bill.Total := Bill.Subtotal + Bill.Tax + Bill.Tip;
    FRestaurantBL.UpdateBill(Bill);
  end;
  pnlEdit.Visible := False;
  LoadBills;
end;

procedure TfrmBillCheck.btnCancelClick(Sender: TObject);
begin
  pnlEdit.Visible := False;
end;

end.
