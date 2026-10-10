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

Formatting is enforced by CSharpier (C#) and Prettier (frontend) — no `dotnet format`:

```bash
dotnet tool restore
dotnet csharpier check .
```

In `src/Raccord.Frontend`: `npm run lint`, `npm run typecheck`, `npm test`, `npm run build`, `npm run format:check`.

### Git hooks (auto-installed on build)

The first `dotnet build` after cloning installs husky hooks (`git config core.hooksPath .husky`):

- **pre-commit**: CSharpier on staged `.cs` files, then lint-staged (ESLint `--fix` + Prettier).
- **commit-msg**: [conventional commits](https://www.conventionalcommits.org/) with types `feat, fix, chore, docs, refactor, test, ci`.

Every commit-time gate is mirrored in CI's `lint` job, so bypassing locally (`HUSKY=0 git commit ...` or `--no-verify`) still produces a red build before merge.

### End-to-end journeys

`tests/Raccord.EndToEndTests` boots the whole Aspire AppHost (fresh session Postgres) and drives the real SPA with Playwright. Docker must be running:

```bash
dotnet test tests/Raccord.EndToEndTests/Raccord.EndToEndTests.csproj -c Release --filter "Quarantine!=true"
```

The PR-blocking selection is pinned by `tests/Raccord.EndToEndTests/per-pr-selection.txt`; regenerate it after adding or renaming journeys:

```bash
bash .github/scripts/check-e2e-selection.sh --update
```

### Security and license gates

The CI `security` job runs TruffleHog (secrets in history since the base), Semgrep (`.semgrep/` in-repo rules + OWASP/secrets registry sets) and Trivy; the `sbom` job Trivy-scans the built image, publishes the Syft SPDX SBOM, and enforces the license allowlist via `tools/dependency-licenses`. To run the license check locally:

```bash
dotnet run --project tools/dependency-licenses/dependency-licenses.csproj -- --check
```

### OpenSpec changes

Every meaningful change ships an OpenSpec change under `openspec/changes/` (proposal, tasks, capability deltas). PDaC (`docs/product/`) stays the product-model source of truth. Validate locally:

```bash
npx -y @fission-ai/openspec@1.12.0 validate --all --no-interactive
```

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
