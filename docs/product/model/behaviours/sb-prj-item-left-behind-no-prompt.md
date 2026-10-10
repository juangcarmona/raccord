---
id: SB-PRJ-ITEM-LEFT-BEHIND-NO-PROMPT
type: structured-behaviour
title: "No status prompt while an item is left behind"
status: draft
illustrates:
  - "BR-PRJ-WAREHOUSE-STATUS-PROMPT"
  - "BR-INT-PROJECT-MANAGEMENT-LIFECYCLE"
given:
  - "A project is In progress with 10 items on location"
when: "The technician books 9 items as returned"
then:
  - "The packing list is not proposed as Returned"
  - "The project does not become Execution done"
uses-terms: []
provenance:
  source: ".private/research/help-center/articles/23361953107346.md"
  confidence: high
  recovered-from: documentation
---

## Intent

The missing transition signals that equipment has been left behind.

## Boundaries

None.
