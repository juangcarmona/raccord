---
id: TERM-RESERVATION
type: domain-term
title: "Reservation"
status: draft
defined-in: BC-EQUIPMENT
synonyms:
  - "reserved quantity"
uses-terms:
  - "TERM-PLANNED-QUANTITY"
  - "TERM-RESERVATION-STATE"
  - "TERM-SOFT-HOLD"
  - "TERM-INVALID-RESERVATION"
provenance:
  source: ".private/research/help-center/articles/360013158280.md, .private/research/help-center/articles/13415175193490.md, .private/research/help-center/articles/360012699859.md, .private/research/help-center/articles/360013771040.md"
  confidence: high
  recovered-from: documentation
---

## Definition

The part of a planned line's quantity that has secured physical own stock for its planning period, giving that project priority over other projects that plan the same article in overlapping periods. A reservation is created automatically when a project is confirmed (for all quantities without shortage), manually by a planner on a project in any status that consumes availability, and when equipment is booked out for a project. It is lowered by early returns and by manual reallocation, and becomes invalid when the stock behind it disappears.

## Distinguish From

Not the plan: a planned quantity without reservation is a soft hold. Not a booking: booking records physical movement.

## Usage

Expressed as a reserved quantity per planned line; summarised as the line's reservation state.
