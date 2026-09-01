object frmOrders: TfrmOrders
  Left = 0
  Top = 0
  Caption = 'Orders Management'
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
      Caption = 'Orders Management'
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
  object grdOrders: TStringGrid
    Left = 0
    Top = 45
    Width = 750
    Height = 255
    Align = alClient
    ColCount = 5
    DefaultRowHeight = 22
    FixedCols = 0
    Options = [goFixedVertLine, goFixedHorzLine, goVertLine, goHorzLine, goRowSelect, goColSizing]
    TabOrder = 1
    OnSelectCell = grdOrdersSelectCell
  end
  object grdDetails: TStringGrid
    Left = 0
    Top = 300
    Width = 750
    Height = 150
    Align = alBottom
    ColCount = 5
    DefaultRowHeight = 22
    FixedCols = 0
    Options = [goFixedVertLine, goFixedHorzLine, goVertLine, goHorzLine, goRowSelect, goColSizing]
    TabOrder = 2
  end
  object pnlBottom: TPanel
    Left = 0
    Top = 450
    Width = 750
    Height = 50
    Align = alBottom
    BevelOuter = bvNone
    TabOrder = 3
    object btnRefresh: TButton
      Left = 16
      Top = 10
      Width = 100
      Height = 30
      Caption = 'Refresh'
      TabOrder = 0
      OnClick = btnRefreshClick
    end
    object btnViewDetails: TButton
      Left = 130
      Top = 10
      Width = 120
      Height = 30
      Caption = 'View Details'
      TabOrder = 1
      OnClick = btnViewDetailsClick
    end
    object btnAdd: TButton
      Left = 264
      Top = 10
      Width = 80
      Height = 30
      Caption = 'Add'
      TabOrder = 2
      OnClick = btnAddClick
    end
    object btnEdit: TButton
      Left = 354
      Top = 10
      Width = 80
      Height = 30
      Caption = 'Edit'
      TabOrder = 3
      OnClick = btnEditClick
    end
    object btnDelete: TButton
      Left = 444
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
    TabOrder = 4
    object lblEdtTableId: TLabel
      Left = 16
      Top = 8
      Width = 49
      Height = 15
      Caption = 'Table ID:'
    end
    object edtTableId: TEdit
      Left = 16
      Top = 26
      Width = 100
      Height = 23
      TabOrder = 0
    end
    object lblEdtStatus: TLabel
      Left = 130
      Top = 8
      Width = 38
      Height = 15
      Caption = 'Status:'
    end
    object cmbEditStatus: TComboBox
      Left = 130
      Top = 26
      Width = 130
      Height = 23
      Style = csDropDownList
      Items.Strings = (
        'Pending'
        'Preparing'
        'Served'
        'Paid'
        'Cancelled')
      TabOrder = 1
    end
    object lblEdtTotal: TLabel
      Left = 274
      Top = 8
      Width = 75
      Height = 15
      Caption = 'Total Amount:'
    end
    object edtTotalAmount: TEdit
      Left = 274
      Top = 26
      Width = 120
      Height = 23
      TabOrder = 2
    end
    object btnSave: TButton
      Left = 274
      Top = 66
      Width = 80
      Height = 30
      Caption = 'Save'
      TabOrder = 3
      OnClick = btnSaveClick
    end
    object btnCancel: TButton
      Left = 364
      Top = 66
      Width = 80
      Height = 30
      Caption = 'Cancel'
      TabOrder = 4
      OnClick = btnCancelClick
    end
  end
end
