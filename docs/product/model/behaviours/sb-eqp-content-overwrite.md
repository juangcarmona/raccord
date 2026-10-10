---
id: SB-EQP-CONTENT-OVERWRITE
type: structured-behaviour
title: "Overwrite content import deletes missing lines"
status: draft
illustrates:
  - "BR-EQP-CONTENT-IMPORT-MODES"
given:
  - "A kit has default content lines A and B"
when: "A content import in overwrite mode lists lines B and C for the kit"
then:
  - "The kit's default content becomes B and C"
  - "Line A is deleted"
uses-terms:
  - "TERM-IMPORT-MODE"
  - "TERM-DEFAULT-CONTENT"
provenance:
  source: ".private/research/help-center/articles/20045646271250.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Contrasts with update mode.

## Boundaries

None.
