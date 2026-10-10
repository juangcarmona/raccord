---
id: SB-FIN-FOREIGN-CLIENT-REVERSE-CHARGE
type: structured-behaviour
title: "Foreign client with reverse charge"
status: draft
illustrates:
  - "BR-FIN-VAT-SCHEME-PRECEDENCE"
  - "BR-FIN-VAT-FROM-CLASS-AND-SCHEME"
given:
  - "A foreign business customer has a reverse-charge scheme"
when: "The customer becomes client of a project and its defaults are applied"
then:
  - "Every line of the project carries 0% VAT"
uses-terms:
  - "TERM-REVERSE-CHARGE"
provenance:
  source: ".private/research/help-center/articles/360013887460.md, .private/research/help-center/articles/360017648360.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Customer taxation overrides the default.

## Boundaries

None.
