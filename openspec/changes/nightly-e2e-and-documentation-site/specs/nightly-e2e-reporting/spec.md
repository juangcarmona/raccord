<!-- pdac-scope: none reason="Engineering test evidence; does not define product behavior." -->

## Purpose

Publish discoverable visual evidence from the current `main` browser journeys without weakening the PR-blocking E2E selection.

## ADDED Requirements

### Requirement: Nightly E2E runs only when main advances

The scheduled E2E workflow SHALL compare the current `main` SHA with the SHA in the latest successfully published nightly report and SHALL run the complete manifest-verified, non-quarantined selection only when those SHAs differ. A manual dispatch SHALL run the selection by default.

#### Scenario: Main has not advanced

- **WHEN** the scheduled workflow starts and `main` matches the latest published report SHA
- **THEN** it skips AppHost startup and E2E execution and does not create a new report

#### Scenario: Main has advanced

- **WHEN** one or more commits have reached `main` since the last published report
- **THEN** the workflow tests that exact current `main` SHA with the complete selection verified by `.github/scripts/check-e2e-selection.sh`

### Requirement: Nightly journeys publish visual evidence and their verdict

When nightly visual capture is enabled, each started journey SHALL write its outcome, a full-page screenshot for a passing result or failure, a trace, and a browser video. Report generation and publication SHALL be attempted after test failure, while the E2E job SHALL retain the failed test verdict.

#### Scenario: Passing journey

- **WHEN** a selected journey completes successfully in the nightly run
- **THEN** its gallery entry shows its passing result, screenshot, video, and trace when produced

#### Scenario: Failing journey

- **WHEN** a selected journey fails
- **THEN** its gallery entry shows the failed result and available failure screenshot, video, and trace, and the workflow remains failed

#### Scenario: Runner fails before any journey result

- **WHEN** AppHost startup or test discovery fails before a journey begins
- **THEN** the published report says that no journey result was recorded and links to the workflow run

### Requirement: Reports are public, attributable, stable and bounded

Each published report SHALL identify its tested main SHA, UTC timestamp, outcome, and workflow run URL, and SHALL be reachable from a stable latest URL and a retained report index. Reports SHALL be publicly viewable, contain only synthetic test data, and expire after 30 days or when the 14-report/800 MiB limit is exceeded, whichever occurs first.

#### Scenario: Report is published

- **WHEN** a nightly run produces a report
- **THEN** its immutable run URL appears in the E2E report index and the stable latest URL resolves to it

#### Scenario: Retention limit is reached

- **WHEN** a report exceeds the age, count, or size retention bound
- **THEN** the next site publication removes the expired report while preserving the newest report and documentation snapshots
