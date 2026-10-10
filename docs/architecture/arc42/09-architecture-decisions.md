# 9. Architecture decisions

Decision records live in [`../decisions/`](../decisions/).

| ADR | Decision |
|---|---|
| [0001](../decisions/ADR-0001-modular-monolith.md) | Start with a modular monolith |
| [0002](../decisions/ADR-0002-license-and-brand.md) | AGPLv3 code with a separate brand boundary |
| [0003](../decisions/ADR-0003-layered-modules-and-executable-rules.md) | Layered projects, module namespaces, executable dependency rules |
| [0004](../decisions/ADR-0004-postgresql-ef-core-append-only.md) | PostgreSQL + EF Core; append-only financial tables enforced by triggers |
| [0005](../decisions/ADR-0005-issued-invoice-representation.md) | Canonical issued-invoice representation with SHA-256 fingerprint |
| [0006](../decisions/ADR-0006-money-rounding-and-vat.md) | Money, rounding and VAT policy |
| [0007](../decisions/ADR-0007-gapless-numbering.md) | Gapless numbering per series and year |
| [0008](../decisions/ADR-0008-deterministic-pdf-renderer.md) | In-house deterministic PDF renderer |
| [0009](../decisions/ADR-0009-document-storage.md) | Document storage: file system and Azure Blob |
| [0010](../decisions/ADR-0010-compliance-seam.md) | Compliance seam before VERI*FACTU |
| [0011](../decisions/ADR-0011-frontend-stack.md) | React + Vite with OpenAPI-generated types |
| [0012](../decisions/ADR-0012-single-organization-per-deployment.md) | One organization per deployment (MVP) |
| [0013](../decisions/ADR-0013-deployment-topology.md) | Deployment topology: Compose, Aspire, Azure |
| [0014](../decisions/ADR-0014-testing-strategy.md) | Testing strategy |
| [0015](../decisions/ADR-0015-build-quality-gates.md) | Build and quality gates |
