---
name: end-to-end-testing
description: End-to-end journeys boot the real Aspire AppHost, drive the SPA in a real headless browser, and run a pinned PR-blocking selection.
status: Accepted
date: 2026-10-10
deciders: ["Juan G. Carmona"]
tags: ["testing", "e2e", "playwright", "aspire", "ci"]
---

# ADR-0017: Run end-to-end journeys against the real AppHost in a real browser with a pinned selection

## Context

ADR-0014 established the testing pyramid: domain units, Testcontainers-backed integration tests, architecture rules. The missing top of the pyramid was journeys that prove the _assembled_ application — the money path a person actually experiences. Integration tests exercise handlers against a database; they never catch the SPA's routing, the Vite proxy wiring, the locale switch or a broken label.

## Decision

We will run end-to-end journeys against the real AppHost in a real browser, with a pinned PR-blocking selection.

- **Real AppHost, not mocks.** `Raccord.EndToEndTests` boots the unmodified Aspire AppHost via `DistributedApplicationTestingBuilder<Projects.Raccord_Aspire>()` (Aspire.Hosting.Testing). Postgres is overridden to `ContainerLifetime.Session` — a fresh, migrated-and-seeded database per run; the dev persistent volume default is deliberately not used. The suite waits for the `web` resource to report healthy before any journey runs.
- **Real browser.** Playwright (Chromium, headless) drives the frontend resource's external URL through the AppHost's port. Contexts pin `Locale = "en-US"` because the SPA picks es/en from `navigator.language` and journeys assert on localized labels — leaving the locale to the runner would make the suite's meaning depend on where it runs. Assertions target accessible roles and labels, never implementation selectors.
- **Journeys encode the money path.** `BrowserJourneyBase` owns the per-journey browser context (tracing on; trace + screenshot artifacts under `playwright-artifacts/` on failure) and shared step helpers: customer → project + billable line → draft → issued invoice. Four smoke journeys today: boot, customer, billable line, invoice finalize.
- **Uniqueness per run.** Names stamped with a per-journey `RunStamp` (cached ticks suffix) so journeys never collide with the demo seed or each other while sharing one database. The stamp is computed once per journey — a fresh ticks value per read made a journey search for a name it never created.
- **Pinned selection.** The PR-blocking filter (`Quarantine!=true`) must match the committed `per-pr-selection.txt` manifest, verified by `.github/scripts/check-e2e-selection.sh` before the journeys run — so a narrowed filter is a red build, not a quieter suite. Quarantined journeys are visible, never silently dropped.
- **CI placement.** A dedicated `e2e` job (Playwright chromium `--with-deps`, manifest check, journey run, artifacts on failure). The backend job excludes the E2E project from its test run so numbers stay comparable.

## Consequences

- **Positive:**
  - Journeys prove the assembled application, including the SPA's routing, the Vite proxy wiring, the locale switch and labels.
  - The manifest makes any attempt to shrink the selection loud.
- **Negative:**
  - The e2e job is the slowest CI leg (a cold AppHost boot runs the frontend's `npm ci`); accepted — PR-gated smoke selection keeps it bounded, and the manifest makes any attempt to shrink the selection loud.
  - Journeys are order-coupled to the seeded demo data and issuer profile; that's the point (they run the real stack), and the session-lifetime database keeps it deterministic per run.
- **Neutral:**
  - Label matching in the step helpers is deliberately substring-based: optional fields render their label as "X (optional)", so exact matching would miss them.

## Alternatives considered

- **Mocked backend instead of the real AppHost:** rejected because it would not prove the assembled application.
- **The dev persistent volume:** deliberately not used; a fresh, migrated-and-seeded session database per run keeps journeys deterministic.
- **Leaving the browser locale to the runner:** rejected because it would make the suite's meaning depend on where it runs.
- **A fresh ticks value per read for run stamps:** rejected because it made a journey search for a name it never created.
- **Exact label matching:** rejected because optional fields render their label as "X (optional)".

## References

- Extends: ADR-0014 (testing strategy)
- Related: ADR-0013, ADR-0016
- Selection check: `.github/scripts/check-e2e-selection.sh`
