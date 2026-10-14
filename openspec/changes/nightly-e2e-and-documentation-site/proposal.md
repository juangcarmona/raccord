<!-- pdac-scope: none reason="CI evidence publication and documentation rendering; changes no product semantics." -->

## Why

The blocking E2E suite currently preserves visual evidence only on failure and only as a short-lived workflow artifact. Maintainers cannot browse the latest app journeys alongside the public architecture and model snapshots from a stable URL.

## What changes

- Add an unattended nightly run of the complete, pinned non-quarantined Playwright selection when `main` has advanced since the last published run.
- Capture passing and failing journey screenshots and videos, generate a searchable static gallery, and publish immutable per-run reports plus a stable latest link.
- Publish arc42 and ProductShape snapshots on changes to their respective source paths using the same public static site.
- Bound retained reports, serialize site publications, and document public access, manual reruns and one-time Pages setup.

## Capabilities

### New capabilities

- `nightly-e2e-reporting`: scheduled visual evidence and a browsable report for the current `main` app.
- `documentation-snapshots`: public, regenerable arc42 and ProductShape snapshots refreshed on source changes.

### Modified capabilities

- `e2e-testing`: retain failure evidence and add opt-in success screenshots and videos for nightly reporting.

## Impact

GitHub Actions, Playwright test infrastructure, static-site tooling, arc42/ADR documentation, and operational documentation change. Product APIs, domain behavior, runtime deployment topology and product semantics do not change. The Pages site is public and contains only fictional E2E seed data.
