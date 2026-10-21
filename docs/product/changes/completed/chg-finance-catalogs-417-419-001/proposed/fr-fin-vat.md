---
id: FR-FIN-VAT
type: functional-requirement
title: "Tax by VAT classes and schemes"
status: active
derived-from:
  - "UC-FIN-CONFIGURE-VAT"
  - "UC-FIN-SELECT-VAT-SCHEME"
  - "BR-FIN-VAT-FROM-CLASS-AND-SCHEME"
  - "BR-FIN-VAT-SCHEME-PRECEDENCE"
  - "BR-FIN-USED-RATE-FROZEN"
verification:
  - scenario-ref: "SB-FIN-SCHEME-MAPS-CLASSES"
  - scenario-ref: "SB-FIN-FOREIGN-CLIENT-REVERSE-CHARGE"
  - scenario-ref: "SB-FIN-NEW-YEAR-VAT-SCHEME"
uses-terms:
  - "TERM-VAT-SCHEME"
  - "TERM-VAT-CLASS"
provenance:
  source: ".private/research/help-center/articles/360013887460.md, .private/research/help-center/articles/360017648360.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST compute VAT from item classes and the applicable scheme (per class, fixed or reverse charge), selectable per customer, project and invoice, and never change a used rate.

## Rationale

Correct taxation across customers and years.
