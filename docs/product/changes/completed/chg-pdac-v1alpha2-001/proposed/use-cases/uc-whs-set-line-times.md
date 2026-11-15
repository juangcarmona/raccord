---
id: UC-WHS-SET-LINE-TIMES
type: use-case
title: "Set planning and usage periods per equipment line"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors: []
bounded-context: "BC-EQUIPMENT"
governed-by:
  - "BR-WHS-LINE-TIMES-DEFAULT"
  - "BR-WHS-LINE-TIMES-LINKED"
  - "BR-WHS-PHYSICAL-KIT-SHARED-TIME"
  - "BR-WHS-PRICE-FROM-USAGE"
  - "BR-WHS-PACKING-LIST-PER-DATE"
  - "BR-WHS-RECALC-ON-SHIFT"
  - "BR-WHS-AVAILABILITY-BY-PLANNING-PERIOD"
uses-terms:
  - "TERM-PLANNING-PERIOD"
  - "TERM-USAGE-PERIOD"
  - "TERM-PLANNED-EQUIPMENT"
  - "TERM-PACKING-LIST"
provenance:
  source: ".private/research/help-center/articles/38542583895186.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Each line leaves, returns and is charged for exactly the time it is needed.

## Trigger

Some equipment is needed for only part of the job.

## Preconditions

Lines are planned; the time schedule has entries or the coordinator creates one.

## Main Flow

1. The coordinator selects one or several lines and chooses a schedule entry of the project as planning period, usage period or both.
2. Optionally the coordinator creates a new schedule entry (name, start, end) in place.
3. The product updates availability, shortages, price factor and line totals, and recomputes packing lists per pickup and return moment.

## Alternative Flows

- Group-wide change via the group's times.

## Failure Conditions

- Content of a physical combination cannot get its own times: refused.
- New interval causes shortages: they are shown.

## Postconditions

Lines carry linked periods; availability and packing lists reflect them.
