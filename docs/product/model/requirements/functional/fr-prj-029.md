---
id: FR-PRJ-029
type: functional-requirement
title: "Tags and custom fields"
status: draft
derived-from:
  - "UC-PRJ-TAG-RECORDS"
  - "UC-PRJ-DEFINE-CUSTOM-FIELD"
  - "BR-PRJ-TAG-FILTER-MODES"
  - "BR-PRJ-CUSTOM-FIELD-FROZEN"
  - "BR-PRJ-CONFIDENTIAL-CUSTOM-FIELD"
verification:
  - scenario-ref: "SB-PRJ-TAG-FILTER-ALL"
  - scenario: "A confidential custom field is hidden from roles without that permission"
uses-terms:
  - "TERM-TAG"
  - "TERM-CUSTOM-FIELD"
provenance:
  source: ".private/research/help-center/articles/360013931999.md, .private/research/help-center/articles/360013637220.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST let users tag records and filter by any or all tags, and let administrators define typed, optionally confidential and searchable custom fields on the documented record kinds.

## Rationale

Company-specific organisation.
