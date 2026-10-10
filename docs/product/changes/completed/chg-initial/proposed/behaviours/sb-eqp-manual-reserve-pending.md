---
id: SB-EQP-MANUAL-RESERVE-PENDING
type: structured-behaviour
title: "Manual reservation on a pending project"
status: draft
illustrates:
  - "BR-WHS-MANUAL-RESERVATION"
given:
  - "A pending project plans 3 projectors that are available"
when: "The planner sets the reserved quantity of the open equipment to 3 for that project"
then:
  - "3 units are reserved for the pending project"
  - "The project is shown as fully reserved for that item"
uses-terms:
  - "TERM-RESERVATION"
provenance:
  source: ".private/research/help-center/articles/360017701459.md, .private/research/help-center/articles/360017701499.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Planners can prioritise projects before confirmation.

## Boundaries

None.
