---
id: SB-FIN-SCHEME-MAPS-CLASSES
type: structured-behaviour
title: "Scheme percentages per class"
status: draft
illustrates:
  - "BR-FIN-VAT-FROM-CLASS-AND-SCHEME"
given:
  - "A per-class scheme sets high rate 23% and low rate 10%"
when: "A project uses that scheme"
then:
  - "High-rate items are charged 23%"
  - "Low-rate items are charged 10%"
uses-terms:
  - "TERM-VAT-SCHEME"
  - "TERM-VAT-CLASS"
provenance:
  source: ".private/research/help-center/articles/360013887460.md, .private/research/help-center/articles/360017648360.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Classes get rates only through schemes.

## Boundaries

None.
