# Token exceptions

CSS consumed by the app and sites must use semantic tokens for shared brand, surface, focus, and status roles. Page layout values and chart-specific data colors may remain local when they are documented and do not redefine shared roles.

The Mermaid diagram renderer requires literal color strings in its configuration. `tools/arc42-snapshot/render.mjs` mirrors canonical values for node fills, text, borders, and lines; the design-system checker verifies those literals still exist in `tokens.css`. The generated snapshot also embeds the canonical token stylesheet for its interface chrome.

The engineering report retains report-specific shadow and media viewport details; shadows use a `color-mix()` from the canonical ink token, and screenshot imagery is not a UI surface token.
