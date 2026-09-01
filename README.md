# Delphi to Blazor Modernized

This repository contains a modernized Blazor version of the original Delphi application. The project keeps the same business domains and workflows, but reimplements the UI and services using ASP.NET Core, Blazor, MudBlazor, and modular .NET projects.

> The folder `Delphi-source-code` contains the original Delphi code of the application. It is the source of the legacy VCL implementation and serves as the reference for the modernization work.

## Project overview

The solution includes a shared library plus multiple domain modules and a host application:

- `CommonLib` – shared infrastructure and middleware
- `MyAdmin` – admin domain
- `MyRestaurant` – restaurant domain
- `MyFinanceAssets.Core`, `MyFinancePayroll.Core`, `MyFinancePersonnel.Core` – finance domains
- `MyInventoryFoodCost.Core`, `MyInventoryMenu.Core`, `MyInventoryStock.Core` – inventory domains
- `ProvidersCore.Core`, `ProvidersSupplies.Core` – provider domains
- `DemoApp` – Blazor host application

## Prerequisites

- .NET SDK 10.0 or later
- A modern Windows, Linux, or macOS environment
- Visual Studio 2022 or VS Code with the C# extension (optional, but convenient)

## Build

From the repository root:

```bash
dotnet restore

dotnet build
```

This will restore NuGet packages and compile the full solution.

## Run the app

Run the host application:

```bash
dotnet run --project DemoApp/DemoApp.csproj
```

Or run from the `DemoApp` folder:

```bash
cd DemoApp
dotnet run
```

The application will start the Blazor server and typically listen on a local ASP.NET Core URL such as:

- http://localhost:60440
- https://localhost:60439

## Development notes

- The app is configured as a Blazor Server project.
- Logging is enabled with Serilog.
- Each domain module registers its own services during startup.
- The project uses centralized package management through `Directory.Packages.props`.

## Solution files

- `CommonLib.sln` – main solution file
- `Directory.Build.props` – build settings for the solution
- `Directory.Packages.props` – centralized NuGet versions
- `DemoApp/` – main UI host
- `Delphi-source-code/` – legacy original Delphi implementation

## Repository purpose

This repository demonstrates a modernization from the original Delphi/VCL desktop application into a modern web-based architecture using Blazor, while preserving feature intent and domain structure.
