---
id: BR-CRW-SHIFT-AVAILABILITY-BLOCKING
type: business-rule
title: "Shifts block availability unless marked otherwise"
status: draft
applies-to:
  - "UC-CRW-CREATE-SHIFT"
  - "UC-CRW-PLAN-RESOURCE-ON-SHIFT"
uses-terms:
  - "TERM-SHIFT"
  - "TERM-AVAILABILITY-STATE"
provenance:
  source: ".private/research/help-center/articles/17290277763090.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

Crew planned on a shift are unavailable for other work during it, unless the shift is marked as leaving them available for other functions.

## Rationale

Some internal work can be interrupted, some cannot.

## Examples

Warehouse crew on a non-blocking shift can still be planned on a delivery.

## Exceptions

None.
