---
id: BR-WHS-STATUS-AVAILABILITY-EFFECT
type: business-rule
title: "Project status decides whether planned equipment consumes availability and is reserved"
status: draft
applies-to:
  - "UC-WHS-RESERVE-EQUIPMENT"
  - "UC-WHS-PLAN-EQUIPMENT"
  - "UC-WHS-VIEW-AVAILABILITY-TIMELINE"
  - "BC-EQUIPMENT"
uses-terms:
  - "TERM-AVAILABILITY-FORECAST"
  - "TERM-RESERVATION"
  - "TERM-SOFT-HOLD"
  - "TERM-PROJECT-STATUS"
provenance:
  source: ".private/research/help-center/articles/360013158280.md, .private/research/help-center/articles/13415175193490.md, .private/research/help-center/articles/360013730520.md, .private/research/help-center/articles/13369374381714.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Planned equipment of a cancelled project consumes no availability and holds no reservation; of an inquiry consumes availability but is never reserved; of a draft (concept) or pending project consumes availability and is reserved only when someone reserves it manually; of a confirmed project, or any later progress status, consumes availability and is reserved automatically.

## Rationale

Lets early-stage proposals show up as risk without blocking stock, while committed jobs get priority.

## Examples

See SB-WHS-INQUIRY-SOFT-HOLD and SB-WHS-CANCEL-RELEASES-STOCK.

## Exceptions

The documentation lists the post-confirmation statuses under their older names (prepped, on location); the mapping to the newer progress statuses (in progress, execution done, complete) is inferred.
