---
id: TERM-PLANNING-PERIOD
type: domain-term
title: "Planning period"
status: draft
defined-in: BC-EQUIPMENT
synonyms:
  - "equipment out period"
uses-terms:
  - "TERM-USAGE-PERIOD"
  - "TERM-PLANNED-EQUIPMENT"
  - "TERM-TIME-SCHEDULE"
provenance:
  source: ".private/research/help-center/articles/360017647460.md, .private/research/help-center/articles/38542583895186.md, .private/research/help-center/articles/360013158280.md, .private/research/help-center/articles/360012699859.md"
  confidence: high
  recovered-from: documentation
---

## Definition

The interval during which planned equipment is physically away from stock: from the moment it leaves the warehouse until it is due back. Availability is computed over this interval. Each planned line carries its own planning period; by default it follows its equipment group's default periods, or the project's when the group has none. A planning period is always a reference to a named entry of the project's time schedule, so it moves with the schedule when the project is shifted. Its start is the warehouse due date of the outgoing packing list and its end the due date of the return.

## Distinguish From

Differs from the usage period, which is what is charged. The planning period is usually wider (load-in, load-out, travel).

## Usage

Drives availability, shortages, packing-list cut, warehouse due dates and delay detection.
