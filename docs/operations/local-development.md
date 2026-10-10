# Local development

## Prerequisites

- .NET SDK 10.0.100 or newer feature band (see `global.json`)
- Node.js ≥ 24.15 (or ≥ 22.22.2) and npm
- Docker (for PostgreSQL, Azurite and integration tests)

## Run everything with .NET Aspire

```bash
dotnet run --project src/Raccord.Aspire --launch-profile http
```

The AppHost starts PostgreSQL and Azurite (with volumes), runs the `migrations` resource (`migrate --seed-demo`, fictional demo data), then the API and the Vite dev server. Open the dashboard link printed in the console to reach the frontend, logs and traces. The `http` profile sets `ASPIRE_ALLOW_UNSECURED_TRANSPORT` for local use only.

## Run with Docker Compose

See [self-hosting](self-hosting.md). Useful to test the production image locally.

## Run pieces individually

```bash
dotnet run --project src/Raccord.Web -- migrate --seed-demo
```

```bash
dotnet run --project src/Raccord.Web
```

Both need `ConnectionStrings__raccord` (for example via `dotnet user-secrets` or an environment variable). Then, in `src/Raccord.Frontend`:

```bash
npm ci
```

```bash
npm run dev
```

Vite proxies `/api` and `/health` to `RACCORD_API_URL` (default `http://localhost:5194`).

## Tests and checks

```bash
dotnet test Raccord.slnx
```

Integration tests start a PostgreSQL container through Testcontainers and create one database per test.

```bash
dotnet format Raccord.slnx --verify-no-changes
```

In `src/Raccord.Frontend`: `npm run lint`, `npm run typecheck`, `npm test`, `npm run build`.

## Changing the API contract

After changing endpoints or contracts, regenerate the OpenAPI document and TypeScript types and commit both (CI checks drift):

```bash
npm run api:generate --prefix src/Raccord.Frontend
```

## Changing the database model

```bash
dotnet tool restore
```

```bash
dotnet ef migrations add <Name> --project src/Raccord.Infrastructure --startup-project src/Raccord.Infrastructure --output-dir Persistence/Migrations
```

Review the generated migration; keep the append-only triggers intact; add data backfills when adding constraints to existing tables.
