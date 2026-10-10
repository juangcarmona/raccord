---
id: SB-FIN-MID-MONTH-RATE-CHANGE
type: structured-behaviour
title: "Mid-month local rate change"
status: draft
illustrates:
  - "BR-FIN-LOCATION-RATE-UPDATES"
given:
  - "A tax rate is linked to a postal code"
when: "The local rate changes on 15 March"
then:
  - "The product still applies the old rate until 1 April"
  - "On 1 April the rate is updated and the old one kept in history"
  - "Affected projects show a notice"
uses-terms:
  - "TERM-LOCATION-TAX-RATE"
provenance:
  source: ".private/research/help-center/articles/21200846347154.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Monthly refresh cadence.

## Boundaries

None.
