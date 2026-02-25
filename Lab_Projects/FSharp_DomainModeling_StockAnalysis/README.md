# FSharp_DomainModeling_StockAnalysis

An F# library and scripting project exploring **functional domain modeling**, **stock market analysis**, and **F#/VB.NET interop**.

## Components

### Domain Modeling (F#)

| Module            | Description                                                                                              |
| :---------------- | :------------------------------------------------------------------------------------------------------- |
| **WrappedString** | Generic framework for validated, canonicalized string wrapper types with trimming and length constraints |
| **String50**      | Value type wrapping strings with max 50-character validation using `Option`                              |
| **EmailAddress**  | Validated email type built on `WrappedString` — regex format validation, success/failure continuations   |
| **ContactInfo**   | Discriminated union: `EmailOnly \| PostOnly \| EmailAndPost`                                             |

### Stock Analysis (F#)

| Module         | Description                                                                                                       |
| :------------- | :---------------------------------------------------------------------------------------------------------------- |
| **Library.fs** | `StockAnalyzer` class — downloads historical prices from Stooq.com, calculates log-returns and standard deviation |
| **Script.fsx** | Interactive scripts: generic `sumOfSquares`, parallel computation, stock price charting with XPlot.Plotly         |

### VB.NET Interop

| Module       | Description                                                                                                                 |
| :----------- | :-------------------------------------------------------------------------------------------------------------------------- |
| **VBModule** | VB.NET console app consuming the F# `StockAnalyzer` — runs both sequential and parallel (`GetAnalyzersParalAsync`) analysis |

## F# Interactive Scripts

| Script                    | Purpose                               |
| :------------------------ | :------------------------------------ |
| `Script.fsx`              | Parallel computation & stock charting |
| `ScriptEmailAddress.fsx`  | Email validation demo                 |
| `ScriptContactInfo.fsx`   | Contact info discriminated union demo |
| `ScriptString50.fsx`      | String50 validation demo              |
| `ScriptWrappedString.fsx` | WrappedString framework demo          |

## Tech Stack

| Technology                     | Usage                              |
| :----------------------------- | :--------------------------------- |
| F# / .NET 8                    | Core language and runtime          |
| VB.NET                         | Interop consumer                   |
| FSharp.Collections.ParallelSeq | Parallel sequence operations       |
| XPlot.Plotly                   | Interactive charting               |
| HttpClient                     | Stock data fetching from Stooq.com |
| Discriminated Unions           | Type-safe domain modeling          |

## How to Run

```bash
# Run the VB.NET interop app
cd VBModule
dotnet run

# Run F# interactive scripts
dotnet fsi MyProject/Script.fsx
```
