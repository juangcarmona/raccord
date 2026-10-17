## Purpose

The app, public site, and engineering site share one Raccord visual language while keeping each surface's content and navigation structure appropriate to its audience.

## ADDED Requirements

### Requirement: Shared visual tokens

The app, public site, and engineering site SHALL consume one canonical set of Raccord color, typography, spacing, shape, focus, and semantic-status tokens. Surface styles MAY define aliases for layout, but MUST NOT redefine canonical token values. The canonical guidance SHALL identify the approved values and SHALL NOT describe the active system as exploratory.

#### Scenario: A surface is built

- **WHEN** the app or either site is built
- **THEN** its shared visual tokens resolve to the canonical values documented in `docs/design/`

#### Scenario: Shared tokens drift

- **WHEN** a consumer redefines a canonical token or diverges from the shared token source
- **THEN** the design-system verification check fails and identifies the consumer and token

### Requirement: Design-system drift is checked locally and in CI

The repository SHALL provide one dependency-free design-system verification check. The Husky pre-commit hook SHALL run it for local feedback. CI SHALL run the same check for every pull request and every push to main, independently of the changed-path classifier, and report a failure when shared tokens drift.

#### Scenario: Contributor commits a change that introduces token drift

- **WHEN** the design-system check finds a shared token redefined or mismatched in the working tree
- **THEN** the pre-commit hook rejects the commit and identifies the affected surface and token

#### Scenario: Pull request changes only the public site

- **WHEN** a pull request changes only public-site, engineering-site, or design documentation paths
- **THEN** the independent CI job still runs the design-system check and reports its result

#### Scenario: Contributor bypasses local hooks

- **WHEN** a contributor skips Husky hooks and pushes a change with token drift
- **THEN** the CI check still runs and reports the drift on the pull request

### Requirement: Shared interaction patterns

Navigation, links, buttons, surfaces, visible keyboard focus, and semantic status treatments SHALL use the shared visual language across the app and both sites while retaining each surface's information architecture and existing behavior. Status meaning MUST remain identifiable without color alone.
<!-- pdac:cite id="QR-ACCESSIBILITY" digest="sha256:65d1d92daf2189c837030c9eaaf7d0ba18a28a0f52cea624e919f6fbc80ac882" -->

#### Scenario: User moves between product surfaces

- **WHEN** a user encounters navigation, a link, a button, a surface, or a status treatment in any of the three surfaces
- **THEN** its visual treatment and interaction states are consistent with the canonical design guidance

#### Scenario: User navigates by keyboard

- **WHEN** a user reaches an interactive element using a keyboard
- **THEN** the element has a visible focus indicator and remains operable

### Requirement: Public locale entry is branded and resilient

The public root route SHALL present a styled, accessible route into the Spanish and English public pages, including usable language links when JavaScript is unavailable. Its shared visual tokens and interaction patterns SHALL match the canonical system.
<!-- pdac:cite id="QR-LOCALIZATION" digest="sha256:defb01ff947a5d42bdc3b6b2fe7d58c7d3aead62e91f759c0118f40749654ddf" -->

#### Scenario: Browser requests the public root

- **WHEN** a visitor opens `/` with JavaScript available or unavailable
- **THEN** the visitor can reach a complete Spanish or English homepage through the locale handoff

#### Scenario: Visitor chooses a language

- **WHEN** a visitor selects Spanish or English at the root route
- **THEN** the corresponding localized homepage opens and language choice remains operable by keyboard

### Requirement: Cross-surface visual evidence

Representative app, public-site, and engineering-site pages SHALL be reviewed at desktop and narrow viewport widths. The review SHALL verify English and Spanish where both locales exist, keyboard focus, readable status meaning, and the shared design tokens; the evidence SHALL be attached to the pull request.
<!-- pdac:cite id="QR-ACCESSIBILITY" digest="sha256:65d1d92daf2189c837030c9eaaf7d0ba18a28a0f52cea624e919f6fbc80ac882" -->
<!-- pdac:cite id="QR-LOCALIZATION" digest="sha256:defb01ff947a5d42bdc3b6b2fe7d58c7d3aead62e91f759c0118f40749654ddf" -->
<!-- pdac:cite id="QR-MOBILE-WEB-APP" digest="sha256:ba2d4a17bef47a1512bec2d45e4c9f8fdd6e2fab7a438280959a62ba70bdb18b" -->

#### Scenario: Owner reviews representative pages

- **WHEN** the planned visual review is completed
- **THEN** desktop and narrow captures cover all three surfaces, both available locales, and record any known surface-specific exceptions

<!-- pdac-scope: cited -->
