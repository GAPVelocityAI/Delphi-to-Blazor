object frmFoodCost: TfrmFoodCost
  Left = 0
  Top = 0
  Caption = 'Food Cost Analysis'
  ClientHeight = 610
  ClientWidth = 750
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -12
  Font.Name = 'Segoe UI'
  Font.Style = []
  Position = poMainFormCenter
  OnCreate = FormCreate
  OnDestroy = FormDestroy
  PixelsPerInch = 96
  TextHeight = 15
  object pnlTop: TPanel
    Left = 0
    Top = 0
    Width = 750
    Height = 45
    Align = alTop
    BevelOuter = bvNone
    TabOrder = 0
    object lblTitle: TLabel
      Left = 16
      Top = 12
      Width = 200
      Height = 20
      Caption = 'Food Cost Analysis'
      Font.Charset = DEFAULT_CHARSET
      Font.Color = clWindowText
      Font.Height = -15
      Font.Name = 'Segoe UI'
      Font.Style = [fsBold]
      ParentFont = False
    end
    object btnClose: TButton
      Left = 654
      Top = 8
      Width = 80
      Height = 30
      Caption = 'Close'
      TabOrder = 0
      OnClick = btnCloseClick
    end
  end
  object grdFoodCost: TStringGrid
    Left = 0
    Top = 45
    Width = 750
    Height = 405
    Align = alClient
    ColCount = 6
    DefaultDrawing = False
    DefaultRowHeight = 22
    FixedCols = 0
    Options = [goFixedVertLine, goFixedHorzLine, goVertLine, goHorzLine, goRowSelect, goColSizing]
    TabOrder = 1
    OnDrawCell = grdFoodCostDrawCell
  end
  object pnlBottom: TPanel
    Left = 0
    Top = 450
    Width = 750
    Height = 50
    Align = alBottom
    BevelOuter = bvNone
    TabOrder = 2
    object lblAvgCostPct: TLabel
      Left = 240
      Top = 18
      Width = 100
      Height = 15
      Caption = 'Avg Cost %: N/A'
    end
    object btnRefresh: TButton
      Left = 16
      Top = 10
      Width = 100
      Height = 30
      Caption = 'Refresh'
      TabOrder = 0
      OnClick = btnRefreshClick
    end
    object btnAdd: TButton
      Left = 380
      Top = 10
      Width = 80
      Height = 30
      Caption = 'Add'
      TabOrder = 1
      OnClick = btnAddClick
    end
    object btnEdit: TButton
      Left = 470
      Top = 10
      Width = 80
      Height = 30
      Caption = 'Edit'
      TabOrder = 2
      OnClick = btnEditClick
    end
    object btnDelete: TButton
      Left = 560
      Top = 10
      Width = 80
      Height = 30
      Caption = 'Delete'
      TabOrder = 3
      OnClick = btnDeleteClick
    end
  end
  object pnlEdit: TPanel
    Left = 0
    Top = 500
    Width = 750
    Height = 110
    Align = alBottom
    BevelOuter = bvNone
    Visible = False
    TabOrder = 3
    object lblEdtRecipeName: TLabel
      Left = 16
      Top = 8
      Width = 75
      Height = 15
      Caption = 'Recipe Name:'
    end
    object edtRecipeName: TEdit
      Left = 16
      Top = 26
      Width = 150
      Height = 23
      TabOrder = 0
    end
    object lblEdtIngredients: TLabel
      Left = 180
      Top = 8
      Width = 67
      Height = 15
      Caption = 'Ingredients:'
    end
    object edtIngredients: TEdit
      Left = 180
      Top = 26
      Width = 80
      Height = 23
      TabOrder = 1
    end
    object lblEdtTotalCost: TLabel
      Left = 274
      Top = 8
      Width = 58
      Height = 15
      Caption = 'Total Cost:'
    end
    object edtTotalCost: TEdit
      Left = 274
      Top = 26
      Width = 100
      Height = 23
      TabOrder = 2
    end
    object lblEdtSellingPrice: TLabel
      Left = 388
      Top = 8
      Width = 71
      Height = 15
      Caption = 'Selling Price:'
    end
    object edtSellingPrice: TEdit
      Left = 388
      Top = 26
      Width = 100
      Height = 23
      TabOrder = 3
    end
    object btnSave: TButton
      Left = 274
      Top = 66
      Width = 80
      Height = 30
      Caption = 'Save'
      TabOrder = 4
      OnClick = btnSaveClick
    end
    object btnCancel: TButton
      Left = 364
      Top = 66
      Width = 80
      Height = 30
      Caption = 'Cancel'
      TabOrder = 5
      OnClick = btnCancelClick
    end
  end
end
