---
id: SB-WSP-ROLL-OVER
type: structured-behaviour
title: "Equipment rolls over from one project to the next"
status: draft
illustrates:
  - "BR-WSP-ROLL-OVER-WITH-STATUSES"
given:
  - "Custom outgoing and incoming roll-over statuses exist"
  - "Two microphones on location in project A are booked into the outgoing roll-over status"
  - "Project B has them pre-booked in the incoming roll-over status"
when: "The crew hands the rolled-over items to the next project"
then:
  - "The items are returned in the first project"
  - "The second project shows them in the incoming roll-over status and the warehouse does not pack them"
  - "Once they arrive they are set on location in the second project"
uses-terms:
  - "TERM-CUSTOM-WAREHOUSE-STATUS"
provenance:
  source: ".private/research/help-center/articles/35217346228370.md"
  confidence: medium
  recovered-from: documentation
---

## Intent

Shows the documented workaround end to end.

## Boundaries

Not a native transfer; availability between the projects depends on the booked statuses.
