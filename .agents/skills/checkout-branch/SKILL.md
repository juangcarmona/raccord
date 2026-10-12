---
name: checkout-branch
description: "Switch the working copy to a branch, fetching when needed, refusing when uncommitted changes would be lost. Use when resuming work on an item."
metadata:
    adapted-from: "agentic-sdlc role template checkout-branch, reference convention"
---

One responsibility: move the working copy to a branch without losing anything.

## Contract

|                  |                                                         |
| ---------------- | ------------------------------------------------------- |
| **Input**        | A branch name.                                          |
| **Output**       | The branch now checked out, and whether it was fetched. |
| **Writes**       | The working copy's checked-out branch.                  |
| **Confirmation** | Confirm when uncommitted changes must be stashed.       |

**Guarantees to the caller**

- Uncommitted changes are never discarded. The switch is refused, or the changes are stashed on explicit confirmation.
- A branch that exists only on the remote is fetched and tracked, not reported missing.

**Never**

- Discard, reset, or clean the working tree.
- Check out a branch already checked out in another worktree: report where it is instead.

## Project bindings

|              |                                                                                                   |
| ------------ | ------------------------------------------------------------------------------------------------- |
| Remote       | `origin`                                                                                          |
| Branch names | `<type>/<issue>-<slug>`; given only an issue number, the branch is the one matching `*/<issue>-*` |

## Workflow

### 1. Resolve the branch name

If the caller passed an issue number rather than a name:

```bash
git fetch origin --prune
git branch --list '*/<issue>-*'
git branch -r --list 'origin/*/<issue>-*'
```

More than one match is reported and the caller chooses; none is reported as not found.

_Done when:_ exactly one branch name is fixed, or the ambiguity or absence is reported.

### 2. Check other worktrees

```bash
git worktree list --porcelain
```

If a `branch refs/heads/<branch>` line belongs to a worktree other than this one, report that worktree's path and stop. Two checkouts of one branch is how a commit gets lost.

_Done when:_ the branch is checked out nowhere else, or its worktree path is reported.

### 3. Protect uncommitted work

```bash
git status --porcelain
```

Empty: continue. Not empty: show the changes and stop, unless the human explicitly confirms stashing them:

```bash
git stash push --include-untracked -m "checkout-branch: <current-branch> -> <branch>"
```

_Done when:_ the working tree is clean, either because it was or because the changes are in a named stash whose reference (`git stash list`, first entry) is reported.

### 4. Fetch and switch

```bash
git fetch origin
git show-ref --verify --quiet refs/heads/<branch> && git switch <branch>
# only on the remote:
git switch -c <branch> --track origin/<branch>
```

If neither `refs/heads/<branch>` nor `refs/remotes/origin/<branch>` exists after the fetch, report not found. If the local branch is behind or has diverged from `origin/<branch>` (`git status -sb`), report it; never reset it.

_Done when:_ `git branch --show-current` prints `<branch>`, and the output says whether a fetch brought it in, whether it tracks `origin/<branch>`, and any stash created.
