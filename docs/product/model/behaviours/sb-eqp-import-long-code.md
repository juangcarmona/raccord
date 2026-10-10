---
id: SB-EQP-IMPORT-LONG-CODE
type: structured-behaviour
title: "Over-long scan codes are truncated"
status: draft
illustrates:
  - "BR-EQP-IMPORT-DUPLICATE-CODES"
  - "CON-EQP-IMPORT-FILE-FORMAT"
given:
  - "Two rows carry 30-character scan codes that differ only in their last characters"
when: "The file is imported"
then:
  - "Both codes are cut to 25 characters"
  - "The second row fails as a duplicate code"
uses-terms:
  - "TERM-SCAN-CODE"
provenance:
  source: ".private/research/help-center/articles/13725992768786.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Shows how the length limit can cause duplicates.

## Boundaries

None.
