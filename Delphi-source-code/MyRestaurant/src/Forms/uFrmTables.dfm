object frmTables: TfrmTables
  Left = 0
  Top = 0
  Caption = 'Tables Management'
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
      Caption = 'Tables Management'
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
  object grdTables: TStringGrid
    Left = 0
    Top = 45
    Width = 750
    Height = 295
    Align = alClient
    ColCount = 5
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
    object btnRefresh: TButton
      Left = 16
      Top = 10
      Width = 100
      Height = 30
      Caption = 'Refresh'
      TabOrder = 0
      OnClick = btnRefreshClick
    end
    object btnFilterAvailable: TButton
      Left = 130
      Top = 10
      Width = 120
      Height = 30
      Caption = 'Show Available'
      TabOrder = 1
      OnClick = btnFilterAvailableClick
    end
    object btnShowAll: TButton
      Left = 264
      Top = 10
      Width = 100
      Height = 30
      Caption = 'Show All'
      TabOrder = 2
      OnClick = btnShowAllClick
    end
    object btnAdd: TButton
      Left = 450
      Top = 10
      Width = 80
      Height = 30
      Caption = 'Add'
      TabOrder = 3
      OnClick = btnAddClick
    end
    object btnEdit: TButton
      Left = 540
      Top = 10
      Width = 80
      Height = 30
      Caption = 'Edit'
      TabOrder = 4
      OnClick = btnEditClick
    end
    object btnDelete: TButton
      Left = 630
      Top = 10
      Width = 80
      Height = 30
      Caption = 'Delete'
      TabOrder = 5
      OnClick = btnDeleteClick
    end
  end
  object pnlEdit: TPanel
    Left = 0
    Top = 340
    Width = 750
    Height = 110
    Align = alBottom
    BevelOuter = bvNone
    TabOrder = 3
    Visible = False
    object lblEdtNumber: TLabel
      Left = 16
      Top = 8
      Width = 80
      Height = 15
      Caption = 'Table Number:'
    end
    object edtNumber: TEdit
      Left = 16
      Top = 26
      Width = 150
      Height = 23
      TabOrder = 0
    end
    object lblEdtCapacity: TLabel
      Left = 186
      Top = 8
      Width = 55
      Height = 15
      Caption = 'Capacity:'
    end
    object edtCapacity: TEdit
      Left = 186
      Top = 26
      Width = 100
      Height = 23
      TabOrder = 1
    end
    object lblEdtStatus: TLabel
      Left = 306
      Top = 8
      Width = 40
      Height = 15
      Caption = 'Status:'
    end
    object cmbEditStatus: TComboBox
      Left = 306
      Top = 26
      Width = 130
      Height = 23
      Style = csDropDownList
      Items.Strings = (
        'Available'
        'Occupied'
        'Reserved'
        'Closed')
      TabOrder = 2
    end
    object lblEdtZone: TLabel
      Left = 456
      Top = 8
      Width = 30
      Height = 15
      Caption = 'Zone:'
    end
    object cmbEditZone: TComboBox
      Left = 456
      Top = 26
      Width = 130
      Height = 23
      Style = csDropDownList
      Items.Strings = (
        'Main Hall'
        'Terrace'
        'Private'
        'Bar Area')
      TabOrder = 3
    end
    object btnSave: TButton
      Left = 456
      Top = 68
      Width = 100
      Height = 30
      Caption = 'Save'
      TabOrder = 4
      OnClick = btnSaveClick
    end
    object btnCancel: TButton
      Left = 570
      Top = 68
      Width = 100
      Height = 30
      Caption = 'Cancel'
      TabOrder = 5
      OnClick = btnCancelClick
    end
  end
end
