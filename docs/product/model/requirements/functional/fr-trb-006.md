---
id: "FR-TRB-006"
type: functional-requirement
title: "Predictable project overview selection"
status: draft
derived-from:
  - "BR-TRB-OVERVIEW-PERIOD-OVERLAP"
  - "BR-TRB-OVERVIEW-DAY-KIND-FILTER"
  - "UC-PRJ-BROWSE-PROJECTS"
verification:
  - scenario-ref: "SB-TRB-PROJECT-OUTSIDE-PERIOD"
  - scenario-ref: "SB-TRB-PROJECT-PARTIAL-OVERLAP"
  - scenario-ref: "SB-TRB-UNDATED-SALE-PROJECT"
  - scenario-ref: "SB-TRB-DAY-KIND-HIDES-PROJECT"
  - scenario-ref: "SB-TRB-TAG-FILTER-HIDES-PROJECT"
uses-terms:
  - "TERM-PROJECT-OVERVIEW"
provenance:
  source: ".private/research/help-center/articles/360014514680.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST list in the project overview every project whose planning period overlaps the selected period and matches the active filters and day kinds, and MUST offer a separate selection for projects without dates.

## Rationale

Users must be able to find every project, including undated sales.
