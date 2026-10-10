---
id: FR-CRW-PLAN-RESOURCES
type: functional-requirement
title: "Plan crew and vehicles on functions"
status: draft
derived-from:
  - "UC-CRW-PLAN-RESOURCE-ON-FUNCTION"
  - "UC-CRW-CHANGE-ASSIGNMENT-DETAILS"
  - "BR-CRW-PLANNABLE-FLAG"
  - "BR-CRW-DEFAULT-RATE-ON-ASSIGNMENT"
  - "BR-CRW-STAFFING-STATUS-DERIVATION"
  - "BR-CRW-AGENCY-MULTIPLE-BOOKING"
verification:
  - scenario-ref: "SB-CRW-HIDDEN-RESOURCE-NOT-PLANNABLE"
  - scenario-ref: "SB-CRW-STAFFING-INVISIBLE"
  - scenario-ref: "SB-CRW-AGENCY-NO-CONFLICT"
uses-terms:
  - "TERM-CREW-ASSIGNMENT"
provenance:
  source: ".private/research/help-center/articles/360014687739.md, .private/research/help-center/articles/360014594620.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST plan crew members on crew functions and vehicles on transport functions, apply the default rate, allow per-assignment rate, period, travel option, project leader and remark, and show each function's staffing status.

## Rationale

Core staffing.
