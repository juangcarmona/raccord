<!-- pdac-scope: cited -->

## Why

The shipped management screens do not yet express the product’s established visual direction, despite a substantial design system and screen portfolio in `docs/design/` and `docs/product/design/`. Align the existing routes so the implemented application feels like the documented Raccord product without pretending that unimplemented mockup workflows already exist.

## What Changes

- Apply the established visual language consistently to the existing application shell and in-scope project, customer, draft, invoice, and issuer-settings screens.
- Record the implemented route-to-design/trace mapping and any gaps; preserve existing API-backed behavior, bilingual copy, keyboard access, and responsive use.
- Record frontend ownership and dependency foundations for the existing `app/`, `features/`, and `shared/` structure without requiring a folder migration.
- Do not add product workflows, APIs, dependencies, or unsupported mockup interactions.

## Capabilities

### New Capabilities

- `implemented-screen-conformance`: Existing implemented management screens present consistent, accessible visual patterns grounded in the approved design references.

### Modified Capabilities

- None. This changes presentation, not product requirements or workflows.

## Impact

Frontend React/CSS and frontend tests; a design-reference mapping document; and an architecture decision record. No API, database, domain, dependency, or deployment contract changes. Product journeys and quality requirements checked: project operation, finished-project billing, invoicing setup, accessibility, and localization. Their widened impact was reviewed; access control remains unchanged. The warehouse/mobile PWA quality requirement was checked and excluded because these are management routes, not field/warehouse workflows. No product-definition drift identified.

<!-- pdac:cite id="JRN-PRJ-RUN-RENTAL-PROJECT" digest="sha256:2fc29ff0184c294517c659ec8fc12660b4f1262d49a53426103d87058fbb51fe" -->
<!-- pdac:cite id="JRN-BILL-FINISHED-PROJECT" digest="sha256:b9530a2b0a9aac0f9cb3a5a9608494d08be1fc1190f395887fc7bb7fb30df28e" -->
<!-- pdac:cite id="JRN-SET-UP-INVOICING" digest="sha256:a47b84f924d17095df0ad8bc4e3436fa67d9d51bd1d241c7228a7624cb8c548e" -->
<!-- pdac:cite id="QR-ACCESSIBILITY" digest="sha256:65d1d92daf2189c837030c9eaaf7d0ba18a28a0f52cea624e919f6fbc80ac882" -->
<!-- pdac:cite id="QR-LOCALIZATION" digest="sha256:defb01ff947a5d42bdc3b6b2fe7d58c7d3aead62e91f759c0118f40749654ddf" -->
