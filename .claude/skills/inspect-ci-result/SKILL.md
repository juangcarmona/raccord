---
name: inspect-ci-result
description: "Return the continuous integration result for a branch or pull request: outcome, failing job, and log excerpts. Use when checking whether CI passed or diagnosing why it did not. Reads the GitHub Actions CI workflow for juangcarmona/raccord."
metadata:
    adapted-from: "agentic-sdlc role template inspect-ci-result, reference github"
---

One responsibility: report what CI actually did for a branch or pull request.

## Contract

|                  |                                                                                                                              |
| ---------------- | ---------------------------------------------------------------------------------------------------------------------------- |
| **Input**        | A branch name or a pull request identifier.                                                                                  |
| **Output**       | Per required check: its name, conclusion, and: where it failed: the failing job and a log excerpt. Plus one overall verdict. |
| **Writes**       | Nothing. Read-only.                                                                                                          |
| **Confirmation** | Not required, no external write.                                                                                             |

**Guarantees to the caller**

- Three states are distinguished and never collapsed: passed, failed, and **not yet run**.
- A check with no run for the current head is reported as having no result, never as passing.
- The set of checks reported is the set the branch protection **requires**, not merely those that happen to have run.

**Never**

- Treat a queued or in-progress run as a passing one.
- Report an empty result set as success, no checks having run is not the same as nothing failing.
- Re-run or cancel anything.

## Project bindings

|                        |                                                                                                                                                                                                                                                                                                                                                                                                                                                             |
| ---------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| CI system              | GitHub Actions in `juangcarmona/raccord`, read via the authenticated `gh` CLI                                                                                                                                                                                                                                                                                                                                                                               |
| Per-change workflow    | `CI` (`.github/workflows/ci.yml`), on `pull_request` and on `push` to `main`                                                                                                                                                                                                                                                                                                                                                                                |
| Its checks             | One per job; the check name is the job's `name:`. Nine today: `.NET restore, analyzers, format, tests, vulnerabilities`; `Lint mirror (format, commit messages)`; `OpenSpec changes`; `E2E journeys (Playwright over the Aspire AppHost)`; `Product definition (PDaC)`; `Frontend lint, types, tests, build, audit, contract drift`; `Security gates (secrets, patterns, vulnerabilities)`; `SBOM and license policy`; `Container image, Compose and Bicep` |
| Required by protection | Nothing. The `main` ruleset has no required status checks and there is no classic branch protection                                                                                                                                                                                                                                                                                                                                                         |
| Not per-change         | `Mutation testing` (nightly) and `Post-merge runtime probing` (after merge to `main`). Report them only when asked                                                                                                                                                                                                                                                                                                                                          |

Because protection requires nothing, the required set is empty, and an empty required set must not read as success. Report that fact, then report every job of `CI` at the head as the expected set, so that a missing job reads as not yet run.

## Workflow

### 1. Resolve the exact head commit

```bash
# pull request
gh pr view <number> -R juangcarmona/raccord --json headRefName,headRefOid,statusCheckRollup
# branch
gh api repos/juangcarmona/raccord/commits/<branch> --jq .sha
```

_Done when:_ one head SHA is fixed. Everything after this is filtered by it.

### 2. Establish the required and expected sets

```bash
gh api repos/juangcarmona/raccord/rules/branches/main --jq '[.[] | select(.type=="required_status_checks") | .parameters.required_status_checks[].context]'
git show <head-sha>:.github/workflows/ci.yml | grep -E '^    name:'
```

The first command is re-run every time rather than trusted from the table above; if it is ever non-empty, those contexts are the required set. The second gives the expected job names at that commit.

_Done when:_ the required set (possibly empty, and said so) and the expected job names at the head are both in hand.

### 3. Read the runs for that commit only

```bash
gh run list -R juangcarmona/raccord --workflow CI --commit <head-sha> --json databaseId,headSha,status,conclusion,event,url
gh pr checks <number> -R juangcarmona/raccord
```

Reject any run whose `headSha` is not the head SHA. When a job was re-run at the same head, take only the latest attempt of each job (the highest `databaseId` for that job name); an earlier failed attempt superseded by a passing re-run does not count, and the reverse does. Never select by branch alone: `gh run list --branch <branch>` returns the latest runs on the branch whatever their commit, so an older green run can stand in for a revision that never ran. Classify each expected job:

| Seen                                                                           | State                                    |
| ------------------------------------------------------------------------------ | ---------------------------------------- |
| `conclusion` `success`                                                         | passed                                   |
| `failure`, `cancelled`, `timed_out`, `action_required`, `startup_failure`      | failed                                   |
| `status` `queued`, `in_progress`, `waiting`, `pending`; or no run for the head | not yet run                              |
| `skipped`, `neutral`                                                           | reported verbatim; not counted as passed |

_Done when:_ every expected job has exactly one of these states for the head SHA.

### 4. Excerpt the failures

```bash
gh run view <run-id> -R juangcarmona/raccord --json jobs --jq '.jobs[] | select(.conclusion=="failure") | {name, steps: [.steps[] | select(.conclusion=="failure") | .name]}'
gh run view <run-id> -R juangcarmona/raccord --log-failed
```

Keep the lines around the first error of each failing step, not the whole log.

_Done when:_ each failed check has its job, failing step and a log excerpt.

### 5. Give one verdict

**Failed** if any expected job failed. Otherwise **not yet run** if any is queued, in progress, skipped without explanation or missing, or if the rollup is empty: a head with no check runs proves nothing, and a commit message carrying a CI-skip marker produces exactly that. Otherwise **passed**.

_Done when:_ one verdict is reported with the head SHA it applies to and the statement that protection requires no checks.
