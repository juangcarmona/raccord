---
id: SB-FIN-CREW-EXPENSE-OVERRIDE
type: structured-behaviour
title: "Expense override for one crew member"
status: draft
illustrates:
  - "BR-FIN-CREW-EXPENSE-INHERITANCE"
given:
  - "A planned function has no accommodation expense"
when: "The planner sets accommodation 120.00 for one scheduled technician"
then:
  - "That technician's accommodation is 120.00 in this project"
  - "Other technicians on the function keep 0.00"
  - "The default function and other projects are unchanged"
uses-terms:
  - "TERM-CREW-EXPENSE"
provenance:
  source: ".private/research/help-center/articles/31405085792146.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Overrides are local.

## Boundaries

None.
