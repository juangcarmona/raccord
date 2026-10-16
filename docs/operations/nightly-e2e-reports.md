# Nightly E2E reports and public snapshots

The public [Raccord engineering snapshots](https://juangcarmona.github.io/raccord/) contain the [arc42 architecture](https://juangcarmona.github.io/raccord/architecture/), the [ProductShape model](https://juangcarmona.github.io/raccord/model/), and [nightly E2E reports](https://juangcarmona.github.io/raccord/e2e/). The stable latest-run URL is [e2e/latest.html](https://juangcarmona.github.io/raccord/e2e/latest.html).

## Nightly runs

The scheduled workflow checks the current `main` commit against the latest published report. It runs the complete manifest-verified, non-quarantined Playwright selection only when `main` has advanced. A failed test run still publishes the screenshots, videos, traces and run link that were produced, and the E2E job remains red. A manual `workflow_dispatch` runs the selection on demand by default.

Each report identifies the tested commit and workflow run, shows the journey outcomes, and provides full-page screenshots, playable WebM recordings and trace downloads. Report artifacts contain fictional data seeded by the E2E AppHost; do not add real or sensitive data to journeys because the site is public.

Reports are retained for at most 30 days and at most 14 runs, with an 800 MiB total E2E-report budget. GitHub Actions also retains a recovery artifact for seven days. Architecture and product snapshots are rebuilt on pushes that change their canonical source paths, not on every commit; the engineering-site publisher serializes its GitHub Pages updates. Public-homepage builds and hosting use the separate [public-site deployment path](public-site-deployment.md).

## One-time repository setup

Before the first successful publication, a repository administrator must enable GitHub Pages for this public repository and set its publishing source to **GitHub Actions** in **Settings → Pages**. The workflow uses the `github-pages` deployment environment and the `pages: write` plus `id-token: write` permissions; it does not need a cloud credential or a personal access token. The first publisher run builds both documentation snapshots if the site is empty.

If Pages is disabled or a report deployment fails, the workflow reports the failure and the previous published site remains available. The E2E recovery artifact is still retained when the test job reached its upload step.
