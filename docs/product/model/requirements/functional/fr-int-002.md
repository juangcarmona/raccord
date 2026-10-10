---
id: FR-INT-002
type: functional-requirement
title: "Page, filter, select and expand collections"
status: draft
derived-from:
  - "UC-INT-READ-RECORDS-THROUGH-API"
  - "BR-INT-PAGE-SIZE-LIMITS"
  - "BR-INT-CURSOR-PAGING-DEFAULT"
  - "BR-INT-OFFSET-PAGING-FOR-OTHER-SORTS"
  - "BR-INT-GENERATED-FIELDS-ON-REQUEST"
  - "BR-INT-CUSTOM-FIELDS-QUERYABLE"
verification:
  - scenario-ref: SB-INT-PAGE-DEFAULT
  - scenario-ref: SB-INT-PAGE-LIMIT-CAPPED
  - scenario-ref: SB-INT-OFFSET-PAGING
  - scenario-ref: SB-INT-GENERATED-FIELD-SELECTED
  - scenario-ref: SB-INT-CUSTOM-FIELD-FILTER
uses-terms:
  - "TERM-COLLECTION-PAGE"
  - "TERM-API-FILTER"
  - "TERM-FIELD-SELECTION"
  - "TERM-RELATION-EXPANSION"
provenance:
  source: ".private/research/help-center/articles/26707946497042.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST offer paged collections (cursor by default, offset for other sort orders), filtering with operators on standard and custom fields, field selection and inline expansion of related records.

## Rationale

Integrations read efficiently and precisely.
