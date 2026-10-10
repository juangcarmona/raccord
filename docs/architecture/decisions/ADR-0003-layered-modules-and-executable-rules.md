# ADR-0003: Layered projects with module namespaces and executable dependency rules

- Status: accepted
- Date: 2026-10-06

## Context

ADR-0001 chose a modular monolith. The required dependency direction is Web → Application → Domain, Web → Contracts, Infrastructure → Application + Domain + Contracts, and a framework-free Domain. Rules that live only in documents erode.

## Decision

- Keep six production projects: `Domain`, `Contracts`, `Application`, `Infrastructure`, `Web`, `ServiceDefaults`, plus `AppHost`.
- Modules (Customers, Projects, Pricing, Billing, Organization, Auditing, Compliance, Documents) are namespaces inside the layered projects. Each module exposes a small public surface (aggregates and value objects in Domain; use-case classes such as `InvoicingWorkflow` in Application) and keeps behavior behind it.
- `Application` references `Contracts` so use cases return wire contracts directly; no third set of DTOs.
- `Raccord.Architecture.Tests` enforces: Domain and Contracts reference only the BCL; Application has no ASP.NET, EF Core, Npgsql, Azure, Aspire or HTTP references; Infrastructure does not reference Web; project references match the allowed graph; core projects add no packages.
- The previous `ValidateArchitecture` MSBuild target only printed a message and was removed.

## Consequences

- Breaking a rule fails the build pipeline (verified by injecting a forbidden reference).
- Module boundaries inside a project are enforced by review and namespaces, not by assemblies. If coupling grows, a module can move to its own assembly without changing the dependency direction.
