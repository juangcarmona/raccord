# Process ledger

This ledger records repeatable improvements discovered during delivery reviews. Each entry names the observation, the durable action, and the check that prevents regression.

## 2026-10-13 · Quote acceptance

- Observation: Acceptance needs an auditable trusted timestamp while retaining the quote's immutable number and pricing.
- Action: Permit acceptance only from `ReadyForSignature`, set `AcceptedAt` from `IClock`, and reject duplicate acceptance in the domain.
- Check: Domain lifecycle tests, migration, generated contracts, OpenSpec validation and a linked workflow visual are required before merge.

## 2026-10-13 · Quote review transition

- Observation: Review readiness is a workflow state, not permission to mutate a commercial snapshot.
- Action: Persist a small explicit state machine and reject repeated or unknown transitions at the domain/API boundary.
- Check: Transition tests, migration, generated contracts, OpenSpec validation and a linked workflow visual are required before merge.

## 2026-10-13 · Quote generation

- Observation: A quote must remain auditable after the project plan changes, while numbering must remain gapless within its series.
- Action: Copy project/customer identity, priced lines, terms and validity into an immutable snapshot; allocate the yearly Q series in the same unit of work and enforce uniqueness in the database.
- Check: Domain tests cover validity and empty plans; migration, generated OpenAPI, OpenSpec citations, and a repository-hosted workflow visual are required before merge.

## 2026-10-13 · Financial settings seam

- Observation: Epic-level commercial behavior needs a stable company-wide settings boundary before quote generation can consume it.
- Action: Persist only validated quote/contract validity and expiry-signing policy, expose idempotent organization endpoints, and keep later quote/signing behavior out of this slice.
- Check: Domain rule tests, migration, generated OpenAPI and a linked workflow visual are required in the PR before integration.

## 2026-10-13 · Compliance submission worker

- Observation: The transactional pending record is the safe outbox boundary; a worker must not recalculate or mutate the issued invoice.
- Action: Submit the immutable representation with the record hash as idempotency key, load optional certificate material only from configuration, and persist response metadata without logging secrets.
- Check: Analyzer-backed build, migration generation, OpenSpec citation validation, and CI container/E2E gates run before merge; authority sandbox tests remain an explicit follow-up until an endpoint is provisioned.

## 2026-10-13 · Rectifying invoices

- Observation: Corrections must preserve the issued record while allowing negative differences and a separate numbering series.
- Action: Create rectification drafts from the immutable invoice snapshot, persist the original ID, method and reason, and delegate numbering to series R.
- Check: Domain tests issue an R-series rectification, assert signed differences, and verify the original totals remain unchanged; migration and OpenSpec validation run before CI.

## 2026-10-12 · Epic #16 module architecture

- Observation: A final integration PR exposed inherited commit-message failures that child PRs did not see against their stacked feature branch.
- Action: Run the complete CI-equivalent gate against the eventual target branch before opening or merging the final integration PR, including commitlint over the full target range.
- Check: The final PR's lint job must pass over `main` to head, not only over the immediately preceding stacked branch.

- Observation: Moving the Aspire host changed both endpoint discovery and the relative Vite path.
- Action: Treat host relocation as a runtime change and exercise the AppHost E2E journey before integration.
- Check: The E2E job must boot the AppHost, pass health checks, and complete the selected journeys.

- Observation: Implemented child issues remained open because merged PRs did not consistently carry closing references.
- Action: Reconcile every child issue during the integration review and close stale implementation issues with links to the merged PRs.
- Check: The review records the final state of the epic and every delivered child issue.

- Observation: The local merge command could not check out `main` because another worktree owned it.
- Action: Use a worktree-safe merge path and never disturb another agent's checkout.
- Check: Verify the remote merge result and worktree status after integration.

## 2026-10-12 · Authentication delivery review

- Observation: The authentication change initially protected the test factory and the Aspire health probe through the global fallback policy, so the first CI attempts failed or waited for a host that could not become healthy.
- Action: Keep production authorization enabled in the real AppHost, make the test bypass explicit, and mark infrastructure health endpoints anonymous. Add a browser journey for the anonymous API boundary so this distinction remains executable.
- Check: The backend test job and the AppHost E2E job must both pass, including the authentication journey, before merging.

- Observation: UI functionality is difficult to review from a mobile PR view without visual evidence, while reproducing the full AppHost locally is expensive when Docker is unavailable.
- Action: Capture a representative sign-in screenshot as versioned PR evidence and reuse the shared browser context for E2E journeys. Do not replace required security, acceptance, or contract gates; reduce cost through shared setup and by avoiding redundant local reruns after the same CI head is green.
- Check: Every PR that adds UI behavior links a screenshot in its description, and the E2E selection manifest names the new journey explicitly.

## Review rule

Every completed FF loop must review this ledger, add new observations and actions when useful, and verify that existing actions were followed. The ledger is process memory for future agents; it is not a substitute for CI or product documentation.

## 2026-10-13 · Project templates and rescheduling

- Observation: Rescheduling must move project-owned planning lines with the aggregate, while availability should remain a derived read rather than duplicated state.
- Action: Keep the offset operation on `Project`, reuse the existing project factory for template creation, and let availability recalculate from persisted dates.
- Check: Domain coverage asserts the aggregate offset; generated API, migration, OpenSpec validation, and CI verify the integration boundary.

## 2026-10-13 · VERI*FACTU responsibility boundary

- Observation: Compliance implementation was blocked by ambiguity about whether an open-source maintainer or a self-hosted operator owns the installation's declaration and certificate.
- Action: Record the deployment boundary in ADR-0023 and retain an explicit no-compliance-claim guard until tax/legal review and official-specification verification are complete.
- Check: Risk R-2, the compliance seam, and future VERI*FACTU PR descriptions link the ADR and preserve the legal-review gate.

## 2026-10-13 · VAT treatment preservation

- Observation: A VAT percentage alone cannot distinguish tax regimes or exemption causes required by a future authority record.
- Action: Carry regime and cause through pricing, grouped totals, public contracts, and versioned issued representations; retain a legacy restore path for existing invoices.
- Check: Domain tests keep equal-rate exemptions separate, representation tests cover the new schema, and CI checks generated contracts.

## 2026-10-13 · Compliance record chaining

- Observation: A pending obligation with only the invoice hash could detect invoice tampering but not record reordering or altered chain context.
- Action: Store predecessor and record hashes in the compliance-owned append-only row and compute them before the issuing transaction commits.
- Check: The migration, append-only database protections, and CI compliance tests must pass before the chain is used by submission work.

## 2026-10-13 · Deterministic invoice QR

- Observation: The invoice PDF needs tax verification data without making the renderer recalculate or depend on timestamps.
- Action: Build the AEAT payload from the immutable invoice, generate level-M QR modules, and draw deterministic vectors in the first-page renderer.
- Check: Payload ordering/encoding, label/legend placement, structural PDF validity, and byte-identical rendering are covered before merge.

## 2026-10-13 · Contact CRUD and pagination

- Observation: A feature can be marked in review before its acceptance criteria are represented by a pushed PR.
- Action: Reconcile issue state against child PRs and executable acceptance tests before closing an epic.
- Check: Contact CRUD, pagination, generated contracts, and web coverage are present before the feature branch is promoted.

## 2026-10-13 · Equipment module scaffold

- Observation: An empty bounded-context module should establish only the seam and ownership boundary.
- Action: Add contracts, implementation, composition registration, and schema naming without inventing item behavior.
- Check: The solution and analyzer-backed module build pass before starting the first equipment feature.

## 2026-10-13 · Equipment items

- Observation: New module projects must be included in the container restore cache, not only the solution.
- Action: Add every new project to Dockerfile dependency-copy stages and verify the container gate before merging.
- Check: Container, backend, migration, and generated-contract checks all pass for the item slice.

## 2026-10-13 · Equipment item details

- Observation: File uploads need both a storage boundary and persisted metadata; validation belongs before storage.
- Action: Keep IDocumentStore provider-agnostic, cap image size/type at the API boundary, and persist only the object key.
- Check: Web/OpenAPI and storage-backed tests plus CI validate the upload path.

## 2026-10-13 · Equipment kits

- Observation: Kit content is a relationship slice that can be useful before stock derivation and planning rules exist.
- Action: Persist explicit kind/content lines, validate positive quantities and existing non-self references, and defer stock calculations to the planning slice.
- Check: Kit migration, API contract generation, and CI architecture tests pass before adding import behavior.

## 2026-10-13 · Equipment import

- Observation: Import validation is easiest to reason about when each row produces an explicit outcome.
- Action: Keep the CSV format narrow for the MVP, validate before adding rows, and return row numbers with rejection reasons.
- Check: Generated API contracts and CI integration coverage verify valid rows persist while invalid rows do not.

## 2026-10-13 · Project planning status

- Observation: A lifecycle field is valuable before downstream planning effects are implemented.
- Action: Persist a fixed status vocabulary and expose explicit transitions while leaving warehouse/invoicing effects to their owning slices.
- Check: Project migration, generated contracts, and CI checks pass before adding planning lines.

## 2026-10-13 · Project equipment planning

- Observation: Line-specific periods need domain validation at the project boundary before availability logic exists.
- Action: Store planned equipment as project-owned lines, require positive quantities, and bound dates to the project period.
- Check: Domain build, migration, generated contracts, and CI validate the period invariant.

## 2026-10-13 · Equipment availability

- Observation: Availability can be exposed safely before reservation commands exist if hold categories remain explicit.
- Action: Exclude cancelled work, classify draft/planned lines as soft holds, and active/completed lines as reservations.
- Check: The endpoint and generated contract pass CI; atomic reservation remains a separate follow-up.

## 2026-10-13 · Equipment shortages

- Observation: A shortage resolution must preserve the catalogue's declared relationship rather than accepting arbitrary substitutions.
- Action: Derive shortage rows from planned lines and permit replacement only through the source item's alternatives.
- Check: Shortage review and resolve endpoints are covered by generated contracts and CI.

## 2026-10-12 · First-administrator setup

- Observation: A fresh worktree has no generated NuGet assets, so a direct `--no-restore` build reports infrastructure errors before compiling the change.
- Action: Run a restore/build preflight in every newly-created worktree before diagnosing implementation failures.
- Check: The first local verification command for a new worktree is `dotnet restore Raccord.slnx`, followed by the normal no-restore build.

- Observation: Seeded demo data made it impossible for an AppHost journey to prove first-run setup.
- Action: Keep demo seeding explicit and make the E2E AppHost boot empty; let the shared browser harness create the first administrator once and reuse that setup for later journeys.
- Check: The E2E selection includes a first-run setup path and the boot journey asserts an empty installation rather than a seeded user or project.

- Observation: Npgsql's retrying execution strategy rejects a manually opened EF transaction unless the complete setup unit is executed through that strategy.
- Action: Wrap transactional first-run setup in `CreateExecutionStrategy().ExecuteAsync(...)` so retries preserve the atomic role/user operation.
- Check: The web setup test and the empty-AppHost E2E run must create the first administrator successfully against PostgreSQL.

- Observation: AppHost journeys share a database, so a boot journey cannot safely assert an empty global collection after another journey creates data.
- Action: Assert that the boot page and its API-backed rendering work; reserve empty-state assertions for isolated fixtures.
- Check: E2E journeys remain order-independent and the boot journey does not depend on another journey's database state.
