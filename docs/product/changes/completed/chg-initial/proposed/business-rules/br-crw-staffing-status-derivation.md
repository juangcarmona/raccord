---
id: BR-CRW-STAFFING-STATUS-DERIVATION
type: business-rule
title: "Staffing status compares planned resources with the quantity"
status: draft
applies-to:
  - "UC-CRW-PLAN-RESOURCE-ON-FUNCTION"
  - "UC-CRW-SWITCH-PLAN-STATUS"
uses-terms:
  - "TERM-STAFFING-STATUS"
provenance:
  source: ".private/research/help-center/articles/360014625560.md, .private/research/help-center/articles/360014607700.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

A function is fully planned when the number of planned resources equals its quantity, and shown as fully planned but invisible when at least one of them is in draft; otherwise it is not fully planned.

## Rationale

Planners see at a glance what still needs staff.

## Examples

Quantity 3 with 2 planned: not fully planned.

## Exceptions

None.
