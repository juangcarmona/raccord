---
id: SB-INT-OFFSET-PAGING
type: structured-behaviour
title: "Sorting by name switches to offsets"
status: draft
illustrates:
  - "BR-INT-OFFSET-PAGING-FOR-OTHER-SORTS"
when: "an application lists contacts sorted by name with an offset of 300"
then:
  - "records 301 to 600 in name order are returned"
  - "no next-page address is included"
uses-terms:
  - "TERM-COLLECTION-PAGE"
provenance:
  source: ".private/research/help-center/articles/26707946497042.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Offset mode for non-identifier sorting.

## Boundaries

None.
