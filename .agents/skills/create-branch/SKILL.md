---
name: create-branch
description: "Create a working branch named by the project's convention, from a named base. Use when starting work on an item. Creates <type>/<issue>-<slug> from a freshly fetched origin/main and pushes it."
metadata:
    adapted-from: "agentic-sdlc role template create-branch, reference convention"
---

One responsibility: create one branch for one work item, from the right base.

## Contract

|                  |                                                                            |
| ---------------- | -------------------------------------------------------------------------- |
| **Input**        | The work item identifier, a short slug, and the base reference.            |
| **Output**       | The branch name created, and the base it was created from.                 |
| **Writes**       | Creates a local branch, and pushes it where the project's convention says. |
| **Confirmation** | Confirm before pushing; local creation needs none.                         |

**Guarantees to the caller**

- The branch name carries the work item identifier, so branch, commits and pull request all correlate to the item.
- The base is fetched immediately before branching, so the branch starts from the true latest.
- An existing branch of that name is reported and reused, never silently recreated.

**Never**

- Branch from a stale local reference.
- Create a second branch for an item that already has one.
- Force-update an existing branch.

## Project bindings

|           |                                                                                                                                |
| --------- | ------------------------------------------------------------------------------------------------------------------------------ |
| Name      | `<type>/<issue>-<slug>`, for example `docs/8-rename-parties-to-contacts`                                                       |
| `<type>`  | One of `feat`, `fix`, `chore`, `docs`, `refactor`, `test`, `ci`: the commitlint `type-enum` in `.config/commitlint.config.cjs` |
| `<issue>` | The GitHub issue number, no `#`                                                                                                |
| `<slug>`  | Lowercase, hyphen-separated, from the issue title                                                                              |
| Base      | `origin/main`, fetched immediately before branching, unless the caller names another base                                      |
| Remote    | `origin`, pushed with upstream tracking                                                                                        |

## Workflow

### 1. Compose the name

Take `<type>` from the caller; it is the conventional commit type of the change. If the caller supplied no slug, derive it from the issue title: lowercase, every run of characters outside `a-z0-9` becomes one hyphen, leading and trailing hyphens dropped, kept short. If `.config/commitlint.config.cjs` no longer lists the type, stop and report it.

_Done when:_ the name matches `^(feat|fix|chore|docs|refactor|test|ci)/[0-9]+-[a-z0-9]+(-[a-z0-9]+)*$`.

### 2. Look for an existing branch for the item

```bash
git fetch origin --prune
git branch --list '*/<issue>-*'
git branch -r --list 'origin/*/<issue>-*'
```

A branch for this issue number already existing locally or on `origin` is reported and returned for reuse, whatever its name; switching to it is `checkout-branch`'s act. Nothing is created.

_Done when:_ no branch for the issue exists, or the existing one is returned.

### 3. Fetch the base

```bash
git fetch origin main
```

_Done when:_ the fetch exited zero immediately before step 4, and `git rev-parse origin/main` is the base SHA reported.

### 4. Create the branch locally

```bash
git status --porcelain
git switch --no-track -c <branch> origin/main
```

If `git status --porcelain` is not empty, report the uncommitted changes before switching: they would come along onto the new branch. `--no-track` stops the branch tracking `main`; its upstream is set by the push.

_Done when:_ `git branch --show-current` prints the new name and `git rev-parse HEAD` equals the base SHA from step 3.

### 5. Confirm and push

Show the branch name and base SHA and confirm, then:

```bash
git push -u origin <branch>
```

Never pass `--force` or `--no-verify`.

_Done when:_ `git rev-parse --abbrev-ref @{u}` prints `origin/<branch>`, and the output reports the branch name and the base it came from.
