# Propissimo

[![Backend CI](https://github.com/sirbu99/propissimo-app/actions/workflows/dotnet.yml/badge.svg)](https://github.com/sirbu99/propissimo-app/actions/workflows/dotnet.yml)

A web application for independent landlords. Propissimo brings properties, rental contracts, renters, rent tracking, documents, utility accounts and reminders together in one place, so a landlord no longer has to manage them across spreadsheets, folders and chat threads.

> **Status:** early development. The domain layer is being built first; the API and the Angular frontend follow.

---

## Features (planned)

- **Properties and owners** – properties belong to an owner and can be managed by several managers
- **Rental contracts** – full rental history per property, renewals linked to the contract they replace, early termination
- **Renters** – several renters per contract, with an optional primary contact and a preferred notification channel
- **Rent tracking** – one rent period per month, marked as paid by a manager
- **Documents** – signed contracts, handover protocols and other files, stored in Azure Blob Storage
- **Utility accounts** – provider credentials stored in Azure Key Vault and shared selectively with renters
- **Reminders** – rent and utility bill reminders by email and WhatsApp
- **Renter portal** – read-only access for renters to their contracts, payments and shared information

---

## Tech stack

| Layer | Technology |
|---|---|
| Backend | .NET 10 Web API (controllers) |
| Architecture | Clean Architecture |
| Data access | Entity Framework Core, Azure SQL |
| Frontend | Angular, TypeScript |
| Storage and secrets | Azure Blob Storage, Azure Key Vault |
| Authentication | ASP.NET Core Identity, resource-based authorization |
| Testing | xUnit v3 on Microsoft.Testing.Platform |
| CI | GitHub Actions |

---

## Architecture

The backend follows Clean Architecture. Dependencies point inward only and are enforced through project references.

```
Domain  ←  Application  ←  Infrastructure
                ↑               ↑
                └──── Api ──────┘
```

| Project | Responsibility |
|---|---|
| `Propissimo.Domain` | Entities, value objects and business rules. No dependencies. |
| `Propissimo.Application` | Use cases and the interfaces they need from the outside world. |
| `Propissimo.Infrastructure` | EF Core, Blob Storage, Key Vault, email and messaging. |
| `Propissimo.Api` | Controllers, authentication, configuration and dependency injection. |

### Domain design

- **Rich entities** – private setters, factory methods and behaviour methods with business names.
- **Value objects** – `Money`, `Address` and `Iban` are immutable `sealed record` types with a private constructor and a validating `Create(...)` factory, so every instance is valid by construction.
- **Domain errors** – rule violations throw a `DomainException` with a stable error code, which the API maps to HTTP 400 and the frontend can translate.
- **Identifiers** – `Guid.CreateVersion7()` (time-ordered); natural keys such as the national ID are unique indexes, never primary keys.

See [docs/DOMAIN-MODEL.md](docs/DOMAIN-MODEL.md) for the full model and business rules, and [docs/TECH-STACK.md](docs/TECH-STACK.md) for technology choices.

---

## Repository structure

```
backend/
  src/
    Propissimo.Domain
    Propissimo.Application
    Propissimo.Infrastructure
    Propissimo.Api
  tests/
    Propissimo.Domain.UnitTests
frontend/          – Angular application (not started yet)
docs/              – domain model and tech stack
.github/workflows/ – CI pipeline
```

---

## Getting started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)

### Build and test

```bash
cd backend
dotnet build
dotnet test
```

Tests run on Microsoft.Testing.Platform, configured in the root `global.json`.

---

## Development workflow

- GitHub Flow: short-lived `feature/...` branches from `main`
- Every change goes through a pull request linked to an issue (`closes #<issue>`)
- `main` is protected: pull requests are squash-merged and require a passing CI build

---

## Roadmap

- [x] Solution skeleton and CI
- [x] Domain foundation: `DomainException`, `Money`, `Address`, `Iban`
- [ ] Entities: `Owner`, `Property`
- [ ] Rental contracts and renters
- [ ] Rent periods, documents, utility accounts and reminders
- [ ] Persistence with EF Core and Azure SQL
- [ ] API with authentication and resource-based authorization
- [ ] Angular frontend
- [ ] Renter portal
