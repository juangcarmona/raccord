---
id: SB-EQP-CONTENT-UPDATE
type: structured-behaviour
title: "Update content import keeps other lines"
status: draft
illustrates:
  - "BR-EQP-CONTENT-IMPORT-MODES"
given:
  - "A kit has default content lines A and B"
when: "A content import in update mode lists lines B and C for the kit"
then:
  - "The kit's default content becomes A, B and C"
uses-terms:
  - "TERM-IMPORT-MODE"
  - "TERM-DEFAULT-CONTENT"
provenance:
  source: ".private/research/help-center/articles/20045646271250.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Contrasts with overwrite mode.

## Boundaries

None.
