object frmPayroll: TfrmPayroll
  Left = 0
  Top = 0
  Caption = 'Payroll'
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
      Caption = 'Payroll'
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
  object grdPayroll: TStringGrid
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
    object lblTotalGross: TLabel
      Left = 120
      Top = 16
      Width = 100
      Height = 15
      Caption = 'Total Gross: $0'
    end
    object lblTotalNet: TLabel
      Left = 330
      Top = 16
      Width = 100
      Height = 15
      Caption = 'Total Net: $0'
    end
    object lblTotalDeductions: TLabel
      Left = 530
      Top = 16
      Width = 100
      Height = 15
      Caption = 'Deductions: $0'
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
    object lblEdtEmployee: TLabel
      Left = 16
      Top = 8
      Width = 54
      Height = 15
      Caption = 'Employee'
    end
    object edtEmployee: TEdit
      Left = 16
      Top = 26
      Width = 150
      Height = 23
      TabOrder = 0
    end
    object lblEdtPeriod: TLabel
      Left = 180
      Top = 8
      Width = 35
      Height = 15
      Caption = 'Period'
    end
    object edtPeriod: TEdit
      Left = 180
      Top = 26
      Width = 100
      Height = 23
      TabOrder = 1
    end
    object lblEdtGrossPay: TLabel
      Left = 294
      Top = 8
      Width = 54
      Height = 15
      Caption = 'Gross Pay'
    end
    object edtGrossPay: TEdit
      Left = 294
      Top = 26
      Width = 120
      Height = 23
      TabOrder = 2
    end
    object lblEdtDeductions: TLabel
      Left = 16
      Top = 52
      Width = 61
      Height = 15
      Caption = 'Deductions'
    end
    object edtDeductions: TEdit
      Left = 16
      Top = 70
      Width = 120
      Height = 23
      TabOrder = 3
    end
    object lblEdtPayDate: TLabel
      Left = 150
      Top = 52
      Width = 47
      Height = 15
      Caption = 'Pay Date'
    end
    object edtPayDate: TEdit
      Left = 150
      Top = 70
      Width = 100
      Height = 23
      TabOrder = 4
    end
    object btnSave: TButton
      Left = 500
      Top = 70
      Width = 80
      Height = 30
      Caption = 'Save'
      TabOrder = 5
      OnClick = btnSaveClick
    end
    object btnCancel: TButton
      Left = 590
      Top = 70
      Width = 80
      Height = 30
      Caption = 'Cancel'
      TabOrder = 6
      OnClick = btnCancelClick
    end
  end
end
