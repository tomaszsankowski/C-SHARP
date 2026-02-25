# C# Projects Archive

This repository serves as an archive for my small and/or old C# projects, including university laboratories and mini-projects. The projects cover various application types, from console utilities to WPF desktop apps and full-stack web solutions.

## Projects Overview

| Project                                                                 | Description                                                                                       |
| :---------------------------------------------------------------------- | :------------------------------------------------------------------------------------------------ |
| [**DirectoryScanner**](./Console_Apps/DirectoryScanner)                 | Scans directory structures, finds oldest files, and demonstrates `BinaryFormatter` serialization. |
| [**LinqToXmlCarQueries**](./Console_Apps/LinqToXmlCarQueries)           | Advanced XML querying using LINQ to XML and XPath for object collections.                         |
| [**TcpCarTuningClientServer**](./Console_Apps/TcpCarTuningClientServer) | Network communication featuring a TCP Client/Server with JSON object exchange.                    |
| [**WpfAsyncNewtonCalculator**](./WPF_Apps/WpfAsyncNewtonCalculator)     | Asynchronous WPF application for Newton's binomial calculations using Tasks.                      |
| [**WpfCarCollectionManager**](./WPF_Apps/WpfCarCollectionManager)       | Desktop manager for car data featuring sorting, searching, and XML serialization.                 |
| [**WpfDirectoryTreeView**](./WPF_Apps/WpfDirectoryTreeView)             | Visual directory browser with tree structure and file system manipulation capabilities.           |
| [**LoginWebApp**](./FullStack_LoginApp/LoginWebApp)                     | ASP.NET Core Web API providing user authentication and management.                                |
| [**LoginWebAppClient**](./FullStack_LoginApp/LoginWebAppClient)         | Angular-based frontend client for the LoginWebApp API.                                            |

## .NET Lab Projects

University laboratory projects covering .NET platform technologies — C#, F#, VB.NET, C++/CLI, WPF, MEF, Roslyn, and more.

| Project                                                                                       | Description                                                                                         |
| :-------------------------------------------------------------------------------------------- | :-------------------------------------------------------------------------------------------------- |
| [**DotNetTooling_ZtmTransitCli**](./Lab_Projects/DotNetTooling_ZtmTransitCli)                 | .NET CLI tool for Gdańsk public transit (ZTM) + Cake build pipeline + partial classes & reflection. |
| [**ExcelDirectoryReportGenerator**](./Lab_Projects/ExcelDirectoryReportGenerator)             | Recursive directory scanner generating Excel reports with charts using EPPlus.                      |
| [**AutofacDependencyInjectionDemo**](./Lab_Projects/AutofacDependencyInjectionDemo)           | IoC/DI patterns with Autofac — constructor, setter & field injection, lifetime scopes, JSON config. |
| [**WpfMefPluginDashboard**](./Lab_Projects/WpfMefPluginDashboard)                             | Modular WPF dashboard with MEF plugin discovery, hot-reload, and event aggregator pattern.          |
| [**CppCLI_WpfRoslynVBCompiler**](./Lab_Projects/CppCLI_WpfRoslynVBCompiler)                   | C++/CLI hosting WPF with runtime VB.NET compilation via Roslyn and reflection-based invocation.     |
| [**FSharp_DomainModeling_StockAnalysis**](./Lab_Projects/FSharp_DomainModeling_StockAnalysis) | F# domain modeling (validated types, discriminated unions), stock analysis, and VB.NET interop.     |
| [**FSharp_LibraryManagementSystem**](./Lab_Projects/FSharp_LibraryManagementSystem)           | F# library system with units of measure (USD/PLN), XML data, and live NBP exchange rates.           |

## Setup and Execution

- **IDE**: Visual Studio 2022 or JetBrains Rider.
- **Framework**: .NET 8.0 / .NET 6.0 / .NET Framework 4.7.2.
- **Tools**: Node.js and Angular CLI (for the web client). C++/CLI workload (for CppCLI project).

Generally, you can open the `.sln` files in Visual Studio or use the `dotnet run` command in the respective project directories.

---

_Note: Each directory contains its own dedicated README.md file with a more detailed description of the specific project._
