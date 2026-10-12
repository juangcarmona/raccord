---
name: merge-pull-request
description: "Complete an approved pull request into its target branch. Use at close-out, once the Done gate has passed and the review is approved. Merges one GitHub pull request into main in juangcarmona/raccord with a merge commit."
metadata:
    adapted-from: "agentic-sdlc role template merge-pull-request, reference github (merge strategy changed from squash to merge commit for this project)"
---

One responsibility: merge one approved pull request, composing the commit message that lands on the target branch.

## Contract

|                  |                                                                                                   |
| ---------------- | ------------------------------------------------------------------------------------------------- |
| **Input**        | A pull request identifier, the merge strategy, and the subject and body for the resulting commit. |
| **Output**       | The merge commit's identifier and the pull request's final state.                                 |
| **Writes**       | Merges into the target branch and, where configured, deletes the source branch.                   |
| **Confirmation** | **Required.** Confirm before merging.                                                             |

**Guarantees to the caller**

- The pull request was still open and still approved immediately before the merge, re-checked rather than assumed.
- The commit subject describes the **implemented change**, not the proposal the pull request opened with.
- The message is set explicitly, so no auto-generated body carries anything unintended onto the target branch.

**Never**

- Merge a draft pull request.
- Merge a pull request whose required checks have not passed.
- Cast an approval on the author's behalf without explicit human confirmation.
- Let the forge auto-generate the squash body.

## Project bindings

|               |                                                                                                                                                                             |
| ------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Forge         | GitHub, repository `juangcarmona/raccord`, via the authenticated `gh` CLI                                                                                                   |
| Target        | `main`, governed by the `main` ruleset: pull request required, 0 required approvals, no required status checks, no force-push, no deletion. Repository admins can bypass it |
| Strategy      | Merge commit: `gh pr merge --merge`. Not squash, not rebase                                                                                                                 |
| Source branch | Kept after merge unless the human confirms deletion; the repository does not delete head branches automatically                                                             |

The ruleset requires no approval and no checks, so GitHub would merge on its own terms with nothing passed. The gates here are therefore this capability's, not the forge's: **approved** means the Owner named in the **Roles** section of `docs/engineering-lifecycle.md` has given Definition of Done item 10 (human review) its own explicit approval in this session, asked as a separate question before the merge confirmation and recorded as a pull request comment. The agent's `gh` identity is the Owner's account, which GitHub does not allow to approve its own pull requests, so `reviewDecision` will not show `APPROVED` here and is never treated as approval; an empty `reviewDecision` is never approval either. **Checks passed** means `inspect-ci-result` returns **passed** for the exact head commit being merged.

**Never use `--admin`.** It bypasses the ruleset, and bypassing the project's own gate is a human decision this capability does not make. Never use `--auto` either: it merges later, against a head nobody re-checked.

## Workflow

### 1. Re-check the pull request, now

```bash
gh pr view <number> -R juangcarmona/raccord --json number,url,state,isDraft,baseRefName,headRefName,headRefOid,reviewDecision,mergeable,mergeStateStatus,title
```

Stop and report if `state` is not `OPEN`, `isDraft` is true, `baseRefName` is not `main`, or `mergeable` is anything other than `MERGEABLE` (`UNKNOWN` means GitHub has not computed it yet: wait and re-read rather than proceed). Then run `inspect-pull-request` for blocking threads and changes requested, and `inspect-ci-result` for this `headRefOid`.

_Done when:_ the pull request is open, not a draft, targets `main`, has no blocking thread or standing change request, CI is **passed** for `headRefOid`, and approval is established as defined above; or the first failing condition is reported and nothing else happens.

### 2. Compose the message

The subject describes what was **implemented**, not what the pull request proposed when it opened; write it as a conventional commit header, matching the convention the repository enforces on every other commit. The body is set explicitly, short, non-empty, and carries no CI-skip marker. Never leave either to GitHub's default.

_Done when:_ subject and body are both fixed strings, and the subject reflects the final diff.

### 3. Obtain the Owner's approval (Done gate item 10)

Ask the Owner, as its own question and not as part of the merge confirmation, whether they approve the change for Definition of Done item 10. On a yes, record it on the pull request:

```bash
gh pr comment <number> -R juangcarmona/raccord --body "Owner approval (Definition of Done item 10) given for head <headRefOid>."
```

_Done when:_ the Owner explicitly approved and the comment exists, or the refusal is reported and nothing else happens.

### 3b. Confirm the merge with the human

Show the pull request, the strategy (merge commit), the head SHA, the subject, the body, and ask whether the source branch should be deleted after the merge (default: keep it).

_Done when:_ the human has explicitly confirmed this merge, with this message, at this head SHA, and answered the branch question.

### 3c. Re-check after confirmation

Run the step 1 commands again. If the state, draft flag, `mergeable`, head SHA, threads, change requests or CI verdict differ from what was confirmed, report the difference and stop.

_Done when:_ every step 1 condition still holds at the confirmed head SHA.

### 4. Merge

```bash
gh pr merge <number> -R juangcarmona/raccord --merge \
  --subject "<subject>" --body "<body>" \
  --match-head-commit <headRefOid> [--delete-branch]
```

`--match-head-commit` makes GitHub refuse the merge if a commit landed after step 1. Pass `--delete-branch` only on the human's explicit yes. If the merge is refused, report the reason; do not retry with other flags.

_Done when:_ the command exited zero, or the refusal is reported verbatim.

### 5. Report

```bash
gh pr view <number> -R juangcarmona/raccord --json state,mergedAt,mergeCommit,headRefName
```

_Done when:_ the output reports state `MERGED`, the merge commit SHA from `mergeCommit.oid`, and whether the source branch was deleted. With `--delete-branch`, `gh` deletes the remote branch and, run from a working copy of this repository, also switches that working copy to `main` and deletes the local branch; report all of it.
