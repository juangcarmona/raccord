# Arc42 diagram visual language

This guide keeps architecture diagrams readable, accessible and grounded in the running repository. It applies to diagrams in `docs/architecture/arc42/` and their generated static snapshot.

## Choose a diagram by question

| Reader question | Diagram form | Arc42 home |
| --- | --- | --- |
| Who or what is outside the system boundary? | Context flowchart | 03 Context and scope |
| What are the major runtime parts and dependency directions? | Building-block flowchart | 05 Building-block view |
| How does one important scenario unfold over time? | Sequence diagram | 06 Runtime view |
| Where do deployables and stateful resources run? | Deployment flowchart | 07 Deployment view |
| Which quality attributes and scenarios matter? | Quality tree or flowchart | 10 Quality requirements |
| What are the steps or branches in one process? | Flowchart | Owning arc42 section |

Prefer one diagram per question. Keep the diagram to about twelve named nodes and a small number of crossing edges; split it when labels or relationships stop being legible at a laptop viewport.

## Visual rules

- Use left-to-right direction for context, structure and data flows; use top-to-bottom direction for dependency trees and deployment nesting; use sequence diagrams for time-ordered interactions.
- Name components by their role in Raccord and use the same terms as the owning section and implementation. Use a technology name only when the actual implementation or infrastructure names it.
- Use solid lines for implemented relationships. A dashed line means optional configuration, development-only wiring or a future seam and must say which one; never draw a connected edge to suggest an integration that does not exist.
- Use arrow labels for protocol, data or responsibility when the relationship would otherwise be ambiguous. Avoid decorative arrows and duplicate edges.
- Use the shared palette: current application elements use pale green `#e6f4ea` with dark green borders `#1b5e36`; callers and system boundaries use pale blue `#eaf2f8` with dark blue borders `#245679`; optional or future elements use pale amber `#fff4d6` with dark amber borders `#805500` and dashed outlines. Keep text contrast high.
- Use the palette as a shared legend. Never use color as the only signal: include labels and use line or border style as a second cue. Add a diagram-specific legend only when its labels and shapes cannot explain the distinction.

## Accessibility and static rendering

- Begin every Mermaid block with `accTitle: <short, specific description>` after the diagram declaration. The snapshot uses this as the figure's accessible name.
- Explain the diagram's scope and the important relationship in nearby prose. The image title complements that explanation; it does not replace it.
- Use sentence-case labels, expand uncommon abbreviations in prose, and avoid text that depends on color perception or a hover interaction.
- Keep diagrams reviewable as Markdown source. Generated SVG and HTML are build artifacts; never edit them by hand or make them canonical.
- The snapshot is one self-contained HTML file. It renders SVG inline, sanitizes it, and has no browser-time diagram service dependency.

## Evidence checklist for pull requests

- [ ] Each node is an implemented component, caller, resource, quality scenario, or explicitly labeled future seam.
- [ ] Each solid edge is verified against source, project references, an API mapping, a deployment file or a documented decision.
- [ ] Each dashed edge or node is labeled optional or future and does not imply a live connection.
- [ ] The owning arc42 prose agrees with the diagram and links the reader to implementation evidence by path.
- [ ] The diagram answers one question, remains readable at 1280 CSS pixels and does not exceed the density guidance without a clear reason.
- [ ] The diagram has an `accTitle`, adequate contrast, labels in addition to color, and explanatory prose nearby.
- [ ] The generated static snapshot renders the diagram without clipping, missing content or external resources.
