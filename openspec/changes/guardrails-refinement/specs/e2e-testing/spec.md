## Purpose

End-to-end journeys prove the assembled application — the real Aspire AppHost, the real browser —
works the way a person experiences it, so "it compiles and the units pass" is never the whole
story. They are the top of the testing pyramid Raccord's other suites build toward.

## ADDED Requirements

### Requirement: Journeys drive the real SPA over the booted Aspire AppHost
Every E2E journey SHALL run Playwright against the Vite frontend resource of the unmodified
AppHost, booted once per run via `DistributedApplicationTestingBuilder` and shared by the whole
suite through the `E2ECollection` fixture.

#### Scenario: AppHost boot
- **WHEN** the E2E suite starts
- **THEN** Postgres runs with `ContainerLifetime.Session` (a fresh, migrated-and-seeded database per run; the dev persistent volume is deliberately overridden) and the suite only proceeds once the `web` resource reports healthy

#### Scenario: Browser drives the real surface
- **WHEN** a journey navigates
- **THEN** it drives the frontend resource's external URL with an English-locale Chromium context, and assertions target accessible roles and labels rather than implementation selectors

### Requirement: The per-PR selection is pinned by a committed manifest
The blocking E2E selection (filter `Quarantine!=true`) SHALL match the committed
`tests/Raccord.EndToEndTests/per-pr-selection.txt` manifest, verified by
`.github/scripts/check-e2e-selection.sh` in CI before the journeys run.

#### Scenario: Journey added
- **WHEN** a new journey is added
- **THEN** the guard fails until the manifest is regenerated (`check-e2e-selection.sh --update`) and committed with the change

#### Scenario: Filter narrowed silently
- **WHEN** the filter or a trait name changes so fewer journeys are discovered
- **THEN** the guard fails naming exactly which journeys stopped being selected, instead of the suite quietly shrinking

#### Scenario: Quarantined journey
- **WHEN** a journey carries the `Quarantine=true` trait
- **THEN** it is excluded from the blocking run and the manifest edit is reviewed alongside the trait, so a journey cannot leave the gate by attribute alone

### Requirement: Failed journeys leave evidence
A failed journey SHALL leave a Playwright trace and a full-page screenshot under
`playwright-artifacts/` (gitignored), uploaded as a CI artifact on failure.

#### Scenario: Journey fails in CI
- **WHEN** a journey throws
- **THEN** the CI job uploads its trace and screenshot so the failure is diagnosable from the run, without a local re-run
