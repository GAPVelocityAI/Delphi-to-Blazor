unit uFrmPayroll;

interface

uses
  Winapi.Windows, Winapi.Messages, System.SysUtils, System.Variants,
  System.Classes, Vcl.Graphics, Vcl.Controls, Vcl.Forms, Vcl.Dialogs,
  Vcl.StdCtrls, Vcl.ExtCtrls, Vcl.Grids,
  uCommonTypes, uAdminBL;

type
  TfrmPayroll = class(TForm)
    pnlTop: TPanel;
    lblTitle: TLabel;
    btnClose: TButton;
    grdPayroll: TStringGrid;
    pnlBottom: TPanel;
    btnRefresh: TButton;
    lblTotalGross: TLabel;
    lblTotalNet: TLabel;
    lblTotalDeductions: TLabel;
    btnAdd: TButton;
    btnEdit: TButton;
    btnDelete: TButton;
    pnlEdit: TPanel;
    lblEdtEmployee: TLabel;
    edtEmployee: TEdit;
    lblEdtPeriod: TLabel;
    edtPeriod: TEdit;
    lblEdtGrossPay: TLabel;
    edtGrossPay: TEdit;
    lblEdtDeductions: TLabel;
    edtDeductions: TEdit;
    lblEdtPayDate: TLabel;
    edtPayDate: TEdit;
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
    FAdminBL: TAdminBL;
    FIsAdding: Boolean;
    FSelectedId: Integer;
    procedure LoadPayroll;
  public
    { Public declarations }
  end;

var
  frmPayroll: TfrmPayroll;

implementation

{$R *.dfm}

procedure TfrmPayroll.FormCreate(Sender: TObject);
begin
  FAdminBL := TAdminBL.Create;
  TGridHelper.ConfigureGrid(grdPayroll,
    ['ID', 'Employee', 'Period', 'Gross Pay', 'Deductions', 'Net Pay', 'Pay Date'],
    [40, 140, 80, 90, 80, 90, 90]);
  LoadPayroll;
end;

procedure TfrmPayroll.FormDestroy(Sender: TObject);
begin
  FAdminBL.Free;
end;

procedure TfrmPayroll.btnCloseClick(Sender: TObject);
begin
  ModalResult := mrCancel;
end;

procedure TfrmPayroll.btnRefreshClick(Sender: TObject);
begin
  LoadPayroll;
end;

procedure TfrmPayroll.btnAddClick(Sender: TObject);
begin
  FIsAdding := True;
  edtEmployee.Text := '';
  edtPeriod.Text := '';
  edtGrossPay.Text := '';
  edtDeductions.Text := '';
  edtPayDate.Text := '';
  pnlEdit.Visible := True;
end;

procedure TfrmPayroll.btnEditClick(Sender: TObject);
var
  Row: Integer;
begin
  Row := grdPayroll.Row;
  if (Row < 1) or (grdPayroll.Cells[0, Row] = '') then
    Exit;

  FIsAdding := False;
  FSelectedId := StrToIntDef(grdPayroll.Cells[0, Row], 0);
  edtEmployee.Text := grdPayroll.Cells[1, Row];
  edtPeriod.Text := grdPayroll.Cells[2, Row];
  edtGrossPay.Text := StringReplace(StringReplace(grdPayroll.Cells[3, Row], '$', '', [rfReplaceAll]), ',', '', [rfReplaceAll]);
  edtDeductions.Text := StringReplace(StringReplace(grdPayroll.Cells[4, Row], '$', '', [rfReplaceAll]), ',', '', [rfReplaceAll]);
  edtPayDate.Text := grdPayroll.Cells[6, Row];
  pnlEdit.Visible := True;
end;

procedure TfrmPayroll.btnDeleteClick(Sender: TObject);
var
  Row: Integer;
  Id: Integer;
begin
  Row := grdPayroll.Row;
  if (Row < 1) or (grdPayroll.Cells[0, Row] = '') then
    Exit;

  Id := StrToIntDef(grdPayroll.Cells[0, Row], 0);
  if MessageDlg('Are you sure you want to delete this payroll entry?', mtConfirmation, [mbYes, mbNo], 0) = mrYes then
  begin
    FAdminBL.DeletePayroll(Id);
    LoadPayroll;
  end;
end;

procedure TfrmPayroll.btnSaveClick(Sender: TObject);
var
  Payroll: TPayrollInfo;
begin
  Payroll.EmployeeName := edtEmployee.Text;
  Payroll.Period := edtPeriod.Text;
  Payroll.GrossPay := StrToCurrDef(StringReplace(StringReplace(edtGrossPay.Text, '$', '', [rfReplaceAll]), ',', '', [rfReplaceAll]), 0);
  Payroll.Deductions := StrToCurrDef(StringReplace(StringReplace(edtDeductions.Text, '$', '', [rfReplaceAll]), ',', '', [rfReplaceAll]), 0);
  Payroll.PayDate := StrToDateDef(edtPayDate.Text, Now);

  if FIsAdding then
    FAdminBL.AddPayroll(Payroll)
  else
  begin
    Payroll.PayrollId := FSelectedId;
    Payroll.NetPay := Payroll.GrossPay - Payroll.Deductions;
    FAdminBL.UpdatePayroll(Payroll);
  end;

  pnlEdit.Visible := False;
  LoadPayroll;
end;

procedure TfrmPayroll.btnCancelClick(Sender: TObject);
begin
  pnlEdit.Visible := False;
end;

procedure TfrmPayroll.LoadPayroll;
var
  Payroll: TArray<TPayrollInfo>;
  I: Integer;
  TotalGross, TotalNet, TotalDeductions: Currency;
begin
  Payroll := FAdminBL.GetPayroll;
  TGridHelper.ClearGrid(grdPayroll);

  if Length(Payroll) > 0 then
    grdPayroll.RowCount := Length(Payroll) + 1
  else
    grdPayroll.RowCount := 2;

  TotalGross := 0;
  TotalNet := 0;
  TotalDeductions := 0;

  for I := 0 to High(Payroll) do
  begin
    grdPayroll.Cells[0, I + 1] := IntToStr(Payroll[I].PayrollId);
    grdPayroll.Cells[1, I + 1] := Payroll[I].EmployeeName;
    grdPayroll.Cells[2, I + 1] := Payroll[I].Period;
    grdPayroll.Cells[3, I + 1] := CurrToStrF(Payroll[I].GrossPay, ffCurrency, 2);
    grdPayroll.Cells[4, I + 1] := CurrToStrF(Payroll[I].Deductions, ffCurrency, 2);
    grdPayroll.Cells[5, I + 1] := CurrToStrF(Payroll[I].NetPay, ffCurrency, 2);
    grdPayroll.Cells[6, I + 1] := DateToStr(Payroll[I].PayDate);

    TotalGross := TotalGross + Payroll[I].GrossPay;
    TotalNet := TotalNet + Payroll[I].NetPay;
    TotalDeductions := TotalDeductions + Payroll[I].Deductions;
  end;

  lblTotalGross.Caption := 'Total Gross: ' + CurrToStrF(TotalGross, ffCurrency, 2);
  lblTotalNet.Caption := 'Total Net: ' + CurrToStrF(TotalNet, ffCurrency, 2);
  lblTotalDeductions.Caption := 'Deductions: ' + CurrToStrF(TotalDeductions, ffCurrency, 2);
end;

end.
