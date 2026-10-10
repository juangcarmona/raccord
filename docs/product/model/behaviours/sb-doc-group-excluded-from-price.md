---
id: "SB-DOC-GROUP-EXCLUDED-FROM-PRICE"
type: structured-behaviour
title: "A cables group appears only on the packing slip"
status: draft
illustrates:
  - "BR-DOC-GROUP-PRICE-VISIBILITY"
given:
  - "The project's cables group is marked as not shown in the price calculation"
when: "The coordinator generates a quotation and a packing slip"
then:
  - "The quotation lists no cables"
  - "The packing slip lists the cables"
uses-terms:
  - "TERM-EQUIPMENT-GROUP"
provenance:
  source: ".private/research/help-center/articles/360017647860.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Price-irrelevant groups stay off customer documents.

## Boundaries

None.
