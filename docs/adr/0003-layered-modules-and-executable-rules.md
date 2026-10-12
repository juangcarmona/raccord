---
name: layered-modules-and-executable-rules
description: Raccord keeps six layered production projects with modules as namespaces inside them, and enforces the dependency rules with architecture tests.
status: Superseded by ADR-0018
date: 2026-10-12
deciders: ["Juan G. Carmona"]
tags: ["architecture", "modules", "dependency-rules", "architecture-tests"]
---

# ADR-0003: Layered projects with module namespaces and executable dependency rules

## Context

ADR-0001 chose a modular monolith. The required dependency direction is Web → Application → Domain, Web → Contracts, Infrastructure → Application + Domain + Contracts, and a framework-free Domain. Rules that live only in documents erode.

## Decision

We will keep layered projects with module namespaces and enforce the dependency rules with executable tests.

- Keep six production projects: `Domain`, `Contracts`, `Application`, `Infrastructure`, `Web`, `ServiceDefaults`, plus `AppHost`.
- Modules (Customers, Projects, Pricing, Billing, Organization, Auditing, Compliance, Documents) are namespaces inside the layered projects. Each module exposes a small public surface (aggregates and value objects in Domain; use-case classes such as `InvoicingWorkflow` in Application) and keeps behavior behind it.
- `Application` references `Contracts` so use cases return wire contracts directly; no third set of DTOs.
- `Raccord.Architecture.Tests` enforces: Domain and Contracts reference only the BCL; Application has no ASP.NET, EF Core, Npgsql, Azure, Aspire or HTTP references; Infrastructure does not reference Web; project references match the allowed graph; core projects add no packages.
- The previous `ValidateArchitecture` MSBuild target only printed a message and was removed.

## Consequences

- **Positive:**
  - Breaking a rule fails the build pipeline (verified by injecting a forbidden reference).
- **Negative:**
  - Module boundaries inside a project are enforced by review and namespaces, not by assemblies.
- **Neutral:**
  - If coupling grows, a module can move to its own assembly without changing the dependency direction.

## Alternatives considered

- **Keep the dependency rules in documents and the `ValidateArchitecture` MSBuild target:** rejected because rules that live only in documents erode, and the target only printed a message.
- **A third set of DTOs:** rejected because `Application` references `Contracts` and use cases return wire contracts directly.

## References

- Related: ADR-0001
- Superseded by: ADR-0018
- Accepted 2026-10-06 and superseded 2026-10-12; the frontmatter date records the latest status change.
