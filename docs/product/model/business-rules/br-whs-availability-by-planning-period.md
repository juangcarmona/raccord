---
id: BR-WHS-AVAILABILITY-BY-PLANNING-PERIOD
type: business-rule
title: "Planned equipment is unavailable for its whole planning period"
status: draft
applies-to:
  - "UC-WHS-PLAN-EQUIPMENT"
  - "UC-WHS-CHECK-AVAILABILITY"
  - "UC-WHS-VIEW-AVAILABILITY-TIMELINE"
  - "UC-WHS-SET-LINE-TIMES"
  - "BC-EQUIPMENT"
uses-terms:
  - "TERM-PLANNING-PERIOD"
  - "TERM-EQUIPMENT-AVAILABILITY"
  - "TERM-PLANNED-EQUIPMENT"
provenance:
  source: ".private/research/help-center/articles/360013158280.md, .private/research/help-center/articles/360012699859.md, .private/research/help-center/articles/38542583895186.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Availability of an article is computed per planned line over that line's planning period: a planned line consumes its planned quantity from the first to the last moment of its own planning period, never over its usage period nor over the whole project unless the line's planning period says so.

## Rationale

Equipment is physically away from stock from load-out to return, regardless of how many days are charged; per-line periods let stock not yet needed serve other jobs.

## Examples

See SB-WHS-STAGGERED-LINE-TIMES-FREE-STOCK. A truss going out Monday and projectors needed from Wednesday: the projectors remain available to other jobs Monday to Wednesday.

## Exceptions

Equipment booked early or late is handled by warehouse movement rules (early return, delay).
