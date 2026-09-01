unit uFrmTables;

interface

uses
  Winapi.Windows, Winapi.Messages, System.SysUtils, System.Variants,
  System.Classes, Vcl.Graphics, Vcl.Controls, Vcl.Forms, Vcl.Dialogs,
  Vcl.Grids, Vcl.StdCtrls, Vcl.ExtCtrls,
  uCommonTypes, uTablesBL;

type
  TfrmTables = class(TForm)
    pnlTop: TPanel;
    lblTitle: TLabel;
    btnClose: TButton;
    grdTables: TStringGrid;
    pnlBottom: TPanel;
    btnRefresh: TButton;
    btnFilterAvailable: TButton;
    btnShowAll: TButton;
    btnAdd: TButton;
    btnEdit: TButton;
    btnDelete: TButton;
    pnlEdit: TPanel;
    lblEdtNumber: TLabel;
    edtNumber: TEdit;
    lblEdtCapacity: TLabel;
    edtCapacity: TEdit;
    lblEdtStatus: TLabel;
    cmbEditStatus: TComboBox;
    lblEdtZone: TLabel;
    cmbEditZone: TComboBox;
    btnSave: TButton;
    btnCancel: TButton;
    procedure FormCreate(Sender: TObject);
    procedure FormDestroy(Sender: TObject);
    procedure btnCloseClick(Sender: TObject);
    procedure btnRefreshClick(Sender: TObject);
    procedure btnFilterAvailableClick(Sender: TObject);
    procedure btnShowAllClick(Sender: TObject);
    procedure btnAddClick(Sender: TObject);
    procedure btnEditClick(Sender: TObject);
    procedure btnDeleteClick(Sender: TObject);
    procedure btnSaveClick(Sender: TObject);
    procedure btnCancelClick(Sender: TObject);
  private
    FTablesBL: TTablesBL;
    FIsAdding: Boolean;
    FSelectedId: Integer;
    procedure LoadTables(const ATables: TArray<TTableInfo>);
  public
    { Public declarations }
  end;

var
  frmTables: TfrmTables;

implementation

{$R *.dfm}

procedure TfrmTables.FormCreate(Sender: TObject);
begin
  FTablesBL := TTablesBL.Create;
  TGridHelper.ConfigureGrid(grdTables,
    ['ID', 'Number', 'Capacity', 'Status', 'Zone'],
    [40, 70, 70, 90, 120]);
  LoadTables(FTablesBL.GetTables);
end;

procedure TfrmTables.LoadTables(const ATables: TArray<TTableInfo>);
var
  I: Integer;
begin
  TGridHelper.ClearGrid(grdTables);
  if Length(ATables) = 0 then
    Exit;

  grdTables.RowCount := Length(ATables) + 1;
  for I := 0 to High(ATables) do
  begin
    grdTables.Cells[0, I + 1] := IntToStr(ATables[I].TableId);
    grdTables.Cells[1, I + 1] := IntToStr(ATables[I].TableNumber);
    grdTables.Cells[2, I + 1] := IntToStr(ATables[I].Capacity);
    grdTables.Cells[3, I + 1] := ATables[I].Status.ToString;
    grdTables.Cells[4, I + 1] := ATables[I].Zone;
  end;
end;

procedure TfrmTables.FormDestroy(Sender: TObject);
begin
  FTablesBL.Free;
end;

procedure TfrmTables.btnCloseClick(Sender: TObject);
begin
  ModalResult := mrCancel;
end;

procedure TfrmTables.btnRefreshClick(Sender: TObject);
begin
  LoadTables(FTablesBL.GetTables);
end;

procedure TfrmTables.btnFilterAvailableClick(Sender: TObject);
begin
  LoadTables(FTablesBL.GetAvailableTables);
end;

procedure TfrmTables.btnShowAllClick(Sender: TObject);
begin
  LoadTables(FTablesBL.GetTables);
end;

procedure TfrmTables.btnAddClick(Sender: TObject);
begin
  FIsAdding := True;
  edtNumber.Text := '';
  edtCapacity.Text := '';
  cmbEditStatus.ItemIndex := 0;
  cmbEditZone.ItemIndex := 0;
  pnlEdit.Visible := True;
end;

procedure TfrmTables.btnEditClick(Sender: TObject);
var
  Row: Integer;
begin
  Row := grdTables.Row;
  if Row < 1 then
    Exit;

  FSelectedId := StrToIntDef(grdTables.Cells[0, Row], 0);
  edtNumber.Text := grdTables.Cells[1, Row];
  edtCapacity.Text := grdTables.Cells[2, Row];

  if grdTables.Cells[3, Row] = 'Available' then
    cmbEditStatus.ItemIndex := 0
  else if grdTables.Cells[3, Row] = 'Occupied' then
    cmbEditStatus.ItemIndex := 1
  else if grdTables.Cells[3, Row] = 'Reserved' then
    cmbEditStatus.ItemIndex := 2
  else if grdTables.Cells[3, Row] = 'Closed' then
    cmbEditStatus.ItemIndex := 3
  else
    cmbEditStatus.ItemIndex := 0;

  cmbEditZone.ItemIndex := cmbEditZone.Items.IndexOf(grdTables.Cells[4, Row]);

  FIsAdding := False;
  pnlEdit.Visible := True;
end;

procedure TfrmTables.btnDeleteClick(Sender: TObject);
var
  Row, Id: Integer;
begin
  Row := grdTables.Row;
  if Row < 1 then
    Exit;

  Id := StrToIntDef(grdTables.Cells[0, Row], 0);
  if MessageDlg('Are you sure you want to delete this table?',
    mtConfirmation, [mbYes, mbNo], 0) = mrYes then
  begin
    FTablesBL.DeleteTable(Id);
    LoadTables(FTablesBL.GetTables);
  end;
end;

procedure TfrmTables.btnSaveClick(Sender: TObject);
var
  Table: TTableInfo;
begin
  Table.TableNumber := StrToIntDef(edtNumber.Text, 0);
  Table.Capacity := StrToIntDef(edtCapacity.Text, 0);
  Table.Status := TTableStatus(cmbEditStatus.ItemIndex);
  Table.Zone := cmbEditZone.Text;

  if FIsAdding then
    FTablesBL.AddTable(Table)
  else
  begin
    Table.TableId := FSelectedId;
    FTablesBL.UpdateTable(Table);
  end;

  pnlEdit.Visible := False;
  LoadTables(FTablesBL.GetTables);
end;

procedure TfrmTables.btnCancelClick(Sender: TObject);
begin
  pnlEdit.Visible := False;
end;

end.
