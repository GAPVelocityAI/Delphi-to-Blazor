program DemoApp;

uses
  Vcl.Forms,
  uMainForm in 'src\uMainForm.pas' {frmMain},
  uCommonTypes in '..\CommonLib\src\uCommonTypes.pas',
  uRestaurantBL in '..\MyRestaurant\src\BL\uRestaurantBL.pas',
  uTablesBL in '..\MyRestaurant\src\BL\uTablesBL.pas',
  uAdminBL in '..\MyAdmin\src\BL\uAdminBL.pas',
  uFrmTables in '..\MyRestaurant\src\Forms\uFrmTables.pas' {frmTables},
  uFrmOrders in '..\MyRestaurant\src\Forms\uFrmOrders.pas' {frmOrders},
  uFrmMenuView in '..\MyRestaurant\src\Forms\uFrmMenuView.pas' {frmMenuView},
  uFrmBillCheck in '..\MyRestaurant\src\Forms\uFrmBillCheck.pas' {frmBillCheck},
  uFrmFoodCost in '..\MyRestaurant\src\Forms\uFrmFoodCost.pas' {frmFoodCost},
  uFrmAssets in '..\MyAdmin\src\Forms\uFrmAssets.pas' {frmAssets},
  uFrmPersonnel in '..\MyAdmin\src\Forms\uFrmPersonnel.pas' {frmPersonnel},
  uFrmPayroll in '..\MyAdmin\src\Forms\uFrmPayroll.pas' {frmPayroll};

{$R *.res}

begin
  Application.Initialize;
  Application.MainFormOnTaskbar := True;
  Application.Title := 'DemoApp - Restaurant Management';
  Application.CreateForm(TfrmMain, frmMain);
  Application.Run;
end.
