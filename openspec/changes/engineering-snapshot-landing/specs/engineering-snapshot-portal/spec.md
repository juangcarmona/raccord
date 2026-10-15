<!-- pdac-scope: none reason="Defines public engineering evidence presentation, not user-facing product capabilities or domain semantics." -->

## ADDED Requirements

### Requirement: Landing page presents latest browser evidence accurately

The engineering snapshot landing page MUST present the latest retained report's tested revision, readable run time, and journey result state without treating workflow success as proof that journeys were recorded.

#### Scenario: Latest report contains journey results

- **WHEN** the static site is generated with a latest report containing journey results
- **THEN** the landing page shows a browser-localized run time, links to the workflow and report, and displays recorded journey totals

#### Scenario: Latest report contains no journey results

- **WHEN** the workflow reports success but the retained report contains no journey results
- **THEN** the landing page explicitly says no journeys were recorded and does not label the run as a successful journey run

### Requirement: Snapshot landing appearance is user-selectable and consistent

The landing page MUST provide Auto, Light and Dark appearance choices consistent with the architecture and product-model snapshots and MUST retain the selected choice for other snapshot pages.

#### Scenario: Visitor selects an appearance

- **WHEN** a visitor chooses Auto, Light or Dark
- **THEN** the page applies that appearance, updates the selected control state, and stores the shared preference
