# Process ledger

This ledger records repeatable improvements discovered during delivery reviews. Each entry names the observation, the durable action, and the check that prevents regression.

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

## 2026-10-13 · Epic #17 continuation

- Observation: Starting the next epic before reconciling all children of the current epic caused avoidable context switching and uncommitted work in the wrong feature worktree.
- Action: At the start of every FF loop, query the epic and all child issue states, then complete the dependency-ordered child list before selecting a new epic. Keep exploratory work isolated and uncommitted until the current epic is reconciled.
- Check: The loop update names the active epic, remaining children, and target base branch; no new-epic PR is opened while a prior epic has open implementation children.

- Observation: Role authorization is security-sensitive and stale cookie claims can mask assignment changes.
- Action: Resolve role permissions from current Identity role claims at authorization time and update the security stamp on assignment changes; cover both permission denial and post-assignment behavior in CI.
- Check: Web authorization tests and the role-management E2E journey run on every PR that changes the permission catalog or role APIs.

- Observation: Invitation joins initially bypassed the PostgreSQL retry execution strategy, producing a runtime failure only in the container-backed test environment.
- Action: Any new Identity or Workspace transaction must execute through `CreateExecutionStrategy().ExecuteAsync(...)`, and the lifecycle test must run against PostgreSQL.
- Check: The .NET CI test job includes the invitation lifecycle and future settings persistence tests before merge.

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
