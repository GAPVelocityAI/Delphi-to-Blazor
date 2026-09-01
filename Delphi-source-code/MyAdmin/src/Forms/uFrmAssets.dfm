object frmAssets: TfrmAssets
  Left = 0
  Top = 0
  Caption = 'Assets Management'
  ClientHeight = 500
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
      Caption = 'Assets Management'
      Font.Charset = DEFAULT_CHARSET
      Font.Color = clWindowText
      Font.Height = -15
      Font.Name = 'Segoe UI'
      Font.Style = [fsBold]
      ParentFont = False
    end
    object btnClose: TButton
      Left = 660
      Top = 8
      Width = 75
      Height = 30
      Caption = 'Close'
      TabOrder = 0
      OnClick = btnCloseClick
    end
  end
  object grdAssets: TStringGrid
    Left = 0
    Top = 45
    Width = 750
    Height = 405
    Align = alClient
    ColCount = 7
    DefaultRowHeight = 22
    FixedCols = 0
    RowCount = 2
    Options = [goFixedVertLine, goFixedHorzLine, goVertLine, goHorzLine, goRangeSelect, goRowSelect, goColSizing]
    TabOrder = 1
  end
  object pnlBottom: TPanel
    Left = 0
    Top = 450
    Width = 750
    Height = 50
    Align = alBottom
    BevelOuter = bvNone
    TabOrder = 2
    object lblTotalValue: TLabel
      Left = 120
      Top = 16
      Width = 80
      Height = 15
      Caption = 'Total Value: $0'
    end
    object lblTotalDepreciated: TLabel
      Left = 350
      Top = 16
      Width = 120
      Height = 15
      Caption = 'Total Depreciated: $0'
    end
    object btnRefresh: TButton
      Left = 16
      Top = 10
      Width = 85
      Height = 30
      Caption = 'Refresh'
      TabOrder = 0
      OnClick = btnRefreshClick
    end
    object btnAdd: TButton
      Left = 520
      Top = 10
      Width = 70
      Height = 30
      Caption = 'Add'
      TabOrder = 1
      OnClick = btnAddClick
    end
    object btnEdit: TButton
      Left = 596
      Top = 10
      Width = 70
      Height = 30
      Caption = 'Edit'
      TabOrder = 2
      OnClick = btnEditClick
    end
    object btnDelete: TButton
      Left = 672
      Top = 10
      Width = 70
      Height = 30
      Caption = 'Delete'
      TabOrder = 3
      OnClick = btnDeleteClick
    end
  end
  object pnlEdit: TPanel
    Left = 0
    Top = 390
    Width = 750
    Height = 110
    Align = alBottom
    BevelOuter = bvNone
    Visible = False
    TabOrder = 3
    object lblEdtName: TLabel
      Left = 16
      Top = 8
      Width = 65
      Height = 15
      Caption = 'Asset Name'
    end
    object edtAssetName: TEdit
      Left = 16
      Top = 26
      Width = 150
      Height = 23
      TabOrder = 0
    end
    object lblEdtCategory: TLabel
      Left = 180
      Top = 8
      Width = 51
      Height = 15
      Caption = 'Category'
    end
    object cmbAssetCategory: TComboBox
      Left = 180
      Top = 26
      Width = 130
      Height = 23
      Style = csDropDownList
      Items.Strings = (
        'Kitchen Equipment'
        'Technology'
        'Furniture'
        'Vehicle'
        'Bar')
      TabOrder = 1
    end
    object lblEdtDate: TLabel
      Left = 324
      Top = 8
      Width = 79
      Height = 15
      Caption = 'Purchase Date'
    end
    object edtPurchaseDate: TEdit
      Left = 324
      Top = 26
      Width = 100
      Height = 23
      TabOrder = 2
    end
    object lblEdtValue: TLabel
      Left = 16
      Top = 52
      Width = 29
      Height = 15
      Caption = 'Value'
    end
    object edtValue: TEdit
      Left = 16
      Top = 70
      Width = 120
      Height = 23
      TabOrder = 3
    end
    object lblEdtDepreciated: TLabel
      Left = 150
      Top = 52
      Width = 67
      Height = 15
      Caption = 'Depreciated'
    end
    object edtDepreciated: TEdit
      Left = 150
      Top = 70
      Width = 120
      Height = 23
      TabOrder = 4
    end
    object lblEdtStatus: TLabel
      Left = 284
      Top = 52
      Width = 34
      Height = 15
      Caption = 'Status'
    end
    object cmbAssetStatus: TComboBox
      Left = 284
      Top = 70
      Width = 120
      Height = 23
      Style = csDropDownList
      Items.Strings = (
        'Active'
        'Needs Repair'
        'Disposed')
      TabOrder = 5
    end
    object btnSave: TButton
      Left = 500
      Top = 70
      Width = 80
      Height = 30
      Caption = 'Save'
      TabOrder = 6
      OnClick = btnSaveClick
    end
    object btnCancel: TButton
      Left = 590
      Top = 70
      Width = 80
      Height = 30
      Caption = 'Cancel'
      TabOrder = 7
      OnClick = btnCancelClick
    end
  end
end
