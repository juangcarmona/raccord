---
name: open-pull-request
description: "Open a pull request from the current branch, linked to its work item. Use when a stage needs a pull request raised, in draft or ready for review. Opens one GitHub pull request against main in juangcarmona/raccord."
metadata:
    adapted-from: "agentic-sdlc role template open-pull-request, reference github"
---

One responsibility: create one pull request for the current branch and link it to its work item.

## Contract

|                  |                                                                                         |
| ---------------- | --------------------------------------------------------------------------------------- |
| **Input**        | The source branch, the target branch, a title, a body, and whether it opens as a draft. |
| **Output**       | The pull request's identifier and URL, and its draft state.                             |
| **Writes**       | Creates a pull request in the forge.                                                    |
| **Confirmation** | **Required.** Confirm before creating.                                                  |

**Guarantees to the caller**

- Exactly one pull request per work item: an existing open one is reported and reused, never duplicated.
- The work item identifier appears in the title or body, so the item and the change are traceable to each other.
- The draft state is exactly as the caller asked; a stage that wants a review gate gets an unmergeable draft.

**Never**

- Open a second pull request for an item that already has one.
- Mark a pull request ready for review; that is a separate role.
- Add reviewers the project's configuration does not name.

## Project bindings

|               |                                                                           |
| ------------- | ------------------------------------------------------------------------- |
| Forge         | GitHub, repository `juangcarmona/raccord`, via the authenticated `gh` CLI |
| Target branch | `main`                                                                    |
| Source branch | `<type>/<issue>-<slug>`, so the issue number is recoverable from it       |
| Linking       | The body contains `Closes #<issue>`                                       |
| Template      | None; the repository has no pull request template                         |

## Workflow

### 1. Check for an existing pull request

```bash
gh pr list -R juangcarmona/raccord --head <branch> --state open --json number,url,isDraft
gh api graphql -f query='query($owner:String!,$repo:String!,$number:Int!){repository(owner:$owner,name:$repo){issue(number:$number){closedByPullRequestsReferences(first:10){nodes{number url state isDraft headRefName}}}}}' -F owner=juangcarmona -F repo=raccord -F number=<issue>
```

An open pull request from this branch, or an open one already linked to the issue, is reported and returned with its number, URL and draft state. Nothing is created. A duplicate splits the review and the history.

_Done when:_ both queries ran and returned no open pull request, or the existing one was returned.

### 2. Check the branch is on the remote

```bash
git ls-remote --heads origin <branch>
```

An empty result means the branch was never pushed. Stop and say so; pushing is `create-branch`'s act, and `gh pr create` would otherwise prompt to push.

_Done when:_ the remote branch exists and its head is the commit the caller expects.

### 3. Compose

Target `main`. Write the body to a temporary file and make sure it contains `Closes #<issue>`: the body keyword is what closes the issue on merge, and it keeps the closing keyword out of the title. Add reviewers only where `docs/engineering-lifecycle.md` names them.

_Done when:_ title, body file, base `main`, draft flag and any configured reviewers are fixed, and the body contains `Closes #<issue>`.

### 4. Confirm

Show the base, head, title, body, draft flag and reviewers.

_Done when:_ the human has confirmed this exact pull request.

### 5. Create

```bash
gh pr create -R juangcarmona/raccord --base main --head <branch> --title "<title>" --body-file <path> [--draft] [--reviewer <login>]
```

Pass `--draft` exactly when the caller asked for a draft. The draft state is the review gate: GitHub refuses to merge a draft. It does **not** stop CI: the `CI` workflow triggers on `pull_request` with the default activity types and no draft filter, so CI runs on drafts here. Whether validation ran is read from the check state, never assumed from the draft flag.

_Done when:_ the command printed one pull request URL.

### 6. Verify and return

```bash
gh pr view <url> -R juangcarmona/raccord --json number,url,isDraft,baseRefName,headRefName,body --jq '{number, url, isDraft, baseRefName, headRefName, links_issue: (.body | contains("Closes #<issue>"))}'
```

_Done when:_ the number, URL and draft state are returned as read back, the draft state equals what the caller asked, and `links_issue` is true.
