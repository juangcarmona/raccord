<!-- pdac-scope: none reason="CI routing changes do not alter product behavior or product requirements; this exemption is approved by the Owner for issue #247." -->

## Why

OpenSpec-only changes currently start application builds, end-to-end journeys, frontend checks, container packaging, and deployment validation even though those checks do not exercise the changed files. This makes documentation and planning updates slower and noisier to review.

## What Changes

- Classify changed paths in the main CI workflow and skip application, browser, frontend, packaging, and deployment jobs when every changed path is under `openspec/`.
- Keep Markdown lint, OpenSpec validation, ProductShape validation and citation verification, and the PR security gates active for OpenSpec-only changes.
- Skip the post-merge runtime probe for pushes that change only OpenSpec files.
- Add deterministic checks for the path classifier and document the routing in the CI reference.

## Capabilities

No product capability changes. This is a CI and repository-maintenance change; `skip_specs: true` is used because it introduces no product or runtime behavior.

## Impact

Affected systems are `.github/workflows/ci.yml`, `.github/workflows/post-merge.yml`, CI routing scripts and their checks, and the deterministic-check reference in `docs/engineering-lifecycle.md`. The existing security gates remain active for every PR. Public-site workflows and deployment behavior are otherwise unchanged.
