# Raccord

An open-source, self-hostable operations and invoicing platform for audiovisual production and equipment-rental companies. Raccord is its own system of record: projects, customers, billable work and invoices live in Raccord, with an auditable path from finished job to issued invoice.

Licensed under AGPLv3 (see [COPYRIGHT.md](COPYRIGHT.md) for the brand boundary).

## Status

Active MVP delivery, with the first operational and commercial slices implemented:

The delivered path runs from workspace setup and authenticated users through customers, projects, priced work, quote generation/review/acceptance/download, invoice drafts, validation, finalization, immutable issued representations, PDF, audit events and the compliance record seam.

Authentication and authorization are implemented on the current `main` line with local ASP.NET Core Identity cookie sessions, first-administrator setup, role permissions and invitations; optional external identity sign-in is also available. Remaining product epics include quote document delivery/templates, payments, warehouse operations, staffing, backups/export, external integrations and VERI*FACTU submission (a seam exists; see [compliance seam](docs/architecture/compliance-seam.md)). Raccord makes **no legal compliance claim**.

Built in roughly one week with Codex, Git worktrees and the GitHub CLI, using OpenSpec/ProductShape traceability, .NET/Aspire, EF Core/PostgreSQL, React/TypeScript, Playwright, Docker and GitHub Actions—turning each small, evidenced change into a green, merged vertical slice.

## Quick start (Docker Compose)

```bash
cp deploy/.env.example deploy/.env
```

Set `POSTGRES_PASSWORD` in `deploy/.env` (and `RACCORD_SEED_DEMO_DATA=true` for fictional demo data), then:

```bash
docker compose -f deploy/docker-compose.yml up -d --build
```

Open <http://localhost:8080>. Details: [self-hosting](docs/operations/self-hosting.md).

## Development

```bash
dotnet run --project src/Raccord.Aspire --launch-profile http
```

```bash
dotnet test Raccord.slnx
```

See [local development](docs/operations/local-development.md).

## Stack

.NET 10 · ASP.NET Core Minimal APIs · .NET Aspire · EF Core + PostgreSQL · React 19 + TypeScript + Vite · Docker Compose · Azure Bicep · xUnit v3, Testcontainers, Vitest.

## Repository map

- `src/` — Domain, Application, Contracts, Infrastructure, Web (API + UI host), ServiceDefaults, AppHost, Frontend
- `tests/` — domain, architecture, adapter and HTTP/PostgreSQL integration tests
- `deploy/` — Dockerfile, Docker Compose, backup/restore scripts, Azure Bicep
- `docs/product/` — Product Definition as Code (PDaC, managed with ProductShape): model and Product Changes (`CHG-INITIAL`)
- `docs/architecture/` — arc42, ADRs, architecture rules, compliance seam
- `docs/research/` — domain and Spanish tax research
- `docs/operations/` — local development, self-hosting, backup/restore, Azure

Read [AGENTS.md](AGENTS.md) before contributing.
