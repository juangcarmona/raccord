---
name: read-work-item
description: "Read a work item by identifier into structured fields. Use when any stage needs an item's current content or state. Reads a GitHub issue in juangcarmona/raccord, with its lifecycle state taken from the status: label."
metadata:
    adapted-from: "agentic-sdlc role template read-work-item, reference github-issues"
---

One responsibility: return one work item's content and current state as structured fields.

## Contract

|                  |                                                                                                                                                                           |
| ---------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **Input**        | A work item identifier.                                                                                                                                                   |
| **Output**       | Structured fields: identifier, title, description, state, type, assignee, labels, acceptance criteria, plus any project-specific field the lifecycle configuration names. |
| **Writes**       | Nothing. Read-only.                                                                                                                                                       |
| **Confirmation** | Not required, no external write.                                                                                                                                          |

**Guarantees to the caller**

- Every field the project's Definition of Ready references is present, or explicitly reported absent.
- The state is reported verbatim as the tracker names it, never normalised into a guess.
- An identifier that does not resolve is reported as not found, never as an empty item.

**Never**

- Modify the item.
- Infer a field the tracker did not return.
- Follow links or fetch attachments: those are separate roles.

## Project bindings

|                 |                                                                                                                                       |
| --------------- | ------------------------------------------------------------------------------------------------------------------------------------- |
| Tracker         | GitHub Issues, repository `juangcarmona/raccord`, via the authenticated `gh` CLI                                                      |
| Identifier      | The issue number, with or without a leading `#`                                                                                       |
| Lifecycle state | Exactly one `status:` label: `status:backlog`, `status:ready`, `status:proposed`, `status:in-review`, `status:done`, `status:blocked` |
| Board           | None. The project uses no GitHub Projects board, so `projectItems` is not read                                                        |

## Workflow

### 1. Fetch the issue

```bash
gh issue view <number> -R juangcarmona/raccord --json number,title,body,state,labels,assignees,milestone,url
gh api repos/juangcarmona/raccord/issues/<number> --jq '{state, state_reason, is_pr: (.pull_request != null)}'
```

A non-zero exit with "Could not resolve" means the number does not exist: report **not found** and stop. If the REST call shows `is_pr: true`, the number is a pull request, not a work item: report that and stop.

_Done when:_ both calls returned for this number, or the item was reported not found or not an issue.

### 2. Map the response onto the contract fields

| Contract field      | Source                                                                                                       |
| ------------------- | ------------------------------------------------------------------------------------------------------------ |
| identifier          | `number`, plus `url`                                                                                         |
| title               | `title`                                                                                                      |
| description         | `body`, Markdown, verbatim                                                                                   |
| state               | the `status:` label name verbatim, reported alongside GitHub's own `state` and `state_reason`                |
| type                | the type-like labels on the issue (`bug`, `enhancement`, `documentation`); the repository has no issue types |
| assignee            | `assignees[].login`                                                                                          |
| labels              | every `labels[].name`, `status:` labels included                                                             |
| acceptance criteria | the body section under the heading the Definition of Ready in `docs/engineering-lifecycle.md` names          |

GitHub's open/closed is not the lifecycle state. When the issue carries **no** `status:` label, report the lifecycle state as "no status label"; when it carries **more than one**, report every one of them. Do not pick one, and do not translate open/closed into a lifecycle state.

_Done when:_ every contract field has a value taken from the response, or is reported absent by name.

### 3. Extract the acceptance criteria

Read the heading name from the configuration's Definition of Ready. Existing issues in this repository use `## Acceptance criteria`. Return the text between that heading and the next heading of the same or higher level, verbatim. If the configuration names no heading, or the body has no such section, report acceptance criteria as **absent**; never derive them from other sections.

_Done when:_ the criteria are returned verbatim, or reported absent with the reason.

### 4. Check against the Definition of Ready's field list

For each field the Definition of Ready in `docs/engineering-lifecycle.md` references, confirm it is in the output. List any that are missing.

_Done when:_ every referenced field is either present or named as absent in the output.
