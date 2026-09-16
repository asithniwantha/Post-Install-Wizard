# Post Install Wizard

A WPF application for managing and running post-installation software setups automatically or interactively. Built targeting .NET 8.0 Windows with Material Design styling and MVVM architecture.

---

## Table of Contents

- [Features](#features)
- [Architecture](#architecture)
- [Prerequisites](#prerequisites)
- [Building & Running](#building--running)
- [Configuration (`config.json`)](#configuration-configjson)
- [Project Structure](#project-structure)
- [License](#license)

---

## Features

- **Automated Countdown Timer**: Automatic installation triggers after a customizable delay unless paused/canceled by the user.
- **Batch Software Installation**: Run `.exe`, `.msi`, `.bat`, `.cmd`, or `.ps1` installers sequentially with custom command-line arguments.
- **Configuration Manager Window**: Graphical user interface to add, remove, edit, and browse installer paths and arguments.
- **Error Logging**: Detailed error logging written to `install_errors.log` if an installer fails or cannot be located.
- **Modern UI**: Styled with [MaterialDesignInXamlToolkit](https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit).

---

## Architecture

The project follows the standard **Model-View-ViewModel (MVVM)** pattern using the `CommunityToolkit.Mvvm` library.

- **Models**: `SoftwareItem` stores metadata for installer software items (Name, IsSelected, FileName, Arguments).
- **ViewModels**:
  - `MainViewModel`: Handles auto-start countdown timer, installer batch processing logic, selection toggles, and navigation to configuration dialogs.
  - `ConfigViewModel`: Manages application items, browsing for executable files, and persistence.
- **Views**:
  - `MainWindow.xaml`: Main interface displaying software list, selection controls, timer, and progress bar.
  - `ConfigWindow.xaml`: Configuration editor dialog for setting up application entries.
- **Services / Utilities**:
  - `ConfigManager.cs`: JSON serialization/deserialization for saving and loading `config.json`.

---

## Prerequisites

- **OS**: Windows 10 / 11
- **SDK**: .NET 8.0 SDK (with WPF support)
- **IDE / Build Tools**: Visual Studio 2022 / JetBrains Rider / .NET CLI

---

## Building & Running

### Using .NET CLI

Build the solution using the command line:

```bash
dotnet build "Post Install Wizard.slnx"
```

To run the application:

```bash
dotnet run --project "Post Install Wizard/Post Install Wizard.csproj"
```

> **Note for Non-Windows Environments / Containers**: The project file includes `<EnableWindowsTargeting>true</EnableWindowsTargeting>` to allow compilation and targeting of `.NET 8.0 Windows` on non-Windows development containers or CI build environments.

---

## Configuration (`config.json`)

The application loads software setup items from a `config.json` file located in the application base directory (`AppDomain.CurrentDomain.BaseDirectory`).

### Configuration Schema

`config.json` consists of a JSON array of software objects:

```json
[
  {
    "Name": "System Utilities Pack",
    "IsSelected": false,
    "FileName": "sysutil.msi",
    "Arguments": "/quiet"
  },
  {
    "Name": "Web Browser Suite",
    "IsSelected": true,
    "FileName": "Installers\\browser.exe",
    "Arguments": "/S"
  }
]
```

### Fields

| Field | Type | Description |
| --- | --- | --- |
| `Name` | `string` | Display name for the software item. |
| `IsSelected` | `boolean` | Initial selection state (whether it will be installed by default). |
| `FileName` | `string` | Path or file name of the installer. Can be an absolute path or relative to the app base directory or `Installers/` directory. |
| `Arguments` | `string` | Silent install or command-line arguments passed to the installer executable. |

---

## Project Structure

```text
Post Install Wizard/
│
├── Models/
│   └── SoftwareItem.cs         # Observable model for software entries
│
├── ViewModels/
│   ├── MainViewModel.cs        # Main window logic, timer, and installation execution
│   └── ConfigViewModel.cs      # Configuration dialog logic
│
├── App.xaml / App.xaml.cs      # Application entry point and WPF resource dictionary
├── ConfigManager.cs            # Loads and saves JSON configuration
├── ConfigWindow.xaml / .cs     # UI for configuration management
├── MainWindow.xaml / .cs       # Main UI window
└── Post Install Wizard.csproj  # WPF project configuration (.NET 8.0-windows)
```

---

## License

See [LICENSE.txt](../LICENSE.txt) for licensing details.
