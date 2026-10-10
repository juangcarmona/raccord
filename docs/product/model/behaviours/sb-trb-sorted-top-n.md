---
id: "SB-TRB-SORTED-TOP-N"
type: structured-behaviour
title: "First 30 contacts by first name without offsets"
status: draft
illustrates:
  - "BR-TRB-OFFSET-PAGING-RETIRED"
when: "The integration requests contacts with page size 30 sorted ascending on first name"
then:
  - "The first page holds the 30 contacts with the alphabetically first first names"
  - "The next-page address keeps the sort order and page size"
uses-terms:
  - "TERM-COLLECTION-PAGE"
provenance:
  source: ".private/research/help-center/articles/37200827246610.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Sorted top-N reads need no offset.

## Boundaries

Contradicts the earlier rule that sorting on a non-identifier column switches to offset paging; see conflict report.
