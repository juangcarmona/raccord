# Visual direction

## Brand character

- Precise, not sterile.
- Professional, not corporate-generic.
- Audiovisual, without cameras, film reels, clapperboards, or play-button clichés.
- Operational and trustworthy, with one energetic signal color.
- Comfortable in dark control-room contexts and bright office environments.

## Product surfaces

The product UI is the primary brand surface. Prefer:

- clear status badges;
- strong tables and filters;
- compact but generous spacing;
- predictable forms;
- visible audit history;
- restrained motion used to communicate state changes;
- dark navigation with light work surfaces, where appropriate.

Avoid:

- decorative gradients behind dense data;
- excessive rounded cards;
- color-only status communication;
- tiny low-contrast metadata;
- visual metaphors that hide operational meaning.

## Shared visual system

The authenticated app, public site, and engineering site use the shared values in [`tokens.css`](tokens.css) for color, typography, spacing, shape, focus, and semantic status. Their information architecture and page-specific layout may differ, while shared patterns such as links, buttons, surfaces, status labels, and keyboard focus use the same roles. Document any renderer-specific color literal in [`token-exceptions.md`](token-exceptions.md) and keep it aligned with the canonical token values.

## Typography

Use a highly legible sans-serif with strong tabular numerals. The final font choice must be checked for licensing, Spanish glyph coverage, number readability, and rendering across Windows and macOS.

Suggested hierarchy:

- Display: product moments and empty states.
- Heading: page and section titles.
- Body: operational content.
- Label: fields, filters, statuses, and table metadata.
- Numeric: invoice totals, quantities, dates, and metrics.
