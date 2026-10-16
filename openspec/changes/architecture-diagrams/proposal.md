<!-- pdac-scope: none reason="This change improves architecture documentation and its static-site verification; it changes no product behavior or product-model semantics." -->

# Architecture diagrams as reviewable static evidence

## Why

The arc42 snapshot already converts Mermaid to sanitized inline SVG, but the five most important architecture views still contain hand-spaced text diagrams. They are difficult to scan, have no shared visual or accessibility contract, and are not rendered in pull-request validation.

## What Changes

- Define a concise diagram guide and review checklist for context, building-block, runtime, deployment, quality, sequence and flow views.
- Replace the weak diagrams in arc42 sections 03, 05, 06, 07 and 10 with concise Mermaid diagrams checked against the current code, deployment files and documented future seams.
- Improve Mermaid theme, accessible names, responsive sizing and overflow behavior while retaining deterministic inline SVG output and sanitization.
- Validate every authored Mermaid block and generate a self-contained arc42 snapshot in the pull-request workflow; retain the HTML and a representative screenshot as review evidence.
- Record a tool-selection decision and private research for optional model-driven, agent-assisted and alternate-renderer workflows. Keep the current Mermaid source as canonical; do not add another renderer or runtime service.

## Capabilities

### New Capabilities

None.

### Modified Capabilities

None. This is architecture documentation and developer tooling only; `.openspec.yaml` uses `skip_specs: true` because no product or application behavior changes.

## Impact

The arc42 Markdown, `tools/arc42-snapshot`, its focused tests, `.github/workflows/engineering-site.yml`, architecture decision records and the process ledger are affected. No application API, database, runtime dependency or public product surface changes. Pull-request validation adds only the documentation renderer and static-page evidence steps.
