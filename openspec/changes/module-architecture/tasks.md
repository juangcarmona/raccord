<!-- pdac-scope: cited -->

## 0. Decisions

- [x] 0.1 Accept ADR-0018 to ADR-0022 (accepted 2026-10-12). Mark ADR-0003 `Superseded by ADR-0018`. Move all ADRs to `docs/adr/` with an index, and list the current decisions in `docs/architecture/arc42/09-architecture-decisions.md`. Verify: section 09 lists 0001, 0002 and 0004–0022, and 0018 represents the superseded 0003.

## 1. Shared building blocks

- [ ] 1.1 Create `src/shared/Raccord.SharedKernel` and move `Domain/Common`, `Domain/Parties` and `Domain/Pricing` into it, with `IClock`, `ICurrentUser` (returning today's "unauthenticated" identity) and `IAuditAppender`. Verify: `dotnet build Raccord.slnx` green; domain tests pass with namespace-only edits.
- [ ] 1.2 Create `src/shared/Raccord.BuildingBlocks` with `ISender` (validation, domain bridge, handler), `ApiResults` (ErrorOr → problem details with `type`, `code` and `issues`), the shared-connection `IUnitOfWork`, and `IDocumentStore` with its two adapters. Add `ErrorOr` and `FluentValidation` to `Directory.Packages.props`. Verify: new unit tests cover each error-type mapping and the rollback of the unit of work across two contexts, against Testcontainers.
- [ ] 1.3 Create `src/shared/Raccord.ArchitectureAnalyzers` (the module rules from `specs/module-boundaries`), its test project, the module MSBuild properties, and the `BeforeBuild` reference guard in `src/modules/Directory.Build.props`. Verify: analyzer tests green; a planted forbidden reference fails the build.

## 2. Modules (one per task, all suites green after each)

- [ ] 2.1 Compliance module: audit and compliance records, `IAuditAppender`, `Compliance.Contracts.IComplianceRecorder`, `compliance` schema with the schema-move migration and the `raccord_reject_mutation()` function. Verify: web and end-to-end tests green; trigger test green on an upgraded database.
- [ ] 2.2 Contacts module: customers, `Contacts.Contracts.IPartyDirectory`, `contacts` schema. Verify: customer web tests green.
- [ ] 2.3 Workspace module: issuer profile, series definitions, `Workspace.Contracts.IInvoiceSeriesCatalog`, `workspace` schema. Verify: organization and series web tests green.
- [ ] 2.4 Projects module: projects, billable lines, `Projects.Contracts.IBillableWork`, `projects` schema. Verify: project web tests green; unknown-customer path unchanged.
- [ ] 2.5 Invoicing module: drafts, number sequences, issued invoices, PDF renderer, document retrieval, `invoicing` schema. Verify: `InvoicingSliceTests`, `InvoicingRulesTests` and the finalize end-to-end journey green; the atomicity and rollback scenarios from `specs/module-persistence` pass.
- [ ] 2.6 Remove `Raccord.Domain`, `Raccord.Application`, `Raccord.Contracts` and `Raccord.Infrastructure`, `Api/Endpoints.cs` and `ApiExceptionHandler.cs`. Move the hosts to `src/root/` and update the Aspire and end-to-end project references, the Dockerfile and `deploy/` paths. Verify: `docker compose -f deploy/docker-compose.yml config`, the image build and the full suite are green.

<!-- pdac:cite id="JRN-BILL-FINISHED-PROJECT" digest="sha256:b9530a2b0a9aac0f9cb3a5a9608494d08be1fc1190f395887fc7bb7fb30df28e" -->

<!-- pdac:cite id="BR-COMPLIANCE-OBLIGATION" digest="sha256:dab51b5c3c98ca25c40abec4a8e477fdc172ac986788fd2233eb2560f14a4757" -->

<!-- pdac:cite id="BR-GAPLESS-NUMBERING" digest="sha256:1d6404217e241d191188f8a20bdaedda2204e10548c344df7d46793d1c204adb" -->

## 3. Rules and tests

- [ ] 3.1 Rewrite `tests/Raccord.Architecture.Tests` for the module graph: domain free of frameworks, SharedKernel and BuildingBlocks references, acyclic contracts, naming test still covering every test assembly. Verify: each new rule fails on a planted violation.
- [ ] 3.2 Upgrade test: migrate a database built from the pre-change migrations, with seeded data, and assert row counts, issued-invoice hash verification and trigger rejection. Verify: green under Testcontainers.

## 4. Documentation

- [ ] 4.1 Update `docs/architecture/architecture-rules.md`, arc42 §4, §5, §6 (R-2, R-4), §8 and §11, `docs/operations/local-development.md` (migration paths), and the `openspec/config.yaml` context. Verify: no doc references `Raccord.Domain`, `Raccord.Application`, `Raccord.Infrastructure` or `Endpoints.cs`.

## 5. Gates

- [ ] 5.1 Run the CI-equivalent gates locally:
  - `dotnet tool restore && dotnet csharpier check .`
  - `dotnet build Raccord.slnx`
  - `dotnet test Raccord.slnx`
  - pending EF model check per module
  - in `src/Raccord.Frontend`: `npm run format:check`, `lint`, `test`, `build` and `api:generate`, with no OpenAPI drift
  - `openspec validate --all`
  - commitlint over the branch

    Verify: all green.
