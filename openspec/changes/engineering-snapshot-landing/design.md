<!-- pdac-scope: none reason="Engineering snapshot presentation and generator design; no product semantics are defined here." -->

# Design: engineering snapshot landing page

## Rendering and timestamps

The static site generator reads the latest retained report manifest to render the landing-page status, run URL, revision, journey results and machine-readable run timestamp. Client-side formatting uses `Intl.DateTimeFormat` with the browser's default locale and time zone. The source timestamp remains in the `<time datetime>` attribute, and its initial text identifies UTC for visitors without JavaScript.

## Truthful run state

The landing page distinguishes a report with journey results from one with an empty journey list. An empty list is presented as “No results” and explains that workflow success does not establish successful test discovery. Journey totals are shown when available. Missing report metadata receives an explicit unavailable message rather than fabricated counts.

## Shared appearance

Use the arc42 snapshot's light/dark color tokens and the same `data-appearance` behavior and `raccord-appearance` local-storage key. The control offers Auto, Light and Dark; Auto follows the operating-system preference. There are no new assets or production dependencies.

## Responsive and accessible layout

Maintain the current title, run panel and three destination cards. Use a three-column card grid on wide screens, two columns at intermediate widths and one column on narrow screens. Preserve visible keyboard focus, semantic landmarks, labeled theme controls, reduced-motion support, and readable status text that does not rely on color alone.

## Verification

Static-site tests cover locale-formatting hooks, theme controls and persistence, journey counts, and the empty-results/successful-workflow edge case. Validate the emitted inline script syntax and inspect desktop/mobile rendering before merge.
