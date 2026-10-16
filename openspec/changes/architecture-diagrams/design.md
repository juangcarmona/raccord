<!-- pdac-scope: none reason="This design changes architecture documentation and its build-time tooling only; it introduces no product behavior or product-model semantics." -->

# Design: Architecture diagrams as reviewable static evidence

## Context

See [proposal.md](proposal.md). The current generator in `tools/arc42-snapshot` renders every Mermaid code fence to sanitized inline SVG in one HTML file. `engineering-site.yml` tests that renderer and builds the hosted snapshot on pull requests, but its PR validation job currently runs only the static-site helper tests; it does not render arc42 or retain visual evidence. Sections 03, 05, 06, 07 and 10 contain ASCII diagrams. The generator accepts the `arc42-snapshot` document contract and constrains SVG width, but has no shared Mermaid theme, authored diagram alternative text, or diagram-contract validation.

## Goals / Non-Goals

**Goals:**

- Keep Markdown next to its owning arc42 section as the canonical source and preserve the one-file, offline snapshot.
- Make boundaries, actor/system roles, runtime direction, deployment groupings, future seams and evidence status visually consistent.
- Render diagrams in the PR validation job, fail on malformed diagrams or missing accessible titles, and save reviewable HTML and PNG evidence.
- Make every diagram concise and traceable to implementation, infrastructure, a current architecture decision or an explicitly marked future seam.
- Evaluate model-based authoring, agent-assisted discovery and an alternate renderer without adding a second canonical model or renderer.

**Non-Goals:**

- Changing application behavior, product requirements, deployment topology or public product UX.
- Adding a hosted rendering service or a production dependency.
- Migrating the arc42 prose into a separate architecture model.
- Creating a visual pixel-baseline test that would make layout noise a CI failure.

## Decisions

### Mermaid source remains canonical

Use the existing Markdown fences and generator. A separate architecture model would duplicate facts already owned by code, infrastructure and arc42 prose, while requiring an ownership and drift policy. A representative model-based evaluation will be retained as research and summarized through a neutral ADR; adopting it would require a separately scoped implementation issue.

### Define a visual and accessibility contract

Add `docs/architecture/arc42/diagrams/visual-language.md` with diagram selection guidance, a small semantic palette, direction and arrow rules, density guidance, accessible naming and a source-evidence checklist. Every diagram carries a Mermaid `accTitle`; surrounding prose explains its scope and any future seams. Use shape and labels as well as color so meaning does not depend on color alone.

### Keep theme and layout in the renderer

Apply one pinned Mermaid configuration for typography and semantic colors. Keep generated SVGs sanitized and IDs stable. Wrap each diagram in a semantic `figure` with the authored title as its accessible name. The page theme stays readable in light and dark modes by keeping diagrams on a light neutral surface. Wide diagrams use a bounded, horizontally scrollable frame and SVGs scale down to the available width.

### Use deterministic structural checks and visual evidence

Add a source-contract command that discovers all Mermaid blocks in the arc42 folder and rejects blocks without `accTitle`. Snapshot generation remains the syntax/render check. On pull requests, the focused site validation job installs only the pinned snapshot toolchain, runs its tests, generates the snapshot, captures screenshots of sections 03, 05, 06, 07 and 10, and uploads those files as a short-retention review artifact. No application build or E2E suite is added to this job. Avoid screenshot pixel comparison because browser and renderer updates cause noise without identifying semantic regressions.

### Record bounded tool evaluations

Compare the current approach with one model-based authoring option, one agent-assisted discovery workflow and one alternate text-to-diagram renderer. Use the same representative context/container facts, inspect source evidence manually, and record capabilities, licensing, local execution, layout/static export, maintenance and hallucination/verification risks. Keep exact external-product research and dated source links under the git-ignored `.private/research/`; the tracked ADR records only the category-level decision. Do not include generated files or external product names in the public snapshot.

## Risks / Trade-offs

- [Diagram facts can still drift] → The guide requires named source evidence and a human review checklist; this change does not claim automatic architecture extraction.
- [A renderer change can alter SVG geometry] → Keep the renderer pinned, check deterministic structure in unit tests, and retain pull-request screenshots for human comparison.
- [Screenshot capture can make CI slower] → Run it only in the existing focused pull-request validation job and capture five section routes in one browser session.
- [A tool evaluation can expose source to external services] → Run candidate tools locally against public repository facts, use no credentials, and keep generated output out of canonical docs.

## Migration Plan

No runtime migration is required. Update the five arc42 sections and renderer, validate with the pinned generator, and allow the existing static-site workflow to publish the new self-contained page after merge. Rollback is a revert of the documentation/tooling change; the existing snapshot generation contract remains usable.

## Open Questions

None. The renderer, hosted static snapshot and repo-local Mermaid sources are already in place, so this change extends an established path.
