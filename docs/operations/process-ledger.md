# Process ledger

This ledger records repeatable improvements discovered during delivery reviews. Each entry names the observation, the durable action, and the check that prevents regression.

## 2026-10-16 · Release mechanics need a pre-release upgrade rehearsal

- Observation: The repository has no published stable application release yet, so the first release implementation cannot honestly claim an upgrade from a preceding supported release.
- Action: Rehearse the Compose install, upgrade, migration-failure and restore mechanics with images built from the PR base and candidate revisions; label this as a pre-release rehearsal until two supported releases exist.
- Check: CI confirms the upgrade and restore preserve customer, project, issued-invoice and audit-event rows plus document-volume content; release notes do not claim compatibility with an unpublished release.

## 2026-10-16 · Restore every module schema from a database dump

- Observation: The first live restore rehearsal dropped only `public`, leaving module schemas behind; `pg_restore` then failed when it tried to recreate an existing schema.
- Action: Drop all application schemas before restoring a full PostgreSQL custom-format dump, then recreate the schema objects and append-only triggers from the dump.
- Check: The release rehearsal restores into a populated database and verifies the customer, project, issued-invoice and audit-event rows, document file, and application readiness.

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

## 2026-10-13 · Public static evidence publication

- Observation: Static-site deployments replace the whole Pages artifact; independent documentation and E2E publishers could silently erase each other's content or lose reports during overlapping runs.
- Action: Route all static snapshots through one serialized publisher, restore retained files from the published manifest, and test source-path refreshes separately from nightly report updates.
- Check: The workflow has one non-cancelling concurrency group; report tests cover the stable index and retention, and snapshot refreshes preserve unrelated site paths.

## Review rule

## 2026-10-15 · Workday events preserve audit identity

- **Observation:** Clock-in/out records can support regulated working-day review only when event time, recording time and actor remain distinct and immutable.
- **Action:** Store append-only typed events with server recording timestamps, actor identity and a bounded future-time guard; leave jurisdiction-specific legal policy as an explicit follow-up.
- **Check:** The workday journey cites the ProductShape Given/When/Then, verifies both event kinds and audit fields, stays in the per-PR manifest, and links hosted screenshot evidence.

## 2026-10-15 · Payroll exports contain only approved hours

- **Observation:** Registered time is not payroll data until it has been reviewed; exporting pending registrations would turn an operational draft into a financial commitment.
- **Action:** Persist source and approval status separately, make approval explicit, and generate deterministic CSV only from approved registrations in the requested period.
- **Check:** The payroll journey cites the ProductShape Given/When/Then, asserts pending exclusion and approved inclusion, stays in the per-PR manifest, and links hosted screenshot evidence.

## 2026-10-15 · Planning assignments are typed by function kind

- **Observation:** Crew and vehicle planning share a project-function boundary but have different valid resources; accepting either resource for every function hides staffing conflicts.
- **Action:** Persist functions, shifts and assignments separately, enforce kind-specific assignment rules and uniqueness, and calculate staffing from the assigned resource type.
- **Check:** The planning journey cites the ProductShape Given/When/Then, verifies crew and transport staffing, stays in the per-PR manifest, and links hosted screenshot evidence before merge.

## 2026-10-15 · Crew availability keeps planning responses durable

- **Observation:** Availability, invitations and job-board signups are separate responses to the same planning function and need independent status and duplicate constraints.
- **Action:** Persist each response as a typed record, keep invitation and signup uniqueness at the database boundary, and exercise the complete journey with ProductShape citations and hosted visual evidence.
- **Check:** The availability journey remains in the per-PR manifest, invalid date ranges are rejected, duplicate participation is rejected, and the hosted artifact is linked before merge.

## 2026-10-15 · Crew records keep planning inputs explicit

- **Observation:** Crew, vehicle and rate data are planning inputs with different validation and defaulting rules; treating them as untyped settings would allow invalid identities, capacity and cost bands into later scheduling flows.
- **Action:** Keep typed contracts and a focused catalog with unique identities, positive vehicle capacity, non-negative ordered rate bands and one default rate per kind. Cover the complete save/list journey with a ProductShape citation and hosted screenshot artifact.
- **Check:** The crew E2E journey remains in the PR selection manifest, API and migration checks pass, and the PR links the hosted Playwright artifact.

## 2026-10-15 · Static evidence uses viewer-local time and test-result truth

- **Observation:** Engineering snapshot timestamps rendered as raw UTC and a green workflow status could imply browser journeys passed even when no journey result had been recorded.
- **Action:** Retain the ISO instant in machine-readable markup, format it with the viewer's browser locale/time zone, and display journey totals and explicit no-results state from the retained report manifest. Share the snapshot appearance preference across pages.
- **Check:** Static-site tests cover generated local-time hooks, shared theme persistence, responsive breakpoints and the successful-workflow/zero-journey edge case; OpenSpec validation and the PR workflow must pass before merge.

## 2026-10-14 · Product behaviors drive E2E journey contracts

- **Observation:** Product structure behaviors already provide the canonical user-facing Given/When/Then semantics.
- **Action:** Every new browser journey must reference its ProductShape behavior ID in the journey docstring and preserve those Given/When/Then steps; API-only slices use domain/integration and contract tests until a browser surface exists.
- **Check:** PR review verifies the behavior citation, the three modeled steps, and the journey’s entry in the E2E selection manifest when applicable.

Every completed FF loop must review this ledger, add new observations and actions when useful, and verify that existing actions were followed. The ledger is process memory for future agents; it is not a substitute for CI or product documentation.

## 2026-10-15 · Standalone and duplicated invoices

- Observation: Making project linkage optional affects the draft aggregate, immutable invoice representation, indexed persistence rows, generated API types, and existing draft UI assumptions; an API-only change leaves nullable values unsafe in the frontend.
- Action: Model nullable project identity end-to-end, regenerate the OpenAPI client, and add explicit standalone/duplicate assertions to both the integration slice and the existing Playwright money journey.
- Check: The migration, repository-wide CSharpier check, frontend typecheck/tests, PostgreSQL-backed integration test, and Playwright trace/screenshot artifact all pass before merge.

## 2026-10-15 · Generated API contracts stay outside source lint

- Observation: Generated API schema files are intentionally ignored by ESLint, but lint-staged still passed them to ESLint and failed every contract-only commit.
- Action: Filter generated API schemas from the source lint-staged task while retaining the repository-wide frontend lint and generated-contract checks in CI.
- Check: A staged schema change must pass the commit hook, and `npm run lint` plus API generation remain required in CI.

## 2026-10-15 · Queue eligibility stays derived from billable state

- Observation: A staged-invoicing queue can drift if it stores a second copy of invoice eligibility.
- Action: Derive queue entries from project billable state, with an explicit date window and overdue flag.
- Check: The queue endpoint and browser journey assert that newly created work appears before draft creation.

## 2026-10-15 · Payments and refundable deposits

- Observation: Payment status is derived data and must never be stored independently from the issued invoice total and payment history; deposit lifecycle has a separate security-liability meaning.
- Action: Keep payments append-only, reject overpayment before persistence, derive unpaid/partially-paid/paid from the aggregate, and model deposits with explicit not-paid/paid/returned transitions.
- Check: Integration coverage settles an invoice through partial then full payment, while the Playwright journey exercises deposit configuration, receipt, return, and payment APIs with hosted evidence.

## 2026-10-15 · Reminder history owns the next due date

- Observation: Overdue status must reflect the latest reminder deadline and current outstanding balance, not a cached flag.
- Action: Derive overdue invoices from issued totals, recorded payments and the latest reminder; keep each generated reminder append-only and auditable.
- Check: Integration and browser journeys list an unpaid overdue invoice, create reminder one with a new deadline, and read the retained history before settlement.

## 2026-10-15 · Delivery journeys must click the product surface

- **Observation:** The delivery stack had API-level document tests, but the invoice download control had no browser journey, so a green API check could still leave the user journey unverified.
- **Action:** Add one smoke journey per user-visible delivery path that follows the ProductShape journey and activates the real accessible control; keep generated screenshots, trace and video in the CI artifact rather than source control.
- **Check:** The journey is listed in `per-pr-selection.txt`, asserts a usable non-empty download, and the PR links the exact hosted evidence artifact.

- **Observation:** The first integration workflow uploaded Playwright output only after a failure and did not enable success capture, leaving successful UI behavior without mobile-reviewable evidence.
- **Action:** Enable success capture for the blocking E2E run and upload its artifacts with `always()`; retain a short artifact lifetime so evidence remains cheap and attached to the exact CI run.
- **Check:** Every successful UI integration run exposes the screenshot, trace and video artifact before merge.

## 2026-10-14 · Project task board progress is status-driven

- **Observation:** A shared board needs one source of truth for task ordering and completion semantics.
- **Action:** Store status completion rules with the task status and recalculate project progress in the coordination service after every task transition.
- **Check:** Domain and CI tests must cover status validation, task movement, and the resulting progress percentage.

## 2026-10-15 · Finance reports stay derived from legal records

- **Observation:** Accountant views can become contradictory if reports, ledgers and exports each persist their own totals.
- **Action:** Derive the finance report and CSV ledger from immutable issued invoices, append-only payments and reminder deadlines; keep one shared period normalization rule.
- **Check:** Integration and browser journeys must compare report totals, ledger entries and export headers for the same period, with successful hosted Playwright evidence linked from the PR.

## 2026-10-15 · Physical stock corrections preserve booked equipment

- **Observation:** A stock count can otherwise overwrite equipment already committed to a project and make the availability figure physically impossible.
- **Action:** Reconcile counts against booked quantity, record every correction with a reason, and keep planned movements as append-only history.
- **Check:** Domain, integration and browser coverage must exercise movement scheduling, count history, and the booked-quantity guard.

## 2026-10-15 · Equipment lifecycle keeps history after disposition

- **Observation:** Selling or retiring an item without preserving its project usage and stock history makes asset audit and traceability impossible.
- **Action:** Model terminal sold/retired statuses, block disposition while booked out, and derive item insights and project history from existing movement, count and packing records.
- **Check:** Integration and warehouse browser journeys must assert sold/retired status, project history and insight counts with hosted evidence.

## 2026-10-15 · Branded templates keep preview output safe

- **Observation:** Template configuration combines reusable user text, styling and image references; previewing raw content would turn administrator input into executable markup.
- **Action:** Persist versioned template settings by document type, keep one default per type, and HTML-encode all preview fields while retaining the logo as a storage key.
- **Check:** Integration and browser journeys must save a branded template, preview it, verify styling and confirm script content is not executable.

## 2026-10-15 · Generated documents expose printable outputs

- **Observation:** A document configuration is not useful to operators until project and warehouse data can be turned into a concrete printable artifact.
- **Action:** Keep generation endpoints derived from persisted project/equipment records, return stable filenames and content types, and HTML-encode all printable fields.
- **Check:** Integration and browser journeys must generate a project summary or equipment label and assert identifiers are present in the output.

## 2026-10-15 · Delivery outcomes remain searchable and append-only

- **Observation:** Sending a document without a durable outcome and searchable failure reason leaves operators unable to investigate delivery problems.
- **Action:** Record pending, sent and failed communication outcomes with recipient, subject, document key and failure reason; expose a filtered communication log.
- **Check:** Integration and browser journeys must create a communication, record a failed delivery, and find it through the searchable log.

## 2026-10-15 · Localized templates keep language selection explicit

- **Observation:** Customer documents can share a document type while requiring independent wording, branding and default selection per locale.
- **Action:** Store a normalized language tag on each template, scope uniqueness and defaults by document type plus language, and expose a language-filtered catalog.
- **Check:** Integration and browser journeys must save a foreign-language variant, select it by locale, preview its translated content, and prove the default in another locale remains intact.

## 2026-10-15 · Project context uses one auditable record stream

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

## 2026-10-16 · Rehearsal migration failures must reach the application

- **Observation:** The deliberate Alpine failure image had no application entrypoint, so Docker failed before the migration command ran and the rehearsal did not cover migration recovery.
- **Action:** Use the candidate application image with a read-only test database to make EF migration writes fail, then restore database writability before testing recovery.
- **Check:** The rehearsal confirms the migration process exits unsuccessfully, the existing web container remains running, and restore returns database, documents and readiness to the backed-up state.

## 2026-10-16 · Restore waits for application readiness

- **Observation:** The restore rehearsal started the web container and probed readiness immediately, before the application had finished starting.
- **Action:** Make the restore command wait for the Compose health check before reporting completion.
- **Check:** The rehearsal confirms the restored application is ready and retains its database records and document files before cleanup.

## 2026-10-16 · Architecture diagrams need generated evidence and source checks

- **Observation:** The arc42 page already renders sanitized SVGs, but the pull-request job tested only helper scripts and reviewers had no artifact covering diagram layout or accessibility names.
- **Action:** Keep Markdown diagrams canonical, follow the [arc42 diagram pull-request checklist](../architecture/arc42/diagrams/visual-language.md), require accessible titles and source evidence, render the complete static snapshot in the focused PR job, and attach the HTML plus diagram screenshots; use a human visual review instead of a fragile pixel baseline. Agent-assisted drafts must pin a clean source revision, carry evidence to each claimed node, pass a renderer's artifact and browser checks, receive visual review, and be transferred into reviewed Mermaid only after each relationship is verified.
- **Check:** Broken diagram source or a missing title fails validation, and the PR artifact contains rendered sections 03, 05, 06, 07 and 10 without invoking the application E2E pipeline.

## 2026-10-16 · Applied Product Changes refresh citations across consumers

- **Observation:** Applying an approved Product Change updates artifact digests and makes citations in existing OpenSpec documents stale.
- **Action:** Regenerate citations for every changed artifact across all consumers and run citation verification with the pinned OpenSpec CLI available on PATH before opening the PR.
- **Check:** ProductShape validation passes, OpenSpec validation passes, and citation verification reports zero stale, tampered or unresolved citations.

## 2026-10-16 · Container-backed feature checks preflight the local runtime

- **Observation:** The profile API test and browser journey compiled, but local execution stopped before application startup because the Docker daemon socket was unavailable.
- **Action:** Check Docker availability before starting container-backed tests; continue compile, contract and frontend checks, then rely on a fresh hosted E2E artifact for runtime approval when the local daemon is unavailable.
- **Check:** The PR records the local runtime limitation and CI must pass the selected AppHost journey with captured browser evidence before the feature is considered done.

## 2026-10-16 · Staged frontend formatting receives its file list

- **Observation:** The #290 commit hook failed because the TypeScript/TSX staged-file handler ran `prettier --write` without the staged paths, after ESLint had succeeded.
- **Action:** Build the Prettier command from the same filtered staged-file list used by ESLint in `src/Raccord.Frontend/.lintstagedrc.mjs`.
- **Check:** Commit a staged TypeScript change and confirm the hook formats the named files and reaches commitlint without a parser or missing-file error.
## 2026-10-16 · API tests select a persisted user in the test auth scheme

- **Observation:** The WebApplicationFactory test host uses a fake authentication scheme with a dummy user ID, so real Identity sign-in is not configured there and the current-user endpoint could not resolve the seeded administrator.
- **Action:** Let the test-only authentication handler take a user ID from an explicit test header; use the seeded administrator's ID when exercising profile routes.
- **Check:** The API test verifies profile persistence and that submitted email data cannot change the account's sign-in address; production authentication remains on the Identity cookie scheme.

## 2026-10-16 · Browser journeys normalize the signed-in profile language

- **Observation:** Pinning Playwright's browser locale did not control the authenticated UI language because the seeded administrator's saved profile preference takes precedence, and sign-in resumes the previously requested protected route.
- **Action:** Normalize the demo account's language after each shared journey sign-in, and have the account journey change it and verify persistence after reload and return to the account route on sign-in.
- **Check:** The hosted AppHost run completes the warehouse customer form with English labels and captures the account journey in Spanish after reauthentication.
