---
id: SB-INT-FIRST-BOOKING-IN-PROGRESS
type: structured-behaviour
title: "First booking sets in progress"
status: draft
illustrates:
  - "BR-INT-PROJECT-MANAGEMENT-LIFECYCLE"
given:
  - "a project is Confirmed"
when: "the warehouse books the first item of the project"
then:
  - "the project status becomes In progress"
uses-terms:
  - "TERM-PROJECT-STATUS"
provenance:
  source: ".private/research/help-center/articles/37629880908050.md"
  confidence: high
  recovered-from: interview
---

## Intent

Automatic transition.

## Boundaries

None.
