# Engineering lifecycle

**Project**: `juangcarmona/raccord` **Adopted**: 2026-10-12 · **Last amended**: 2026-10-12

This is raccord's lifecycle contract for the agentic-sdlc capabilities (`refine`, `propose`, `implement`, `integrate`, `review`, `status`). Its section headings are an interface: add sections freely, but never rename or delete one. Repository conventions stay in `AGENTS.md`. Product intent stays in `docs/product/`. Decisions stay in `docs/adr/`.

## Definition of Ready

Governs the **Ready** gate at the end of the `refine` capability: _is the problem sufficiently defined?_ An agent verifies it semantically and alerts a human when a dimension is missing, vague or contradictory, rather than refusing outright.

An item is ready when every applicable dimension is present. Inapplicable ones are marked `N/A` with a one-line reason.

| #   | Dimension            | This project requires                                                                                                                                                                                                         |
| --- | -------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| 1   | Title                | Kept as written: the title states the desired outcome, not the activity.                                                                                                                                                      |
| 2   | Description          | Kept as written: the intent, the problem and the relevant context.                                                                                                                                                            |
| 3   | Actor / stakeholder  | Adapted: user-facing items name the product-model actor or persona (`docs/product/model/actors/`). `N/A` for tooling, CI and infrastructure items.                                                                            |
| 4   | Priority             | Dropped: a single maintainer orders the backlog by hand, so there is no formal priority field.                                                                                                                                |
| 5   | Dependencies         | Kept as written: known dependencies are listed, or explicitly "none".                                                                                                                                                         |
| 6   | Acceptance criteria  | Kept as written: observable, verifiable conditions, under an `## Acceptance criteria` heading in the issue.                                                                                                                   |
| 7   | Scope                | Kept, always with an explicit out-of-scope list.                                                                                                                                                                              |
| 8   | Product rules        | Adapted: cite the product-model artifact ids involved (`UC-`, `BR-`, `FR-`, `JRN-`, `QR-`, `CON-`). New product scope needs an approved Product Change in `docs/product/changes/` first; an item never invents product scope. |
| 9   | Affected behaviour   | When applicable: the existing use cases, journeys and API endpoints the change may alter.                                                                                                                                     |
| 10  | Quality expectations | Adapted: state the relevant product-model quality requirements. Always state them for money, tax, numbering, finalization, audit and authentication.                                                                          |
| 11  | Test impact          | When applicable: new or affected end-to-end journeys and their entry in `tests/Raccord.EndToEndTests/per-pr-selection.txt`. The test plan itself belongs to `propose`.                                                        |
| 12  | Existing decisions   | Kept: link the relevant ADRs (`docs/adr/`), OpenSpec specs and changes (`openspec/`) and product-model artifacts. Never ask for a new decision at this gate.                                                                  |
| 13  | Unknowns / risks     | Adapted: recording the uncertainty is enough, except for legal or tax-compliance unknowns, which must be resolved or escalated to the Owner before planning.                                                                  |

### Additional to this project

| Dimension | This project requires                                                                                                                                                                                     |
| --------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Red lines | The item states whether it touches credentials, shared git history or anything production-facing. If it does, it names the human decision needed, because `AGENTS.md` requires stopping and asking first. |

## Definition of Done

Governs the **Done** gate at the end of the `implement` capability: _is the resulting change sufficiently verified?_ The `integrate` capability refuses to merge a change that has not passed it.

**In this project, "done" means**: finished and proven, ready to integrate. Deployment is not part of Done.

Lane is `deterministic` (an agent or CI settles it), `judgement` (a named role decides), or `both`. Judgement items name a role from the **Roles** section.

| #   | Dimension                         | This project requires                                                                                                                                                                                                                                                                                                     | Lane          | Role  |
| --- | --------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ------------- | ----- |
| 1   | Acceptance criteria               | Every criterion maps to a passing test or to recorded runtime evidence. The Owner confirms that each mapping really demonstrates the criterion.                                                                                                                                                                           | both          | Owner |
| 2   | Implementation completeness       | Every task in the OpenSpec change's `tasks.md` is ticked or dropped with a recorded reason. No TODO placeholders, no dead code, no silent scope loss.                                                                                                                                                                     | both          | Owner |
| 3   | Tests                             | The CI suites are green. New behaviour starts with a failing test (TDD). Money, tax, numbering, finalization and audit never change without a test. Tests follow `Thing_Should_Outcome` naming with Shouldly. The Owner judges whether the tests cover the risk.                                                          | both          | Owner |
| 4   | Quality checks                    | CSharpier, analyzers with warnings as errors, Prettier, ESLint, TypeScript typecheck, commitlint, the pending EF Core model check and the OpenAPI contract-drift check all pass.                                                                                                                                          | deterministic | none  |
| 5   | Security                          | TruffleHog, Semgrep, Trivy (filesystem and image), `npm audit`, the .NET vulnerable-package check and the license policy pass, and the SBOM is produced. The Owner judges exposure for any change to authentication, authorization, personal data or network surface. The post-merge ZAP baseline runs after integration. | both          | Owner |
| 6   | Architecture / design conformance | Architecture tests and analyzers pass. The Owner confirms the change matches the plan approved at the Planned gate and the accepted ADRs, and that deviations are recorded rather than silent.                                                                                                                            | both          | Owner |
| 7   | Documentation                     | When behaviour or structure changes, the affected arc42 sections (`docs/architecture/`), ADRs (`docs/adr/`) and operations docs (`docs/operations/`) are updated in the same pull request.                                                                                                                                | both          | Owner |
| 8   | Product / spec consistency        | `prodshape validate`, `openspec validate --all` and `prodshape citations verify --provider openspec` pass: every current OpenSpec document is bound to the product model or exempt with an approved reason. The Owner confirms the Product Change, the OpenSpec change and the code agree.                                | both          | Owner |
| 9   | Evidence                          | A green `CI` run for the head commit is linked from the pull request. UI changes attach screenshots or a runtime capture.                                                                                                                                                                                                 | deterministic | none  |
| 10  | Human review                      | The Owner approves the pull request. The `main` ruleset requires 0 approvals, so this is a process rule that `integrate` enforces, not a forge setting.                                                                                                                                                                   | judgement     | Owner |
| 11  | Known limitations                 | What is deliberately not addressed is listed in the pull request description, each with a follow-up issue.                                                                                                                                                                                                                | both          | Owner |

### Additional to this project

| Dimension | This project requires     | Lane | Role |
| --------- | ------------------------- | ---- | ---- |
| None      | No additions at adoption. | -    | -    |

### Deterministic checks

`verify-like-ci` derives the commands from `.github/workflows/ci.yml`. This list records what the team expects to see, so that a check disappearing silently gets noticed.

| Check                 | How it runs                                                                                                                                                       | Evidence it produces                  |
| --------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------- | ------------------------------------- |
| Format (C#)           | `dotnet tool restore && dotnet csharpier check .`                                                                                                                 | `backend` and `lint` jobs             |
| Build and analyzers   | `dotnet build Raccord.slnx -c Release`                                                                                                                            | `backend` job                         |
| Backend tests         | `dotnet test Raccord.slnx -c Release --no-build --filter "FullyQualifiedName!~Raccord.EndToEndTests"`                                                             | `backend` job                         |
| Vulnerable packages   | `dotnet list Raccord.slnx package --vulnerable --include-transitive`                                                                                              | `backend` job                         |
| Pending model changes | `dotnet ef migrations has-pending-model-changes` (per DbContext project)                                                                                          | `backend` job                         |
| Format and commits    | `npx prettier --check "**/*.{ts,tsx,json,md,yml,yaml,css}"` and commitlint over the PR range (from `src/Raccord.Frontend`)                                        | `lint` job                            |
| OpenSpec              | `npx -y @fission-ai/openspec@1.12.0 validate --all --no-interactive`                                                                                              | `openspec` job                        |
| Product model         | `npx -y @prodshape/cli@0.22.0 validate`, plus `change validate <id>` per active Product Change                                                                    | `product` job                         |
| Product grounding     | `npx -y @prodshape/cli@0.22.0 citations verify --provider openspec` (needs the pinned OpenSpec CLI on PATH)                                                       | `product` job                         |
| End-to-end journeys   | `bash .github/scripts/check-e2e-selection.sh`, then the Playwright journeys over the Aspire AppHost                                                               | `e2e` job; artifacts on failure       |
| Frontend              | `npm ci && npm run format:check && npm run lint && npm run typecheck && npm test && npm run build && npm audit --audit-level=high`, then the contract-drift check | `frontend` job                        |
| Security              | TruffleHog, Semgrep, Trivy (filesystem)                                                                                                                           | `security` job                        |
| SBOM and licenses     | Trivy (image), Syft SBOM, `dotnet run --project tools/dependency-licenses/dependency-licenses.csproj -- --check`                                                  | `sbom` job; `sbom.spdx.json` artifact |
| Deployment artifacts  | `docker build -f deploy/docker/Dockerfile .`, `docker compose config`, Bicep build and lint                                                                       | `deployment` job                      |
| Mutation testing      | Stryker over the domain, nightly                                                                                                                                  | `mutation.yml`, not a per-PR gate     |
| Runtime probing       | ZAP baseline against the Compose stack, after merge to `main`                                                                                                     | `post-merge.yml`, not a per-PR gate   |

## Lifecycle stages and states

The lifecycle state is the single `status:*` label on a GitHub issue in `juangcarmona/raccord`, not GitHub's open/closed state. The labels were created and verified on 2026-10-12. An issue with no `status:` label counts as `status:backlog`.

| Stage       | State on entry                                                 | State on exit               | Transition                                                                                 | Fired by               |
| ----------- | -------------------------------------------------------------- | --------------------------- | ------------------------------------------------------------------------------------------ | ---------------------- |
| `refine`    | `status:backlog` (or no label)                                 | `status:ready`              | label `status:backlog` → `status:ready`                                                    | agent, human-confirmed |
| `propose`   | `status:ready`                                                 | `status:proposed`           | label `status:ready` → `status:proposed`                                                   | `propose`              |
| `implement` | `status:proposed` (plan approved by the Owner in the draft PR) | `status:in-review`          | label `status:proposed` → `status:in-review`                                               | `implement`            |
| `integrate` | `status:in-review`                                             | `status:done`, issue closed | label `status:in-review` → `status:done`; the merge closes the issue through `Closes #<n>` | `integrate`            |
| `review`    | `status:done`                                                  | `status:done`               | -                                                                                          | -                      |

### Transitions no capability ever fires

| Transition                     | Meaning                                                   | Who fires it |
| ------------------------------ | --------------------------------------------------------- | ------------ |
| Add or remove `status:blocked` | The item cannot progress until something external changes | human only   |
| Close as not planned           | The item is rejected or deprioritised                     | human only   |
| Reopen                         | A closed item comes back                                  | human only   |

## Work item fields

GitHub issues have no transition screens. Stages write to these fields:

| Field             | Identifier                                    | Required by                     | Notes                                                                                       |
| ----------------- | --------------------------------------------- | ------------------------------- | ------------------------------------------------------------------------------------------- |
| Title             | issue `title`                                 | `create-work-item`              | States the outcome (Definition of Ready, dimension 1)                                       |
| Body              | issue `body`                                  | `refine`                        | Markdown. Holds the Definition of Ready content, with `## Acceptance criteria` as a heading |
| Lifecycle state   | one `status:*` label                          | every transition                | Exactly one at a time                                                                       |
| Type              | label `bug`, `enhancement` or `documentation` | `create-work-item`              | GitHub issue types are not enabled on this repository                                       |
| Pull request link | `Closes #<n>` in the pull request body        | `propose` (`open-pull-request`) | Links the PR to the issue and closes the issue on merge                                     |

## Work in progress

**WIP cap**: Not used. **Query used at pickup**: Not used. **Sprint commitment**: Not used. **Board**: Not used. The project has a single maintainer and no GitHub Projects board.

## Roles

| Role  | Who                              | Decides                                                                                                                                          |
| ----- | -------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------ |
| Owner | Juan G. Carmona (`juangcarmona`) | Every judgement item of the Definition of Done (1, 2, 3, 5, 6, 7, 8, 10, 11), plan approval at the Planned gate, and every human-only transition |

## Installed roles

Role capabilities that `adopt` installed into `.agents/skills/`, which is the source this project owns. The role skills' copies in `.claude/skills/` are byte-identical and committed. Edit the source, then re-copy; never edit a copy. Every role was adapted from the template reference named below. Its workflow steps were written for this project around that reference's commands, and `merge-pull-request` departs from its squash-only reference by using merge commits.

| Role skill             | Adapted from                                            | Installed  | Projected to       |
| ---------------------- | ------------------------------------------------------- | ---------- | ------------------ |
| `read-work-item`       | `github-issues`                                         | 2026-10-12 | Codex, Claude Code |
| `read-work-item-links` | `github-issues`                                         | 2026-10-12 | Codex, Claude Code |
| `create-work-item`     | `github-issues`                                         | 2026-10-12 | Codex, Claude Code |
| `update-work-item`     | `github-issues`                                         | 2026-10-12 | Codex, Claude Code |
| `comment-work-item`    | `github-issues`                                         | 2026-10-12 | Codex, Claude Code |
| `transition-work-item` | `github-issues` (labels path)                           | 2026-10-12 | Codex, Claude Code |
| `create-branch`        | `convention`: `<type>/<issue>-<slug>`, commitlint types | 2026-10-12 | Codex, Claude Code |
| `checkout-branch`      | `convention`                                            | 2026-10-12 | Codex, Claude Code |
| `open-pull-request`    | `github`                                                | 2026-10-12 | Codex, Claude Code |
| `update-pull-request`  | `github`                                                | 2026-10-12 | Codex, Claude Code |
| `inspect-pull-request` | `github`                                                | 2026-10-12 | Codex, Claude Code |
| `inspect-ci-result`    | `github`                                                | 2026-10-12 | Codex, Claude Code |
| `merge-pull-request`   | `github`, adapted to merge commits                      | 2026-10-12 | Codex, Claude Code |

Not installed, by decision:

- `read-work-item-attachments`: issues carry their content inline.
- `discover-related-docs`, `read-kb-page`, `search-kb`, `sync-doc-to-kb` and `classify-doc-drift`: documentation lives in the repository, with no separate wiki or knowledge base.
- `commit-to-sprint` and `wip-query`: sprints and WIP limits are not used.
- The plan-artifact contracts are provided by OpenSpec (see **Provided roles**).

The lifecycle package itself is not vendored for Claude Code: `.claude/settings.json` registers the `agent-toolkit` marketplace (`juangcarmona/agent-toolkit`) and enables the `agentic-sdlc` plugin, so Claude Code offers to install it when the project is opened. The plugin brings the seven stage commands (for example `/agentic-sdlc:refine`), the `done-gate-auditor` and `flow-reporter` agents, the stage skills, and the `adr`, `rebase-safely` and `git-worktrees` skills `integrate` and `propose` call. Codex has no plugin mechanism, so the same skills are committed under `.agents/skills/`, installed with the skills CLI and locked in `skills-lock.json`: `npx skills add juangcarmona/agent-toolkit/packages/agentic-sdlc -a codex --copy`, and `npx skills add juangcarmona/agent-toolkit --skill adr --skill rebase-safely --skill git-worktrees -a codex --copy`. Agents are a Claude Code feature: with Codex, run `done-gate-auditor`'s instructions as a prompt in a fresh session, since the Done gate depends on the audit not being done by the session that wrote the change.

## Provided roles

OpenSpec 1.12.0 generates these skills; `openspec init --tools claude,codex` was run on 2026-10-12. Names resolved from the generated files.

| Contract         | Provided by                       | Skill to invoke           |
| ---------------- | --------------------------------- | ------------------------- |
| `propose-change` | OpenSpec 1.12.0 (`openspec init`) | `openspec-propose`        |
| `apply-change`   | OpenSpec 1.12.0 (`openspec init`) | `openspec-apply-change`   |
| `sync-specs`     | OpenSpec 1.12.0 (`openspec init`) | `openspec-sync-specs`     |
| `archive-change` | OpenSpec 1.12.0 (`openspec init`) | `openspec-archive-change` |

## Amendment log

| Date       | Section                                      | Change                                                                                                                                                                                                   | Why                                                                                              |
| ---------- | -------------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------ |
| 2026-10-12 | All                                          | Bootstrapped by `adopt`                                                                                                                                                                                  | Onboarding onto agentic-sdlc                                                                     |
| 2026-10-12 | Definition of Done (8), Deterministic checks | Added the OpenSpec citation gate and moved ProductShape to 0.22.0                                                                                                                                        | ProductShape's OpenSpec integration was adopted, so product grounding became checkable in CI     |
| 2026-10-12 | Installed roles                              | Replaced the APM install of agentic-sdlc with the Claude Code plugin (project settings) plus the skills CLI for Codex; the missing `rebase-safely` skill and `done-gate-auditor` agent are now delivered | The package could not be used without APM; agent-toolkit now publishes a Claude Code marketplace |
