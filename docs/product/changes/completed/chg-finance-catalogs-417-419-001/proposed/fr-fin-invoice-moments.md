---
id: FR-FIN-INVOICE-MOMENTS
type: functional-requirement
title: "Configure invoice moments"
status: active
derived-from:
  - "UC-FIN-CONFIGURE-INVOICE-MOMENTS"
  - "BR-FIN-MOMENT-TRIGGER-DATES"
  - "BR-FIN-MOMENT-PERCENTAGES"
  - "BR-FIN-MOMENT-DEFAULTS"
verification:
  - scenario-ref: "SB-FIN-FIFTY-FIFTY-MOMENTS"
uses-terms:
  - "TERM-INVOICE-MOMENT"
provenance:
  source: ".private/research/help-center/articles/360013628279.md, .private/research/help-center/articles/360017647300.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST support invoice moments of up to three percentage parts triggered on confirmation, before the first day or after the last day, with defaults per project type and customer.

## Rationale

Staged billing is common.
