unit uFrmPersonnel;

interface

uses
  Winapi.Windows, Winapi.Messages, System.SysUtils, System.Variants,
  System.Classes, Vcl.Graphics, Vcl.Controls, Vcl.Forms, Vcl.Dialogs,
  Vcl.StdCtrls, Vcl.ExtCtrls, Vcl.Grids,
  uCommonTypes, uAdminBL;

type
  TfrmPersonnel = class(TForm)
    pnlTop: TPanel;
    lblTitle: TLabel;
    btnClose: TButton;
    grdPersonnel: TStringGrid;
    pnlBottom: TPanel;
    btnRefresh: TButton;
    btnShowActive: TButton;
    btnShowAll: TButton;
    lblCount: TLabel;
    btnAdd: TButton;
    btnEdit: TButton;
    btnDelete: TButton;
    pnlEdit: TPanel;
    lblEdtFirstName: TLabel;
    edtFirstName: TEdit;
    lblEdtLastName: TLabel;
    edtLastName: TEdit;
    lblEdtPosition: TLabel;
    cmbPosition: TComboBox;
    lblEdtHireDate: TLabel;
    edtHireDate: TEdit;
    lblEdtSalary: TLabel;
    edtSalary: TEdit;
    lblEdtActive: TLabel;
    cmbActive: TComboBox;
    btnSave: TButton;
    btnCancel: TButton;
    procedure FormCreate(Sender: TObject);
    procedure FormDestroy(Sender: TObject);
    procedure btnCloseClick(Sender: TObject);
    procedure btnRefreshClick(Sender: TObject);
    procedure btnShowActiveClick(Sender: TObject);
    procedure btnShowAllClick(Sender: TObject);
    procedure btnAddClick(Sender: TObject);
    procedure btnEditClick(Sender: TObject);
    procedure btnDeleteClick(Sender: TObject);
    procedure btnSaveClick(Sender: TObject);
    procedure btnCancelClick(Sender: TObject);
  private
    FAdminBL: TAdminBL;
    FIsAdding: Boolean;
    FSelectedId: Integer;
    procedure LoadEmployees(AActiveOnly: Boolean);
  public
    { Public declarations }
  end;

var
  frmPersonnel: TfrmPersonnel;

implementation

{$R *.dfm}

procedure TfrmPersonnel.FormCreate(Sender: TObject);
begin
  FAdminBL := TAdminBL.Create;
  TGridHelper.ConfigureGrid(grdPersonnel,
    ['ID', 'First Name', 'Last Name', 'Position', 'Hire Date', 'Salary', 'Active'],
    [40, 100, 100, 120, 90, 80, 60]);
  LoadEmployees(False);
end;

procedure TfrmPersonnel.FormDestroy(Sender: TObject);
begin
  FAdminBL.Free;
end;

procedure TfrmPersonnel.btnCloseClick(Sender: TObject);
begin
  ModalResult := mrCancel;
end;

procedure TfrmPersonnel.btnRefreshClick(Sender: TObject);
begin
  LoadEmployees(False);
end;

procedure TfrmPersonnel.btnShowActiveClick(Sender: TObject);
begin
  LoadEmployees(True);
end;

procedure TfrmPersonnel.btnShowAllClick(Sender: TObject);
begin
  LoadEmployees(False);
end;

procedure TfrmPersonnel.btnAddClick(Sender: TObject);
begin
  FIsAdding := True;
  edtFirstName.Text := '';
  edtLastName.Text := '';
  cmbPosition.ItemIndex := -1;
  edtHireDate.Text := '';
  edtSalary.Text := '';
  cmbActive.ItemIndex := 0;
  pnlEdit.Visible := True;
end;

procedure TfrmPersonnel.btnEditClick(Sender: TObject);
var
  Row: Integer;
  SalaryStr: string;
begin
  Row := grdPersonnel.Row;
  if (Row < 1) or (grdPersonnel.Cells[0, Row] = '') then
    Exit;

  FIsAdding := False;
  FSelectedId := StrToIntDef(grdPersonnel.Cells[0, Row], 0);
  edtFirstName.Text := grdPersonnel.Cells[1, Row];
  edtLastName.Text := grdPersonnel.Cells[2, Row];
  cmbPosition.Text := grdPersonnel.Cells[3, Row];
  edtHireDate.Text := grdPersonnel.Cells[4, Row];
  SalaryStr := grdPersonnel.Cells[5, Row];
  SalaryStr := StringReplace(SalaryStr, '$', '', [rfReplaceAll]);
  SalaryStr := StringReplace(SalaryStr, ',', '', [rfReplaceAll]);
  edtSalary.Text := SalaryStr;
  if grdPersonnel.Cells[6, Row] = 'Yes' then
    cmbActive.ItemIndex := 0
  else
    cmbActive.ItemIndex := 1;
  pnlEdit.Visible := True;
end;

procedure TfrmPersonnel.btnDeleteClick(Sender: TObject);
var
  Row: Integer;
  Id: Integer;
begin
  Row := grdPersonnel.Row;
  if (Row < 1) or (grdPersonnel.Cells[0, Row] = '') then
    Exit;

  Id := StrToIntDef(grdPersonnel.Cells[0, Row], 0);
  if MessageDlg('Are you sure you want to delete this employee?', mtConfirmation, [mbYes, mbNo], 0) = mrYes then
  begin
    FAdminBL.DeleteEmployee(Id);
    LoadEmployees(False);
  end;
end;

procedure TfrmPersonnel.btnSaveClick(Sender: TObject);
var
  Employee: TEmployeeInfo;
begin
  Employee.FirstName := edtFirstName.Text;
  Employee.LastName := edtLastName.Text;
  Employee.Position := cmbPosition.Text;
  Employee.HireDate := StrToDateDef(edtHireDate.Text, Now);
  Employee.Salary := StrToCurrDef(StringReplace(StringReplace(edtSalary.Text, '$', '', [rfReplaceAll]), ',', '', [rfReplaceAll]), 0);
  Employee.Active := (cmbActive.ItemIndex = 0);

  if FIsAdding then
    FAdminBL.AddEmployee(Employee)
  else
  begin
    Employee.EmployeeId := FSelectedId;
    FAdminBL.UpdateEmployee(Employee);
  end;

  pnlEdit.Visible := False;
  LoadEmployees(False);
end;

procedure TfrmPersonnel.btnCancelClick(Sender: TObject);
begin
  pnlEdit.Visible := False;
end;

procedure TfrmPersonnel.LoadEmployees(AActiveOnly: Boolean);
var
  Employees: TArray<TEmployeeInfo>;
  I: Integer;
begin
  if AActiveOnly then
    Employees := FAdminBL.GetActiveEmployees
  else
    Employees := FAdminBL.GetEmployees;

  TGridHelper.ClearGrid(grdPersonnel);

  if Length(Employees) > 0 then
    grdPersonnel.RowCount := Length(Employees) + 1
  else
    grdPersonnel.RowCount := 2;

  for I := 0 to High(Employees) do
  begin
    grdPersonnel.Cells[0, I + 1] := IntToStr(Employees[I].EmployeeId);
    grdPersonnel.Cells[1, I + 1] := Employees[I].FirstName;
    grdPersonnel.Cells[2, I + 1] := Employees[I].LastName;
    grdPersonnel.Cells[3, I + 1] := Employees[I].Position;
    grdPersonnel.Cells[4, I + 1] := DateToStr(Employees[I].HireDate);
    grdPersonnel.Cells[5, I + 1] := CurrToStrF(Employees[I].Salary, ffCurrency, 2);
    if Employees[I].Active then
      grdPersonnel.Cells[6, I + 1] := 'Yes'
    else
      grdPersonnel.Cells[6, I + 1] := 'No';
  end;

  lblCount.Caption := 'Employees: ' + IntToStr(Length(Employees));
end;

end.
