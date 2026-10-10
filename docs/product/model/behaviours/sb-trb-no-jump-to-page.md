---
id: "SB-TRB-NO-JUMP-TO-PAGE"
type: structured-behaviour
title: "Page five cannot be requested directly"
status: draft
illustrates:
  - "BR-TRB-CURSOR-SEQUENTIAL-ONLY"
when: "The integration wants the fifth page of projects after offset paging has been removed"
then:
  - "It must follow the next-page address from the first page four times"
  - "No parameter lets it request page five directly"
uses-terms:
  - "TERM-PAGINATION-CURSOR"
provenance:
  source: ".private/research/help-center/articles/37200827246610.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Random access is lost with cursors.

## Boundaries

None.
