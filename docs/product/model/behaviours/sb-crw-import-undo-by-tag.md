---
id: SB-CRW-IMPORT-UNDO-BY-TAG
type: structured-behaviour
title: "Undoing an import by its tag"
status: draft
illustrates:
  - "BR-CRW-IMPORT-TAG"
given:
  - "An import partly failed"
when: "The planner selects all crew members with the tag of the faulty import and deletes them"
then:
  - "Only the records of that import are removed"
uses-terms:
  - "TERM-TAG"
provenance:
  source: ".private/research/help-center/articles/360014366299.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Safe redo.

## Boundaries

None.
