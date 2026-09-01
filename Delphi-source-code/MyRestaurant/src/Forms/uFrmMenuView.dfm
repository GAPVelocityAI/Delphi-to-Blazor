object frmMenuView: TfrmMenuView
  Left = 0
  Top = 0
  Caption = 'Menu'
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
      Width = 50
      Height = 20
      Caption = 'Menu'
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
  object grdMenu: TStringGrid
    Left = 0
    Top = 45
    Width = 750
    Height = 405
    Align = alClient
    ColCount = 6
    DefaultRowHeight = 22
    FixedCols = 0
    Options = [goFixedVertLine, goFixedHorzLine, goVertLine, goHorzLine, goRowSelect, goColSizing]
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
    object lblCategory: TLabel
      Left = 130
      Top = 16
      Width = 102
      Height = 15
      Caption = 'Filter by Category:'
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
    object cmbCategory: TComboBox
      Left = 240
      Top = 13
      Width = 150
      Height = 23
      Style = csDropDownList
      Items.Strings = (
        'All'
        'Appetizer'
        'Main Course'
        'Dessert'
        'Beverage')
      TabOrder = 1
      OnChange = cmbCategoryChange
    end
    object btnAdd: TButton
      Left = 420
      Top = 10
      Width = 80
      Height = 30
      Caption = 'Add'
      TabOrder = 2
      OnClick = btnAddClick
    end
    object btnEdit: TButton
      Left = 510
      Top = 10
      Width = 80
      Height = 30
      Caption = 'Edit'
      TabOrder = 3
      OnClick = btnEditClick
    end
    object btnDelete: TButton
      Left = 600
      Top = 10
      Width = 80
      Height = 30
      Caption = 'Delete'
      TabOrder = 4
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
    object lblEdtName: TLabel
      Left = 16
      Top = 8
      Width = 35
      Height = 15
      Caption = 'Name:'
    end
    object edtItemName: TEdit
      Left = 16
      Top = 26
      Width = 150
      Height = 23
      TabOrder = 0
    end
    object lblEdtCategory: TLabel
      Left = 180
      Top = 8
      Width = 55
      Height = 15
      Caption = 'Category:'
    end
    object cmbEditCategory: TComboBox
      Left = 180
      Top = 26
      Width = 130
      Height = 23
      Style = csDropDownList
      Items.Strings = (
        'Appetizer'
        'Main Course'
        'Dessert'
        'Beverage')
      TabOrder = 1
    end
    object lblEdtPrice: TLabel
      Left = 324
      Top = 8
      Width = 31
      Height = 15
      Caption = 'Price:'
    end
    object edtPrice: TEdit
      Left = 324
      Top = 26
      Width = 100
      Height = 23
      TabOrder = 2
    end
    object lblEdtCost: TLabel
      Left = 16
      Top = 52
      Width = 28
      Height = 15
      Caption = 'Cost:'
    end
    object edtCost: TEdit
      Left = 16
      Top = 70
      Width = 100
      Height = 23
      TabOrder = 3
    end
    object lblEdtActive: TLabel
      Left = 130
      Top = 52
      Width = 38
      Height = 15
      Caption = 'Active:'
    end
    object cmbEditActive: TComboBox
      Left = 130
      Top = 70
      Width = 80
      Height = 23
      Style = csDropDownList
      Items.Strings = (
        'Yes'
        'No')
      TabOrder = 4
    end
    object btnSave: TButton
      Left = 324
      Top = 66
      Width = 80
      Height = 30
      Caption = 'Save'
      TabOrder = 5
      OnClick = btnSaveClick
    end
    object btnCancel: TButton
      Left = 414
      Top = 66
      Width = 80
      Height = 30
      Caption = 'Cancel'
      TabOrder = 6
      OnClick = btnCancelClick
    end
  end
end
