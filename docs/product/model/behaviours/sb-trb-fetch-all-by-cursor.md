---
id: "SB-TRB-FETCH-ALL-BY-CURSOR"
type: structured-behaviour
title: "Reading a full collection by cursor"
status: draft
illustrates:
  - "BR-TRB-OFFSET-PAGING-RETIRED"
  - "BR-INT-CURSOR-PAGING-DEFAULT"
given:
  - "The installation has 35 contacts"
when: "The integration requests contacts with page size 10 and keeps following the next-page address"
then:
  - "It receives three full pages and one page of 5 contacts"
  - "The last page's next-page address is empty"
uses-terms:
  - "TERM-PAGINATION-CURSOR"
provenance:
  source: ".private/research/help-center/articles/37200827246610.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Standard migration pattern.

## Boundaries

None.
