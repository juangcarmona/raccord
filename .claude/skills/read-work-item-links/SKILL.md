---
name: read-work-item-links
description: "Return a work item's linked items with their link types, plus its parent. Use when assessing dependencies, blockers, or parent context. Reads cross-references, sub-issues and body references of a GitHub issue in juangcarmona/raccord."
metadata:
    adapted-from: "agentic-sdlc role template read-work-item-links, reference github-issues"
---

One responsibility: report what one work item is connected to.

## Contract

|                  |                                                                                            |
| ---------------- | ------------------------------------------------------------------------------------------ |
| **Input**        | A work item identifier.                                                                    |
| **Output**       | Linked items with their link types and states, the parent or epic, and any external links. |
| **Writes**       | Nothing. Read-only.                                                                        |
| **Confirmation** | Not required, no external write.                                                           |

**Guarantees to the caller**

- Link direction is preserved: blocks and is-blocked-by are not the same fact, and collapsing them inverts the dependency.
- Each linked item's state is included, so a blocker already resolved is visible as resolved.

**Never**

- Follow links recursively without being asked.
- Create, change or remove a link.

## Project bindings

|                                   |                                                                                  |
| --------------------------------- | -------------------------------------------------------------------------------- |
| Tracker                           | GitHub Issues, repository `juangcarmona/raccord`, via the authenticated `gh` CLI |
| Lifecycle state of a linked issue | Its `status:` label, reported with GitHub's open/closed                          |

GitHub has no first-class link type. Relationships are expressed as sub-issues and parent issues, as cross-reference events in the timeline, as pull requests that close the issue, and as references written in the body. Direction is therefore a convention here, not a typed fact, and the output must say so.

## Workflow

### 1. Read the parent and sub-issues

```bash
gh api graphql -f query='query($owner:String!,$repo:String!,$number:Int!){repository(owner:$owner,name:$repo){issue(number:$number){parent{number title state url} subIssues(first:50){totalCount nodes{number title state url}}}}}' -F owner=juangcarmona -F repo=raccord -F number=<number>
```

A `null` parent and a zero `totalCount` are reported as "no parent" and "no sub-issues", never left as empty fields. A null `issue` means the number does not resolve: report not found and stop.

_Done when:_ the parent and every sub-issue are listed, or their absence is stated explicitly.

### 2. Read the pull requests that close the item

```bash
gh api graphql -f query='query($owner:String!,$repo:String!,$number:Int!){repository(owner:$owner,name:$repo){issue(number:$number){closedByPullRequestsReferences(first:10,includeClosedPrs:true){nodes{number url state isDraft headRefName}}}}}' -F owner=juangcarmona -F repo=raccord -F number=<number>
```

Report each as link type "closed by pull request", with its state (`OPEN`, `CLOSED`, `MERGED`) and draft flag.

_Done when:_ every closing pull request is listed with its state, or none is stated.

### 3. Read cross-references from the timeline

```bash
gh api repos/juangcarmona/raccord/issues/<number>/timeline --paginate \
  --jq '.[] | select(.event=="cross-referenced") | .source.issue | {number, title, state, html_url, is_pr: (.pull_request != null)}'
```

Report each as link type "mentioned by", meaning the other item refers to this one. That is the only direction the event proves.

_Done when:_ every cross-reference event is listed with its source item's state.

### 4. Read references written in the body

```bash
gh issue view <number> -R juangcarmona/raccord --json body --jq .body
```

Collect every `#<n>` reference and every URL. Where the sentence states a direction, such as "blocked by #12" or "blocks #12", keep that wording as the link type and attribute it to this item's body. Where no direction is stated, report the type as "referenced". Report URLs outside the repository as external links.

_Done when:_ every reference in the body appears in the output with the wording that typed it.

### 5. Read each linked item's state

For every issue number collected in steps 2 to 4 that is not already carrying a state:

```bash
gh issue view <n> -R juangcarmona/raccord --json number,title,state,labels,url \
  --jq '{number, title, state, url, status: [.labels[].name | select(startswith("status:"))]}'
```

Read one level only. Do not read the links of a linked item unless the caller asked.

_Done when:_ every linked item carries its GitHub state and its `status:` labels, and the output states that direction comes from wording and convention rather than a typed link.
