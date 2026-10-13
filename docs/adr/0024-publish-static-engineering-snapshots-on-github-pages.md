---
name: publish-static-engineering-snapshots-on-github-pages
description: Publish public architecture, product-model, and nightly browser evidence as static GitHub Pages content.
status: Accepted
date: 2026-10-13
deciders: [Juan G. Carmona]
tags: [documentation, testing, deployment, github-pages]
---

# ADR-0024: Publish static engineering snapshots through GitHub Pages

## Context

The arc42 architecture and ProductShape model are maintained as Markdown in the repository. Playwright E2E journeys run in CI and currently expose failure artifacts only through short-lived workflow artifacts. There is no static documentation host or published browser-evidence gallery, and maintainers need a stable way to inspect current architecture, product intent, and the latest tested application state.

The repository is public and the E2E AppHost uses fictional demo data. Browser videos and screenshots are therefore suitable for public viewing, provided the report pipeline publishes only synthetic test output and no credentials or production data.

## Decision

We will publish these regenerable static views together through the repository's GitHub Pages site: an arc42 snapshot, a ProductShape model snapshot, and per-run nightly Playwright galleries. One serialized workflow owns all site publication. It rebuilds the architecture and product snapshots only when their canonical source paths change, runs E2E nightly only when `main` has advanced since the latest published E2E run, and retains at most 14 reports and 30 days of history within an 800 MiB report budget.

The canonical sources remain in Git. GitHub Pages is a read-only public rendering, not a system of record. The workflow runs with least-privilege Pages deployment permissions and publishes only the fictional seeded E2E data.

## Consequences

- **Positive:** Maintainers have stable, public URLs for the current architecture, product model, and latest E2E screenshots/videos; the PR E2E gate remains independent of hosting availability.
- **Negative:** Reports are publicly accessible, each deployment restores retained files from the public Pages site, and the repository must enable Pages with GitHub Actions as its source. Retention and site-size bounds must be enforced by the publisher.
- **Neutral:** Snapshot outputs are regenerated and never committed as canonical documentation. A Pages deployment failure makes the publishing job fail and leaves the previous published site intact.

## Alternatives considered

- **GitHub Actions artifacts only:** rejected because artifacts expire and do not provide a stable, browsable latest-report URL.
- **Azure static website storage:** rejected because this repository has no existing documentation host or CI identity for it, so it would add cloud resources and credential setup solely for static output.
- **Rebuild the architecture and model snapshots on every commit:** rejected because those snapshots are derived from source; path-based refreshes avoid needless browser rendering and site deployments.

## References

- Issue: [#134](https://github.com/juangcarmona/raccord/issues/134)
- Related: ADR-0017 (E2E testing strategy), ADR-0013 (deployment topology)
- Operations: [nightly E2E reports](../operations/nightly-e2e-reports.md)
