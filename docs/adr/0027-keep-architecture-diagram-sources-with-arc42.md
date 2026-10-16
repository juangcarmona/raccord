---
name: keep-architecture-diagram-sources-with-arc42
description: Keep arc42 Markdown and its inline Mermaid source canonical, with static SVG snapshots as generated evidence.
status: Accepted
date: 2026-10-16
deciders: []
tags: [architecture, documentation, diagrams]
---

# ADR-0027: Keep architecture diagram sources beside arc42 prose

## Context

The arc42 documents are the architecture source of truth, while source code and deployment files establish implementation facts. The repository already has a deterministic build that renders Mermaid blocks into sanitized SVGs in a self-contained static page. Separate model languages, agent-generated diagrams and alternate renderers could add richer layouts, but they would also create more toolchain and source-ownership choices.

## Decision

We will keep architecture diagrams as Mermaid source beside the owning arc42 prose and generate inline SVG snapshots at build time. Agent-assisted diagram generation may be used experimentally to draft source, but every node and relationship must be verified against repository evidence before it enters arc42. We will defer a second canonical architecture model and a second renderer unless the documented visual contract cannot be met with the current renderer.

## Consequences

- **Positive:** Architecture explanations and diagrams stay together, reviewable in Git, and available in an offline, self-contained page.
- **Negative:** One generic diagram language may require more manual layout work for specialized views, and agents cannot make architecture claims authoritative.
- **Neutral:** A future decision can add a bounded model or renderer evaluation if a concrete diagram fails the visual contract; public static pages retain no generation-time service dependency.

## Alternatives considered

- **Maintain a separate canonical C4 model:** rejected for now because it would duplicate component and relationship facts owned by code, deployment configuration and arc42 prose, and would need an explicit drift and ownership policy.
- **Use agent-generated HTML as the canonical source:** rejected because generated labels and relationships still require human verification and HTML does not fit the existing Markdown review path.
- **Add another diagram renderer by default:** rejected because the existing renderer can provide vector output, styling, responsive framing and accessible names without a second toolchain.

## References

- Issues: [#253](https://github.com/juangcarmona/raccord/issues/253), [#259](https://github.com/juangcarmona/raccord/issues/259), [#257](https://github.com/juangcarmona/raccord/issues/257), [#260](https://github.com/juangcarmona/raccord/issues/260)
- OpenSpec change: [architecture-diagrams](../../openspec/changes/architecture-diagrams/)
- Guide: [arc42 diagram visual language](../architecture/arc42/diagrams/visual-language.md)
- Static snapshot: [arc42 snapshot generator](../../tools/arc42-snapshot/generate.mjs)
