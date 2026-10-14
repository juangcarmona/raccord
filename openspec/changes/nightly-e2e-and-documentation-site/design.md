<!-- pdac-scope: none reason="CI evidence publication and documentation rendering; changes no product semantics." -->

## Decision: one public static site and one publisher

Use the repository's GitHub Pages site for all generated documentation and E2E output. One workflow handles nightly, manual, and source-path push events and uses a shared non-cancelling concurrency group. It restores the current site using a generated file manifest, updates only the relevant content, prunes expired reports, and deploys one complete site artifact. This prevents separate publishers from replacing each other's pages.

The site is explicitly public. E2E runs use the existing Aspire test fixture and fictional seed data; no live application, external database, production secrets, or cloud resources are involved. Pages deployment is isolated to a job with `pages: write` and `id-token: write`.

## Decision: report evidence and verdict are separate

Visual evidence capture is opt-in through `PLAYWRIGHT_CAPTURE_EVIDENCE`, keeping ordinary PR runs' artifact volume unchanged. The nightly job enables it, writes a result record per journey, and records videos after closing the browser context. A report-generation step runs even when the test step fails; the E2E job preserves the nonzero verdict while the Pages publisher uses `always()` to publish available evidence. If no journey starts, the report states that explicitly and links the failed workflow.

## Decision: bounded site state

The static site has a generated manifest so each deployment can reconstruct its existing content without a write token or mutable artifact store. Reports expire after 30 days, are capped at 14 runs and have an 800 MiB aggregate E2E evidence budget. The static index exposes both the latest stable URL and retained immutable run pages.

## Alternatives and constraints

- Azure static hosting would introduce a new cloud resource and CI identity not present in Raccord's current documentation path.
- GitHub Actions artifacts alone expire and do not provide a stable static latest-report URL.
- Rebuilding and redeploying snapshots for every source commit would waste time; the push path filter and source diff limit refreshes to the architecture or model that changed.
- The repository administrator must enable Pages with GitHub Actions as the publishing source once. No personal access token is added to repository secrets.
