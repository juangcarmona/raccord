<!-- pdac-scope: none reason="CI and static delivery infrastructure do not alter product behavior or product requirements; this exemption is approved by the Owner for issue #247." -->

## Why

Repository documentation updates and public-homepage edits currently start application, browser, and packaging jobs that cannot exercise those changes. The existing static publisher also watches all of `src/**` and owns one combined GitHub Pages artifact, so adding a separate public website would couple its deployment to engineering snapshots and nightly evidence.

## What Changes

- Classify non-empty diffs wholly under `openspec/` or `src/public-site/` (including the public-site workflow) so they retain documentation and security checks while skipping unrelated application, E2E, frontend, packaging, and deployment jobs.
- Keep mixed or ambiguous diffs on the full CI route and keep the post-merge runtime probe for application changes.
- Rename `static-site.yml` to `engineering-site.yml`, preserve its serialized GitHub Pages publisher and E2E evidence, and watch `src/**` while excluding `src/public-site/**` so public-site-only changes do not run engineering-site work.
- Add `public-site.yml` to validate the public site on relevant pull requests and `main` updates, and verify that its build output contains only the public-site assets.
- Prepare Cloudflare Pages Git integration through a checked-in Wrangler configuration and operational setup instructions, with Cloudflare builds limited to `src/public-site/*`. Preview deployments use non-production branches; `main` is the designated production branch, with production deployments and the custom domain left disabled until Owner approval.

## Capabilities

No product capability changes. This is repository CI and static delivery infrastructure; `skip_specs: true` is used because it introduces no product or runtime behavior.

## Impact

Affected systems are `.github/workflows/ci.yml`, `.github/workflows/post-merge.yml`, `.github/workflows/engineering-site.yml`, `.github/workflows/public-site.yml`, CI routing scripts, `src/public-site/` build configuration, and engineering/public-site operations documentation. The existing PR security checks and serialized engineering publisher remain active. Cloudflare deployment configuration is isolated from the existing GitHub Pages artifact and does not add a repository-held deployment credential.
