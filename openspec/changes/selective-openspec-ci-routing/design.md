<!-- pdac-scope: none reason="This CI design does not depend on or change product requirements; this exemption is approved by the Owner for issue #247." -->

## Context

See [proposal.md](proposal.md). The main CI workflow currently starts all jobs for every pull request and push to `main`. Its security job is an explicit every-PR guardrail in `docs/architecture/architecture-rules.md`. The post-merge workflow builds and probes the runtime on every push to `main`.

## Goals / Non-Goals

**Goals:** Keep documentation correctness and PR security checks active while avoiding application and runtime work for OpenSpec-only diffs. Keep mixed diffs on the full relevant CI path.

**Non-Goals:** Change branch protection, weaken security gates, change product behavior, or alter public-site hosting and deployment.

## Decisions

- Classify the complete changed-file list as OpenSpec-only only when it is non-empty and every path begins with `openspec/`. Any mixed or empty diff takes the full CI path. This avoids brittle allowlists that could misclassify newly added files.
- Produce the changed-file list from the PR base/head SHAs or the push before/after SHAs, then pass it to a small repository script. Test the classifier with OpenSpec-only, mixed, non-OpenSpec, and empty input fixtures.
- Keep Markdown lint, OpenSpec validation, ProductShape baseline/change validation, ProductShape citation verification, and the existing security job active. Gate only backend build/tests, code formatter and commit checks, E2E, frontend, SBOM/container, and deployment jobs.
- Add `openspec/**` to the push path-ignore for the post-merge runtime probe. Keep manual runs available and do not change the specialized static-site workflow's existing triggers.

## Risks / Trade-offs

- A path classifier regression could skip required application checks. → Exercise all-file classification cases in a deterministic script test; default empty or mixed input to the full CI path.
- OpenSpec-only changes still run repository security scans by design. → This preserves the documented every-PR security requirement while removing unrelated application and runtime jobs.
- CI job names will appear as skipped for OpenSpec-only changes. → No required check contexts are configured for branch protection; retain the existing workflow and job names for normal changes.
