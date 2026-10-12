---
name: update-pull-request
description: "Update an existing pull request in place (title, description, reviewers, draft state), never creating a duplicate. Use when a pull request needs a refreshed summary or must be marked ready for review. Edits one GitHub pull request in juangcarmona/raccord."
metadata:
    adapted-from: "agentic-sdlc role template update-pull-request, reference github"
---

One responsibility: change one existing pull request in place.

## Contract

|                  |                                                         |
| ---------------- | ------------------------------------------------------- |
| **Input**        | A pull request identifier and the properties to change. |
| **Output**       | The properties actually changed.                        |
| **Writes**       | The pull request in the forge.                          |
| **Confirmation** | **Required.** Confirm before writing.                   |

**Guarantees to the caller**

- The existing pull request is updated. A duplicate is never created, whatever the caller asked for.
- Marking ready for review is reported explicitly, because it starts the review gate and notifies people.

**Never**

- Create a pull request.
- Merge one.
- Silently drop a description section the project's template requires.

## Project bindings

|                      |                                                                                                  |
| -------------------- | ------------------------------------------------------------------------------------------------ |
| Forge                | GitHub, repository `juangcarmona/raccord`, via the authenticated `gh` CLI                        |
| Required in the body | `Closes #<issue>`. There is no pull request template, so this line is the one the body must keep |

## Workflow

### 1. Read the pull request

```bash
gh pr view <number> -R juangcarmona/raccord --json number,url,state,isDraft,title,body,reviewRequests
gh pr view <number> -R juangcarmona/raccord --json body --jq .body > <before-path>
```

A pull request that is not `OPEN` is reported with its state, and nothing is changed. A request with no pull request to update is refused; opening one is `open-pull-request`'s act.

_Done when:_ the pull request is open and the current value of every property to change is in hand.

### 2. Prepare the change

For the body, copy `<before-path>` to `<after-path>` and edit only the requested part. Reviewers are added only where `docs/engineering-lifecycle.md` names them.

_Done when:_ `diff -u <before-path> <after-path>` shows only the requested change, the new body still contains `Closes #<issue>`, and every other property has its new value prepared.

### 3. Confirm

Show the diff and each property's old and new value. If the change marks the pull request ready for review, say so on its own line: it starts the review gate and notifies people.

_Done when:_ the human has confirmed this exact change.

### 4. Apply

Run only the commands for properties being changed:

```bash
gh pr edit <number> -R juangcarmona/raccord [--title "<title>"] [--body-file <after-path>] [--add-reviewer <login>] [--remove-reviewer <login>]
gh pr ready <number> -R juangcarmona/raccord          # draft -> ready for review
gh pr ready <number> -R juangcarmona/raccord --undo   # ready -> draft
```

Neither `gh pr ready` nor a title edit starts CI on this project: the `CI` workflow triggers on `pull_request` with the default activity types, which do not include `ready_for_review` or `edited`. Its checks will already have run on the draft's head; read them with `inspect-ci-result` rather than assuming a new run.

_Done when:_ every command exited zero.

### 5. Verify and report

Read the pull request again as in step 1.

_Done when:_ each property changed is reported with its new value as read back, any change of draft state is reported explicitly, and the body still contains `Closes #<issue>`.
