object frmBillCheck: TfrmBillCheck
  Left = 0
  Top = 0
  Caption = 'Bills & Checks'
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
      Caption = 'Bills & Checks'
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
  object grdBills: TStringGrid
    Left = 0
    Top = 45
    Width = 750
    Height = 405
    Align = alClient
    ColCount = 8
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
    object pnlSummary: TPanel
      Left = 350
      Top = 4
      Width = 390
      Height = 42
      BevelOuter = bvNone
      TabOrder = 1
      object lblTotalBills: TLabel
        Left = 8
        Top = 4
        Width = 70
        Height = 15
        Caption = 'Total Bills: 0'
      end
      object lblTotalRevenue: TLabel
        Left = 8
        Top = 22
        Width = 120
        Height = 15
        Caption = 'Total Revenue: $0.00'
      end
    end
    object btnAdd: TButton
      Left = 130
      Top = 10
      Width = 80
      Height = 30
      Caption = 'Add'
      TabOrder = 2
      OnClick = btnAddClick
    end
    object btnEdit: TButton
      Left = 220
      Top = 10
      Width = 80
      Height = 30
      Caption = 'Edit'
      TabOrder = 3
      OnClick = btnEditClick
    end
    object btnDelete: TButton
      Left = 310
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
    object lblEdtOrderId: TLabel
      Left = 16
      Top = 8
      Width = 52
      Height = 15
      Caption = 'Order ID:'
    end
    object edtOrderId: TEdit
      Left = 16
      Top = 26
      Width = 100
      Height = 23
      TabOrder = 0
    end
    object lblEdtSubtotal: TLabel
      Left = 130
      Top = 8
      Width = 50
      Height = 15
      Caption = 'Subtotal:'
    end
    object edtSubtotal: TEdit
      Left = 130
      Top = 26
      Width = 120
      Height = 23
      TabOrder = 1
    end
    object lblEdtTip: TLabel
      Left = 264
      Top = 8
      Width = 21
      Height = 15
      Caption = 'Tip:'
    end
    object edtTip: TEdit
      Left = 264
      Top = 26
      Width = 100
      Height = 23
      TabOrder = 2
    end
    object lblEdtPayment: TLabel
      Left = 16
      Top = 52
      Width = 52
      Height = 15
      Caption = 'Payment:'
    end
    object cmbEditPayment: TComboBox
      Left = 16
      Top = 70
      Width = 130
      Height = 23
      Style = csDropDownList
      Items.Strings = (
        'Cash'
        'Credit Card'
        'Debit Card')
      TabOrder = 3
    end
    object btnSave: TButton
      Left = 264
      Top = 66
      Width = 80
      Height = 30
      Caption = 'Save'
      TabOrder = 4
      OnClick = btnSaveClick
    end
    object btnCancel: TButton
      Left = 354
      Top = 66
      Width = 80
      Height = 30
      Caption = 'Cancel'
      TabOrder = 5
      OnClick = btnCancelClick
    end
  end
end
