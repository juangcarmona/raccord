## Context

The SPA already has working API-backed routes but uses generic styling. The source of visual direction is `docs/design/`; the screen portfolio and behavior traces are in `docs/product/design/`. The closest references include project creation/detail, contact detail, invoicing queue/list/draft editor, and issuer profile. Several references depict richer workflows than the current API/UI supports.

## Goals / Non-Goals

**Goals:** Bring existing routes into the visual system; make route/reference/gap decisions discoverable; validate desktop and narrow layouts, keyboard access, and both locales.

**Non-Goals:** New behavior, API/domain changes, replacement of React/Vite, a component-library dependency, or implementation of every control shown in design concepts.

## Decisions

- Treat `docs/design/visual-direction.md`, `color-tokens.md`, and logo guidance as design-system authority; treat screen HTML/PNG and `screen-traces.md` as route-level references. The existing design-system agent skill is at `.agents/skills/raccord-design-system/SKILL.md` in the companion design-system worktree; keep its unmerged changes isolated.
- Keep the accepted React/TypeScript/Vite, semantic HTML, and CSS approach; no new dependency or architecture exception.
- Document the existing frontend structure in ADR-0023: thin `app/` composition, capability-owned `features/`, a small cross-cutting `shared/` kernel, and dependency direction `app → features → shared`. Keep the rule proportional; this change does not migrate folders or add a new layer taxonomy. Automated enforcement is not claimed until a CI boundary check exists.
- Map `/projects`, `/projects/new`, `/projects/:id` to project list/creation/detail references; `/customers` and `/customers/new` to contact-detail patterns with an explicit list/form gap; `/drafts` and `/drafts/:id` to invoicing queue/draft editor; `/invoices` and `/invoices/:id` to invoice list/detail patterns; `/settings/issuer` to issuer profile. The shared shell follows the common dark navigation/light workspace direction.
- Use existing API capabilities as the hard boundary: omit mockup-only template selection, extra profile fields, payment/filter interactions, and any unimplemented workflow. Document these limitations in `docs/product/design/implemented-screen-mapping.md`.
- Use tokens for shared colors, spacing, typography, focus, surfaces, and status treatments; preserve contrast and avoid relying on color alone.
- Use the selected blue token for keyboard focus rings rather than the exploratory cyan accent: it remains visible against the light workspace and dark navigation surfaces.
- Considered a UI component-library migration and rejected it: this is not needed to close a visual gap and would expand the architectural/dependency scope.

## Risks / Trade-offs

The reference portfolio includes aspirational concepts and may imply more functionality than the current application. Route mapping and explicit omissions mitigate that risk. Screenshot review is needed because passing unit tests cannot establish visual conformance.

<!-- pdac-scope: cited -->
<!-- pdac:cite id="JRN-PRJ-RUN-RENTAL-PROJECT" digest="sha256:2fc29ff0184c294517c659ec8fc12660b4f1262d49a53426103d87058fbb51fe" -->
<!-- pdac:cite id="JRN-BILL-FINISHED-PROJECT" digest="sha256:b9530a2b0a9aac0f9cb3a5a9608494d08be1fc1190f395887fc7bb7fb30df28e" -->
<!-- pdac:cite id="JRN-SET-UP-INVOICING" digest="sha256:a47b84f924d17095df0ad8bc4e3436fa67d9d51bd1d241c7228a7624cb8c548e" -->
<!-- pdac:cite id="QR-ACCESSIBILITY" digest="sha256:65d1d92daf2189c837030c9eaaf7d0ba18a28a0f52cea624e919f6fbc80ac882" -->
<!-- pdac:cite id="QR-LOCALIZATION" digest="sha256:defb01ff947a5d42bdc3b6b2fe7d58c7d3aead62e91f759c0118f40749654ddf" -->
