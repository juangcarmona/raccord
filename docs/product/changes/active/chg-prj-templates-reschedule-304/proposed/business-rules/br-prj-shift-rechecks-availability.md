---
id: BR-PRJ-SHIFT-RECHECKS-AVAILABILITY
type: business-rule
title: "A project shift rechecks availability atomically"
status: active
applies-to:
  - "UC-PRJ-SHIFT-PROJECT-TIMES"
uses-terms:
  - "TERM-TIME-PERIOD"
provenance:
  source: "GitHub issue #304 acceptance criteria"
  confidence: high
  recovered-from: observation
---

## Rule

Before applying a shift to a project schedule, the product rechecks availability for every affected equipment reservation as one operation. If any affected reservation conflicts, the shift is refused and no project period, linked planning item or reservation is changed.

## Rationale

A project schedule and its equipment availability must not diverge after a partial move.

## Examples

A postponed project conflicts with another reservation for one item; the product refuses the shift and keeps all project periods and reservations at their original times.

## Exceptions

None.
