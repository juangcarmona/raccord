---
id: BR-WHS-RECALC-ON-SHIFT
type: business-rule
title: "Moving a project recalculates availability and may create shortages"
status: draft
applies-to:
  - "UC-WHS-SET-LINE-TIMES"
  - "UC-WHS-REVIEW-SHORTAGES"
  - "BC-EQUIPMENT"
  - "UC-PRJ-SHIFT-PROJECT-TIMES"
uses-terms:
  - "TERM-EQUIPMENT-AVAILABILITY"
  - "TERM-SHORTAGE"
provenance:
  source: ".private/research/help-center/articles/17035832287506.md, .private/research/help-center/articles/23280754011154.md"
  confidence: high
  recovered-from: documentation
---

## Rule

When a project's schedule is shifted, every linked planning period moves with it and availability and shortages are recalculated for the new interval; new shortages may appear and others disappear.

## Rationale

Times are references to the schedule, so availability must follow.

## Examples

A festival moved one week later now overlaps a confirmed corporate job and shows shortages of shared lighting.

## Exceptions

None.
