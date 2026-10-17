<!-- pdac-scope: cited -->

## Why

Issue #347 asks the authenticated app, the public site, and the engineering site to read as one Raccord product. They currently diverge: the app uses the exploratory navy/cyan palette in `docs/design/color-tokens.md`, the public site uses a separate forest/lime palette, and engineering reports and architecture snapshots carry other inline colors. The public root route also renders only an unstyled “Raccord” link until JavaScript redirects to a locale page.

## What Changes

- Promote the existing app token values as the proposed canonical base, complete the token set for typography, spacing, shape, focus, and semantic status, and replace the exploratory status in the design guidance.
- Make the app, public site, and engineering site consume one CSS token source while retaining surface-specific page structure and content.
- Apply common brand patterns to navigation, links, buttons, surfaces, focus, and status presentation. Give the public root route a styled, accessible locale handoff that remains usable without JavaScript.
- Add a deterministic check that fails when a surface redefines shared tokens or drifts from the canonical source.
- Review representative routes from all three surfaces at desktop and narrow widths, in English and Spanish where applicable, and attach visual evidence.

## Capabilities

### New Capabilities

- `cross-surface-design-system`: One canonical design-token source and shared visual patterns across the app, public site, and engineering site, with drift verification.

### Modified Capabilities

None.

## Impact

Affected sources include `docs/design/`, `src/Raccord.Frontend/`, `src/public-site/`, `tools/static-site/`, `tools/arc42-snapshot/`, and their build/test workflows. No API, database, deployment configuration, runtime dependency, product workflow, public claim, or page information architecture changes are planned.

Product-model review: the full ProductShape graph validated 1,880 artifacts. This is presentation-only and introduces no product-definition drift. The affected quality requirements are accessibility, localization, and responsive web usability; their direct and indirect neighbors were inspected. The related billing, invoicing, crew, and warehouse journeys and their business rules were left out because this change preserves their behavior and does not change those capabilities.

<!-- pdac:cite id="QR-ACCESSIBILITY" digest="sha256:65d1d92daf2189c837030c9eaaf7d0ba18a28a0f52cea624e919f6fbc80ac882" -->
<!-- pdac:cite id="QR-LOCALIZATION" digest="sha256:defb01ff947a5d42bdc3b6b2fe7d58c7d3aead62e91f759c0118f40749654ddf" -->
<!-- pdac:cite id="QR-MOBILE-WEB-APP" digest="sha256:ba2d4a17bef47a1512bec2d45e4c9f8fdd6e2fab7a438280959a62ba70bdb18b" -->
