# WpfMefPluginDashboard

A **modular WPF dashboard application** using the **Managed Extensibility Framework (MEF)** for runtime plugin discovery and hot-reloading.

## Architecture

```
DashboardApp (Host)
├── Loads plugins via DirectoryCatalog from Plugins/ folder
├── FileSystemWatcher for hot-reload on DLL changes
└── Publishes DataSubmittedEvent to all widgets

Contracts (Shared interfaces)
├── IWidget — Name + View
├── IEventAggregator — Pub/Sub messaging
└── DataSubmittedEvent

TextWidget (Plugin)
└── Subscribes to events, displays character & word count stats

ChartsWidget (Plugin)
└── Subscribes to events, renders dynamic bar charts from numeric data
```

## Features

- **Plugin architecture** — widgets are discovered and loaded at runtime from a `Plugins/` directory
- **Hot-reload** — `FileSystemWatcher` detects new/removed plugin DLLs and refreshes the dashboard live
- **Event Aggregator pattern** — decoupled communication between host and plugins via `IEventAggregator`
- **Dynamic UI** — user enters text, clicks "Wyślij dane" (Send data), and all active widgets react

## Projects

| Project          | Description                             |
| :--------------- | :-------------------------------------- |
| **DashboardApp** | WPF host application (`net8.0-windows`) |
| **Contracts**    | Shared interfaces and event definitions |
| **TextWidget**   | Plugin: text statistics display         |
| **ChartsWidget** | Plugin: dynamic bar chart visualization |

## Tech Stack

| Technology        | Usage                                                    |
| :---------------- | :------------------------------------------------------- |
| C# / .NET 8       | Core language and runtime                                |
| WPF               | Desktop UI framework                                     |
| MEF               | Plugin composition (`System.ComponentModel.Composition`) |
| FileSystemWatcher | Live plugin hot-reload                                   |

## How to Run

```bash
cd DashboardApp
dotnet run
```

Build the widget projects and place their DLLs in the `Plugins/` folder for the dashboard to discover them.
