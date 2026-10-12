---
name: comment-work-item
description: "Add a formatted comment to a work item. Use when posting a summary, a question, an outcome, or a link back to the item. Posts one Markdown comment on a GitHub issue in juangcarmona/raccord."
metadata:
    adapted-from: "agentic-sdlc role template comment-work-item, reference github-issues"
---

One responsibility: post one comment on one work item.

## Contract

|                  |                                                            |
| ---------------- | ---------------------------------------------------------- |
| **Input**        | A work item identifier and the comment body.               |
| **Output**       | The comment's identifier or URL.                           |
| **Writes**       | Adds a comment in the tracker.                             |
| **Confirmation** | **Required.** Show the comment and confirm before posting. |

**Guarantees to the caller**

- The comment is posted once. A retry after an ambiguous failure checks for an existing identical comment first.
- Formatting is rendered in the tracker's own markup, not pasted as raw text.

**Never**

- Edit or delete an existing comment.
- Post the same summary on every stage: a comment nobody reads is noise that trains people to ignore the ones that matter.

## Project bindings

|         |                                                                                  |
| ------- | -------------------------------------------------------------------------------- |
| Tracker | GitHub Issues, repository `juangcarmona/raccord`, via the authenticated `gh` CLI |
| Markup  | GitHub-flavoured Markdown, rendered natively                                     |

## Workflow

### 1. Write the body to a file

Write the comment as Markdown to a temporary file. Use `--body-file`, not `--body`: shell quoting mangles multi-line bodies in ways that are hard to see until they are posted.

_Done when:_ the file holds exactly the text to post.

### 2. Check for an identical comment

```bash
gh api repos/juangcarmona/raccord/issues/<number>/comments --paginate \
  | jq -s --rawfile body <path> 'add // [] | .[] | select((.body | rtrimstr("\n")) == ($body | rtrimstr("\n"))) | .html_url'
```

A match means the comment is already posted: return its URL and post nothing. Also check whether a comment with substantially the same summary already exists from an earlier stage, and if so say so before posting another.

_Done when:_ no identical comment exists, or its URL was returned and nothing else happened.

### 3. Show and confirm

Show the issue number and the comment exactly as it will be posted.

_Done when:_ the human has confirmed this exact comment.

### 4. Post

```bash
gh issue comment <number> -R juangcarmona/raccord --body-file <path>
```

The command prints the new comment's URL. If it fails ambiguously, repeat step 2 before any retry.

_Done when:_ exactly one comment URL is returned.
