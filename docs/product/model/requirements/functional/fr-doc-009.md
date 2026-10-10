---
id: "FR-DOC-009"
type: functional-requirement
title: "Manage letterheads"
status: draft
derived-from:
  - "UC-DOC-ADD-LETTERHEAD"
  - "UC-DOC-EDIT-LETTERHEAD"
  - "BR-DOC-LETTERHEAD-PAGE-MAPPING"
  - "BR-DOC-LETTERHEAD-MARGINS"
  - "BR-DOC-LETTERHEAD-TYPE-AVAILABILITY"
  - "BR-DOC-PAGE-NUMBERS"
verification:
  - scenario-ref: "SB-DOC-LETTERHEAD-SHORTER-THAN-DOCUMENT"
  - scenario-ref: "SB-DOC-TOP-MARGIN-RESERVES-HEADER"
  - scenario-ref: "SB-DOC-LETTERHEAD-NOT-OFFERED-FOR-TYPE"
  - scenario-ref: "SB-DOC-PAGE-NUMBERS-DISABLED"
uses-terms:
  - "TERM-LETTERHEAD"
provenance:
  source: ".private/research/help-center/articles/360014055700.md, .private/research/help-center/articles/360017648260.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST let administrators upload PDF letterheads with page size, margins, allowed document types and page-number option, and MUST render generated documents on them page by page.

## Rationale

Company stationery without redesigning templates.
