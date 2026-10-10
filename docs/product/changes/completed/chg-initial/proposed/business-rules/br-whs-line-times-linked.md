---
id: BR-WHS-LINE-TIMES-LINKED
type: business-rule
title: "Equipment times always reference schedule entries"
status: draft
applies-to:
  - "UC-WHS-SET-LINE-TIMES"
  - "UC-WHS-ORGANIZE-GROUPS"
uses-terms:
  - "TERM-PLANNING-PERIOD"
  - "TERM-USAGE-PERIOD"
  - "TERM-TIME-SCHEDULE"
provenance:
  source: ".private/research/help-center/articles/38542583895186.md, .private/research/help-center/articles/360017647460.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Every planning or usage period on a line or group is a reference to a named time-schedule entry of the project (new entries can be created in place); free-typed times are not allowed, so equipment times follow schedule shifts.

## Rationale

Prevents equipment times silently drifting from the project when it moves.

## Examples

Shifting a project two days moves all equipment periods two days.

## Exceptions

Before per-line times were introduced, group times not linked to schedule entries did not move with shifts; existing data is not converted.
