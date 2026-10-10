---
id: FR-PRJ-104
type: functional-requirement
title: "Show physical and price totals of planned equipment"
status: draft
derived-from:
  - "UC-PRJ-VIEW-EQUIPMENT-TOTALS"
  - "BR-PRJ-EQUIPMENT-TOTALS"
verification:
  - id: "EQUIPMENT-TOTALS"
    scenario: "Given a planned item in a group of a project, with weight, volume and power recorded on some items only, when the coordinator opens the item's details, totals of weight, volume, power usage, power, rental price and discounted rental price are shown for its group and project, and items without data contribute nothing."
uses-terms: []
provenance:
  source: ".private/research/help-center/articles/24621478894354.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST show, from the details of a planned item, the totals of weight, volume, power usage, power, rental price and discounted rental price for the item's group and project, counting items without recorded data as contributing nothing, and MUST NOT change any data by showing them.

## Rationale

Transport and power planning depend on knowing how heavy, large and demanding the planned equipment is.
