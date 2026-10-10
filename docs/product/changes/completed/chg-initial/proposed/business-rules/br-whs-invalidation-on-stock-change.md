---
id: BR-WHS-INVALIDATION-ON-STOCK-CHANGE
type: business-rule
title: "Stock changes invalidate reservations they can no longer back"
status: draft
applies-to:
  - "UC-WHS-REVIEW-SHORTAGES"
  - "UC-WHS-MARK-LOST"
  - "UC-WHS-MARK-DELAYED"
  - "BC-EQUIPMENT"
uses-terms:
  - "TERM-INVALID-RESERVATION"
  - "TERM-RESERVATION-STATE"
provenance:
  source: ".private/research/help-center/articles/360013836939.md, .private/research/help-center/articles/360012699859.md"
  confidence: high
  recovered-from: documentation
---

## Rule

When stock decreases after reservations were made (loss, delayed return extending a hold, booking out for another project), reservations that are no longer covered become invalid and every affected project shows an invalid reservation shortage; the product does not silently reassign reservations.

## Rationale

Reflects the real state of the shelves rather than an outdated promise.

## Examples

See SB-WHS-LOSS-INVALIDATES-RESERVATIONS and SB-WHS-DELAY-CAUSES-SHORTAGE-ELSEWHERE.

## Exceptions

None.
