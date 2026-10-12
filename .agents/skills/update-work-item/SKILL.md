---
name: update-work-item
description: "Update fields of an existing work item, changing only the fields supplied. Use when recording shaped requirements or correcting content. Edits the title, body, labels or assignees of a GitHub issue in juangcarmona/raccord."
metadata:
    adapted-from: "agentic-sdlc role template update-work-item, reference github-issues"
---

One responsibility: change named fields on one work item and nothing else.

## Contract

|                  |                                                         |
| ---------------- | ------------------------------------------------------- |
| **Input**        | A work item identifier and the fields to change.        |
| **Output**       | The fields actually changed, with their new values.     |
| **Writes**       | The work item's fields in the tracker.                  |
| **Confirmation** | **Required.** Show the diff and confirm before writing. |

**Guarantees to the caller**

- Only the fields supplied are touched. Every other field is left byte-identical.
- The change is reported field by field, so the caller can see what moved.

**Never**

- Change the item's state; that is a separate role.
- Clear a field because the caller omitted it.
- Reformat a field it was not asked to change.

## Project bindings

|         |                                                                                                        |
| ------- | ------------------------------------------------------------------------------------------------------ |
| Tracker | GitHub Issues, repository `juangcarmona/raccord`, via the authenticated `gh` CLI                       |
| State   | Any `status:` label is state. Adding or removing one is `transition-work-item`'s act, never this one's |

## Workflow

### 1. Read the current values

```bash
gh issue view <number> -R juangcarmona/raccord --json number,title,body,labels,assignees,milestone,url
gh issue view <number> -R juangcarmona/raccord --json body --jq .body > <before-path>
```

Refuse any requested change to a `status:` label, and say which role owns it.

_Done when:_ the current value of every field the caller wants changed is in hand, and no state change is in the request.

### 2. Build the new values

`--body` replaces the whole body. To change one section, copy `<before-path>` to `<after-path>`, edit only that section in the copy, and leave every other byte alone. Never assemble a new body from scratch: that drops whatever the project added by hand.

_Done when:_ `diff -u <before-path> <after-path>` shows only the lines of the requested section, and every other requested field has its new value prepared.

### 3. Show the diff and confirm

Show the body diff and, for each other field, old value and new value.

_Done when:_ the human has confirmed this exact diff.

### 4. Write

Pass only the flags for fields being changed:

```bash
gh issue edit <number> -R juangcarmona/raccord \
  [--title "<title>"] [--body-file <after-path>] \
  [--add-label "<label>"] [--remove-label "<label>"] \
  [--add-assignee "<login>"] [--remove-assignee "<login>"]
```

_Done when:_ the command exited zero.

### 5. Verify and report

Read the issue again as in step 1. Compare the fresh body with `<after-path>` and every untouched field with its step 1 value.

_Done when:_ each changed field is reported with its new value, and every untouched field is confirmed unchanged.
