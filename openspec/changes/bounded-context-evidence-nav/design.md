<!-- pdac-scope: none reason="This design only changes test metadata and engineering evidence rendering." -->

## Context

See proposal.md - Why. Concrete E2E classes already declare repeatable `Area` traits. The result writer currently stores only journey name, status, and viewport, and the report renderer builds a flat list. Existing report manifests are retained across runs, so the renderer must handle old journey records without context metadata.

## Goals / Non-Goals

**Goals:** preserve the owning journey's bounded-context labels through artifact generation and render an accessible context-grouped navigation, including empty contexts and cross-cutting journeys.

**Non-Goals:** change test selection semantics, application behavior, result counts, report retention, or the public-site audience.

## Decisions

- Use the existing repeatable xUnit `Area` trait as the source of context metadata. Standardize its values to the bounded-context slugs and read all values declared on the concrete journey class.
- Remove the AppHost's named PostgreSQL data-volume annotation in the E2E testing builder before applying session lifetime. Container lifetime alone does not isolate credentials from data left in a persistent volume; removing the mount from the test resource leaves developer data untouched while each E2E run gets fresh storage.
- Write an `areas` string array into each per-journey JSON result. Treat missing, malformed, and unknown values as unclassified; never fail a test or report build because metadata is absent.
- Keep a canonical ordered list of the ten bounded contexts in the static report builder. Render a navigation section for each context even when it has no journeys, and render a separate Cross-cutting group for platform journeys.
- Preserve `areas` while building reports from new artifacts. Retained historical manifests without `areas` render under an Unclassified group so their evidence remains accessible without guessing its context.
- Keep each journey and its assets represented once in report data; navigation links may appear in multiple context groups for a multi-context journey.
- Treat `-mobile` and `-publication` screenshots as supplemental assets for their named journey. Allow a journey result to explicitly list supplemental asset filenames whose names do not follow those suffixes, so every captured asset remains attached to its owning journey rather than appearing as an unknown journey.

Alternatives considered: encode the context in artifact filenames (rejected because it duplicates metadata and disrupts stable asset names); infer context from namespace or folder (rejected because those names are not the product model and cross-context journeys are common); omit empty contexts (rejected because absence of evidence should remain visible).

## Risks / Trade-offs

- A journey may be assigned the wrong context → review every concrete E2E trait mapping against the product bounded-context definitions.
- Old reports have no context data → mark them Unclassified and retain the existing links and assets.
- Multi-context navigation can repeat links → deduplicate each journey's context values and keep the report record and assets unique.

## Migration Plan

No data migration or deployment change is needed. New test runs emit context metadata; historical reports render as Unclassified. Re-rendering retained reports remains deterministic.
