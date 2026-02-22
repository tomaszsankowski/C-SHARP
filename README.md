# C# Projects Archive

This repository serves as an archive for my small and/or old C# projects, including university laboratories and mini-projects. The projects have been organized into categories based on their application type.

## Repository Structure

### Console Apps

- **[DirectoryScanner](Console_Apps/DirectoryScanner)**: A console application that scans a directory, displays its contents, finds the oldest file, and serializes/deserializes a collection using `BinaryFormatter`.
- **[LinqToXmlCarQueries](Console_Apps/LinqToXmlCarQueries)**: A console application that uses LINQ to XML and XPath to query and serialize a collection of Cars and Engines.
- **[TcpCarTuningClientServer](Console_Apps/TcpCarTuningClientServer)**: A console application featuring a TCP Client and Server that communicate by sending a `Car` object serialized as JSON.

### WPF Apps

- **[WpfAsyncNewtonCalculator](WPF_Apps/WpfAsyncNewtonCalculator)**: A WPF application that calculates Newton's binomial coefficient asynchronously using Tasks and multithreading.
- **[WpfCarCollectionManager](WPF_Apps/WpfCarCollectionManager)**: A WPF application for managing a collection of Cars and Engines, featuring XML serialization, sorting, and searching functionalities.
- **[WpfDirectoryTreeView](WPF_Apps/WpfDirectoryTreeView)**: A WPF application that opens a directory, displays its structure in a TreeView, and allows creating or deleting files and folders.

### 🌐 Full-Stack App

- **[LoginWebApp](FullStack_LoginApp/LoginWebApp)**: An ASP.NET Core Web API providing authentication and user management.
- **[LoginWebAppClient](FullStack_LoginApp/LoginWebAppClient)**: An Angular client application that interacts with the `LoginWebApp` API.

## Setup and Execution

Each project contains its own `README.md` with specific instructions on how to run it. Generally, you can open the `.sln` files in Visual Studio or use the `dotnet run` command in the respective project directories.
