# Source layout

```text
src/
├── Raccord.Domain/           # business rules: Common, Pricing, Parties, Customers, Organization, Projects, Billing, Auditing
├── Raccord.Contracts/        # public HTTP contracts (records)
├── Raccord.Application/      # use cases (CustomerDirectory, ProjectWorkspace, InvoicingWorkflow, OrganizationSettings, AuditLog) and ports
├── Raccord.Infrastructure/   # EF Core/PostgreSQL, migrations, PDF renderer, document stores, compliance recorder, clock
├── Raccord.Web/              # Minimal API endpoints, problem details, `migrate` command, SPA hosting
├── Raccord.ServiceDefaults/  # health checks, OpenTelemetry, resilience
├── Raccord.AppHost/          # .NET Aspire orchestration
└── Raccord.Frontend/         # React + TypeScript UI
```

Dependency rules are in [architecture rules](../docs/architecture/architecture-rules.md) and enforced by `tests/Raccord.Architecture.Tests`.
