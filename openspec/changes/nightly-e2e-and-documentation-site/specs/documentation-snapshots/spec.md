<!-- pdac-scope: none reason="Rendering canonical documentation into static HTML does not alter product intent." -->

## Purpose

Make canonical architecture and model documents easy to browse without replacing their Markdown sources.

## ADDED Requirements

### Requirement: Architecture and product snapshots refresh by source change

The public static site SHALL provide a generated arc42 snapshot and a generated ProductShape model snapshot. A change to architecture/ADR sources SHALL refresh the architecture snapshot; a change to the canonical ProductShape model or its configuration SHALL refresh the model snapshot. A first publication SHALL generate both.

#### Scenario: Architecture changes

- **WHEN** a push to `main` changes architecture documents, ADRs, deployment topology, the AppHost, CI architecture configuration, or the arc42 renderer
- **THEN** the publisher regenerates the architecture snapshot and preserves the model snapshot and E2E report history

#### Scenario: Product model changes

- **WHEN** a push to `main` changes `docs/product/model/` or `.product/config.yaml`
- **THEN** the publisher regenerates the ProductShape snapshot and preserves the architecture snapshot and E2E report history

#### Scenario: No relevant source changes

- **WHEN** an E2E nightly updates the site
- **THEN** the existing architecture and model snapshot files are preserved without regeneration
