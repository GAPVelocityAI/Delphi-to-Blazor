unit uMainForm;

interface

uses
  Winapi.Windows, Winapi.Messages, System.SysUtils, System.Variants,
  System.Classes, Vcl.Graphics, Vcl.Controls, Vcl.Forms, Vcl.Dialogs,
  Vcl.StdCtrls, Vcl.ExtCtrls, Vcl.ComCtrls;

type
  TfrmMain = class(TForm)
    pnlHeader: TPanel;
    lblTitle: TLabel;
    pnlSidebar: TPanel;
    pnlRestaurant: TPanel;
    lblRestaurant: TLabel;
    btnTables: TButton;
    btnOrders: TButton;
    btnMenu: TButton;
    btnBillCheck: TButton;
    btnFoodCost: TButton;
    pnlAdmin: TPanel;
    lblAdmin: TLabel;
    btnAssets: TButton;
    btnPersonnel: TButton;
    btnPayroll: TButton;
    pnlContent: TPanel;
    lblWelcome: TLabel;
    StatusBar1: TStatusBar;
    procedure FormCreate(Sender: TObject);
    procedure btnTablesClick(Sender: TObject);
    procedure btnOrdersClick(Sender: TObject);
    procedure btnMenuClick(Sender: TObject);
    procedure btnBillCheckClick(Sender: TObject);
    procedure btnFoodCostClick(Sender: TObject);
    procedure btnAssetsClick(Sender: TObject);
    procedure btnPersonnelClick(Sender: TObject);
    procedure btnPayrollClick(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  frmMain: TfrmMain;

implementation

{$R *.dfm}

uses
  uFrmTables, uFrmOrders, uFrmMenuView, uFrmBillCheck, uFrmFoodCost,
  uFrmAssets, uFrmPersonnel, uFrmPayroll;

procedure TfrmMain.FormCreate(Sender: TObject);
begin
  StatusBar1.Panels[0].Text := 'Ready';
  StatusBar1.Panels[1].Text := FormatDateTime('dddd, mmmm d, yyyy', Now);
end;

procedure TfrmMain.btnTablesClick(Sender: TObject);
var
  F: TfrmTables;
begin
  F := TfrmTables.Create(Self);
  try
    F.ShowModal;
  finally
    F.Free;
  end;
end;

procedure TfrmMain.btnOrdersClick(Sender: TObject);
var
  F: TfrmOrders;
begin
  F := TfrmOrders.Create(Self);
  try
    F.ShowModal;
  finally
    F.Free;
  end;
end;

procedure TfrmMain.btnMenuClick(Sender: TObject);
var
  F: TfrmMenuView;
begin
  F := TfrmMenuView.Create(Self);
  try
    F.ShowModal;
  finally
    F.Free;
  end;
end;

procedure TfrmMain.btnBillCheckClick(Sender: TObject);
var
  F: TfrmBillCheck;
begin
  F := TfrmBillCheck.Create(Self);
  try
    F.ShowModal;
  finally
    F.Free;
  end;
end;

procedure TfrmMain.btnFoodCostClick(Sender: TObject);
var
  F: TfrmFoodCost;
begin
  F := TfrmFoodCost.Create(Self);
  try
    F.ShowModal;
  finally
    F.Free;
  end;
end;

procedure TfrmMain.btnAssetsClick(Sender: TObject);
var
  F: TfrmAssets;
begin
  F := TfrmAssets.Create(Self);
  try
    F.ShowModal;
  finally
    F.Free;
  end;
end;

procedure TfrmMain.btnPersonnelClick(Sender: TObject);
var
  F: TfrmPersonnel;
begin
  F := TfrmPersonnel.Create(Self);
  try
    F.ShowModal;
  finally
    F.Free;
  end;
end;

procedure TfrmMain.btnPayrollClick(Sender: TObject);
var
  F: TfrmPayroll;
begin
  F := TfrmPayroll.Create(Self);
  try
    F.ShowModal;
  finally
    F.Free;
  end;
end;

end.
