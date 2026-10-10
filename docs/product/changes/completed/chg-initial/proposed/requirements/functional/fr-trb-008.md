---
id: "FR-TRB-008"
type: functional-requirement
title: "Cursor-only collection paging"
status: draft
derived-from:
  - "BR-TRB-OFFSET-PAGING-RETIRED"
  - "BR-TRB-CURSOR-SEQUENTIAL-ONLY"
verification:
  - scenario-ref: "SB-TRB-FETCH-ALL-BY-CURSOR"
  - scenario-ref: "SB-TRB-SORTED-TOP-N"
  - scenario-ref: "SB-TRB-NO-JUMP-TO-PAGE"
uses-terms:
  - "TERM-PAGINATION-CURSOR"
provenance:
  source: ".private/research/help-center/articles/37200827246610.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The public interface MUST page every collection, including sorted and filtered ones, through an opaque next-page address that preserves the query, and MUST retire caller-supplied offsets after the announced transition.

## Rationale

Consistent paging while data changes.
