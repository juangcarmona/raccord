# Color tokens

These are the canonical Raccord tokens, defined in [`tokens.css`](tokens.css). Consumers import or embed that file and may add layout-specific variables, but must not redefine shared values.

| Token | Value | Intended use |
| --- | --- | --- |
| `--raccord-ink` | `#0B1020` | Primary text, navigation, dark surfaces |
| `--raccord-cyan` | `#2DE2E6` | Primary action, links, active states |
| `--raccord-coral` | `#FF6B5E` | Warnings, highlights, small signal accents |
| `--raccord-cloud` | `#F5F7FA` | Light application backgrounds |
| `--raccord-slate` | `#667085` | Secondary text and metadata |
| `--raccord-white` | `#FFFFFF` | Cards and high-contrast text |
| `--raccord-selected` | `#0B7FBF` | Selected rows, checkboxes and selection bars (decided 2026-10-10: blue, not cyan) |
| `--raccord-selected-soft` | `#EDF6FC` | Selected row surfaces |
| `--raccord-selected-badge` | `#D6EBF7` | Selected badges |
| `--raccord-border` | `#D0D5DD` | Default outlines and dividers |
| `--raccord-action` | `#075A88` | Links and action text on light surfaces |
| `--raccord-action-hover` | `#08699E` | Hovered action surface |
| `--raccord-info` | `#075A88` | Informational status text |
| `--raccord-info-border` | `#B8DCEE` | Informational status outline |

Selection tints: `--raccord-selected-soft` (`#EDF6FC`) for row fill and `--raccord-selected-badge` (`#D6EBF7`) for badges.

Typography, spacing, radii, focus, semantic state colors, and dark-mode mappings are defined alongside the palette in `tokens.css`. Shared status colors always pair a semantic color with text or an icon.

## Status tints

Statuses pair a text color with a pale background; both are shown so status remains readable in grayscale:

| Status kind | Text | Background |
| --- | --- | --- |
| Paid / success | `#1D7A4F` | `#E3F5EE` |
| Warning / pending | `#8A5A00` | `#FFF3D6` |
| Error / delayed | `#B3261E` | `#FDE3E0` |

Success and error also have matching outline tokens: `--raccord-success-border` (`#B5E3D0`) and the semantic status borders use their status color where a separate border token is not defined.

## Typography, spacing, shape, and focus

The shared sans-serif and monospace stacks are `--raccord-font-sans` and `--raccord-font-mono`. Type sizes use `--raccord-text-xs` through `--raccord-text-xl`; spacing uses the four-point `--raccord-space-*` scale from `0.25rem` to `2rem`. `--raccord-radius-sm`, `--raccord-radius-md`, and `--raccord-radius-lg` define shared corners. Keyboard indicators use `--raccord-focus-width` (`3px`) and `--raccord-focus-offset` (`2px`). The full values live in `tokens.css`.

## Semantic guidance

Do not use the brand cyan or coral as the only indicator of success, warning, or error. Pair color with text, an icon, or a state label. Tax and invoice statuses must remain understandable in grayscale and for color-vision differences.

## Contrast

Contrast ratios below were computed from the token values using the WCAG relative-luminance formula. Normal text requires 4.5:1; large text and non-text controls require 3:1.

| Foreground | Background | Contrast | Use |
| --- | --- | ---: | --- |
| `--raccord-ink` | `--raccord-cloud` | 17.64:1 | Primary text |
| `--raccord-slate` | `--raccord-cloud` | 4.64:1 | Secondary text |
| `--raccord-action` | `--raccord-cloud` | 6.92:1 | Links and action labels |
| `--raccord-success` | `--raccord-success-soft` | 4.70:1 | Success status |
| `--raccord-warning` | `--raccord-warning-soft` | 5.37:1 | Warning status |
| `--raccord-error` | `--raccord-error-soft` | 5.37:1 | Error status |

The brighter `--raccord-selected` blue is reserved for selection fills and focus outlines, not small text. Action labels use the darker `--raccord-action`. Semantic status colors pair with their pale backgrounds and a visible label or icon. Recalculate both themes when changing values.

The corresponding dark-theme ratios are 17.64:1 for primary text, 10.34:1 for secondary text, 11.93:1 for links, and at least 6.70:1 for semantic status text on its status background.

## Dark mode

The semantic token values switch for dark preference. Consumers use the same semantic names and must retain readable working surfaces, borders, and focus indicators.
