---
id: SB-PRJ-SHIFT-CONFLICT-REFUSED
type: structured-behaviour
title: "A conflicting whole-project shift changes nothing"
status: active
illustrates:
  - "BR-PRJ-SHIFT-RECHECKS-AVAILABILITY"
  - "UC-PRJ-SHIFT-PROJECT-TIMES"
given:
  - "A project has multiple periods and linked equipment reservations"
  - "Moving the whole schedule would create an equipment availability conflict"
when: "The coordinator previews and applies the project shift"
then:
  - "The product reports the availability conflict"
  - "No project period is moved"
  - "No equipment reservation is changed"
uses-terms:
  - "TERM-TIME-PERIOD"
provenance:
  source: "GitHub issue #304 acceptance criteria"
  confidence: high
  recovered-from: observation
---

## Intent

A project schedule shift either preserves all linked availability or is refused in full.

## Boundaries

This scenario states the project-level outcome; equipment reservation implementation is delivered by #311.
