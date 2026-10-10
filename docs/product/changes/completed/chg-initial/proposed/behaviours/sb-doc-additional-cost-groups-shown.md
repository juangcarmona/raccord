---
id: "SB-DOC-ADDITIONAL-COST-GROUPS-SHOWN"
type: structured-behaviour
title: "Cost groups with subtotals"
status: draft
illustrates:
  - "BR-DOC-ADDITIONAL-COST-PRESENTATION"
given:
  - "A project has expenses of 300 and consumables of 50 as additional costs, without discounts"
when: "A quotation is generated with groups switched on"
then:
  - "Expenses are listed under their group name with subtotal 300"
  - "Consumables are listed under their group name with subtotal 50"
  - "A total of 350 closes the additional costs"
uses-terms:
  - "TERM-ADDITIONAL-COSTS-MODULE"
provenance:
  source: ".private/research/help-center/articles/27353381539602.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Groups make costs readable.

## Boundaries

None.
