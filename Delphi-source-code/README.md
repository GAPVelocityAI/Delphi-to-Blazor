# DemoApp-Delphi

A restaurant management demo application built with **Delphi RAD Studio 13** (ProjectVersion 20.4) using the **VCL Framework**. The solution contains 12 projects organized by domain, featuring in-memory CRUD operations, standard VCL controls, and a container-form architecture.

## Architecture

The application uses a **container form pattern** where the main form (`frmMain`) acts as a navigation hub. Each module opens as a **modal dialog** from sidebar buttons. Data is managed entirely **in-memory** using class-level storage in Business Logic (BL) classes — no database or external APIs required.

```
DemoApp.exe (Main Container)
  |
  |-- MyRestaurant (DLL)
  |     |-- Tables Management      [CRUD]
  |     |-- Orders Management      [CRUD + Master-Detail]
  |     |-- Menu                   [CRUD + Category Filter]
  |     |-- Bills & Checks         [CRUD + Summary]
  |     |-- Food Cost Analysis     [CRUD + Color-Coded Grid]
  |
  |-- MyAdmin (DLL)
  |     |-- Assets Management      [CRUD + Value Totals]
  |     |-- Personnel Management   [CRUD + Active Filter]
  |     |-- Payroll                [CRUD + Summary Totals]
  |
  |-- CommonLib (DLL)           -- Shared types, enums, grid helper
  |-- MyFinance (3 DLLs)       -- Finance BL classes (Assets, Personnel, Payroll)
  |-- MyInventory (3 DLLs)     -- Inventory BL classes (Stock, FoodCost, Menu)
  |-- Providers (2 DLLs)       -- Provider/Supply BL classes
```

## Project Structure

```
DemoApp-Delphi/
  DemoApp.groupproj              -- Solution file (all 12 projects)
  |
  CommonLib/
    CommonLib.dpr / .dproj
    src/
      uCommonTypes.pas           -- 13 record types, 3 enums, TGridHelper
  |
  DemoApp/
    DemoApp.dpr / .dproj         -- Main EXE project
    src/
      uMainForm.pas / .dfm      -- Container form with navigation sidebar
  |
  MyRestaurant/
    MyRestaurant.dpr / .dproj
    src/
      BL/
        uTablesBL.pas            -- Tables CRUD (15 tables, 4 zones)
        uRestaurantBL.pas        -- Menu/Orders/Bills/FoodCost CRUD
      Forms/
        uFrmTables.pas / .dfm    -- Tables grid + edit panel
        uFrmOrders.pas / .dfm    -- Orders master-detail + edit panel
        uFrmMenuView.pas / .dfm  -- Menu grid + category filter + edit panel
        uFrmBillCheck.pas / .dfm -- Bills grid + summary + edit panel
        uFrmFoodCost.pas / .dfm  -- Food cost + color-coded grid + edit panel
  |
  MyAdmin/
    MyAdmin.dpr / .dproj
    src/
      BL/
        uAdminBL.pas             -- Employees/Assets/Payroll CRUD
      Forms/
        uFrmAssets.pas / .dfm    -- Assets grid + value totals + edit panel
        uFrmPersonnel.pas / .dfm -- Personnel grid + active filter + edit panel
        uFrmPayroll.pas / .dfm   -- Payroll grid + summary totals + edit panel
  |
  MyFinance/
    MyFinanceAssets/    src/uFinanceAssetsBL.pas
    MyFinancePersonnel/ src/uFinancePersonnelBL.pas
    MyFinancePayroll/   src/uFinancePayrollBL.pas
  |
  MyInventory/
    MyInventoryStock/    src/uInventoryStockBL.pas
    MyInventoryFoodCost/ src/uInventoryFoodCostBL.pas
    MyInventoryMenu/     src/uInventoryMenuBL.pas
  |
  Providers/
    ProvidersCore/    src/uProvidersBL.pas
    ProvidersSupplies/ src/uProviderSuppliesBL.pas
```

## Projects (12)

| # | Project | Type | Description |
|---|---------|------|-------------|
| 1 | **DemoApp** | EXE | Main application with container form and navigation |
| 2 | **CommonLib** | DLL | Shared record types, enums, and TGridHelper utility |
| 3 | **MyRestaurant** | DLL | Restaurant operations: tables, orders, menu, bills, food cost |
| 4 | **MyAdmin** | DLL | Administration: assets, personnel, payroll |
| 5 | **MyFinanceAssets** | DLL | Finance BL for asset depreciation and valuation |
| 6 | **MyFinancePersonnel** | DLL | Finance BL for labor costs and headcount |
| 7 | **MyFinancePayroll** | DLL | Finance BL for payroll summaries and tax withholdings |
| 8 | **MyInventoryStock** | DLL | Inventory BL for stock levels and reorder logic |
| 9 | **MyInventoryFoodCost** | DLL | Inventory BL for recipe costs and trends |
| 10 | **MyInventoryMenu** | DLL | Inventory BL for menu cost margins |
| 11 | **ProvidersCore** | DLL | Provider management and performance tracking |
| 12 | **ProvidersSupplies** | DLL | Supply chain and purchase order tracking |

## Forms (9)

| Form | Module | Features |
|------|--------|----------|
| **frmMain** | DemoApp | Navy header, sidebar with 8 navigation buttons, status bar |
| **frmTables** | MyRestaurant | Grid with filter (Available/All), CRUD with zone/status combos |
| **frmOrders** | MyRestaurant | Master-detail split grid, CRUD with status tracking |
| **frmMenuView** | MyRestaurant | Category filter combo, CRUD with price/cost fields |
| **frmBillCheck** | MyRestaurant | 8-column grid, revenue summary, CRUD with auto-tax (8%) |
| **frmFoodCost** | MyRestaurant | Color-coded Cost% column (red >35%, yellow 25-35%, green <25%), CRUD |
| **frmAssets** | MyAdmin | Value/depreciation totals, CRUD with category/status combos |
| **frmPersonnel** | MyAdmin | Active filter, CRUD with position combo (9 roles) |
| **frmPayroll** | MyAdmin | Gross/Net/Deductions totals, CRUD with auto-NetPay calculation |

## CRUD Operations

All 8 module forms support full **Create, Read, Update, Delete** operations performed entirely in-memory:

- **Add** — Opens an inline edit panel with empty fields. Save assigns an auto-incremented ID and appends the record.
- **Edit** — Populates the edit panel from the selected grid row. Save updates the record in-place.
- **Delete** — Confirmation dialog, then removes the record from the in-memory array.
- **Persistence** — BL classes use `class var` storage, so data survives across form open/close cycles within the same application session.

### Auto-Calculations

| Entity | Auto-Calculated Fields |
|--------|----------------------|
| Bills | Tax = Subtotal x 8%, Total = Subtotal + Tax + Tip |
| Food Cost | CostPercentage = (TotalCost / SellingPrice) x 100 |
| Payroll | NetPay = GrossPay - Deductions |

## Shared Types (uCommonTypes.pas)

### Record Types (13)
`TTableInfo`, `TMenuItemInfo`, `TOrderInfo`, `TOrderDetailInfo`, `TBillInfo`, `TEmployeeInfo`, `TAssetInfo`, `TPayrollInfo`, `TInventoryItem`, `TFoodCostInfo`, `TMenuCostInfo`, `TProviderInfo`, `TSupplyInfo`

### Enums (3)
- `TTableStatus` — Available, Occupied, Reserved, Closed
- `TOrderStatus` — Pending, Preparing, Served, Paid, Cancelled
- `TPaymentMethod` — Cash, Credit Card, Debit Card

### Utilities
- `TGridHelper` — `ConfigureGrid` (sets columns, widths, options) and `ClearGrid`
- Record helpers with `ToString` for all three enums

## Synthetic Data

Each BL class initializes with realistic sample data on first access:

| Entity | Records | Highlights |
|--------|--------:|------------|
| Tables | 15 | 4 zones: Main Hall, Terrace, Private, Bar Area |
| Menu Items | 12 | 4 categories: Appetizer, Main Course, Dessert, Beverage |
| Orders | 8 | Various statuses, linked to tables |
| Order Details | 23 | Linked to orders and menu items |
| Bills | 6 | 8% tax rate, multiple payment methods |
| Food Costs | 10 | Cost percentages ranging from 26% to 39% |
| Employees | 10 | 9 positions, 1 inactive |
| Assets | 8 | 5 categories, 1 needs repair |
| Payroll | 10 | Monthly period, 20% deduction rate |

## Controls Used

Standard VCL only — no third-party components:

`TStringGrid`, `TButton`, `TLabel`, `TPanel`, `TEdit`, `TComboBox`, `TRadioButton`, `TStatusBar`

## Code Statistics

| Metric | Value |
|--------|------:|
| Projects | 12 |
| Source files (.pas) | 21 |
| Form files (.dfm) | 9 |
| Program files (.dpr) | 12 |
| Total source lines | ~7,100 |
| Max line budget | 15,000 |

## Requirements

- **RAD Studio 13** (Delphi, ProjectVersion 20.4)
- **Windows** (Win32 target)
- No external dependencies, no database, no network access

## How to Build

1. Open `DemoApp.groupproj` in RAD Studio 13
2. Build All (Ctrl+Shift+F9)
3. Run `DemoApp.exe`

## Design Rules

- Maximum 15,000 lines total across all source files
- No external APIs or TCP connections
- No login/authentication
- Container form with modal sub-forms
- Standard VCL controls only
- BL classes contain SQL query constants (reference only, not executed)
- All data is hardcoded/synthetic and managed in-memory
