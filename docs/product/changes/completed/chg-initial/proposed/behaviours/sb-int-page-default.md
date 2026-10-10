---
id: SB-INT-PAGE-DEFAULT
type: structured-behaviour
title: "Default page size"
status: draft
illustrates:
  - "BR-INT-PAGE-SIZE-LIMITS"
  - "BR-INT-CURSOR-PAGING-DEFAULT"
when: "an application lists the 700 contacts without a limit"
then:
  - "it receives pages of 300, 300 and 100 contacts"
  - "the last page has an empty next-page address"
uses-terms:
  - "TERM-COLLECTION-PAGE"
  - "TERM-PAGINATION-CURSOR"
provenance:
  source: ".private/research/help-center/articles/26707946497042.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Default page size and end-of-collection signal.

## Boundaries

None.
