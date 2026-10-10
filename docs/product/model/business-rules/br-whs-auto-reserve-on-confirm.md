---
id: BR-WHS-AUTO-RESERVE-ON-CONFIRM
type: business-rule
title: "Confirming a project reserves everything that is available"
status: draft
applies-to:
  - "UC-WHS-RESERVE-EQUIPMENT"
  - "BC-EQUIPMENT"
uses-terms:
  - "TERM-RESERVATION"
  - "TERM-RESERVATION-STATE"
  - "TERM-SHORTAGE"
  - "TERM-PROJECT-STATUS"
provenance:
  source: ".private/research/help-center/articles/360013158280.md, .private/research/help-center/articles/13415175193490.md"
  confidence: high
  recovered-from: documentation
---

## Rule

When a project becomes confirmed, the product reserves for each planned line the part of its planned quantity covered by free own stock; the remainder stays an unreserved shortage. Saving a confirmed project again re-runs this automatic reservation for quantities that have become free.

## Rationale

Confirmed work must secure stock without manual steps.

## Examples

See SB-WHS-CONFIRM-RESERVES-AVAILABLE and SB-WHS-RESAVE-RESERVES-FREED-STOCK.

## Exceptions

Reservations already given manually to other projects are not taken away by confirmation.
