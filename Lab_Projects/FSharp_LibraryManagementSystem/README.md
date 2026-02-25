# FSharp_LibraryManagementSystem

An F# console application implementing a **library management system** with rich domain modeling, XML data persistence, and live currency conversion.

## Features

Interactive console menu with the following operations:

1. **Display reader details** — shows personal data, contact info, and deposit converted from USD to PLN
2. **Add fines** — applies fines in PLN to a reader's account
3. **Compare fines vs. deposit** — cross-currency comparison using a live exchange rate
4. **Promote to Premium** — upgrades a reader's status (requires >5 loans and >365 days membership)

### Live Exchange Rate

Fetches the current **USD/PLN exchange rate** from the **Polish National Bank (NBP) API** for real-time currency conversion.

## Domain Model (F#)

| Concept             | Implementation                                                                        |
| :------------------ | :------------------------------------------------------------------------------------ |
| **Currency**        | Units of measure: `[<Measure>] type USD`, `[<Measure>] type PLN`                      |
| **Contact details** | Discriminated union: `Telefon \| Adres \| TelefonIAdres`                              |
| **Contact info**    | Record type with optional email                                                       |
| **Reader**          | Record with ID, personal data, deposit (USD), join date, mutable status & fines (PLN) |
| **Library**         | Class managing readers and loan history with query methods                            |

## Project Structure

| File               | Purpose                                                    |
| :----------------- | :--------------------------------------------------------- |
| `LibraryTypes.fs`  | Domain types, records, discriminated unions, Library class |
| `LibraryTypes.fsi` | Signature file — public API surface                        |
| `DataAccess.fs`    | XML data loading (readers & loans)                         |
| `Program.fs`       | Interactive console UI and NBP API integration             |
| `data/readers.xml` | Reader data                                                |
| `data/loans.xml`   | Loan history                                               |

## Tech Stack

| Technology                | Usage                                     |
| :------------------------ | :---------------------------------------- |
| F# / .NET 8               | Core language and runtime                 |
| Units of Measure          | Type-safe USD/PLN handling                |
| Discriminated Unions      | Contact info modeling                     |
| F# Signature Files (.fsi) | API encapsulation                         |
| System.Xml.Linq           | XML data access (XDocument / LINQ to XML) |
| HttpClient                | NBP exchange rate API                     |

## How to Run

```bash
cd LibraryApp
dotnet run
```
