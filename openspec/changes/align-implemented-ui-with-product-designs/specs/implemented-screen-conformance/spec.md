## Purpose

Defines how existing management screens consistently apply Raccord’s documented visual system while retaining supported product behavior and accessible, localized use.

## ADDED Requirements

### Requirement: Existing screens follow documented design references

The application SHALL apply the documented Raccord visual direction and shared design tokens consistently across its shell and implemented project, customer, invoice-draft, invoice, and issuer-settings routes. Each route SHALL have a recorded closest screen reference and trace, or an explicit documented gap where no exact reference exists. A reference SHALL NOT imply unsupported behavior.

#### Scenario: Route has a closest design reference

- **WHEN** a user opens an in-scope implemented route
- **THEN** its composition and shared visual patterns follow the documented closest reference to the extent supported by existing behavior

#### Scenario: Reference depicts unsupported workflow

- **WHEN** a design reference contains controls or workflow steps absent from the implemented product behavior
- **THEN** the screen does not present those controls as functional and the mapping records the limitation

### Requirement: Visual alignment preserves accessible localized behavior

The application SHALL retain existing API-backed behavior, English and Spanish localization, semantic structure, keyboard operation, visible focus, and usability at desktop and narrow viewport widths while applying the design system.

#### Scenario: Keyboard user navigates a screen

- **WHEN** a user operates an in-scope route using only a keyboard
- **THEN** interactive controls remain reachable, visibly focused, and semantically identifiable

#### Scenario: User switches locale or viewport

- **WHEN** a user views an in-scope route in either supported language or at a narrow viewport
- **THEN** content remains legible and usable without losing existing route behavior

<!-- pdac-scope: cited -->
<!-- pdac:cite id="JRN-BILL-FINISHED-PROJECT" digest="sha256:b9530a2b0a9aac0f9cb3a5a9608494d08be1fc1190f395887fc7bb7fb30df28e" -->
<!-- pdac:cite id="QR-ACCESSIBILITY" digest="sha256:65d1d92daf2189c837030c9eaaf7d0ba18a28a0f52cea624e919f6fbc80ac882" -->
<!-- pdac:cite id="QR-LOCALIZATION" digest="sha256:defb01ff947a5d42bdc3b6b2fe7d58c7d3aead62e91f759c0118f40749654ddf" -->
