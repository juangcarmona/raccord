---
id: FR-CRW-PAYROLL-EXPORT
type: functional-requirement
title: "Export hours for payroll"
status: draft
derived-from:
  - "UC-CRW-EXPORT-PAYROLL-DATA"
  - "BR-CRW-EXPORT-PENDING-WARNING"
  - "BR-CRW-PAYROLL-CODE-IN-EXPORT"
  - "BR-CRW-NO-PAYROLL-PROCESSING"
verification:
  - scenario-ref: "SB-CRW-EXPORT-WITH-PENDING"
  - scenario: "Exporting hours of a type with payroll code OT01 includes OT01 on each exported row"
uses-terms:
  - "TERM-PAYROLL-EXPORT"
provenance:
  source: ".private/research/help-center/articles/23814779870610.md, .private/research/help-center/articles/27336532785298.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST export selected registrations with chosen registration and activity columns and payroll codes to a spreadsheet, warning when pending registrations are included.

## Rationale

Payroll is external.
