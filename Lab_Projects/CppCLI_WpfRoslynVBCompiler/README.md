# CppCLI_WpfRoslynVBCompiler

A multi-language solution demonstrating **C++/CLI** interop with **WPF** and **runtime VB.NET code compilation** via **Roslyn**.

## Architecture

```
CppCLIWPFCodeDOM (C++/CLI)
├── Hosts a WPF window (XAML loaded via XamlReader)
├── DataModel with INotifyPropertyChanged bindings
└── UI for editing, compiling, and invoking VB.NET code

VBCompilationHelper (C#)
├── Wraps Roslyn's Microsoft.CodeAnalysis.VisualBasic API
├── Compiles VB.NET source code in-memory
└── Uses reflection to discover types, methods, and fields

VBCode (VB.NET)
└── Sample VB.NET class library used as compilation input

CppCLIWPFCodeDOMRun (C#)
└── Thin launcher — sets STA thread and calls C++/CLI entry point
```

## Features

- **Edit VB.NET code** in a text box within the WPF UI
- **Compile at runtime** using Roslyn (Microsoft.CodeAnalysis.VisualBasic)
- **Browse discovered types** — methods and fields extracted via reflection
- **Invoke methods** with parameters and view return values
- **Get/set field values** on the compiled object instance
- **C++/CLI ↔ WPF interop** — managed C++ hosting a WPF window with full data binding

## Projects

| Project                 | Description                                   |
| :---------------------- | :-------------------------------------------- |
| **CppCLIWPFCodeDOM**    | C++/CLI project hosting WPF UI and data model |
| **CppCLIWPFCodeDOMRun** | C# launcher (STA thread setup)                |
| **VBCompilationHelper** | C# wrapper around Roslyn VB.NET compiler      |
| **VBCode**              | Sample VB.NET class library                   |

## Tech Stack

| Technology             | Usage                                                    |
| :--------------------- | :------------------------------------------------------- |
| C++/CLI                | Managed C++ interop layer                                |
| WPF                    | Desktop UI (XAML loaded at runtime)                      |
| VB.NET                 | Sample source code for compilation                       |
| Roslyn                 | Runtime compilation (Microsoft.CodeAnalysis.VisualBasic) |
| Reflection             | Type/method/field discovery                              |
| INotifyPropertyChanged | MVVM data binding                                        |

## How to Run

Build the solution in Visual Studio (requires C++/CLI workload installed), then run `CppCLIWPFCodeDOMRun`.
