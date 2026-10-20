---
id: BR-PRJ-SINGLE-RUNNING-TIMER
type: business-rule
title: "One running task timer per person"
status: active
applies-to:
  - "UC-PRJ-TRACK-TASK-TIME"
uses-terms:
  - "TERM-TRACKED-TIME"
provenance:
  source: ".private/research/help-center/articles/38423178380050.md"
  confidence: high
  recovered-from: documentation
---

## Rule

A person has at most one running task timer at a time, synchronised across the web and the mobile web app; it can be stopped from either.

## Rationale

Hours must not be double counted.

## Examples

A timer started on the phone is stopped at the desk.

## Exceptions

None.
