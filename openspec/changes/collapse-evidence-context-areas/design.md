<!-- pdac-scope: none reason="This design changes only the static engineering evidence report, with no product-semantic dependency." -->

## Context

The evidence report renders its left navigation in `tools/static-site/static-site.mjs`. The ten canonical bounded contexts, Cross-cutting, and historical Unclassified groups are generated as separate navigation sections. The report already uses a small inline script for existing controls, but area disclosure does not need JavaScript.

## Decisions

- Render each area as a native `<details open>` disclosure with a `<summary>` title. Native disclosure controls provide keyboard operation and expanded-state semantics without another event handler or dependency.
- Keep each group initially open to preserve the current first-load view. The browser owns each group's independent open state for the current page session; no persistence is added.
- Keep the current journey links and empty-context message inside the disclosure body. Do not change journey records, result counts, filters, anchors, evidence assets, or the separate run-history section.
- Use the existing navigation styles and design tokens for the summary title and focus indicator. Retain the platform disclosure marker to communicate the control affordance.
- Do not change product specs or the PDaC model because this is an internal evidence-report presentation change.

## Alternatives considered

- A custom button plus JavaScript would require maintaining `aria-expanded`, focus behavior, and content visibility by hand. Native disclosure semantics are simpler and work without script.
- Start groups collapsed to shorten the initial page was rejected because it would hide existing evidence on first load and change the current browsing behavior.

## Risks and mitigations

- Styling could suppress the native disclosure marker or keyboard focus: preserve the browser marker and add a visible focus style; assert native `<details open>` and `<summary>` markup in tests.
- A markup change could detach links from the wrong area: keep the existing group contents unchanged and assert representative grouped, empty, and Unclassified content.

## Architecture impact

No deviation from `docs/architecture/architecture-rules.md`. No production dependency, API, or deployment configuration changes.
