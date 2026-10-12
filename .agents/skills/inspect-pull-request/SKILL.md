---
name: inspect-pull-request
description: "Return a pull request's review state: status, reviewer votes, comment threads, merge readiness. Use when checking whether a pull request is approved or why it cannot merge. Reads one GitHub pull request in juangcarmona/raccord."
metadata:
    adapted-from: "agentic-sdlc role template inspect-pull-request, reference github"
---

One responsibility: report where one pull request stands with its reviewers.

## Contract

|                  |                                                                                                      |
| ---------------- | ---------------------------------------------------------------------------------------------------- |
| **Input**        | A pull request identifier.                                                                           |
| **Output**       | Draft state, reviewer votes, unresolved comment threads, required-check status, and merge readiness. |
| **Writes**       | Nothing. Read-only.                                                                                  |
| **Confirmation** | Not required, no external write.                                                                     |

**Guarantees to the caller**

- Approval is reported as of **now**, not as of an earlier read: votes reset when new commits land.
- Unresolved threads are listed with their content, so the caller knows what is actually blocking.

**Never**

- Approve, vote, or resolve a thread.
- Report a draft pull request as merge-ready.

## Project bindings

|                   |                                                                                                                                                                                     |
| ----------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Forge             | GitHub, repository `juangcarmona/raccord`, via the authenticated `gh` CLI                                                                                                           |
| What gates `main` | The ruleset named `main`: a pull request is required with **0** required approvals, no required status checks, no required thread resolution. There is no classic branch protection |

Because no approval is required, `reviewDecision` is usually empty here. **Empty is not approval.** Report it as "no review required by the ruleset; no approval recorded", and leave the decision on whether the human review has happened to the caller and the Definition of Done.

## Workflow

### 1. Read the pull request now

```bash
gh pr view <number> -R juangcarmona/raccord --json number,url,state,isDraft,reviewDecision,latestReviews,reviewRequests,mergeable,mergeStateStatus,headRefOid,baseRefName
```

Read this fresh on every invocation; never reuse an earlier result.

_Done when:_ the state, draft flag, `reviewDecision`, each reviewer's latest review state with its commit, pending review requests, `mergeable` and `mergeStateStatus` are in hand for the current `headRefOid`.

### 2. Read the review threads

```bash
gh api graphql -f query='query($owner:String!,$repo:String!,$number:Int!){repository(owner:$owner,name:$repo){pullRequest(number:$number){reviewThreads(first:100){pageInfo{hasNextPage} nodes{isResolved isOutdated path line comments(first:1){nodes{author{login} body url}}}}}}}' -F owner=juangcarmona -F repo=raccord -F number=<number>
```

A thread that is neither `isResolved` nor `isOutdated` is blocking. List each blocking thread with its path, line, author and first comment. If `hasNextPage` is true, say the list is truncated at 100.

_Done when:_ every blocking thread is listed with its content, or none is stated.

### 3. Read the check status

Nothing is required by the ruleset, so report the checks actually present on the head commit and state that none is required:

```bash
gh pr view <number> -R juangcarmona/raccord --json statusCheckRollup \
  --jq '.statusCheckRollup[] | {name, status, conclusion, workflowName}'
```

An empty rollup is no result, never a pass. For per-job failure detail, the caller uses `inspect-ci-result`.

_Done when:_ each check is reported as passed, failed, or not yet run for the current head, and an empty rollup is reported as no result.

### 4. Judge merge readiness

Merge-ready only when all hold: `state` is `OPEN`, `isDraft` is false, `mergeable` is `MERGEABLE`, no blocking thread remains, and no `CHANGES_REQUESTED` review stands. Report each condition that fails. A draft is never merge-ready.

_Done when:_ one verdict is given, with every failing condition named.
