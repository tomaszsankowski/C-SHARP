# ExcelDirectoryReportGenerator

A console application that recursively scans a directory and generates a comprehensive Excel (`.xlsx`) report using the **EPPlus** library.

## Features

The generated workbook contains two worksheets:

### 1. "Struktura katalogu" (Directory Structure)

- Full recursive listing of all files and directories
- Columns: path, extension, size, attributes
- Hierarchical **outline/grouping** of rows mirroring the folder tree

### 2. "Statystyki" (Statistics)

- **Top 10 largest files** with sizes
- **Per-extension statistics**: file count and total size
- Two **3D pie charts**:
  - File count share by extension
  - File size share by extension

## Tech Stack

| Technology                         | Usage                     |
| :--------------------------------- | :------------------------ |
| C# / .NET 8                        | Core language and runtime |
| EPPlus                             | Excel file generation     |
| Microsoft.Extensions.Configuration | App configuration         |

## How to Run

```bash
dotnet run
```

The application will scan the configured directory and produce an `.xlsx` report file.
