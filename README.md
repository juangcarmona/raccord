# Raccord

An open-source, self-hostable operations and invoicing platform for audiovisual production and equipment-rental companies. Raccord is its own system of record: projects, customers, billable work and invoices live in Raccord, with an auditable path from finished job to issued invoice.

Licensed under AGPLv3 (see [COPYRIGHT.md](COPYRIGHT.md) for the brand boundary).

## Status

The current interface lets a team register customers, create projects, record billable work, prepare invoice drafts and issue invoices with a downloadable PDF and audit history.

Sign-in is implemented and required to use the application. There is no hosted demo or public registration. The interface does not yet support payment management; rectifying invoices and VERI*FACTU submission are also unavailable. See the [compliance seam](docs/architecture/compliance-seam.md). Raccord makes **no legal compliance claim**.

## Quick start (Docker Compose)

When a stable release is available, choose its image digest from [GitHub Releases](https://github.com/juangcarmona/raccord/releases) and run these commands from the repository root. If the release list is empty, there is no supported official image to install yet.

```bash
cd deploy
cp .env.example .env
```

Set `RACCORD_IMAGE` to the selected digest and `POSTGRES_PASSWORD` in `.env`, then pull and start the stack:

```bash
docker compose pull
docker compose up -d
```

Open <http://localhost:8080>. Follow the [self-hosting guide](docs/operations/self-hosting.md) to create the first administrator and to upgrade or restore an installation.

## Development

```bash
dotnet run --project src/root/Raccord.Aspire --launch-profile http
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
