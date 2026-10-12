---
name: transition-work-item
description: "Apply exactly one workflow transition to one work item, chosen from those valid from its current state. Use when a stage advances an item. Moves a GitHub issue in juangcarmona/raccord from one status: label to the next."
metadata:
    adapted-from: "agentic-sdlc role template transition-work-item, reference github-issues (labels)"
---

One responsibility: move one work item through exactly one state transition.

## Contract

|                  |                                                                  |
| ---------------- | ---------------------------------------------------------------- |
| **Input**        | A work item identifier, and the target state or transition name. |
| **Output**       | The item's new state, and the transition actually applied.       |
| **Writes**       | The work item's state in the tracker.                            |
| **Confirmation** | **Required.** Confirm before applying.                           |

**Guarantees to the caller**

- The transition applied was fetched from the item's **current** state, never assumed from a mapping table.
- Exactly one transition per invocation.
- A transition unavailable from the current state is reported together with the list of what is available.

**Never**

- Chain several transitions to reach a target state.
- Fire a transition the lifecycle configuration marks human-only.
- Reverse a human-only transition found already applied: report it as a signal instead.

## Project bindings

The lifecycle state is **not** GitHub's open/closed. It is exactly one `status:` label on the issue in `juangcarmona/raccord`. GitHub enforces no workflow on labels, so the label workflow below is the tracker's workflow definition, and this capability is what enforces it.

| Transition           | From label         | To label           | Fired by    |
| -------------------- | ------------------ | ------------------ | ----------- |
| backlog → ready      | `status:backlog`   | `status:ready`     | `refine`    |
| ready → proposed     | `status:ready`     | `status:proposed`  | `propose`   |
| proposed → in-review | `status:proposed`  | `status:in-review` | `implement` |
| in-review → done     | `status:in-review` | `status:done`      | `integrate` |

**Human-only, never fired here:** setting or clearing `status:blocked`, closing an issue as not planned, and reopening an issue. If `docs/engineering-lifecycle.md` marks further transitions human-only, they are refused too; the configuration is the authority and this table is only a convenience copy of it, so step 0 reads the configuration on every run and stops when the two disagree.

An issue with **no** `status:` label is in the backlog for one purpose only: the backlog → ready transition, which then adds `status:ready` and removes nothing. For any other target, a missing label is a mismatch.

## Workflow

### 0. Read the lifecycle configuration

Read the **Lifecycle stages and states** and **Transitions no capability ever fires** sections of `docs/engineering-lifecycle.md`. Confirm that every row of the table above (its from-label, to-label and firing stage) appears there, that the configuration defines no agent transition missing here, and that every human-only transition there is refused by this skill.

_Done when:_ the configuration was read in this invocation and agrees with the table above, or the disagreement is reported with both versions and nothing else happens.

### 1. Resolve the requested transition

Map the caller's target state or transition name onto one row of the table. A target of `status:blocked`, closing as not planned, or reopening is human-only: refuse. A target of `status:backlog` is not a defined transition: refuse. In both cases list the four agent transitions.

_Done when:_ exactly one row is selected, or the refusal is reported with the available transitions.

### 2. Read the current state, immediately before acting

```bash
gh issue view <number> -R juangcarmona/raccord --json number,state,labels,url \
  --jq '{number, state, url, status: [.labels[].name | select(startswith("status:"))]}'
gh api repos/juangcarmona/raccord/issues/<number> --jq '{state, state_reason, is_pr: (.pull_request != null)}'
```

Decide from what was just read, in this order:

| Found                                          | Outcome                                                                                                                                            |
| ---------------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------- |
| Number does not resolve                        | Report not found. Stop                                                                                                                             |
| `is_pr` is true                                | Report that the number is a pull request, not an issue. Stop                                                                                       |
| `status:blocked` present                       | Report it as a human signal. Do not remove it. Stop                                                                                                |
| `state_reason` is `not_planned`                | Report the human closure. Do not reopen. Stop                                                                                                      |
| `state` is `closed`, any other reason          | Report a closed issue as a mismatch (only `in-review → done` may meet a closed issue, when the merge closed it first). Stop unless that is the row |
| `state_reason` is `reopened`                   | Report the human reopening as a signal; continue only if the human confirms in step 3                                                              |
| More than one `status:` label                  | Report every label as a mismatch. Stop                                                                                                             |
| No `status:` label, target `status:ready`      | Treat as `status:backlog`. Continue, adding only                                                                                                   |
| No `status:` label, any other target           | Report the mismatch. Stop                                                                                                                          |
| One label, equal to the row's from-label       | Continue                                                                                                                                           |
| One label, different from the row's from-label | Report the current label and the transition available from it (the row whose from-label it is, if any). Stop                                       |

_Done when:_ the labels were read in this invocation and exactly one outcome from the table is named.

### 3. Confirm

Show the issue, its current label, the target label, and the exact command for step 4.

_Done when:_ the human has confirmed this one transition.

### 3a. Re-read after confirmation

Confirmation can take any amount of time. Run both commands of step 2 again and compare with what was confirmed. If anything changed (labels, state, `state_reason`), report the difference and stop without applying.

_Done when:_ the state read after confirmation equals the state that was confirmed.

### 4. Apply

Remove the old label in the same call; two `status:` labels on one issue make every downstream query ambiguous, and nothing in GitHub prevents it.

```bash
gh issue edit <number> -R juangcarmona/raccord --remove-label "status:<from>" --add-label "status:<to>"
```

For the no-label backlog case: `gh issue edit <number> -R juangcarmona/raccord --add-label "status:ready"`.

Closing is a separate act from reaching `status:done`. On this project the merged pull request's `Closes #<number>` closes the issue, and `integrate` owns making sure it is closed; this capability neither closes nor reopens.

_Done when:_ the command exited zero.

### 5. Verify and report

Repeat the first command of step 2.

If the issue now carries two `status:` labels (GitHub applies the add and the remove as separate writes, so one can succeed without the other), report a partial transition with both labels and stop; never repair it automatically.

_Done when:_ the issue carries exactly one `status:` label, equal to the target, and the output reports the new state, the transition applied (from-label → to-label), and GitHub's open/closed state.
