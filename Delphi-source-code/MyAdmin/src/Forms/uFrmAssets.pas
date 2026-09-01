unit uFrmAssets;

interface

uses
  Winapi.Windows, Winapi.Messages, System.SysUtils, System.Variants,
  System.Classes, Vcl.Graphics, Vcl.Controls, Vcl.Forms, Vcl.Dialogs,
  Vcl.StdCtrls, Vcl.ExtCtrls, Vcl.Grids,
  uCommonTypes, uAdminBL;

type
  TfrmAssets = class(TForm)
    pnlTop: TPanel;
    lblTitle: TLabel;
    btnClose: TButton;
    grdAssets: TStringGrid;
    pnlBottom: TPanel;
    btnRefresh: TButton;
    lblTotalValue: TLabel;
    lblTotalDepreciated: TLabel;
    btnAdd: TButton;
    btnEdit: TButton;
    btnDelete: TButton;
    pnlEdit: TPanel;
    lblEdtName: TLabel;
    edtAssetName: TEdit;
    lblEdtCategory: TLabel;
    cmbAssetCategory: TComboBox;
    lblEdtDate: TLabel;
    edtPurchaseDate: TEdit;
    lblEdtValue: TLabel;
    edtValue: TEdit;
    lblEdtDepreciated: TLabel;
    edtDepreciated: TEdit;
    lblEdtStatus: TLabel;
    cmbAssetStatus: TComboBox;
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
    procedure LoadAssets;
  public
    { Public declarations }
  end;

var
  frmAssets: TfrmAssets;

implementation

{$R *.dfm}

procedure TfrmAssets.FormCreate(Sender: TObject);
begin
  FAdminBL := TAdminBL.Create;
  TGridHelper.ConfigureGrid(grdAssets,
    ['ID', 'Asset Name', 'Category', 'Purchase Date', 'Value', 'Depreciated', 'Status'],
    [40, 150, 100, 90, 80, 80, 80]);
  LoadAssets;
end;

procedure TfrmAssets.FormDestroy(Sender: TObject);
begin
  FAdminBL.Free;
end;

procedure TfrmAssets.btnCloseClick(Sender: TObject);
begin
  ModalResult := mrCancel;
end;

procedure TfrmAssets.btnRefreshClick(Sender: TObject);
begin
  LoadAssets;
end;

procedure TfrmAssets.btnAddClick(Sender: TObject);
begin
  FIsAdding := True;
  edtAssetName.Text := '';
  cmbAssetCategory.ItemIndex := -1;
  edtPurchaseDate.Text := '';
  edtValue.Text := '';
  edtDepreciated.Text := '';
  cmbAssetStatus.ItemIndex := -1;
  pnlEdit.Visible := True;
end;

procedure TfrmAssets.btnEditClick(Sender: TObject);
var
  Row: Integer;
begin
  Row := grdAssets.Row;
  if (Row < 1) or (grdAssets.Cells[0, Row] = '') then
    Exit;

  FIsAdding := False;
  FSelectedId := StrToIntDef(grdAssets.Cells[0, Row], 0);
  edtAssetName.Text := grdAssets.Cells[1, Row];
  cmbAssetCategory.Text := grdAssets.Cells[2, Row];
  edtPurchaseDate.Text := grdAssets.Cells[3, Row];
  edtValue.Text := StringReplace(StringReplace(grdAssets.Cells[4, Row], '$', '', [rfReplaceAll]), ',', '', [rfReplaceAll]);
  edtDepreciated.Text := StringReplace(StringReplace(grdAssets.Cells[5, Row], '$', '', [rfReplaceAll]), ',', '', [rfReplaceAll]);
  cmbAssetStatus.Text := grdAssets.Cells[6, Row];
  pnlEdit.Visible := True;
end;

procedure TfrmAssets.btnDeleteClick(Sender: TObject);
var
  Row: Integer;
  Id: Integer;
begin
  Row := grdAssets.Row;
  if (Row < 1) or (grdAssets.Cells[0, Row] = '') then
    Exit;

  Id := StrToIntDef(grdAssets.Cells[0, Row], 0);
  if MessageDlg('Are you sure you want to delete this asset?', mtConfirmation, [mbYes, mbNo], 0) = mrYes then
  begin
    FAdminBL.DeleteAsset(Id);
    LoadAssets;
  end;
end;

procedure TfrmAssets.btnSaveClick(Sender: TObject);
var
  Asset: TAssetInfo;
begin
  Asset.AssetName := edtAssetName.Text;
  Asset.Category := cmbAssetCategory.Text;
  Asset.PurchaseDate := StrToDateDef(edtPurchaseDate.Text, Now);
  Asset.Value := StrToCurrDef(StringReplace(edtValue.Text, '$', '', [rfReplaceAll]), 0);
  Asset.DepreciatedValue := StrToCurrDef(StringReplace(edtDepreciated.Text, '$', '', [rfReplaceAll]), 0);
  Asset.Status := cmbAssetStatus.Text;

  if FIsAdding then
    FAdminBL.AddAsset(Asset)
  else
  begin
    Asset.AssetId := FSelectedId;
    FAdminBL.UpdateAsset(Asset);
  end;

  pnlEdit.Visible := False;
  LoadAssets;
end;

procedure TfrmAssets.btnCancelClick(Sender: TObject);
begin
  pnlEdit.Visible := False;
end;

procedure TfrmAssets.LoadAssets;
var
  Assets: TArray<TAssetInfo>;
  I: Integer;
  TotalValue, TotalDepreciated: Currency;
begin
  Assets := FAdminBL.GetAssets;
  TGridHelper.ClearGrid(grdAssets);

  if Length(Assets) > 0 then
    grdAssets.RowCount := Length(Assets) + 1
  else
    grdAssets.RowCount := 2;

  TotalValue := 0;
  TotalDepreciated := 0;

  for I := 0 to High(Assets) do
  begin
    grdAssets.Cells[0, I + 1] := IntToStr(Assets[I].AssetId);
    grdAssets.Cells[1, I + 1] := Assets[I].AssetName;
    grdAssets.Cells[2, I + 1] := Assets[I].Category;
    grdAssets.Cells[3, I + 1] := DateToStr(Assets[I].PurchaseDate);
    grdAssets.Cells[4, I + 1] := CurrToStrF(Assets[I].Value, ffCurrency, 2);
    grdAssets.Cells[5, I + 1] := CurrToStrF(Assets[I].DepreciatedValue, ffCurrency, 2);
    grdAssets.Cells[6, I + 1] := Assets[I].Status;

    TotalValue := TotalValue + Assets[I].Value;
    TotalDepreciated := TotalDepreciated + Assets[I].DepreciatedValue;
  end;

  lblTotalValue.Caption := 'Total Value: ' + CurrToStrF(TotalValue, ffCurrency, 2);
  lblTotalDepreciated.Caption := 'Total Depreciated: ' + CurrToStrF(TotalDepreciated, ffCurrency, 2);
end;

end.
