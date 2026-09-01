object frmMain: TfrmMain
  Left = 0
  Top = 0
  Caption = 'DemoApp - Restaurant Management'
  ClientHeight = 600
  ClientWidth = 900
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -12
  Font.Name = 'Segoe UI'
  Font.Style = []
  Position = poScreenCenter
  OnCreate = FormCreate
  PixelsPerInch = 96
  TextHeight = 15
  object pnlHeader: TPanel
    Left = 0
    Top = 0
    Width = 900
    Height = 60
    Align = alTop
    BevelOuter = bvNone
    Color = clNavy
    ParentBackground = False
    TabOrder = 0
    object lblTitle: TLabel
      Left = 20
      Top = 14
      Width = 460
      Height = 32
      Caption = 'DemoApp - Restaurant Management System'
      Font.Charset = DEFAULT_CHARSET
      Font.Color = clWhite
      Font.Height = -24
      Font.Name = 'Segoe UI'
      Font.Style = [fsBold]
      ParentFont = False
    end
  end
  object pnlSidebar: TPanel
    Left = 0
    Top = 60
    Width = 220
    Height = 521
    Align = alLeft
    BevelOuter = bvNone
    Color = cl3DLight
    ParentBackground = False
    TabOrder = 1
    object pnlRestaurant: TPanel
      Left = 10
      Top = 10
      Width = 200
      Height = 268
      BevelOuter = bvNone
      Color = cl3DLight
      ParentBackground = False
      TabOrder = 0
      object lblRestaurant: TLabel
        Left = 10
        Top = 4
        Width = 75
        Height = 17
        Caption = 'Restaurant'
        Font.Charset = DEFAULT_CHARSET
        Font.Color = clNavy
        Font.Height = -13
        Font.Name = 'Segoe UI'
        Font.Style = [fsBold]
        ParentFont = False
      end
      object btnTables: TButton
        Left = 10
        Top = 28
        Width = 180
        Height = 36
        Caption = 'Tables'
        Font.Charset = DEFAULT_CHARSET
        Font.Color = clWindowText
        Font.Height = -13
        Font.Name = 'Segoe UI'
        Font.Style = []
        ParentFont = False
        TabOrder = 0
        OnClick = btnTablesClick
      end
      object btnOrders: TButton
        Left = 10
        Top = 70
        Width = 180
        Height = 36
        Caption = 'Orders'
        Font.Charset = DEFAULT_CHARSET
        Font.Color = clWindowText
        Font.Height = -13
        Font.Name = 'Segoe UI'
        Font.Style = []
        ParentFont = False
        TabOrder = 1
        OnClick = btnOrdersClick
      end
      object btnMenu: TButton
        Left = 10
        Top = 112
        Width = 180
        Height = 36
        Caption = 'Menu'
        Font.Charset = DEFAULT_CHARSET
        Font.Color = clWindowText
        Font.Height = -13
        Font.Name = 'Segoe UI'
        Font.Style = []
        ParentFont = False
        TabOrder = 2
        OnClick = btnMenuClick
      end
      object btnBillCheck: TButton
        Left = 10
        Top = 154
        Width = 180
        Height = 36
        Caption = 'Bill / Check'
        Font.Charset = DEFAULT_CHARSET
        Font.Color = clWindowText
        Font.Height = -13
        Font.Name = 'Segoe UI'
        Font.Style = []
        ParentFont = False
        TabOrder = 3
        OnClick = btnBillCheckClick
      end
      object btnFoodCost: TButton
        Left = 10
        Top = 196
        Width = 180
        Height = 36
        Caption = 'Food Cost'
        Font.Charset = DEFAULT_CHARSET
        Font.Color = clWindowText
        Font.Height = -13
        Font.Name = 'Segoe UI'
        Font.Style = []
        ParentFont = False
        TabOrder = 4
        OnClick = btnFoodCostClick
      end
    end
    object pnlAdmin: TPanel
      Left = 10
      Top = 288
      Width = 200
      Height = 200
      BevelOuter = bvNone
      Color = cl3DLight
      ParentBackground = False
      TabOrder = 1
      object lblAdmin: TLabel
        Left = 10
        Top = 4
        Width = 100
        Height = 17
        Caption = 'Administration'
        Font.Charset = DEFAULT_CHARSET
        Font.Color = clNavy
        Font.Height = -13
        Font.Name = 'Segoe UI'
        Font.Style = [fsBold]
        ParentFont = False
      end
      object btnAssets: TButton
        Left = 10
        Top = 28
        Width = 180
        Height = 36
        Caption = 'Assets'
        Font.Charset = DEFAULT_CHARSET
        Font.Color = clWindowText
        Font.Height = -13
        Font.Name = 'Segoe UI'
        Font.Style = []
        ParentFont = False
        TabOrder = 0
        OnClick = btnAssetsClick
      end
      object btnPersonnel: TButton
        Left = 10
        Top = 70
        Width = 180
        Height = 36
        Caption = 'Personnel'
        Font.Charset = DEFAULT_CHARSET
        Font.Color = clWindowText
        Font.Height = -13
        Font.Name = 'Segoe UI'
        Font.Style = []
        ParentFont = False
        TabOrder = 1
        OnClick = btnPersonnelClick
      end
      object btnPayroll: TButton
        Left = 10
        Top = 112
        Width = 180
        Height = 36
        Caption = 'Payroll'
        Font.Charset = DEFAULT_CHARSET
        Font.Color = clWindowText
        Font.Height = -13
        Font.Name = 'Segoe UI'
        Font.Style = []
        ParentFont = False
        TabOrder = 2
        OnClick = btnPayrollClick
      end
    end
  end
  object pnlContent: TPanel
    Left = 220
    Top = 60
    Width = 680
    Height = 521
    Align = alClient
    BevelOuter = bvNone
    TabOrder = 2
    object lblWelcome: TLabel
      Left = 200
      Top = 240
      Width = 280
      Height = 20
      Caption = 'Select a module from the sidebar'
      Font.Charset = DEFAULT_CHARSET
      Font.Color = clGray
      Font.Height = -15
      Font.Name = 'Segoe UI'
      Font.Style = []
      ParentFont = False
    end
  end
  object StatusBar1: TStatusBar
    Left = 0
    Top = 581
    Width = 900
    Height = 19
    Panels = <
      item
        Text = 'Ready'
        Width = 200
      end
      item
        Text = ''
        Width = 50
      end>
  end
end
