---
id: SB-EQP-PACKED-PER-VOLUME
type: structured-behaviour
title: "Packed-per rounds volume up"
status: draft
illustrates:
  - "BR-EQP-TRANSPORT-VOLUME-PACKED-PER"
given:
  - "Glasses are packed per 10"
  - "The transport volume entered is that of one crate of 10"
when: "11 glasses are planned on a project"
then:
  - "The transport volume counted is that of 2 crates"
  - "Availability is reduced by 11 glasses, not 20"
uses-terms:
  - "TERM-PACKED-PER"
  - "TERM-TRANSPORT-VOLUME"
provenance:
  source: ".private/research/help-center/articles/13141562920338.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Separates transport packaging from stock.

## Boundaries

None.
