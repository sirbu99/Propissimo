# Propissimo – Tech Stack

Propissimo is a web application for independent landlords. It brings properties, rental contracts, renters, payments, documents and notifications together in one place.

This document describes the technologies the application is built on and how the codebase is organized.

---

## Overview

| Layer | Technology |
|---|---|
| Backend | .NET 10 Web API |
| Architecture | Clean Architecture |
| Data access | Entity Framework Core |
| Frontend | Angular, TypeScript |
| Database | Azure SQL |
| File storage | Azure Blob Storage |
| Secrets | Azure Key Vault |
| Notifications | Email, WhatsApp |
| Hosting | Azure App Service (API), Azure Static Web Apps (frontend) |
| CI/CD | GitHub Actions |
| Project management | GitHub Projects |

---

## Backend

### .NET 10 Web API
A REST API that serves the frontend and, in a later phase, the renter portal. Built on .NET 10 (LTS) with controller-based endpoints.

### Clean Architecture
The backend solution is split into four projects:

```
Domain  ←  Application  ←  Infrastructure
                ↑               ↑
                └──── Api ──────┘
```

| Project | Responsibility |
|---|---|
| **Propissimo.Domain** | Entities and business rules (for example, a rental contract has exactly one primary renter). Has no dependencies. |
| **Propissimo.Application** | Use cases such as creating a contract or generating a payment schedule. Defines the interfaces it needs from the outside world. |
| **Propissimo.Infrastructure** | Implementations of those interfaces: database access, file storage, secrets, email and messaging. |
| **Propissimo.Api** | Entry point: controllers, authentication, configuration and dependency injection setup. |

Dependencies point inward only, enforced through project references. Business rules can be tested in isolation, and infrastructure providers can be replaced without changes to the domain.

### Entity Framework Core
ORM for mapping domain entities to the relational database. Schema changes are managed through EF Core migrations.

### Testing
- **Unit tests** (xUnit) for the Domain and Application layers
- **Integration tests** for the API *(planned)*

---

## Frontend

### Angular + TypeScript
The landlord-facing web application, created with the Angular CLI. The codebase uses modern Angular: standalone components, signals and the built-in control flow syntax (`@if`, `@for`). It consumes the backend exclusively through the REST API.

---

## Data and storage

### Azure SQL
Relational database for all application data. The domain is strongly relational:

```
Property → RentalContract → ContractRenter → Renter
                ↓
             Payment
```

A property can have several active rental contracts at the same time (shared apartments), and a contract can have several renters, one of whom is the primary contact.

SQL Server LocalDB is used for local development, as it runs the same engine as Azure SQL.

### Azure Blob Storage
Storage for uploaded documents such as signed contracts (PDF). The database stores only metadata and a reference to each blob. Azurite is used as a local emulator.

### Azure Key Vault
Encrypted storage for utility provider credentials, kept per property and shared selectively with renters. Credentials are never stored in the database in plain text.

---

## Notifications

| Channel | Purpose |
|---|---|
| **Email** | Payment reminders, due dates and overdue notices |
| **WhatsApp** | Messages to individual renters and to a shared group per contract |

Notifications are sent from background jobs, outside the request pipeline. The email and WhatsApp providers are being finalized.

---

## Hosting and DevOps

| Component | Service |
|---|---|
| API | Azure App Service |
| Frontend | Azure Static Web Apps |
| CI/CD | GitHub Actions (build, test, deploy) |
| Task tracking | GitHub Projects |

### Repository structure
The project is a monorepo:

```
/backend               – .NET solution
    /src               – Domain, Application, Infrastructure, Api
    /tests             – test projects
/frontend              – Angular application
/infra                 – infrastructure as code (Azure)
/docs                  – documentation
/.github/workflows     – CI/CD pipelines
```

---

## Development environment

| Tool | Use |
|---|---|
| Visual Studio Community | Backend development |
| VS Code + Angular Language Service | Frontend development |
| .NET 10 SDK + `dotnet-ef` | Build, run, EF Core migrations |
| Node.js LTS + Angular CLI | Frontend build and dev server |
| SQL Server LocalDB | Local database |
| Azurite | Local Blob Storage emulator |
| Git, GitHub CLI | Version control |
| PowerShell 7 | Terminal |
| Azure CLI | Provisioning and deployment |

---

## Roadmap

Planned additions as the application grows:

- **Authentication and authorization:** JWT, policy-based authorization, per-landlord data isolation
- **Background processing:** scheduled payment generation, overdue detection, notification dispatch
- **Caching:** in-memory, then distributed (Redis)
- **Resilience and operations:** rate limiting, health checks
- **Observability:** structured logging (Serilog), OpenTelemetry / Application Insights
- **Renter portal:** read-only access to contracts, payments and shared utility information
