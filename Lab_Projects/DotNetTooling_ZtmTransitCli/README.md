# DotNetTooling_ZtmTransitCli

A multi-project solution demonstrating .NET build tooling, CLI tool packaging, and C# language features.

## Projects

### Zad1 — Partial Classes & Reflection

A simple console app showcasing **partial classes** and **partial methods**. Uses reflection to read assembly version info at runtime.

### Zad8 — ZTM Gdańsk Transit CLI Tool

A command-line .NET tool (`ztm`) that queries the Gdańsk public transit (ZTM) REST API in real time. Given a bus/tram stop ID, it fetches:

- Route number & direction
- Estimated arrival time
- Delay information
- Stop name

Data source: `ckan2.multimediagdansk.pl`

### Ztm.Tests — Unit Tests

xUnit tests verifying JSON deserialization of the ZTM API responses.

### Build.Cake — CI Pipeline

A Cake (C# Make) build script defining a CI-like pipeline: **Clean → Restore → Build → Test → Publish**.

## Tech Stack

| Technology       | Usage                        |
| :--------------- | :--------------------------- |
| C# / .NET 8      | Core language and runtime    |
| HttpClient       | REST API communication       |
| System.Text.Json | JSON deserialization         |
| xUnit            | Unit testing                 |
| Cake             | Build automation             |
| Reflection       | Assembly metadata inspection |

## How to Run

```bash
# Build & run the transit CLI
cd Zad8
dotnet run -- <stop_id>

# Run tests
dotnet test

# Run Cake build pipeline
dotnet cake Build.Cake
```
