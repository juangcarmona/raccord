---
id: UC-WHS-REVIEW-SHORTAGES
type: use-case
title: "Review shortages across projects"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors: []
bounded-context: "BC-EQUIPMENT"
governed-by:
  - "BR-WHS-SHORTAGE-DETECTION"
  - "BR-WHS-SHORTAGE-EXPLAINED"
  - "BR-WHS-RESOLUTION-USABILITY"
  - "BR-WHS-RECALC-ON-SHIFT"
uses-terms:
  - "TERM-SHORTAGE"
  - "TERM-SHORTAGE-KIND"
  - "TERM-SALES-SHORTAGE"
provenance:
  source: ".private/research/help-center/articles/360013836939.md, .private/research/help-center/articles/360017700639.md, .private/research/help-center/articles/17035832287506.md, .private/research/help-center/articles/360021519799.md"
  confidence: high
  recovered-from: documentation
---

## Goal

All uncovered demand is known and can be acted upon.

## Trigger

Regular planning review or a project shift.

## Preconditions

None.

## Main Flow

1. The coordinator opens the rental shortages, filtered by period and saved filters, grouped by project.
2. The coordinator opens a project from a shortage and sees each short line with its kind.
3. The coordinator picks lines and starts a resolution.

## Alternative Flows

- Sales shortages: the coordinator sees when sale articles will run short, and stock at a date, and schedules a stock increase.
- Project overview: projects with shortages show a failing equipment-planning indicator.

## Failure Conditions

- A period or filter with no uncovered demand shows an empty list.
- A user without access to the project or warehouse is not shown its shortages.
- Starting a resolution with no lines picked is refused with the reason.
- A resolution not usable for a shortage is marked as such and cannot be applied (BR-WHS-RESOLUTION-USABILITY).
- After a schedule shift, shortages are recalculated and may differ from what was previously reviewed (BR-WHS-RECALC-ON-SHIFT).

## Postconditions

Shortages are understood; resolutions may start.
