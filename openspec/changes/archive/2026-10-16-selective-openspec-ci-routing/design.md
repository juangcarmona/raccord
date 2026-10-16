<!-- pdac-scope: none reason="This CI and static delivery design does not depend on or change product requirements; this exemption is approved by the Owner for issue #247." -->

## Context

See [proposal.md](proposal.md). The repository has a GitHub Pages publisher for architecture snapshots, the product model, and nightly E2E reports. Its broad `src/**` trigger would couple the new public homepage to that publisher. The main CI workflow also performs application, browser, and packaging checks for changes that never touch the application.

## Goals / Non-Goals

**Goals:** Keep documentation correctness and PR security checks active while skipping unrelated application work for OpenSpec-only and public-site-only diffs. Keep mixed application diffs on the full relevant CI path. Give engineering evidence and the public homepage independent source, validation, and hosting paths.

**Non-Goals:** Change branch protection, weaken security gates, change product behavior, publish the public site to `raccord.dev`, or bind the purchased domain.

## Decisions

- Treat only non-empty diffs wholly under `openspec/`, `src/public-site/`, or the public-site workflow as app-free. Mixed paths and empty diffs use the full application CI route. Keep Markdown lint, OpenSpec, ProductShape, and security validation active for both lightweight routes.
- Rename the current publisher to `engineering-site.yml` and keep watching `src/**` while explicitly excluding `src/public-site/**`. Keep E2E tests, engineering snapshots, the GitHub Pages artifact, and its non-cancelling concurrency behavior in that workflow.
- Use a separate `public-site.yml` path-filtered to `src/public-site/**` and its own workflow file. Build the site with its dependency-free Node script, run its output assertions, and do not upload to GitHub Pages.
- Use a Cloudflare Pages Git integration for static hosting. Keep `src/public-site/` as the project root, watch only `src/public-site/*` in Cloudflare Build watch paths, and set `dist/` as the Pages build output. Non-production branches produce previews; `main` is the production branch. Keep production branch deployments disabled until Owner approval and leave custom-domain binding out of this change.
- Use the Pages Git integration instead of a GitHub Actions Wrangler deployment so builds and previews do not require a stored Cloudflare API token. The checked-in Wrangler configuration records project identity and output directory; Cloudflare account connection and branch controls remain dashboard setup.
- Do not watch OpenSpec paths in the engineering-site workflow. Ignore `openspec/**`, `src/public-site/**`, and the public-site workflow in the post-merge runtime probe, preserving the probe for mixed or application-source pushes.

## Risks / Trade-offs

- Workflow path filters can drift from route tests. → Keep deterministic classifier cases for OpenSpec-only, public-site-only, mixed, and empty diffs, and assert both workflow path sets.
- Cloudflare Git integration, Build watch paths, and branch controls require one-time account configuration. → Document the repository root, build command, output directory, preview behavior, and production-disablement state without storing credentials.
- A mistaken production toggle could publish the site before domain review. → Keep production branch deployments disabled until the Owner approves publication; do not configure the custom domain here.
- Engineering snapshots and the public site live on separate hosts. → Keep their workflow triggers, artifacts, documentation, and deployment setup explicitly separate.
