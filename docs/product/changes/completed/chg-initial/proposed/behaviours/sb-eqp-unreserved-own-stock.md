---
id: SB-EQP-UNRESERVED-OWN-STOCK
type: structured-behaviour
title: "Own stock available but not reserved"
status: draft
illustrates:
  - "BR-WHS-SHORTAGE-EXPLAINED"
given:
  - "A confirmed project could not reserve all units earlier"
  - "Units became free after another project was cancelled"
when: "The planned line is checked"
then:
  - "It is marked as short of reserved own stock although stock is available"
  - "Reserving manually or saving the project again resolves it"
uses-terms:
  - "TERM-UNRESERVED-STOCK-SHORTAGE"
provenance:
  source: ".private/research/help-center/articles/360017698379.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Explains a flagged line that is solvable without obtaining more equipment.

## Boundaries

None.
