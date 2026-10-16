Warning: truncated output (original token count: 7845)
Total output lines: 327

# Process ledger

This ledger records repeatable improvements discovered during delivery reviews. Each entry names the observation, the durable action, and the check that prevents regression.

## 2026-10-16 · Release mechanics need a pre-release upgrade rehearsal

- Observation: The repository has no published stable application release yet, so the first release implementation cannot honestly claim an upgrade from a preceding supported release.
- Action: Rehearse the Compose install, upgrade, migration-failure and restore mechanics with images built from the PR base and candidate revisions; label this as a pre-release rehearsal until two supported releases exist.
- Check: CI confirms the upgrade preserves seeded database records and document-volume content, and the release notes do not claim backward compatibility with an unpublished release.

## 2026-10-16 · Restore every module schema from a database dump

- Observation: The first live restore rehearsal dropped only `public`, leaving module schemas behind; `pg_restore` then failed when it tried to recreate an existing schema.
- Action: Drop all application schemas before restoring a full PostgreSQL custom-format dump, then recreate the schema objects and append-only triggers from the dump.
- Check: The release rehearsal restores into a populated database and verifies the preserved customer and project rows, document file, and application readiness.

## 2026-10-16 · Each E2E entry point prepares the static homepage

- Observation: PR CI built `src/public-site/dist/` before E2E and passed, but the separate post-merge engineering E2E job did not; its homepage journey failed with `ERR_FILE_NOT_FOUND` in run [38853228885](https://github.com/juangcarmona/raccord/actions/runs/38853228885).
- Action: Build the public homepage in the engineering E2E job and assert that both CI E2E entry points build the assets before compiling the suite.
- Check: The workflow-routing fixture fails if either workflow removes or moves its homepage build after the E2E compile step; the post-merge engineering E2E run must pass on this fix.

## 2026-10-16 · Documentation and public-site workflow routing

- Observation: OpenSpec-only and public-site-only updates started application, browser, and packaging work that could not exercise the changed files, while the engineering publisher watched every path under `src/`.
- Action: Keep docs/security checks active, skip unrelated app jobs only for non-empty diffs wholly under `openspec/` or `src/public-site/`, and route the public homepage through its own static build and Cloudflare Pages setup.
- Check: Classifier fixtures cover OpenSpec-only, public-site-only, mixed, and empty changes; workflow routing fixtures assert separate public and engineering paths; the post-merge runtime probe ignores docs/public-site-only pushes.

## 2026-10-14 · Project request conversion preserves source context

- Observation: Project creation had no durable intake state, so converting incoming work could lose the requester context and classification decisions.
- Action: Persist request status, source contact, requested dates, project type and team; convert transactionally while retaining the resolved request.
- Check: Domain lifecycle tests cover received, one-way conversion and reasoned decline; the API contract and migration are validated by CI.

## 2026-10-13 · Frontend boundary enforcement

- Observation: The frontend dependency direction was documented in ADR-0023 but could drift because CI did not inspect imports.
- Action: Add a small import-graph checker and fixture tests; run it in the existing frontend CI job without adding layers or dependencies.
- Check: Reject app/features/shared reverse imports and peer-feature imports, allow app composition and feature-to-shared imports, and keep the ADR/rules marked as enforced.

## 2026-10-13 · Architecture documentation evidence

- Observation: Authentication and frontend route protection had shipped, but arc42 still described the system as unauthenticated and omitted the auth feature slice.
- Action: Review architecture sections against source/configuration before each epic handoff; document only implemented boundaries and leave future role permissions as explicit risk/debt.
- Check: Architecture documentation review verifies source paths, runtime authorization behavior, ADR links, Markdown lint and CI evidence before merge.

## 2026-10-13 · Organization-authoritative invoice dates

- Observation: The draft screen derived a legal invoice date from the browser clock even though the API owns the organization-timezone default.
- Action: Keep the date field blank until the user chooses a value and send `null` otherwise, allowing the server to apply its authoritative default.
- Check: Test an explicit date and a browser/org midnight boundary; retain server-side numbering and date-order validation as the final authority.

## 2026-10-13 · Focused route behavior tests

- Observation: Untested route components left loading, empty and mutation regressions invisible despite broad end-to-end coverage.
- Action: Prefer small rendered-UI tests with the existing API stub for high-value route states, while keeping E2E journeys for cross-process integration.
- Check: Each new route behavior suite asserts accessible user-visible outcomes and request payloads, and runs in the existing Vitest CI step.

## 2026-10-13 · Browser identity evidence

- Observation: Product-facing visual changes are hard to verify from a mobile PR view without a durable visual artifact and an explicit responsive/accessibility check.
- Action: Include repository-hosted visual evidence with every UI PR, and test decorative versus informative image semantics in the frontend suite.
- Check: UI PR descriptions link the evidence asset, frontend tests assert the intended accessible name, and CI runs the browser journey or visual verification relevant to the change.

## 2026-10-13 · Mutation and dependency gates

- Observation: The scheduled mutation job failed before running because its working directory was combined with a repository-relative config path; the PR security gate also exposed vulnerable transitive crypto packages despite passing tests.
- Action: Resolve tool configuration paths relative to the declared working directory and keep centrally managed .NET servicing packages current.
- Check: Run the mutation workflow manually after workflow changes and run `dotnet list Raccord.slnx package --vulnerable --include-transitive` during every FF loop.

## 2026-10-13 · Authenticated web session

- Observation: A server session without client restoration leaves protected navigation inconsistent and encourages direct untyped fetch calls.
- Action: Centralize current-user, sign-in and sign-out calls behind the generated API client, guard routes for UX, and keep server authorization authoritative.
- Check: Frontend behavior tests, authenticated browser sign-in/sign-out journey, accurate 401 OpenAPI response, OpenSpec validation and all CI gates are required before merge.

## 2026-10-12 · Epic #16 module architecture

- Observation: A final integration PR exposed inherited commit-message failures that child PRs did not see against their stacked feature branch.
- Action: Run the complete CI-equivalent gate against the eventual target branch before opening or merging the final integration PR, including commitlint over the full target range.
- Check: The final PR's lint job must pass over `main` to head, not only over the immediately preceding stacked branch.

- Observation: Moving the Aspire host changed both endpoint discovery and the relative Vite path.
- Action: Treat host relocation as a runtime change and exercise the AppHost E2E journey before integration.
- Check: The E2E job must boot the AppHost, pass health checks, and complete the selected journeys.

- Observation: Implemented child issues remained open because merged PRs did not consistently carry closing references.
- Action: Re…3845 tokens truncated…le record stream

- **Observation:** Notes, files, folders, fields, tags and saved views all need project scoping and history; separate MVP tables would duplicate ordering and audit behavior.
- **Action:** Persist a typed project-context entry with optional payload fields, append a shared audit event for every addition, and expose one context read model for the project page.
- **Check:** The ProductShape-cited browser journey creates all six record kinds and verifies their order plus six retained history events; hosted success capture supplies screenshot, trace and video evidence.

## 2026-10-15 · Hosted browser setup needs recovery

- **Observation:** A hosted E2E run can remain indefinitely in Playwright browser installation with no log progress, blocking the feedback loop before any journey executes.
- **Action:** Treat an unchanged setup step as runner failure, cancel the run, and retrigger from a small process-ledger update; keep the application merge blocked until a fresh E2E artifact exists.
- **Check:** A restarted run must reach journey execution and upload success evidence before merge.

## 2026-10-15 · Time reporting stays append-only

- **Observation:** Plan-versus-actual reporting becomes misleading if recording actual work mutates the planned source or stores a cached variance.
- **Action:** Store each time record immutably with planned and actual hours, then derive variance and project totals when reporting.
- **Check:** Domain and browser journeys must retain both source values, aggregate multiple entries, and expose the derived variance with hosted evidence.

## 2026-10-15 · Publish browser evidence after main advances

- **Observation:** Waiting for the nightly schedule to publish browser evidence leaves a gap after a change has already passed PR E2E and merged.
- **Action:** Run the public evidence journey on relevant pushes to `main`; use the tested commit SHA to let the nightly schedule skip work already evidenced for that revision.
- **Check:** A new main SHA must trigger journey capture and site publication, while a duplicate push or nightly run for the same SHA must not repeat E2E.

## 2026-10-15 · Scoped API access keeps credentials revocable

- **Observation:** External integrations need stable scope boundaries without exposing reusable plaintext credentials through storage or read APIs.
- **Action:** Hash API keys at rest, return plaintext only on issue or rotation, enforce administrator permission for token management and explicit read/write scopes for integration routes.
- **Check:** The ProductShape-cited journey issues and rotates a token, proves read access and rejected write access, and retains hosted screenshot and trace evidence.

## 2026-10-15 · Personal calendar feeds are private, revocable projections

- **Observation:** A calendar subscription must remain useful to the individual without exposing the project workspace or leaving an unrevocable URL behind.
- **Action:** Store only a hash of each feed token, derive the ICS response from the authenticated user's relevant schedule, and rotate or revoke access through the integration boundary.
- **Check:** The ProductShape-cited journey downloads a private ICS feed, verifies the relevant event, proves an old token is rejected after rotation, and links hosted browser evidence.

## 2026-10-15 · External providers fail closed

- **Observation:** Registry and payment integrations cross an external boundary where an unavailable provider or duplicated callback must not create false business state.
- **Action:** Return an empty registry result when no provider is configured, validate payment scope and payloads at the API edge, and enforce idempotency at persistence boundaries.
- **Check:** ProductShape-cited journeys prove safe no-match behavior, scoped payment reporting, duplicate rejection, and hosted evidence for both integration paths.

## 2026-10-15 · Payment reporting reuses legal invoice state

- **Observation:** External payment notifications must not invent a second invoice or payment model.
- **Action:** Gate invoice reads and payment writes with explicit API scopes and delegate recording to the existing invoicing workflow, preserving its idempotency and validation rules.
- **Check:** The ProductShape-cited payment journey proves the scoped invoice read and payment path, with hosted screenshot and trace evidence.

## 2026-10-15 · Inbound e-invoice status is an auditable local boundary

- **Observation:** Inbound e-invoice protocol and legal exchange requirements are not selected yet, so pretending to implement a public-platform connector would create false compliance confidence.
- **Action:** Preserve the received XML and content hash locally, expose explicit pending/accepted/paid transitions, and leave external exchange behind a future adapter boundary.
- **Check:** The ProductShape-cited journey receives a UBL document, verifies acceptance and payment transitions, lists the final state, and links hosted browser evidence.

## 2026-10-15 · Structured exports derive from immutable invoices

- **Observation:** A structured e-invoice must be a reproducible legal projection, not a second editable invoice representation.
- **Action:** Build the supported UBL document from the issued invoice aggregate, validate the required party and amount fields, and record the export hash in the audit trail.
- **Check:** The ProductShape-cited journey parses the UBL namespace, parties, lines and payable amount, captures hosted evidence, and leaves no pending model changes.

## 2026-10-16 · Security findings become executable response policy

- **Observation:** A ZAP baseline report exposed missing browser security headers on both the SPA shell and static assets.
- **Action:** Apply the response policy at the web pipeline boundary so every response receives the same CSP, cross-origin, permissions, referrer and MIME protections.
- **Check:** Web tests assert the complete header contract; the hosted security scan must show the baseline findings resolved before merge.

## 2026-10-16 · Browser policy and production bundles are checked together

- **Observation:** The Aspire browser drives Vite in development, while the production SPA and Content-Security-Policy are served by the web resource; a source-map URL in the production bundle also exposes source metadata.
- **Action:** Assert the policy on the web resource in the browser journey, then continue the existing SPA workflow; restrict styles and connections to same-origin and disable production source maps when neither is required.
- **Check:** The frontend build must publish no source maps, the hosted E2E artifact must show the seeded SPA journey passing, web hosting tests must pin the full CSP, and scanner request-metadata notices must not be mistaken for missing response headers.

## 2026-10-16 · Technical VAT policy does not substitute for accountant review

- **Observation:** Public Spanish and EU sources specify the required invoice data without prescribing line-level VAT rounding, while issue #60 explicitly requires an accountant to review the practical policy.
- **Action:** Record the per-rate algorithm and its legal-source limits as technical policy, without presenting research or implementation as accountant approval.
- **Check:** ADR-0006, BR-VAT-PER-RATE and the disputed-cent domain test agree; #60 stays open until an accountant's confirmation or amendment is recorded, and #24 stays open until every child is complete.

## 2026-10-16 · Full-tree formatting includes project files

- **Observation:** A focused CSharpier check on the new browser test passed locally, but hosted full-repository CSharpier caught the test project file's long content include.
- **Action:** Format every changed C# and project file, then run `dotnet csharpier check .` before pushing instead of checking only the edited C# source.
- **Check:** The complete repository formatter check exits successfully on the same tree that CI will verify.
