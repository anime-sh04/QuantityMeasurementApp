<p align="center">
  <img src="https://img.shields.io/badge/Status-Active-brightgreen?style=for-the-badge" />
  <img src="https://img.shields.io/badge/Focus-Learning-blue?style=for-the-badge" />
  <img src="https://img.shields.io/badge/Primary-C%23-purple?style=for-the-badge" />
  
  <br/>

  <img src="https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" />
  <img src="https://img.shields.io/badge/ASP.NET_Core-5C2D91?style=for-the-badge&logo=dotnet&logoColor=white" />
  <img src="https://img.shields.io/badge/ADO.NET-008080?style=for-the-badge&logo=database&logoColor=white" />
  <img src="https://img.shields.io/badge/SQL_Server-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white" />
  <img src="https://img.shields.io/badge/Redis-D82C20?style=for-the-badge&logo=redis&logoColor=white" />
  <img src="https://img.shields.io/badge/JWT-black?style=for-the-badge&logo=jsonwebtokens" />
</p>

# ⭐ QUANTITY MEASUREMENT APP

> [!IMPORTANT]
> 🧠 **This is not a showcase repo, this is a growth log.* 
> Everything here exists to document progress, mistakes, and improvement.

## Overview

A C# console application for handling length, weight, and volume measurements with support for equality checks, unit conversions, and arithmetic operations. Demonstrates OOP principles, DRY design, generics, and REST API integration.

---

## Features at a Glance

- Multi-unit length support: feet, inches, yards, centimeters
- Generic `Quantity<T>` class for length, volume, and weight
- Arithmetic operations: add, subtract, divide
- Temperature support with selective arithmetic (equality/conversion only)
- REST API via ASP.NET Core with Swagger UI
- JWT authentication, Redis caching, and password hashing
- SQL Server persistence with ADO.NET and Repository Pattern

---

## Use Cases

### UC1 – Feet Measurement Equality
Compares two feet values for equality. Handles `null`, non-numeric, and type validation.

### UC2 – Feet and Inches Equality
Adds inches measurement alongside feet. Equality checks are performed independently for each unit.

### UC3 – Generic Quantity Class *(DRY Principle)*
Introduces `QuantityLength` with a `LengthUnit` enum. Eliminates duplication from UC1 & UC2. Supports cross-unit equality (e.g., `1 foot == 12 inches`).

### UC4 – Extended Unit Support
Adds `Yards` and `Centimeters`. Existing equality logic scales automatically. Demonstrates enum extensibility.

### UC5 – Unit-to-Unit Conversion
Explicit conversion API between units.

```csharp
QuantityLength.Convert(1.0, FEET, INCHES); // → 12.0
```

### UC6 – Addition of Two Length Units
Adds two lengths; result expressed in the first operand's unit.

```csharp
// 1 FEET + 12 INCHES → 2 FEET
// 2.54 CM + 1 INCH  → ~5.08 CM
```

### UC7 – Addition with Target Unit
Extends UC6 with an optional target unit parameter.

```csharp
Add(1, FEET, 12, INCHES, FEET); // → 2 FEET
```

### UC8 – Subtraction of Two Length Units
Cross-unit subtraction with optional target unit, negative result support, and epsilon-based floating-point comparison.

### UC9 – Multi-Category Measurement
Introduces separate classes,`QuantityLength`, `QuantityVolume`, `QuantityWeight`,each with its own unit enum. Prevents cross-category operations.

### UC10 – Generic Quantity with Unit Interface
Refactors UC9 into a single `Quantity<T>` generic class implementing `IUnit`. Centralises conversion logic and follows Open/Closed Principle (OCP).

### UC11 – Generic Quantity with `IMeasurable`
Uses a shared `IMeasurable` interface for compile-time type safety across all categories.

### UC12 – Arithmetic on Quantities
Adds `Add`, `Subtract`, and `Divide` to the generic class. Division returns a dimensionless scalar (ratio). Supports cross-unit operands.

### UC13 – Centralised Arithmetic *(DRY Principle)*
Introduces a private helper with an `ArithmeticOperation` enum (`ADD`, `SUBTRACT`, `DIVIDE`) to eliminate duplicated validation and conversion logic.

### UC14 – Temperature with Selective Arithmetic
Adds `TemperatureUnit` (Celsius, Fahrenheit, Kelvin). Supports equality and conversion, but blocks arithmetic via `SupportsArithmetic` interface,throws `UnsupportedOperationException`.

### UC15 – Persistence via ADO.NET
Stores measurement operations (inputs, units, results, errors) in SQL Server using `SqlConnection` and `SqlCommand`. Enables operation history retrieval.

### UC16 – Repository Pattern
Refactors ADO.NET code with a Repository Pattern for separation of concerns. Improves testability and reduces data-access duplication.

### UC17 – ASP.NET Core REST API
Exposes RESTful endpoints via controllers:

| Endpoint | Description |
|---|---|
| `POST /compare` | Compare two quantities |
| `POST /convert` | Convert between units |
| `POST /calculate` | Perform arithmetic |
| `GET /history` | Retrieve operation history |

Uses DTOs, model validation, Swagger UI, and a `Controller → Service → Repository` layered architecture.

### UC18 – Auth, Security & Caching

- JWT authentication with Login/Signup
- Password hashing with salting
- Encryption/decryption for sensitive data
- Redis caching for operation history
- Role-based/token-based API authorization

---

## 🏗️ Architecture

```
Controller  →  Service  →  Repository  →  SQL Server
                                ↓
                           Redis Cache
```

---

## 🛠️ Tech Stack

| Layer | Technology |
|---|---|
| Language | C# / .NET |
| API | ASP.NET Core Web API |
| Database | SQL Server (SSMS) via ADO.NET |
| Cache | Redis |
| Auth | JWT + BCrypt password hashing |
| Docs | Swagger / OpenAPI |

---

## 📐 Design Principles

- **DRY** – Generic `Quantity<T>` eliminates per-category duplication
- **OCP** – Adding a new measurement category requires minimal changes
- **Encapsulation** – Immutable quantity objects throughout
- **Type Safety** – Compile-time prevention of cross-category operations