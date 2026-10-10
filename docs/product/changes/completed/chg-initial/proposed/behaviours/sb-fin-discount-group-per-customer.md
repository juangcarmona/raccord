---
id: SB-FIN-DISCOUNT-GROUP-PER-CUSTOMER
type: structured-behaviour
title: "Different customers get different group discounts"
status: draft
illustrates:
  - "BR-FIN-DISCOUNT-GROUP-PERCENTAGE"
given:
  - "Customer A has 10% on rigging and 20% on audio"
  - "Customer B has 15% on audio only"
when: "Customer B becomes the client of a project with audio and rigging equipment"
then:
  - "Audio items get a 15% discount"
  - "Rigging items get no group discount"
uses-terms:
  - "TERM-DISCOUNT-GROUP"
provenance:
  source: ".private/research/help-center/articles/360013890780.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Discounts depend on both customer and group.

## Boundaries

None.
