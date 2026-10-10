# pen.dev trial — working brief

Issue: juangcarmona/raccord#1 · Branch: `trial/pen-dev-design-canvas`

Goal: judge whether pen.dev (ex Pencil.dev) works as the Product Design canvas
(dimension 2), driven from our PDaC artifacts, with output that can land in the
React frontend.

## Setup (one-time, manual)

1. Install the pen.dev desktop app from <https://www.pen.dev/> (needs a login;
   it is free while plans launch).
2. Open a `.pen` document, then **Settings (⚙️) → MCP → enable Claude Code CLI**.
3. Restart Claude Code in this repo; `/mcp` must list `pencil`.
4. Optional headless CLI: `npm install -g @pen.dev/cli` (Node ≥ 22.19), `pen login`.

## What we generate (from PDaC, not from imagination)

Three representative screens, each traced to artifacts in
`docs/product/model/`:

| Screen | Mode | Source artifacts |
|---|---|---|
| Invoice list / project billing | management | journeys (invoicing), `UC-FIN-*` invoice use cases, `BR-FIN-*` rules |
| Field check-in / workday record | field PWA | workday-record use cases, QR-MOBILE-WEB-APP, DEC mobile decisions in CHG-INITIAL |
| Read-only large-screen board | board | board/read-only use cases from the mobile delivery decision |

Brand guardrails: see `docs/design/color-tokens.md`, `visual-direction.md`,
`logo-usage.md`. The mark is `docs/design/raccord-mark.svg`.

## Artifacts to produce

- `docs/product/design/*.pen` (open JSON format) + PNG export for review
- Keep the `.pen` files in this branch; they are the trial's deliverable

## Evaluation (fill after the pass)

- [ ] Spec fidelity: do the screens match the PDaC use cases?
- [ ] Iteration: is refining by prompt (agent on canvas) fast?
- [ ] Export: HTML/Tailwind quality; effort to translate to React components?
- [ ] Does MCP driving from Claude Code actually work day-to-day?
- [ ] Verdict: adopt as canvas, or extract the pattern only?
