<!-- pdac-scope: none reason="This is a public-site rendering and responsive layout correction with no product behavior or product-model semantics." -->

# Design: Public brand and architecture diagram

## Decisions

- Reuse `src/Raccord.Frontend/public/raccord-mark.svg` as the canonical existing mark and copy it into the generated site's root.
- Use relative asset URLs derived from each route's depth, preserving working links on nested pages and both locales.
- Place the browser and application on the first row, then place PostgreSQL and document storage in two equal columns below the application. On narrow screens, stack all nodes in reading order.
- Keep the diagram's existing textual accessible name and mark the decorative logo image as hidden from assistive technology because the adjacent wordmark names the link.
- Add deterministic build assertions for emitted markup and asset resolution. Capture desktop and mobile browser evidence and check node rectangles for overlap.

## Verification

Run the public-site build tests and site verification. Open the generated English and Spanish architecture routes at desktop and mobile widths, verify no node rectangles overlap, and capture evidence under the public-site bounded context.
