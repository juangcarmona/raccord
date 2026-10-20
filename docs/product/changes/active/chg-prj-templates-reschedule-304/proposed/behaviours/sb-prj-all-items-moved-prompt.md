---
id: SB-PRJ-ALL-ITEMS-MOVED-PROMPT
type: structured-behaviour
title: "Moving all equipment to the next stage prompts a project status change"
status: active
illustrates:
  - "BR-PRJ-WAREHOUSE-STATUS-PROMPT"
given:
  - "A confirmed project has one packing list with 10 items"
  - "9 items are already booked as prepped"
when: "The technician books the tenth item as prepped"
then:
  - "The product proposes Prepped as the packing list's warehouse status"
  - "The project status becomes In progress, not Prepped"
uses-terms: []
provenance:
  source: ".private/research/help-center/articles/23361953107346.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Warehouse completion drives the packing list, and execution start drives the project.

## Boundaries

None.
