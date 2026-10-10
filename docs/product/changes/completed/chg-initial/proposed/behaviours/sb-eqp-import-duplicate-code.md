---
id: SB-EQP-IMPORT-DUPLICATE-CODE
type: structured-behaviour
title: "Row with existing scan code fails"
status: draft
illustrates:
  - "BR-EQP-IMPORT-DUPLICATE-CODES"
given:
  - "A row carries a scan code already used in the catalog"
when: "The file is imported"
then:
  - "That row is not imported"
  - "The result lists it with the duplicate code as reason"
  - "Other rows are imported"
uses-terms:
  - "TERM-SCAN-CODE"
provenance:
  source: ".private/research/help-center/articles/13725992768786.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Fixes partial import behaviour.

## Boundaries

None.
