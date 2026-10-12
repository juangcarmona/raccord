# pen.dev trial — working brief

Issue: juangcarmona/raccord#1 · Branch: `trial/pen-dev-design-canvas`

Goal: judge whether pen.dev (ex Pencil.dev) works as the Product Design canvas (dimension 2), driven from our PDaC artifacts, with output that can land in the React frontend.

## Setup (one-time, manual)

1. Install the pen.dev desktop app from <https://www.pen.dev/> (needs a login; it is free while plans launch).
2. Open a `.pen` document, then **Settings (⚙️) → MCP → enable Claude Code CLI**.
3. Restart Claude Code in this repo; `/mcp` must list `pencil`.
4. Optional headless CLI: `npm install -g @pen.dev/cli` (Node ≥ 22.19), `pen login`.

## What we generate (from PDaC, not from imagination)

Three representative screens, each traced to artifacts in `docs/product/model/`:

| Screen | Mode | Source artifacts |
| --- | --- | --- |
| Invoice list / project billing | management | journeys (invoicing), `UC-FIN-*` invoice use cases, `BR-FIN-*` rules |
| Field check-in / workday record | field PWA | workday-record use cases, QR-MOBILE-WEB-APP, DEC mobile decisions in CHG-INITIAL |
| Read-only large-screen board | board | board/read-only use cases from the mobile delivery decision |

Brand guardrails: see `docs/design/color-tokens.md`, `visual-direction.md`, `logo-usage.md`. The mark is `docs/design/raccord-mark.svg`.

## Artifacts to produce

- `docs/product/design/*.pen` (open JSON format) + PNG export for review
- Keep the `.pen` files in this branch; they are the trial's deliverable

## Evaluation (filled after the three-screen pass)

- [x] Spec fidelity: do the screens match the PDaC use cases? Yes — every element traces to an artifact; see [screen-traces.md](screen-traces.md). The agent had to be steered back to the model a few times (it drifted toward generic shapes early on screen 1), so the PDaC-first workflow holds, but the prompt has to name the artifacts.
- [x] Iteration: is refining by prompt (agent on canvas) fast? Yes for content/structure (chip states, rows, columns, cards: one call each). Layout bugs (Move landing in the wrong parent) happen and need a verify screenshot after each change; `edits` patching of failed snippets works but the find-must-match-exactly dance is slow when a snippet fails twice.
- [x] Export: HTML/Tailwind quality; effort to translate to React components? `html-tailwind` output is clean, class-only Tailwind, flex/grid that maps almost 1:1 to JSX; translation to React is mostly mechanical retagging (frames → div, text nodes → styled spans). No component decomposition in the export — that stays manual.
- [x] Does MCP driving from Claude Code actually work day-to-day? Yes with friction: state persists across calls only sometimes (globals lost between calls = the biggest gotcha), `document` root reads need a visitor, error messages are good and rolled-back transactions make retries safe. Roughly comparable effort to writing the same screens in raw HTML+Tailwind, but with automatic export, tokens, and a visual verify loop.
- [x] Verdict: adopt as canvas, or extract the pattern only?

  **Adopt as canvas** for spec-driven screen generation. The workflow (read PDaC artifacts → generate screen → verify iteratively → export HTML → translate to React) is repeatable and the exports are good enough to hand off. The `.pen` files stay in the repo as the design source of truth.

  Caveats to record in future iterations:
  - Selected/interactive state used blue instead of cyan on the invoice list — needs a token decision in `docs/design/color-tokens.md` (add a semantic `selected` token, don't hardcode).
  - The `.pen` copy in the repo is a snapshot; remember to re-export after canvas edits (the canvas app renaming `untitled.pen` broke one sync).
  - Harness: `xlsx`-level fidelity to exact PDaC vocabulary comes from naming the artifacts in the prompt; without that the agent invents plausible generic content.
