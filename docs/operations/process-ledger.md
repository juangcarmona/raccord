# Process ledger

This ledger records repeatable improvements discovered during delivery reviews. Each entry names the observation, the durable action, and the check that prevents regression.

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
