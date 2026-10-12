---
name: create-work-item
description: "Create a work item from structured input, returning its identifier and URL. Use when a shaped idea, a follow-up, or a discovered defect must become trackable. Creates one GitHub issue in juangcarmona/raccord."
metadata:
    adapted-from: "agentic-sdlc role template create-work-item, reference github-issues"
---

One responsibility: create one work item and return how to find it again.

## Contract

|                  |                                                                                       |
| ---------------- | ------------------------------------------------------------------------------------- |
| **Input**        | The item's type, title, description, and the fields the project requires at creation. |
| **Output**       | The new item's identifier and URL.                                                    |
| **Writes**       | Creates a work item in the tracker.                                                   |
| **Confirmation** | **Required.** Show the content and confirm before creating.                           |

**Guarantees to the caller**

- Every field the project requires at creation is set, or the creation is refused with the missing fields named.
- The identifier and URL are returned, so the caller never has to search for what it just made.

**Never**

- Create several items in one invocation.
- Transition the new item; that is a separate role.
- Invent a value for a required field the caller did not supply.

## Project bindings

|                 |                                                                                                                      |
| --------------- | -------------------------------------------------------------------------------------------------------------------- |
| Tracker         | GitHub Issues, repository `juangcarmona/raccord`, via the authenticated `gh` CLI                                     |
| Type            | A label. Type-like labels that exist: `bug`, `enhancement`, `documentation`. The repository has no issue types       |
| Issue template  | None exists, so the body's shape comes from the Definition of Ready in `docs/engineering-lifecycle.md`               |
| Lifecycle state | A `status:` label, applied by `transition-work-item`, never here. An issue with no `status:` label is in the backlog |

## Workflow

### 1. Validate the input

Check that the caller supplied a title, a body, and every field `docs/engineering-lifecycle.md` requires at creation. Check that every requested label exists:

```bash
gh label list -R juangcarmona/raccord --limit 200 --json name --jq '.[].name'
```

Refuse, naming each missing field or unknown label, rather than filling anything in. Strip any `status:` label from the request and say so: setting state is `transition-work-item`'s act, and creation and state are two acts so that a failure in one is not hidden by the other.

_Done when:_ every required field is present and every label exists, or the refusal names what is missing.

### 2. Check for an existing item

```bash
gh issue list -R juangcarmona/raccord --state open --search "<title> in:title" --json number,title,url
```

Report any open issue with the same or a near-identical title and ask whether to proceed.

_Done when:_ the search ran and any likely duplicate was shown to the human.

### 3. Show and confirm

Write the body to a temporary file. Show the title, the labels and the full body exactly as they will be posted, and get explicit confirmation.

_Done when:_ the human has confirmed this exact content.

### 4. Create

```bash
gh issue create -R juangcarmona/raccord --title "<title>" --body-file <path> --label "<label>" [--label "<label>" ...]
```

`gh issue create` prints the new issue's URL. The number is its last path segment. Create exactly one issue; if the command failed ambiguously, run step 2 again before any retry.

_Done when:_ the command printed one issue URL.

### 5. Return

```bash
gh issue view <number> -R juangcarmona/raccord --json number,url,title,labels
```

_Done when:_ the number and URL are returned, read back from the tracker rather than assumed.
