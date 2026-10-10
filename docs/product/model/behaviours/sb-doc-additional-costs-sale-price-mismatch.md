---
id: "SB-DOC-ADDITIONAL-COSTS-SALE-PRICE-MISMATCH"
type: structured-behaviour
title: "Sale price alone does not add up"
status: draft
illustrates:
  - "BR-DOC-PRICE-IN-PROJECT"
given:
  - "An additional cost has quantity 2, sale price 100 and a 10% discount"
when: "An invoice is generated showing only the sale price per line"
then:
  - "The line shows 100"
  - "The additional-costs total shows 180"
  - "Showing the price in project, the line would show 180"
uses-terms:
  - "TERM-ADDITIONAL-COSTS-MODULE"
provenance:
  source: ".private/research/help-center/articles/27353381539602.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Explains why the price in project should be shown.

## Boundaries

None.
