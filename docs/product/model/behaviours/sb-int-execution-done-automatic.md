---
id: SB-INT-EXECUTION-DONE-AUTOMATIC
type: structured-behaviour
title: "Last return sets execution done"
status: draft
illustrates:
  - "BR-INT-PROJECT-MANAGEMENT-LIFECYCLE"
given:
  - "a project is In progress"
  - "all other packing moments are returned"
when: "the last item of the last packing moment is booked back"
then:
  - "the project status becomes Execution done"
  - "Complete is not set until a user sets it"
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
