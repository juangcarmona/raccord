# Color tokens

These are the exploratory v1 tokens. They should become CSS variables and, later, design-token source files shared by React components.

| Token | Value | Intended use |
|---|---|---|
| `--raccord-ink` | `#0B1020` | Primary text, navigation, dark surfaces |
| `--raccord-cyan` | `#2DE2E6` | Primary action, links, active states |
| `--raccord-coral` | `#FF6B5E` | Warnings, highlights, small signal accents |
| `--raccord-cloud` | `#F5F7FA` | Light application backgrounds |
| `--raccord-slate` | `#667085` | Secondary text and metadata |
| `--raccord-white` | `#FFFFFF` | Cards and high-contrast text |
| `--raccord-selected` | `#0B7FBF` | Selected rows, checkboxes and selection bars (decided 2026-10-10: blue, not cyan) |

Selection tints (from the invoice-list screen): row fill `#EDF6FC`, badge fill `#D6EBF7`.

## Status tints

Statuses pair a text color with a pale background; both are shown so status remains readable in grayscale:

| Status kind | Text | Background |
|---|---|---|
| Paid / success | `#1D7A4F` | `#E3F5EE` |
| Warning / pending | `#8A5A00` | `#FFF3D6` |
| Error / delayed | `#B3261E` | `#FDE3E0` |

## Semantic guidance

Do not use the brand cyan or coral as the only indicator of success, warning, or error. Pair color with text, an icon, or a state label. Tax and invoice statuses must remain understandable in grayscale and for color-vision differences.

## Dark mode

The dark surface should use ink as the base, but content areas should not become a wall of near-black. Use cloud or a slightly tinted surface for working areas and preserve cyan focus states with sufficient contrast.
