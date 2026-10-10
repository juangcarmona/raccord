---
id: FR-WHS-035
type: functional-requirement
title: "Loss reporting"
status: draft
derived-from:
  - "UC-WHS-MARK-LOST"
  - "BR-WHS-DEFECT-LOSS-TIMING"
  - "BR-WHS-CHARGE-TO-PROJECT"
  - "BR-WHS-LOSS-REDUCES-STOCK"
verification:
  - scenario-ref: "SB-WHS-LOSS-CHARGED"
  - scenario-ref: "SB-WHS-FOUND-RESTORES"
uses-terms:
  - "TERM-LOST-EQUIPMENT"
provenance:
  source: ".private/research/help-center/articles/17531415206418.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST let technicians report losses (reducing stock) during return, optionally charging the customer through a project cost and optionally booking back at once, and MUST let lost units be marked found.

## Rationale

Recovery of losses.
