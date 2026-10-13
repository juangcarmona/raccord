# Process ledger

This ledger records repeatable improvements discovered during delivery reviews. Each entry names the observation, the durable action, and the check that prevents regression.

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

## Review rule

Every completed FF loop must review this ledger, add new observations and actions when useful, and verify that existing actions were followed. The ledger is process memory for future agents; it is not a substitute for CI or product documentation.
