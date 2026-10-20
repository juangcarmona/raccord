<!-- pdac-scope: cited -->

## Work item and acceptance evidence

Work item [#30](https://github.com/juangcarmona/raccord/issues/30); criteria numbered AC1–AC8 in their issue order. Proposed tests below are implementation outputs, not claims that code or verification already exists. Add new tests before each new seam; run existing behavior-preservation tests before and after each extraction checkpoint.

<!-- pdac:cite id="JRN-BILL-FINISHED-PROJECT" digest="sha256:b9530a2b0a9aac0f9cb3a5a9608494d08be1fc1190f395887fc7bb7fb30df28e" -->
<!-- pdac:cite id="UC-CREATE-INVOICE-DRAFT" digest="sha256:f7310fed1fddd6fc7c813db19db677f8aea9feff2f179549c9a9425607804d33" -->
<!-- pdac:cite id="UC-ISSUE-INVOICE" digest="sha256:08b2bb0bd93fc7fbb0b745ee1e6e7f3f70d064d44da287ed8c478a8da7761afc" -->
<!-- pdac:cite id="BR-MONEY-ROUNDING" digest="sha256:51ad0cc84544ce872e3a93e25fb8110b701a85ec9efe737bc1ec15b8b985b87f" -->
<!-- pdac:cite id="BR-LINE-PRICING-BOUNDS" digest="sha256:652792a573f685a43f131a08b467a4de67c176c48e81ea5ba38aa1367e7c599c" -->
<!-- pdac:cite id="BR-TAX-ID-NORMALIZATION" digest="sha256:aadb655c57c6486610d3a3346f45bdd19696a407dd954d4fc37cd9cefd3391ca" -->
<!-- pdac:cite id="BR-PARTY-SNAPSHOT" digest="sha256:f278c6a11a99fe63feadf0105b0ce359e74c71580b9b070a4fbf95c3b34b1604" -->
<!-- pdac:cite id="BR-GAPLESS-NUMBERING" digest="sha256:1d6404217e241d191188f8a20bdaedda2204e10548c344df7d46793d1c204adb" -->
<!-- pdac:cite id="BR-ISSUED-INVOICE-IMMUTABLE" digest="sha256:8138f31518261c618e3fef1dc93d5deaef6f55a4c3b2bfd41dfc0c5a4b75aecc" -->
<!-- pdac:cite id="BR-AUDITED-ACTIONS" digest="sha256:33a6a76f11ff3ed15dcbd22798ea6b65a05d65d4130cb17c7751eb516266e54c" -->
<!-- pdac:cite id="BR-COMPLIANCE-OBLIGATION" digest="sha256:dab51b5c3c98ca25c40abec4a8e477fdc172ac986788fd2233eb2560f14a4757" -->
<!-- pdac:cite id="BR-INTEGRITY-VERIFICATION" digest="sha256:19dff5afbeb5e4cd25a3baaa4f1395ee74b0c0722572021e496e654cb44aaf34" -->
<!-- pdac:cite id="FR-AUDIT-001" digest="sha256:7646b5398f69286e802b2a3175edcf8b18cfc3b742edc9d3c2a44476c6159417" -->
<!-- pdac:cite id="FR-COMPLIANCE-001" digest="sha256:a53549df92d28a28435890b7663e0a9c92ec126ff0988b6cb103c39d5aa050d8" -->
<!-- pdac:cite id="QR-DETERMINISM" digest="sha256:2186a4638c7ad311864a05314e15638df09365c839eaa932ceb57eddcb1559e9" -->
<!-- pdac:cite id="QR-CONCURRENCY-CONSISTENCY" digest="sha256:320945ebf06be10006380b4e183377f48569c75224692b903f3cf093887736a2" -->

| Criterion | Evidence after implementation |
| --- | --- |
| AC1: project/dependency boundaries | Architecture tests independently assert SharedKernel BCL-only and BuildingBlocks → SharedKernel only; exact interim reference graph and framework-free Domain/Application checks remain enforced. |
| AC2: shared value objects/ports and live consumers | Existing Domain tests, PDF tests, web suite and Release build; compare assertion changes and moved source bodies. |
| AC3: current identity and audit append | Web unauthenticated attribution assertion; shared port appends exactly once, preserves all audit facts and stores a copied detail dictionary through the current DbContext. |
| AC4: sender | Validation prevents handler side effects; valid input runs one handler; domain/concurrency mapping, cancellation and defect passthrough tests. |
| AC5: HTTP result mapping | Real serialized IResult response JSON/status/content type for every error type and special existing invoice failure. |
| AC6: cross-context atomicity | PostgreSQL tests show commit-both/rollback-both through fresh connection reads, both save orders, callback failure, cancellation and conflict mapping. |
| AC7: storage compatibility | Filesystem read/overwrite/null/containment tests and Azure transport contract tests; unchanged key/config/content-type handling plus PDF/representation suite. |
| AC8: dependencies and compatibility | Central version diff, package/license audit, solution build, web/E2E suites and generated OpenAPI diff. |

## Concrete new tests

Use xUnit v3, Shouldly and `TestContext.Current.CancellationToken`. Put sender/HTTP/storage tests in `tests/Raccord.BuildingBlocks.Tests`; adapter/actor tests live in the current Web/Infrastructure suites where the owning types and fixtures are available.

| Test method | Important assertion |
| --- | --- |
| `Sender_Should_NotInvokeHandlerWhenValidationFails` | Handler counter and side-effect list remain zero/empty; all validation failures returned. |
| `Sender_Should_InvokeHandlerOnceForValidRequest` | Handler count one; result and cancellation token preserved. |
| `Sender_Should_PreserveMultipleValidatorOrder` | Stable validator registration order and all code/message pairs. |
| `Sender_Should_PreserveDomainRuleCode` | Rule discriminator/code preserved. |
| `Sender_Should_MapAlreadyIssuedDraftToConflict` | `draft.not-open` yields 409, never 422. |
| `Sender_Should_PropagateIntegrityFailures` | Original integrity exception escapes; existing web test still sees 500/code and critical path. |
| `Sender_Should_PropagateCancellationAndUnknownExceptions` | Neither becomes a business ErrorOr failure. |
| `ApplicationExceptionMapper_Should_PreserveReadinessIssues` | `draft.invalid`, original title/detail and every readiness issue retained. |
| `ApiResults_Should_MapEveryErrorType` | Parameterized rows from design section 4; serialized problem shape and content type. |
| `ApiResults_Should_IncludeAllValidationIssues` | `issues` objects contain code/message only; no leaked property value. |
| `ApiResults_Should_NotExposeUnexpectedFailureDetails` | Safe 500 message with no exception/request payload. |
| `AuditAppender_Should_PersistOneEntryWithOriginalFacts` | Query after commit sees exactly one event with supplied facts; later mutation of caller details does not change it. |
| `CurrentUser_Should_KeepUnauthenticatedAuditAttribution` | Current web audit data retains `unauthenticated`. |
| `DocumentStorage_Should_SelectConfiguredProvider` | Original FileSystem/AzureBlob options and singleton instance resolution work. |
| `FileSystemDocumentStore_Should_OverwriteAndReadExactBytes` | Readback equals newest bytes under the original key. |
| `AzureBlobDocumentStore_Should_PreserveUploadOptionsAndMissingRead` | Controlled SDK transport verifies container creation, overwrite request, content type, exact bytes and null on 404; no live account is used. |

Keep transferred filesystem safety tests and `tests/Raccord.Infrastructure.Tests/Documents/PdfInvoiceRendererTests.cs` expectations intact. Do not require a generated PDF to be committed.

## PostgreSQL integration scenarios

Create one test container (`postgres:17-alpine`, matching the existing Web fixture) and an isolated database per test. Two lightweight DbContexts own two distinct test tables; each uses the same external NpgsqlConnection. Use raw SQL for test-only DDL or a dedicated fixture builder, not production migrations. A fresh connection verifies committed row counts after every operation; do not verify through the failed contexts' trackers.

1. `SharedUnitOfWork_Should_CommitBothContexts`: valid rows in both; exactly one transaction commits.
2. `SharedUnitOfWork_Should_RollBackBothContextsWhenLaterSaveFails`: unique-constraint failure after a successful first save, parameterized to reverse context order; neither row exists.
3. `SharedUnitOfWork_Should_RollBackOnCallbackFailure`: callback throws; no commit.
4. `SharedUnitOfWork_Should_RollBackAfterCancellation`: a test SaveChanges interceptor cancels after the first save; rollback still happens and fresh reads show zero rows.
5. `SharedUnitOfWork_Should_RejectIndependentConnections`: identical connection strings with different objects still fail before either saves.
6. `SharedUnitOfWork_Should_EnlistContextsRegisteredDuringExecution`: second participant registered during callback is included; inject a later failure to prove both roll back.
7. `SharedUnitOfWork_Should_RegisterEachContextOnce`: repeated registration does not duplicate enlistment or saving.
8. `SharedUnitOfWork_Should_MapUniqueAndConcurrencyConflicts`: real PostgreSQL 23505 and stale optimistic version produce the shared conflict exception, and the legacy adapter exposes today's concurrency exception/code.
9. `SharedUnitOfWork_Should_KeepCallerOwnedConnectionUsable`: connection ownership/open-state is preserved after success and failure; a fresh scope can perform the next operation.
10. `SharedUnitOfWork_Should_RejectNestedOrExternallyOwnedTransactions`: no hidden commit or ambiguous nested owner.
11. `SharedUnitOfWork_Should_RejectSaveInsideExecute`: nested compatibility flush fails before it can commit.
12. `SharedUnitOfWork_Should_RejectPerContextRetries`: an enabled retry strategy cannot silently execute individual saves.

Tests for rollback/cancellation must instrument an actual successful first save; a failure before any INSERT does not prove rollback.

## E2E and existing behavior

Run all four committed selections in `tests/Raccord.EndToEndTests/per-pr-selection.txt`: app boot, customer registration, project/billable-line creation and invoice finalization. Keep the manifest byte-identical and run its selection guard. This internal refactor introduces no new journey. Do not quarantine, narrow filters or rewrite expected invoice/audit/numbering results to obtain green.

## Checkpoint A: baseline and extraction

From the repository root:

```bash
dotnet tool restore
dotnet restore Raccord.slnx
dotnet build Raccord.slnx -c Release --no-restore
dotnet test tests/Raccord.Domain.Tests -c Release --no-build
dotnet test tests/Raccord.Infrastructure.Tests -c Release --no-build
dotnet test tests/Raccord.Architecture.Tests -c Release --no-build
dotnet test tests/Raccord.Web.Tests -c Release --no-build
```

Baseline recorded on 2026-10-12 at plan commit `69f5656`: tool restore, solution restore, Release build, Domain tests (59), Infrastructure tests (11), and Architecture tests (14) passed. Web tests were attempted and could not run because Testcontainers was denied access to `/var/run/docker.sock`; this is an environment failure, not a green test result. After adding the new test project, also run `dotnet test tests/Raccord.BuildingBlocks.Tests -c Release --no-build` at every messaging/persistence checkpoint. Docker must be available for PostgreSQL tests. Record baseline environmental failures; do not claim a failed/unavailable suite is green.

## Checkpoint B: compatibility

```bash
dotnet build Raccord.slnx -c Release
dotnet test Raccord.slnx -c Release --no-build --filter "FullyQualifiedName!~Raccord.EndToEndTests"
dotnet ef migrations has-pending-model-changes --project src/Raccord.Infrastructure --startup-project src/Raccord.Infrastructure --no-build --configuration Release
bash .github/scripts/check-e2e-selection.sh
dotnet tests/Raccord.EndToEndTests/bin/Release/net10.0/Raccord.EndToEndTests.dll -filter "/[Quarantine!=true]" -stopOnFail
```

Install the matching Playwright Chromium runtime using the command from current CI before E2E execution. From `src/Raccord.Frontend`, run `npm ci`, `npm run api:generate`; inspect the generated diff. Routes, request/response schemas and existing failure responses must be unchanged. Review any operation-id/tag-only change explicitly.

## Checkpoint C: Done evidence

Use current `.github/workflows/ci.yml` as the authoritative command source. The remaining commands include:

```bash
dotnet csharpier check .
npx -y markdownlint-cli2@0.23.3
npx -y @fission-ai/openspec@1.12.0 validate --all --no-interactive
npx -y @prodshape/cli@0.22.0 validate
npx -y --package @fission-ai/openspec@1.12.0 --package @prodshape/cli@0.22.0 prodshape citations verify --provider openspec
dotnet list Raccord.slnx package --vulnerable --include-transitive
dotnet run --project tools/dependency-licenses/dependency-licenses.csproj -- --check
docker compose -f deploy/docker-compose.yml config
docker build -f deploy/docker/Dockerfile .
git diff --check
```

Run the existing frontend format/lint/typecheck/test/build/audit commands, commitlint over the PR range, and CI security/image/SBOM checks. The new shared projects must participate in existing runtime dependency/license scanning. No tests/builds are claimed at planning time; record actual results on the PR during implementation.

## Definition of Done mapping

All eleven dimensions from `docs/engineering-lifecycle.md` apply:

| Dimension | Required closure evidence |
| --- | --- |
| 1. Acceptance criteria | AC1–AC8 mapped above to passing evidence, with Owner confirmation. |
| 2. Implementation completeness | This task list complete or explicit justified drops; parent tasks 1.1/1.2 marked only after verification. |
| 3. Tests | Green suites, recorded new-test red/green evidence and naming/Shouldly conformance; Owner assesses coverage. |
| 4. Quality checks | Format, analyzers, TypeScript/lint/commitlint, pending-model and OpenAPI checks. |
| 5. Security | Current secrets/static/dependency/container/license/SBOM gates; Owner reviews new infrastructure dependencies; post-merge ZAP belongs to integration. |
| 6. Architecture/design | Interim graph, pure SharedKernel and bounded adapters verified; deviations reported to Owner. |
| 7. Documentation | Interim architecture rules and arc42 5/8 updated; existing operation/migration paths remain accurate. |
| 8. Product/spec consistency | Baseline/OpenSpec/citations valid; no accepted-model edits; Owner confirms agreement. |
| 9. Evidence | Link green CI for exact head commit. UI capture is N/A because no UI changes. |
| 10. Human review | Owner approves the implementation PR before integration. |
| 11. Known limitations | Module migration/analyzers linked to #31–#34 and epic #16; endpoint adoption and full finalization remain later Invoicing work. Document storage is outside PostgreSQL rollback. |

## Handoff to a smaller model

Use this prompt after the Owner approves the plan:

> Implement issue #30 on the existing PR branch. Read AGENTS.md and openspec/changes/shared-module-foundation/{proposal,design,tasks,test-plan}.md plus its spec. Work through this change's unchecked tasks in order, one checkpoint at a time. Use the exact signatures, source/destination paths and temporary adapters in design.md. Write the named failing tests before each new seam; preserve existing domain/web/E2E assertions. Run each task's verification before ticking it. Do not execute the parent epic's module/schema migration tasks, change the product model, weaken architecture/selection/contract checks, or merge/deploy. Record test results and AC1–AC8 evidence in the existing PR. If an invariant fails, report the concrete diff/failing test rather than inventing a new design. Finish by requesting the Owner's Done review.
