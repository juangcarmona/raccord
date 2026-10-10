---
id: FR-FIN-LOCATION-TAX
type: functional-requirement
title: "Maintain location-based tax rates"
status: draft
derived-from:
  - "UC-FIN-MAINTAIN-LOCATION-TAX-RATES"
  - "BR-FIN-LOCATION-RATE-UPDATES"
  - "BR-FIN-TAXABLE-FLAG"
verification:
  - scenario-ref: "SB-FIN-MID-MONTH-RATE-CHANGE"
uses-terms:
  - "TERM-LOCATION-TAX-RATE"
provenance:
  source: ".private/research/help-center/articles/21200846347154.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST derive sales-tax rates from postal codes, refresh them monthly with history and apply them only to taxable equipment.

## Rationale

Local sales-tax systems change often.
