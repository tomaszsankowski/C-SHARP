# AutofacDependencyInjectionDemo

A demonstration of **Inversion of Control (IoC)** and **Dependency Injection (DI)** patterns using the **Autofac** container.

## Concepts Demonstrated

### Injection Patterns

- **Constructor injection** (`Worker`)
- **Setter/property injection** (`Worker2`)
- **Field injection** (`Worker3`)

### Implementations

Three `ICalculator` implementations showcasing different behaviors:

- `CatCalc` — string concatenation
- `PlusCalc` — integer addition
- `StateCalc` — stateful calculator (appends & increments a counter)

### Lifetime Scopes

- `InstancePerLifetimeScope` — shared `UnitOfWork` within a scope
- `InstancePerMatchingLifetimeScope("transaction")` — `TransactionContext` shared within a tagged scope

### Configuration Modes

- **Programmatic** registration via Autofac API
- **Declarative** registration via JSON (`appsettings.json`) using `Autofac.Configuration.ConfigurationModule`

## Projects

| Project   | Description                                                                               |
| :-------- | :---------------------------------------------------------------------------------------- |
| **App**   | Console app with DI setup and execution                                                   |
| **Tests** | MSTest unit tests validating all injection modes, singleton behavior, and scope lifetimes |

## Tech Stack

| Technology                         | Usage                       |
| :--------------------------------- | :-------------------------- |
| C# / .NET 8                        | Core language and runtime   |
| Autofac                            | IoC container               |
| Autofac.Configuration              | JSON-based DI configuration |
| MSTest                             | Unit testing framework      |
| Microsoft.Extensions.Configuration | Configuration provider      |

## How to Run

```bash
# Run the app
cd App
dotnet run

# Run tests
dotnet test
```
